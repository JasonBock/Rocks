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
			var isExplicitImplementation = method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes;
			var mockTypeName = method.MockType.FullyQualifiedName;
			var containingTypeName = method.ContainingType.FullyQualifiedName;
			var namingContext = new VariableNamingContext(method);
			var needsGenerationWithDefaults = false;

			//var thisParameter = isExplicitImplementation ?
			//	$"this global::Rocks.Expectations.ExplicitMethodExpectations<{mockTypeName}, {containingTypeName}> @{namingContext["self"]}" :
			//	$"this global::Rocks.Expectations.MethodExpectations<{mockTypeName}> @{namingContext["self"]}";
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
				$"global::Rocks.MethodAdornmentsV4<{mockTypeName}, {callbackDelegateTypeName}>" :
				method.ReturnType.IsPointer ?
					$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(method.ReturnType, method.MockType, AdornmentType.Method, method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)}<{mockTypeName}, {callbackDelegateTypeName}>" :
					$"global::Rocks.MethodAdornmentsV4<{mockTypeName}, {callbackDelegateTypeName}, {returnType}>";
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
					p => p.HasExplicitDefaultValue || p.IsParams ? $"global::Rocks.Arg.Is(@{p.Name})" : $"@{p.Name}"));
				writer.WriteLine($"internal {returnValue} {method.Name}({instanceParameters}){extensionConstraints} =>");
				writer.Indent++;
				writer.WriteLine($"this.{method.Name}({parameterValues});");
				writer.Indent--;
			}
			else if (method.Parameters.Length == 0)
			{
				writer.WriteLine($"internal {returnValue} {method.Name}({instanceParameters}){extensionConstraints}");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLines(
					$$"""
					var handler = new {{expectationsFullyQualifiedName}}.Handler{{method.MemberIdentifier}}
					{
						Callback = null,
						CallCount = 0,
						ExpectedCallCount = 1,
						ReturnValue = null
					};

					this.expectations.handlers{{method.MemberIdentifier}}.Add(handler);
					return new(handler);
					""");
				writer.Indent--;
				writer.WriteLine("}");
			}
			else
			{
				writer.WriteLine($"internal {returnValue} {method.Name}({instanceParameters}){extensionConstraints}");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var parameter in method.Parameters)
				{
					writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
				}

				writer.WriteLine();
				writer.WriteLines(
					$$"""
					var handler = new {{expectationsFullyQualifiedName}}.Handler{{method.MemberIdentifier}}
					{
					""");
				writer.Indent++;

				var handlerNamingContext = HandlerVariableNamingContextV4.Create();

				foreach (var parameter in method.Parameters)
				{
					if (parameter.HasExplicitDefaultValue && method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
					{
						writer.WriteLine($"{handlerNamingContext[parameter.Name]} = @{parameter.Name}.Transform({parameter.ExplicitDefaultValue}),");
					}
					else if (parameter.RefKind == RefKind.Out)
					{
						writer.WriteLine($"{handlerNamingContext[parameter.Name]} = global::Rocks.Arg.Any<{parameter.Type.FullyQualifiedName}{(parameter.RequiresNullableAnnotation ? "?" : string.Empty)}>(),");
					}
					else
					{
						writer.WriteLine($"{handlerNamingContext[parameter.Name]} = @{parameter.Name},");
					}
				}

				writer.Indent--;
				writer.WriteLines(
					$$"""
						Callback = null,
						CallCount = 0,
						ExpectedCallCount = 1,
						ReturnValue = null
					};

					this.expectations.handlers{{method.MemberIdentifier}}.Add(handler);
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