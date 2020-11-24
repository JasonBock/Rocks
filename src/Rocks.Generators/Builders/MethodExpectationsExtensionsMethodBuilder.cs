using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders
{
	internal static class MethodExpectationsExtensionsMethodBuilder
	{
		internal static void Build(IndentedTextWriter writer, MethodMockableResult result)
		{
			var method = result.Value;
			var isExplicitImplementation = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes;
			var mockTypeName = result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var containingTypeName = method.ContainingType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			
			var thisParameter = isExplicitImplementation ?
				$"this ExplicitMethodExpectations<{mockTypeName}, {containingTypeName}> self" :
				$"this MethodExpectations<{mockTypeName}> self";
			var instanceParameters = method.Parameters.Length == 0 ? thisParameter :
				string.Join(", ", thisParameter,
					string.Join(", ", method.Parameters.Select(_ => $"Arg<{_.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}> {_.Name}")));
			var parameterTypes = string.Join(", ", method.Parameters.Select(_ => _.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));

			var adornmentsType = method.ReturnsVoid ? 
				$"MethodAdornments<{mockTypeName}, {DelegateBuilder.GetDelegate(method.Parameters)}>" :
				$"MethodAdornments<{mockTypeName}, {DelegateBuilder.GetDelegate(method.Parameters, method.ReturnType)}, {method.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			writer.WriteLine($"internal static {returnValue} {method.Name}({instanceParameters}) =>");
			writer.Indent++;

			var addReturnValue = method.ReturnsVoid ? string.Empty : $"<{method.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>";

			if(method.Parameters.Length == 0)
			{
				writer.WriteLine($"{newAdornments}(self.Add{addReturnValue}({result.MemberIdentifier}, new List<Arg>()));");
			}
			else
			{
				writer.WriteLine($"{newAdornments}(self.Add{addReturnValue}({result.MemberIdentifier}, new List<Arg> {{ {string.Join(", ", method.Parameters.Select(_ => _.Name))} }}));");
			}

			writer.Indent--;
		}
	}
}