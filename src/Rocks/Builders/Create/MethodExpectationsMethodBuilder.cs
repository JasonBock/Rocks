using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MethodExpectationsMethodBuilder
{
	internal static void Build(IndentedTextWriter writer, MethodModel method, string expectationsFullyQualifiedName,
		Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		static void BuildImplementation(IndentedTextWriter writer, MethodModel method, bool isGeneratedWithDefaults,
			string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
		{
			var namingContext = new VariableNamingContext(method);
			var needsGenerationWithDefaults = false;
			var typeArgumentsNamingContext = method.IsGenericMethod ?
				new VariableNamingContext(method.MockType.TypeArguments.ToImmutableHashSet()) :
				new VariableNamingContext();

			var instanceParameters = method.Parameters.Length == 0 ? string.Empty :
				string.Join(", ", method.Parameters.Select(_ =>
				{
					if (_.Type.IsEsoteric)
					{
						var argName = _.Type.IsPointer ?
							PointerArgTypeBuilder.GetProjectedFullyQualifiedName(_.Type, method.MockType) :
							RefLikeArgTypeBuilder.GetProjectedFullyQualifiedName(_.Type, method.MockType);
						return $"{argName} @{_.Name}";
					}
					else
					{
						var requiresNullable = _.RequiresNullableAnnotation ? "?" : string.Empty;
						var typeName = method.IsGenericMethod && method.TypeArguments.Contains(_.Type.FullyQualifiedName) ?
							typeArgumentsNamingContext[_.Type.FullyQualifiedName] : _.Type.FullyQualifiedName;

						if (isGeneratedWithDefaults)
						{
							if (_.HasExplicitDefaultValue)
							{
								return $"{typeName}{requiresNullable} @{_.Name} = {_.ExplicitDefaultValue}";
							}
							else
							{
								return _.IsParams ?
									$"params {typeName}{requiresNullable} @{_.Name}" :
									$"global::Rocks.Argument<{typeName}{requiresNullable}> @{_.Name}";
							}
						}

						if (!isGeneratedWithDefaults)
						{
							// Only set this flag if we're currently not generating with defaults.
							needsGenerationWithDefaults |= _.HasExplicitDefaultValue;
						}

						return $"global::Rocks.Argument<{typeName}{requiresNullable}> @{_.Name}";
					}
				}));

			var typeArguments = method.IsGenericMethod ?
				$"<{string.Join(", ", method.TypeArguments.Select(_ => !method.MockType.TypeArguments.Contains(_) ? _ : typeArgumentsNamingContext[_]))}>" : string.Empty;
			var callbackDelegateTypeName = method.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(method, method.MockType) :
				DelegateBuilder.Build(method);

			string adornmentsType;

			if (method.ReturnType.TypeKind == TypeKind.FunctionPointer ||
				method.ReturnType.TypeKind == TypeKind.Pointer)
			{
				var projectedAdornmentTypeName = MockProjectedAdornmentsTypesBuilder.GetProjectedAdornmentsFullyQualifiedNameName(method.ReturnType, method.MockType);
				adornmentsType = $"{projectedAdornmentTypeName}<AdornmentsForHandler{method.MemberIdentifier}{typeArguments}, {callbackDelegateTypeName}>";
			}
			else
			{
				var handlerTypeName = $"{expectationsFullyQualifiedName}.Handler{method.MemberIdentifier}{typeArguments}";
				var returnType =
					method.ReturnsVoid ?
						string.Empty :
						method.ReturnType.IsRefLikeType ?
							MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(method, method.MockType) :
							method.IsGenericMethod && method.TypeArguments.Contains(method.ReturnType.FullyQualifiedName) ?
								typeArgumentsNamingContext[method.ReturnType.FullyQualifiedName] : method.ReturnType.FullyQualifiedName;

				adornmentsType = method.ReturnsVoid ?
					$"global::Rocks.Adornments<AdornmentsForHandler{method.MemberIdentifier}{typeArguments}, {handlerTypeName}, {callbackDelegateTypeName}>" :
					$"global::Rocks.Adornments<AdornmentsForHandler{method.MemberIdentifier}{typeArguments}, {handlerTypeName}, {callbackDelegateTypeName}, {returnType}>";
			}

			var constraints = method.Constraints.Length > 0 ?
				$" {string.Join(" ", method.Constraints.Select(_ => _.ToString(typeArgumentsNamingContext, method)))}" : "";
			var hiding = method.RequiresHiding == RequiresHiding.Yes ? "new " : string.Empty;

			adornmentsFQNsPipeline(new(adornmentsType, typeArguments, constraints, method.MemberIdentifier));

			if (isGeneratedWithDefaults)
			{
				var parameterValues = string.Join(", ", method.Parameters.Select(
					p => p.HasExplicitDefaultValue || p.IsParams ? $"global::Rocks.Arg.Is(@{p.Name})" : $"@{p.Name}"));

				writer.WriteLines(
					$$"""
					internal {{hiding}}{{expectationsFullyQualifiedName}}.Adornments.AdornmentsForHandler{{method.MemberIdentifier}}{{typeArguments}} {{method.Name}}{{typeArguments}}({{instanceParameters}}){{constraints}} =>
						this.{{method.Name}}{{typeArguments}}({{parameterValues}});
					""");
			}
			else if (method.Parameters.Length == 0)
			{
				writer.WriteLines(
					$$"""
					internal {{hiding}}{{expectationsFullyQualifiedName}}.Adornments.AdornmentsForHandler{{method.MemberIdentifier}}{{typeArguments}} {{method.Name}}{{typeArguments}}({{instanceParameters}}){{constraints}}
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						var handler = new {{expectationsFullyQualifiedName}}.Handler{{method.MemberIdentifier}}{{typeArguments}}();
						if (this.Expectations.handlers{{method.MemberIdentifier}} is null ) { this.Expectations.handlers{{method.MemberIdentifier}} = new(handler); }
						else { this.Expectations.handlers{{method.MemberIdentifier}}.Add(handler); }
						return new(handler);
					}
					""");
			}
			else
			{
				var handlerContext = new VariableNamingContext(method);
				writer.WriteLine($"internal {hiding}{expectationsFullyQualifiedName}.Adornments.AdornmentsForHandler{method.MemberIdentifier}{typeArguments} {method.Name}{typeArguments}({instanceParameters}){constraints}");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLine("global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);");

				foreach (var parameter in method.Parameters)
				{
					writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
				}

				writer.WriteLine();
				writer.WriteLines(
					$$"""
					var @{{handlerContext["handler"]}} = new {{expectationsFullyQualifiedName}}.Handler{{method.MemberIdentifier}}{{typeArguments}}
					{
					""");
				writer.Indent++;

				var handlerNamingContext = HandlerVariableNamingContext.Create();

				foreach (var parameter in method.Parameters)
				{
					if (parameter.HasExplicitDefaultValue && method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
					{
						writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = @{parameter.Name}.Transform({parameter.ExplicitDefaultValue}),");
					}
					else if (parameter.RefKind == RefKind.Out)
					{
						writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = global::Rocks.Arg.Any<{parameter.Type.FullyQualifiedName}{(parameter.RequiresNullableAnnotation ? "?" : string.Empty)}>(),");
					}
					else
					{
						writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = @{parameter.Name},");
					}
				}

				writer.Indent--;
				writer.WriteLines(
					$$"""
					};

					if (this.Expectations.handlers{{method.MemberIdentifier}} is null ) { this.Expectations.handlers{{method.MemberIdentifier}} = new(@{{handlerContext["handler"]}}); }
					else { this.Expectations.handlers{{method.MemberIdentifier}}.Add(@{{handlerContext["handler"]}}); }
					return new(@{{handlerContext["handler"]}});
					""");

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (needsGenerationWithDefaults)
			{
				BuildImplementation(writer, method, true, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
			}
		}

		BuildImplementation(writer, method, false, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
	}
}