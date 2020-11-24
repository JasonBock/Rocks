using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders
{
	internal static class ExplicitPropertyExpectationsExtensionsPropertyBuilder
	{
		private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, string containingTypeName)
		{
			var property = result.Value;
			var propertyReturnValue = property.GetMethod!.ReturnType.GetName();
			var thisParameter = $"this ExplicitPropertyGetterExpectations<{result.MockType.GetName()}, {containingTypeName}> self";
			var mockTypeName = result.MockType.GetName();
			var adornmentsType = $"PropertyAdornments<{mockTypeName}, {DelegateBuilder.GetDelegate(ImmutableArray<IParameterSymbol>.Empty, property.Type)}, {propertyReturnValue}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			writer.WriteLine($"internal static {returnValue} {property.Name}({thisParameter}) =>");
			writer.Indent++;

			writer.WriteLine($"{newAdornments}(self.Add<{propertyReturnValue}>({memberIdentifier}, new List<Arg>()));");
			writer.Indent--;
		}

		private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, string containingTypeName)
		{
			var property = result.Value;
			var propertyParameterValue = property.SetMethod!.Parameters[0].Type.GetName();
			var thisParameter = $"this ExplicitPropertySetterExpectations<{result.MockType.GetName()}, {containingTypeName}> self";
			var mockTypeName = result.MockType.GetName();
			var adornmentsType = $"PropertyAdornments<{mockTypeName}, {DelegateBuilder.GetDelegate(property.SetMethod!.Parameters)}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			writer.WriteLine($"internal static {returnValue} {property.Name}({thisParameter}, Arg<{propertyParameterValue}> value) =>");
			writer.Indent++;

			writer.WriteLine($"{newAdornments}(self.Add({memberIdentifier}, new List<Arg> {{ value }}));");
			writer.Indent--;
		}

		internal static void Build(IndentedTextWriter writer, PropertyMockableResult result, PropertyAccessor accessor, string containingTypeName)
		{
			var memberIdentifier = result.MemberIdentifier;

			if(accessor == PropertyAccessor.Get)
			{
				ExplicitPropertyExpectationsExtensionsPropertyBuilder.BuildGetter(writer, result, memberIdentifier, containingTypeName);
			}
			else
			{
				if(result.Accessors == PropertyAccessor.GetAndSet)
				{
					memberIdentifier++;
				}

				ExplicitPropertyExpectationsExtensionsPropertyBuilder.BuildSetter(writer, result, memberIdentifier, containingTypeName);
			}
		}
	}
}