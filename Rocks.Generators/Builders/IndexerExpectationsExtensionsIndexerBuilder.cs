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
			var propertyReturnValue = property.GetMethod!.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var thisParameter = $"this IndexerExpectations<{result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}> self";
			var mockTypeName = result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var adornmentsType = $"IndexerAdornments<{mockTypeName}, {propertyReturnValue}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			var instanceParameters = string.Join(", ", thisParameter,
				string.Join(", ", property.GetMethod!.Parameters.Select(_ =>
				{
					return $"Arg<{_.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}> {_.Name}";
				})));

			writer.WriteLine($"internal static {returnValue} GetThis({instanceParameters}) =>");
			writer.Indent++;

			writer.WriteLine($"{newAdornments}(self.Add<{propertyReturnValue}>({memberIdentifier}, new List<Arg> {{ {string.Join(", ", property.GetMethod!.Parameters.Select(_ => _.Name))} }}));");
			writer.Indent--;
		}

		private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier)
		{
			var property = result.Value;
			var propertyParameterValue = property.SetMethod!.Parameters[0].Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var thisParameter = $"this IndexerExpectations<{result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}> self";
			var mockTypeName = result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var adornmentsType = $"IndexerAdornments<{mockTypeName}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			var instanceParameters = string.Join(", ", thisParameter,
				string.Join(", ", property.SetMethod!.Parameters.Select(_ =>
				{
					return $"Arg<{_.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}> {_.Name}";
				})));

			writer.WriteLine($"internal static {returnValue} SetThis({instanceParameters}) =>");
			writer.Indent++;

			writer.WriteLine($"{newAdornments}(self.Add({memberIdentifier}, new List<Arg> {{ {string.Join(", ", property.SetMethod!.Parameters.Select(_ => _.Name))} }}));");
			writer.Indent--;
		}

		internal static void Build(IndentedTextWriter writer, PropertyMockableResult result)
		{
			var memberIdentifier = result.MemberIdentifier;

			if(result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet)
			{
				IndexerExpectationsExtensionsIndexerBuilder.BuildGetter(writer, result, memberIdentifier);
				memberIdentifier++;
			}

			if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet)
			{
				IndexerExpectationsExtensionsIndexerBuilder.BuildSetter(writer, result, memberIdentifier);
			}
		}
	}
}