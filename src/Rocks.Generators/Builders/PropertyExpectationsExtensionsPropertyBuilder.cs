using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders
{
	internal static class PropertyExpectationsExtensionsPropertyBuilder
	{
		private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier)
		{
			var property = result.Value;
			var propertyReturnValue = property.GetMethod!.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var thisParameter = $"this PropertyExpectations<{result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}> self";
			var mockTypeName = result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var adornmentsType = $"PropertyAdornments<{mockTypeName}, {DelegateBuilder.GetDelegate(ImmutableArray<IParameterSymbol>.Empty, property.Type)}, {propertyReturnValue}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			writer.WriteLine($"internal static {returnValue} Get{property.Name}({thisParameter}) =>");
			writer.Indent++;

			writer.WriteLine($"{newAdornments}(self.Add<{propertyReturnValue}>({memberIdentifier}, new List<Arg>()));");
			writer.Indent--;
		}

		private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier)
		{
			var property = result.Value;
			var propertyParameterValue = property.SetMethod!.Parameters[0].Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var thisParameter = $"this PropertyExpectations<{result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}> self";
			var mockTypeName = result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var adornmentsType = $"PropertyAdornments<{mockTypeName}, {DelegateBuilder.GetDelegate(property.SetMethod!.Parameters)}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			writer.WriteLine($"internal static {returnValue} Set{property.Name}({thisParameter}, Arg<{propertyParameterValue}> value) =>");
			writer.Indent++;

			writer.WriteLine($"{newAdornments}(self.Add({memberIdentifier}, new List<Arg> {{ value }}));");
			writer.Indent--;
		}

		internal static void Build(IndentedTextWriter writer, PropertyMockableResult result)
		{
			var memberIdentifier = result.MemberIdentifier;

			if(result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet)
			{
				PropertyExpectationsExtensionsPropertyBuilder.BuildGetter(writer, result, memberIdentifier);
				memberIdentifier++;
			}

			if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet)
			{
				PropertyExpectationsExtensionsPropertyBuilder.BuildSetter(writer, result, memberIdentifier);
			}
		}
	}
}