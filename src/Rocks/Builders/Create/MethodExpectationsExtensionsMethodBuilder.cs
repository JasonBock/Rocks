using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders.Create
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
				$"this {WellKnownNames.Explicit}{WellKnownNames.Method}{WellKnownNames.Expectations}<{mockTypeName}, {containingTypeName}> self" :
				$"this {WellKnownNames.Method}{WellKnownNames.Expectations}<{mockTypeName}> self";
			var instanceParameters = method.Parameters.Length == 0 ? thisParameter :
				string.Join(", ", thisParameter,
					string.Join(", ", method.Parameters.Select(_ =>
					{
						if(_.Type.IsEsoteric())
						{
							var argName = _.Type.IsPointer() ? PointerArgTypeBuilder.GetProjectedName(_.Type) :
								RefLikeArgTypeBuilder.GetProjectedName(_.Type);
							return $"{argName} {_.Name}";
						}
						else
						{
							return $"{nameof(Argument)}<{_.Type.GetName()}> {_.Name}";
						}
					})));
			var parameterTypes = string.Join(", ", method.Parameters.Select(_ => _.Type.GetName()));

			var delegateTypeName = method.RequiresProjectedDelegate() ?
				MockProjectedDelegateBuilder.GetProjectedDelegateName(method) :
					method.ReturnsVoid ? DelegateBuilder.Build(method.Parameters) : DelegateBuilder.Build(method.Parameters, method.ReturnType);
			var adornmentsType = method.ReturnsVoid ? 
				$"{WellKnownNames.Method}{WellKnownNames.Adornments}<{mockTypeName}, {delegateTypeName}>" :
				method.ReturnType.IsEsoteric() ? 
					$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(method.ReturnType, AdornmentType.Method, result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)}<{mockTypeName}, {delegateTypeName}>" :
					$"{WellKnownNames.Method}{WellKnownNames.Adornments}<{mockTypeName}, {delegateTypeName}, {method.ReturnType.GetName()}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			writer.WriteLine($"internal static {returnValue} {method.GetName()}({instanceParameters}) =>");
			writer.Indent++;

			var addReturnValue = method.ReturnsVoid ? string.Empty : $"<{method.ReturnType.GetName()}>";

			var addMethod = method.ReturnsVoid ? "Add" :
				method.ReturnType.IsEsoteric() ? 
				MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(method.ReturnType) : $"Add<{method.ReturnType.GetName()}>";

			if (method.Parameters.Length == 0)
			{
				writer.WriteLine($"{newAdornments}(self.{addMethod}({result.MemberIdentifier}, new List<{nameof(Argument)}>()));");
			}
			else
			{
				var parameters = string.Join(", ", method.Parameters.Select(_ =>
					{
						if (_.HasExplicitDefaultValue)
						{
							return $"{_.Name}.{WellKnownNames.Transform}({_.ExplicitDefaultValue.GetDefaultValue()})";
						}
						else if (_.RefKind == RefKind.Out)
						{
							return $"Arg.Any<{_.Type.GetName()}>()";
						}
						else 
						{ 
							return _.Name;
						}
					}));
				writer.WriteLine($"{newAdornments}(self.{addMethod}({result.MemberIdentifier}, new List<{nameof(Argument)}> {{ {parameters} }}));");
			}

			writer.Indent--;
		}
	}
}