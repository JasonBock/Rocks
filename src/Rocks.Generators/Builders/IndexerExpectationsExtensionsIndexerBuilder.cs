using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders
{
	internal static class IndexerExpectationsExtensionsIndexerBuilder
	{
		private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier)
		{
			var property = result.Value;
			var propertyReturnValue = property.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var mockTypeName = result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var thisParameter = $"this IndexerGetterExpectations<{mockTypeName}> self";
			var adornmentsType = $"IndexerAdornments<{mockTypeName}, {DelegateBuilder.GetDelegate(property.Parameters, property.Type)}, {propertyReturnValue}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			var instanceParameters = string.Join(", ", thisParameter,
				string.Join(", ", property.GetMethod!.Parameters.Select(_ =>
				{
					return $"Arg<{_.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}> {_.Name}";
				})));

			writer.WriteLine($"internal static {returnValue} This({instanceParameters}) =>");
			writer.Indent++;

			writer.WriteLine($"{newAdornments}(self.Add<{propertyReturnValue}>({memberIdentifier}, new List<Arg> {{ {string.Join(", ", property.GetMethod!.Parameters.Select(_ => _.Name))} }}));");
			writer.Indent--;
		}

		private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier)
		{
			var property = result.Value;
			var mockTypeName = result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var thisParameter = $"this IndexerSetterExpectations<{mockTypeName}> self";
			var adornmentsType = $"IndexerAdornments<{mockTypeName}, {DelegateBuilder.GetDelegate(property.SetMethod!.Parameters)}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			var instanceParameters = string.Join(", ", thisParameter,
				string.Join(", ", property.SetMethod!.Parameters.Select(_ =>
				{
					return $"Arg<{_.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}> {_.Name}";
				})));

			writer.WriteLine($"internal static {returnValue} This({instanceParameters}) =>");
			writer.Indent++;

			writer.WriteLine($"{newAdornments}(self.Add({memberIdentifier}, new List<Arg> {{ {string.Join(", ", property.SetMethod!.Parameters.Select(_ => _.Name))} }}));");
			writer.Indent--;
		}

		internal static void Build(IndentedTextWriter writer, PropertyMockableResult result, PropertyAccessor accessor)
		{
			var memberIdentifier = result.MemberIdentifier;

			if(accessor == PropertyAccessor.Get)
			{
				IndexerExpectationsExtensionsIndexerBuilder.BuildGetter(writer, result, memberIdentifier);
			}
			else
			{
				if (result.Accessors == PropertyAccessor.GetAndSet)
				{
					memberIdentifier++;
				}

				IndexerExpectationsExtensionsIndexerBuilder.BuildSetter(writer, result, memberIdentifier);
			}
		}
	}
}