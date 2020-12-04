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
			var mockTypeName = result.MockType.GetName();
			var containingTypeName = method.ContainingType.GetName();
			
			var thisParameter = isExplicitImplementation ?
				$"this ExplicitMethodExpectations<{mockTypeName}, {containingTypeName}> self" :
				$"this MethodExpectations<{mockTypeName}> self";
			var instanceParameters = method.Parameters.Length == 0 ? thisParameter :
				string.Join(", ", thisParameter,
					string.Join(", ", method.Parameters.Select(_ => $"Arg<{_.Type.GetName()}> {_.Name}")));
			var parameterTypes = string.Join(", ", method.Parameters.Select(_ => _.Type.GetName()));

			var adornmentsType = method.ReturnsVoid ? 
				$"MethodAdornments<{mockTypeName}, {DelegateBuilder.Build(method.Parameters)}>" :
				$"MethodAdornments<{mockTypeName}, {DelegateBuilder.Build(method.Parameters, method.ReturnType)}, {method.ReturnType.GetName()}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			writer.WriteLine($"internal static {returnValue} {method.GetName()}({instanceParameters}) =>");
			writer.Indent++;

			var addReturnValue = method.ReturnsVoid ? string.Empty : $"<{method.ReturnType.GetName()}>";

			if(method.Parameters.Length == 0)
			{
				writer.WriteLine($"{newAdornments}(self.Add{addReturnValue}({result.MemberIdentifier}, new List<Arg>()));");
			}
			else
			{
				var parameters = string.Join(", ", method.Parameters.Select(
					_ => _.HasExplicitDefaultValue ? $"{_.Name}.Transform({_.ExplicitDefaultValue.GetDefaultValue()})" : _.Name));
				writer.WriteLine($"{newAdornments}(self.Add{addReturnValue}({result.MemberIdentifier}, new List<Arg> {{ {parameters} }}));");
			}

			writer.Indent--;
		}
	}
}