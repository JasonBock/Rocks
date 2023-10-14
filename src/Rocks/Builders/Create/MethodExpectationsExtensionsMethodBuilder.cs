using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MethodExpectationsExtensionsMethodBuilder
{
	internal static void Build(IndentedTextWriter writer, MethodModel method)
	{
		static void BuildImplementation(IndentedTextWriter writer, MethodModel method, bool isGeneratedWithDefaults)
		{
			var isExplicitImplementation = method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes;
			var mockTypeName = method.MockType.FullyQualifiedName;
			var containingTypeName = method.ContainingType.FullyQualifiedName;
			var namingContext = new VariableNamingContext(method);
			var needsGenerationWithDefaults = false;

			var thisParameter = isExplicitImplementation ?
				$"this global::Rocks.Expectations.ExplicitMethodExpectations<{mockTypeName}, {containingTypeName}> @{namingContext["self"]}" :
				$"this global::Rocks.Expectations.MethodExpectations<{mockTypeName}> @{namingContext["self"]}";
			var instanceParameters = method.Parameters.Length == 0 ? thisParameter :
				string.Join(", ", thisParameter,
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

							if (isGeneratedWithDefaults && _.HasExplicitDefaultValue)
							{
								return $"{_.Type.FullyQualifiedName}{requiresNullable} @{_.Name} = {_.ExplicitDefaultValue}";
							}

							if (!isGeneratedWithDefaults)
							{
								// Only set this flag if we're currently not generating with defaults.
								needsGenerationWithDefaults |= _.HasExplicitDefaultValue;
							}

							return $"global::Rocks.Argument<{_.Type.FullyQualifiedName}{requiresNullable}> @{_.Name}";
						}
					})));

			var callbackDelegateTypeName = method.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(method, method.MockType) :
				method.ReturnsVoid ?
					DelegateBuilder.Build(method.Parameters) :
					DelegateBuilder.Build(method.Parameters, method.ReturnType);
			var returnType = method.ReturnsVoid ? string.Empty :
				method.ReturnType.IsRefLikeType ?
					MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(method, method.MockType) :
					method.ReturnType.FullyQualifiedName;
			var adornmentsType = method.ReturnsVoid ?
				$"global::Rocks.MethodAdornments<{mockTypeName}, {callbackDelegateTypeName}>" :
				method.ReturnType.IsPointer ?
					$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(method.ReturnType, method.MockType, AdornmentType.Method, method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)}<{mockTypeName}, {callbackDelegateTypeName}>" :
					$"global::Rocks.MethodAdornments<{mockTypeName}, {callbackDelegateTypeName}, {returnType}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			var addMethod = method.ReturnsVoid ? "Add" :
				method.ReturnType.IsPointer ?
					MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(method.ReturnType) :
					$"Add<{returnType}>";

			var constraints = method.Constraints;
			var extensionConstraints = constraints.Length > 0 ?
				$" {string.Join(" ", constraints)}" : "";

			if (isGeneratedWithDefaults)
			{
				var parameterValues = string.Join(", ", method.Parameters.Select(
					p => p.HasExplicitDefaultValue ? $"global::Rocks.Arg.Is(@{p.Name})" : $"@{p.Name}"));
				writer.WriteLine($"internal static {returnValue} {method.Name}({instanceParameters}){extensionConstraints} =>");
				writer.Indent++;
				writer.WriteLine($"@{namingContext["self"]}.{method.Name}({parameterValues});");
				writer.Indent--;
			}
			else if (method.Parameters.Length == 0)
			{
				writer.WriteLine($"internal static {returnValue} {method.Name}({instanceParameters}){extensionConstraints} =>");
				writer.Indent++;
				writer.WriteLine($"{newAdornments}(@{namingContext["self"]}.{addMethod}({method.MemberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>()));");
				writer.Indent--;
			}
			else
			{
				writer.WriteLine($"internal static {returnValue} {method.Name}({instanceParameters}){extensionConstraints}");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var parameter in method.Parameters)
				{
					writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
				}

				var parameters = string.Join(", ", method.Parameters.Select(_ =>
				{
					if (_.HasExplicitDefaultValue && method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
					{
						return $"@{_.Name}.Transform({_.ExplicitDefaultValue})";
					}
					else if (_.RefKind == RefKind.Out)
					{
						return $"global::Rocks.Arg.Any<{_.Type.FullyQualifiedName}{(_.RequiresNullableAnnotation ? "?" : string.Empty)}>()";
					}
					else
					{
						return $"@{_.Name}";
					}
				}));
				writer.WriteLine($"return {newAdornments}(@{namingContext["self"]}.{addMethod}({method.MemberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>({method.Parameters.Length}) {{ {parameters} }}));");

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (needsGenerationWithDefaults)
			{
				BuildImplementation(writer, method, true);
			}
		}

		BuildImplementation(writer, method, false);
	}
}