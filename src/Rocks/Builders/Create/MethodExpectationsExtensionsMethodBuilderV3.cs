using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MethodExpectationsExtensionsMethodBuilderV3
{
	internal static void Build(IndentedTextWriter writer, MethodModel method)
	{
		var isExplicitImplementation = method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes;
		var mockTypeName = method.MockType.FullyQualifiedName;
		var containingTypeName = method.ContainingTypeFullyQualifiedName;
		var namingContext = new VariableNamingContextV3(method);

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
							PointerArgTypeBuilderV3.GetProjectedFullyQualifiedName(_.Type, method.MockType) :
							RefLikeArgTypeBuilderV3.GetProjectedFullyQualifiedName(_.Type, method.MockType);
						return $"{argName} @{_.Name}";
					}
					else
					{
						var requiresNullable = _.RequiresNullableAnnotation ? "?" : string.Empty;
						return $"global::Rocks.Argument<{_.Type.FullyQualifiedName}{requiresNullable}> @{_.Name}";
					}
				})));

		var callbackDelegateTypeName = method.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilderV3.GetProjectedCallbackDelegateFullyQualifiedName(method, method.MockType) :
			method.ReturnsVoid ? 
				DelegateBuilderV3.Build(method.Parameters) :
				DelegateBuilderV3.Build(method.Parameters, method.ReturnType);
		var returnType = method.ReturnsVoid ? string.Empty :
			method.ReturnType.IsRefLikeType ?
				MockProjectedDelegateBuilderV3.GetProjectedReturnValueDelegateFullyQualifiedName(method, method.MockType) :
				method.ReturnType.FullyQualifiedName;
		var adornmentsType = method.ReturnsVoid ?
			$"global::Rocks.MethodAdornments<{mockTypeName}, {callbackDelegateTypeName}>" :
			method.ReturnType.IsPointer ?
				$"{MockProjectedTypesAdornmentsBuilderV3.GetProjectedAdornmentFullyQualifiedNameName(method.ReturnType, method.MockType, AdornmentType.Method, method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)}<{mockTypeName}, {callbackDelegateTypeName}>" :
				$"global::Rocks.MethodAdornments<{mockTypeName}, {callbackDelegateTypeName}, {returnType}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		var addMethod = method.ReturnsVoid ? "Add" :
			method.ReturnType.IsPointer ?
				MockProjectedTypesAdornmentsBuilderV3.GetProjectedAddExtensionMethodName(method.ReturnType) : 
				$"Add<{returnType}>";

		var constraints = method.Constraints;
		var extensionConstraints = constraints.Length > 0 ?
			method.Parameters.Length == 0 ? $" {string.Join(" ", constraints)} " : $" {string.Join(" ", constraints)}" : 
			method.Parameters.Length == 0 ? " " : "";

		if (method.Parameters.Length == 0)
		{
			writer.WriteLine($"internal static {returnValue} {method.Name}({instanceParameters}){extensionConstraints}=>");
			writer.Indent++;
			writer.WriteLine($"{newAdornments}(@{namingContext["self"]}.{addMethod}({method.MemberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>()));");
			writer.Indent--;
		}
		else
		{
			writer.WriteLine($"internal static {returnValue} {method.Name}({instanceParameters}){extensionConstraints}");
			writer.WriteLine("{");
			writer.Indent++;

			foreach(var parameter in method.Parameters)
			{
				writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
			}

			var parameters = string.Join(", ", method.Parameters.Select(_ =>
			{
				if (_.HasExplicitDefaultValue)
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
	}
}