using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MethodExpectationsExtensionsMethodBuilder
{
	internal static void Build(IndentedTextWriter writer, MethodMockableResult result)
	{
		var method = result.Value;
		var isExplicitImplementation = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes;
		var mockTypeName = result.MockType.GetReferenceableName();
		var containingTypeName = method.ContainingType.GetReferenceableName();

		var thisParameter = isExplicitImplementation ?
			$"this global::Rocks.Expectations.ExplicitMethodExpectations<{mockTypeName}, {containingTypeName}> self" :
			$"this global::Rocks.Expectations.MethodExpectations<{mockTypeName}> self";
		var instanceParameters = method.Parameters.Length == 0 ? thisParameter :
			string.Join(", ", thisParameter,
				string.Join(", ", method.Parameters.Select(_ =>
				{
					if (_.Type.IsEsoteric())
					{
						var argName = _.Type.IsPointer() ?
							PointerArgTypeBuilder.GetProjectedName(_.Type) :
							RefLikeArgTypeBuilder.GetProjectedName(_.Type);
						return $"{argName} {_.Name}";
					}
					else
					{
						return $"global::Rocks.Argument<{_.Type.GetReferenceableName()}> {_.Name}";
					}
				})));
		var parameterTypes = string.Join(", ", method.Parameters.Select(_ => _.Type.GetReferenceableName()));

		var callbackDelegateTypeName = method.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateName(method) :
			method.ReturnsVoid ? 
				DelegateBuilder.Build(method.Parameters) : 
				DelegateBuilder.Build(method.Parameters, method.ReturnType);
		var returnType = method.ReturnsVoid ? string.Empty :
			method.ReturnType.IsRefLikeType ?
				MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateName(method) :
				method.ReturnType.GetReferenceableName();
		var adornmentsType = method.ReturnsVoid ?
			$"global::Rocks.MethodAdornments<{mockTypeName}, {callbackDelegateTypeName}>" :
			method.ReturnType.IsPointer() ?
				$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(method.ReturnType, AdornmentType.Method, result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)}<{mockTypeName}, {callbackDelegateTypeName}>" :
				$"global::Rocks.MethodAdornments<{mockTypeName}, {callbackDelegateTypeName}, {returnType}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		var addMethod = method.ReturnsVoid ? "Add" :
			method.ReturnType.IsPointer() ?
				MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(method.ReturnType) : 
				$"Add<{returnType}>";

		if (method.Parameters.Length == 0)
		{
			writer.WriteLine($"internal static {returnValue} {method.GetName()}({instanceParameters}) =>");
			writer.Indent++;
			writer.WriteLine($"{newAdornments}(self.{addMethod}({result.MemberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>()));");
			writer.Indent--;
		}
		else
		{
			writer.WriteLine($"internal static {returnValue} {method.GetName()}({instanceParameters})");
			writer.WriteLine("{");
			writer.Indent++;

			foreach(var parameter in method.Parameters)
			{
				writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull({parameter.Name});");
			}

			var parameters = string.Join(", ", method.Parameters.Select(_ =>
			{
				if (_.HasExplicitDefaultValue)
				{
					return $"{_.Name}.Transform({_.ExplicitDefaultValue.GetDefaultValue(_.Type.IsValueType)})";
				}
				else if (_.RefKind == RefKind.Out)
				{
					return $"global::Rocks.Arg.Any<{_.Type.GetReferenceableName()}>()";
				}
				else
				{
					return _.Name;
				}
			}));
			writer.WriteLine($"return {newAdornments}(self.{addMethod}({result.MemberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>({method.Parameters.Length}) {{ {parameters} }}));");

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}