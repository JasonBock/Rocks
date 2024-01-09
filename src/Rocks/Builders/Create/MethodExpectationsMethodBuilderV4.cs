using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MethodExpectationsMethodBuilderV4
{
	internal static void Build(IndentedTextWriter writer, MethodModel method, string expectationsFullyQualifiedName)
	{
		static void BuildImplementation(IndentedTextWriter writer, MethodModel method, bool isGeneratedWithDefaults, string expectationsFullyQualifiedName)
		{
			var namingContext = new VariableNamingContext(method);
			var needsGenerationWithDefaults = false;

			var instanceParameters = method.Parameters.Length == 0 ? string.Empty :
				string.Join(", ", method.Parameters.Select(_ =>
				{
					if (_.Type.IsEsoteric)
					{
						var argName = _.Type.IsPointer ?
							PointerArgTypeBuilderV4.GetProjectedFullyQualifiedName(_.Type, method.MockType) :
							RefLikeArgTypeBuilderV4.GetProjectedFullyQualifiedName(_.Type, method.MockType);
						return $"{argName} @{_.Name}";
					}
					else
					{
						var requiresNullable = _.RequiresNullableAnnotation ? "?" : string.Empty;

						if (isGeneratedWithDefaults)
						{
							if (_.HasExplicitDefaultValue)
							{
								return $"{_.Type.FullyQualifiedName}{requiresNullable} @{_.Name} = {_.ExplicitDefaultValue}";
							}
							else
							{
								return _.IsParams ?
									$"params {_.Type.FullyQualifiedName}{requiresNullable} @{_.Name}" :
									$"global::Rocks.Argument<{_.Type.FullyQualifiedName}{requiresNullable}> @{_.Name}";
							}
						}

						if (!isGeneratedWithDefaults)
						{
							// Only set this flag if we're currently not generating with defaults.
							needsGenerationWithDefaults |= _.HasExplicitDefaultValue;
						}

						return $"global::Rocks.Argument<{_.Type.FullyQualifiedName}{requiresNullable}> @{_.Name}";
					}
				}));

			var typeArguments = method.IsGenericMethod ?
				$"<{string.Join(", ", method.TypeArguments)}>" : string.Empty;
			var callbackDelegateTypeName = method.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilderV4.GetProjectedCallbackDelegateFullyQualifiedName(method, method.MockType) :
				method.ReturnsVoid ?
					DelegateBuilder.Build(method.Parameters) :
					DelegateBuilder.Build(method.Parameters, method.ReturnType);

			string adornmentsType;

			if (method.ReturnType.TypeKind == TypeKind.FunctionPointer ||
				method.ReturnType.TypeKind == TypeKind.Pointer)
			{
				var projectedAdornmentTypeName = MockProjectedAdornmentsTypesBuilderV4.GetProjectedAdornmentsFullyQualifiedNameName(method.ReturnType, method.MockType);
				adornmentsType = $"{projectedAdornmentTypeName}<{callbackDelegateTypeName}>";
			}
			else
			{
				var handlerTypeName = $"{expectationsFullyQualifiedName}.Handler{method.MemberIdentifier}{typeArguments}";
				var returnType = 
					method.ReturnsVoid ? 
						string.Empty :
						method.ReturnType.IsRefLikeType ?
							MockProjectedDelegateBuilderV4.GetProjectedReturnValueDelegateFullyQualifiedName(method, method.MockType) :
							method.ReturnType.FullyQualifiedName;

				adornmentsType = method.ReturnsVoid ?
					$"global::Rocks.AdornmentsV4<{handlerTypeName}, {callbackDelegateTypeName}>" :
					$"global::Rocks.AdornmentsV4<{handlerTypeName}, {callbackDelegateTypeName}, {returnType}>";
			}

			var constraints = method.Constraints;
			var extensionConstraints = constraints.Length > 0 ?
				$" {string.Join(" ", constraints)}" : "";
			var hiding = method.RequiresHiding == RequiresHiding.Yes ? "new " : string.Empty;

			if (isGeneratedWithDefaults)
			{
				var parameterValues = string.Join(", ", method.Parameters.Select(
					p => p.HasExplicitDefaultValue || p.IsParams ? $"global::Rocks.Arg.Is(@{p.Name})" : $"@{p.Name}"));

				writer.WriteLine($"internal {hiding}{adornmentsType} {method.Name}({instanceParameters}){extensionConstraints} =>");
				writer.Indent++;
				writer.WriteLine($"this.{method.Name}({parameterValues});");
				writer.Indent--;
			}
			else if (method.Parameters.Length == 0)
			{
				writer.WriteLine($"internal {hiding}{adornmentsType} {method.Name}({instanceParameters}){extensionConstraints}");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLines(
					$$"""
					var handler = new {{expectationsFullyQualifiedName}}.Handler{{method.MemberIdentifier}}{{typeArguments}}();
					this.Expectations.handlers{{method.MemberIdentifier}}.Add(handler);
					return new(handler);
					""");

				writer.Indent--;
				writer.WriteLine("}");
			}
			else
			{
				writer.WriteLine($"internal {hiding}{adornmentsType} {method.Name}({instanceParameters}){extensionConstraints}");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var parameter in method.Parameters)
				{
					writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
				}

				writer.WriteLine();
				writer.WriteLines(
					$$"""
					var handler = new {{expectationsFullyQualifiedName}}.Handler{{method.MemberIdentifier}}{{typeArguments}}
					{
					""");
				writer.Indent++;

				var handlerNamingContext = HandlerVariableNamingContextV4.Create();

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

					this.Expectations.handlers{{method.MemberIdentifier}}.Add(handler);
					return new(handler);
					""");

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (needsGenerationWithDefaults)
			{
				BuildImplementation(writer, method, true, expectationsFullyQualifiedName);
			}
		}

		BuildImplementation(writer, method, false, expectationsFullyQualifiedName);
	}
}