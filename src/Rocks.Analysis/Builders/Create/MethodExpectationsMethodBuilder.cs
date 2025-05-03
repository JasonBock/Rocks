using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class MethodExpectationsMethodBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type, MethodModel method, string expectationsFullyQualifiedName,
		Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		static void BuildImplementation(IndentedTextWriter writer, TypeMockModel type, MethodModel method, bool isGeneratedWithDefaults,
			string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
		{
			var needsGenerationWithDefaults = false;
			var typeArgumentsNamingContext = method.IsGenericMethod ?
				new TypeArgumentsNamingContext(method) :
				new TypeArgumentsNamingContext();

			var instanceParameters = method.Parameters.Length == 0 ? string.Empty :
				string.Join(", ", method.Parameters.Select(_ =>
				{
					var argumentTypeName = ProjectionBuilder.BuildArgument(
						_.Type, typeArgumentsNamingContext, _.RequiresNullableAnnotation);

				   if (_.Type.IsPointer)
				   {
						return $"{argumentTypeName} @{_.Name}";
					}
					else
					{
						var requiresNullable = _.RequiresNullableAnnotation ? "?" : string.Empty;
						var typeName = _.Type.BuildName(typeArgumentsNamingContext);

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
									$"{argumentTypeName} @{_.Name}";
							}
						}

						if (!isGeneratedWithDefaults)
						{
							// Only set this flag if we're currently not generating with defaults.
							needsGenerationWithDefaults |= _.HasExplicitDefaultValue;
						}

						return $"{argumentTypeName} @{_.Name}";
					}
				}));

			var typeArguments = method.IsGenericMethod ?
				$"<{string.Join(", ", method.TypeArguments.Select(_ => _.BuildName(typeArgumentsNamingContext)))}>" : string.Empty;
			var callbackDelegateTypeName = method.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(method, method.MockType, expectationsFullyQualifiedName, method.MemberIdentifier) :
				DelegateBuilder.Build(method);

			string adornmentsType;

			var handlerTypeName = $"{expectationsFullyQualifiedName}.Handler{method.MemberIdentifier}{typeArguments}";

			if (method.ReturnType.IsPointer)
			{
				adornmentsType = $"global::Rocks.Adornments<AdornmentsForHandler{method.MemberIdentifier}{typeArguments}, {handlerTypeName}, {callbackDelegateTypeName}>";
			}
			else
			{
				var returnType =
					method.ReturnsVoid ?
						string.Empty :
						method.ReturnType.IsRefLikeType || method.ReturnType.AllowsRefLikeType ?
							$"global::System.Func<{method.ReturnType.BuildName(typeArgumentsNamingContext)}>" :
							method.ReturnType.BuildName(typeArgumentsNamingContext);

				adornmentsType = method.ReturnsVoid ?
					$"global::Rocks.Adornments<AdornmentsForHandler{method.MemberIdentifier}{typeArguments}, {handlerTypeName}, {callbackDelegateTypeName}>" :
					$"global::Rocks.Adornments<AdornmentsForHandler{method.MemberIdentifier}{typeArguments}, {handlerTypeName}, {callbackDelegateTypeName}, {returnType}>";
			}

			var constraints = method.Constraints.Length > 0 ?
				$" {string.Join(" ", method.Constraints.Select(_ => _.ToString(typeArgumentsNamingContext, method)))}" : "";
			var hiding = method.RequiresHiding == RequiresHiding.Yes ? "new " : string.Empty;

			adornmentsFQNsPipeline(new(adornmentsType, typeArguments, constraints, method, method.MemberIdentifier));

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
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.{{type.ExpectationsPropertyName}}.WasInstanceInvoked);
						var handler = new {{expectationsFullyQualifiedName}}.Handler{{method.MemberIdentifier}}{{typeArguments}}();
						if (this.{{type.ExpectationsPropertyName}}.handlers{{method.MemberIdentifier}} is null) { this.{{type.ExpectationsPropertyName}}.handlers{{method.MemberIdentifier}} = new(handler); }
						else { this.{{type.ExpectationsPropertyName}}.handlers{{method.MemberIdentifier}}.Add(handler); }
						return new(handler);
					}
					""");
			}
			else
			{
				var handlerContext = new VariablesNamingContext(method);
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
						writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = global::Rocks.Arg.Any<{parameter.Type.BuildName(typeArgumentsNamingContext)}{(parameter.RequiresNullableAnnotation ? "?" : string.Empty)}>(),");
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

					if (this.{{type.ExpectationsPropertyName}}.handlers{{method.MemberIdentifier}} is null) { this.{{type.ExpectationsPropertyName}}.handlers{{method.MemberIdentifier}} = new(@{{handlerContext["handler"]}}); }
					else { this.{{type.ExpectationsPropertyName}}.handlers{{method.MemberIdentifier}}.Add(@{{handlerContext["handler"]}}); }
					return new(@{{handlerContext["handler"]}});
					""");

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (needsGenerationWithDefaults)
			{
				BuildImplementation(writer, type, method, true, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
			}
		}

		BuildImplementation(writer, type, method, false, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
	}
}