using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create
{
	internal static class PropertyExpectationsExtensionsPropertyBuilder
	{
		private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier)
		{
			var property = result.Value;
			var propertyReturnValue = property.GetMethod!.ReturnType.GetName();
			var thisParameter = $"this PropertyGetterExpectations<{result.MockType.GetName()}> self";
			var mockTypeName = result.MockType.GetName();
			var adornmentsType = $"PropertyAdornments<{mockTypeName}, {DelegateBuilder.Build(ImmutableArray<IParameterSymbol>.Empty, property.Type)}, {propertyReturnValue}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			writer.WriteLine($"internal static {returnValue} {property.Name}({thisParameter}) =>");
			writer.Indent++;

			writer.WriteLine($"{newAdornments}(self.Add<{propertyReturnValue}>({memberIdentifier}, new List<Arg>()));");
			writer.Indent--;
		}

		private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier)
		{
			var property = result.Value;
			var propertyParameterValue = property.SetMethod!.Parameters[0].Type.GetName();
			var thisParameter = $"this PropertySetterExpectations<{result.MockType.GetName()}> self";
			var mockTypeName = result.MockType.GetName();
			var adornmentsType = $"PropertyAdornments<{mockTypeName}, {DelegateBuilder.Build(property.SetMethod!.Parameters)}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			writer.WriteLine($"internal static {returnValue} {property.Name}({thisParameter}, Arg<{propertyParameterValue}> value) =>");
			writer.Indent++;

			writer.WriteLine($"{newAdornments}(self.Add({memberIdentifier}, new List<Arg> {{ value }}));");
			writer.Indent--;
		}

		internal static void Build(IndentedTextWriter writer, PropertyMockableResult result, PropertyAccessor accessor)
		{
			var memberIdentifier = result.MemberIdentifier;

			if(accessor == PropertyAccessor.Get)
			{
				PropertyExpectationsExtensionsPropertyBuilder.BuildGetter(writer, result, memberIdentifier);
			}
			else
			{
				if(result.Accessors == PropertyAccessor.GetAndSet)
				{
					memberIdentifier++;
				}

				PropertyExpectationsExtensionsPropertyBuilder.BuildSetter(writer, result, memberIdentifier);
			}
		}
	}
}