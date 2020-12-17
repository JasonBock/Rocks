using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders.Create
{
	internal static class IndexerExpectationsExtensionsIndexerBuilder
	{
		private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier)
		{
			var property = result.Value;
			var propertyReturnValue = property.Type.GetName();
			var mockTypeName = result.MockType.GetName();
			var thisParameter = $"this IndexerGetterExpectations<{mockTypeName}> self";

			var delegateTypeName = property.GetMethod!.RequiresProjectedDelegate() ?
				MockProjectedDelegateBuilder.GetProjectedDelegateName(property.GetMethod!) :
				DelegateBuilder.Build(property.Parameters, property.Type);
			var adornmentsType = property.GetMethod!.RequiresProjectedDelegate() ?
				$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(property.Type, AdornmentType.Indexer, false)}<{mockTypeName}, {delegateTypeName}>" :
				$"IndexerAdornments<{mockTypeName}, {delegateTypeName}, {propertyReturnValue}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			var instanceParameters = string.Join(", ", thisParameter,
				string.Join(", ", property.GetMethod!.Parameters.Select(_ =>
				{
					return $"Arg<{_.Type.GetName()}> {_.Name}";
				})));

			writer.WriteLine($"internal static {returnValue} This({instanceParameters}) =>");
			writer.Indent++;

			var parameters = string.Join(", ", property.GetMethod!.Parameters.Select(
				_ => _.HasExplicitDefaultValue ? $"{_.Name}.Transform({_.ExplicitDefaultValue.GetDefaultValue()})" : _.Name));
			var addMethod = property.Type.IsEsoteric() ?
				MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(property.Type) : $"Add<{propertyReturnValue}>";
			writer.WriteLine($"{newAdornments}(self.{addMethod}({memberIdentifier}, new List<Arg> {{ {parameters} }}));");
			writer.Indent--;
		}

		private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier)
		{
			var property = result.Value;
			var mockTypeName = result.MockType.GetName();
			var thisParameter = $"this IndexerSetterExpectations<{mockTypeName}> self";
			var delegateTypeName = property.SetMethod!.RequiresProjectedDelegate() ?
				MockProjectedDelegateBuilder.GetProjectedDelegateName(property.SetMethod!) :
				DelegateBuilder.Build(property.SetMethod!.Parameters);
			var adornmentsType = property.SetMethod!.RequiresProjectedDelegate() ?
				$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(property.Type, AdornmentType.Indexer, false)}<{mockTypeName}, {delegateTypeName}>" :
				$"IndexerAdornments<{mockTypeName}, {delegateTypeName}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			var instanceParameters = string.Join(", ", thisParameter,
				string.Join(", ", property.SetMethod!.Parameters.Select(_ =>
				{
					return $"Arg<{_.Type.GetName()}> {_.Name}";
				})));

			writer.WriteLine($"internal static {returnValue} This({instanceParameters}) =>");
			writer.Indent++;

			var parameters = string.Join(", ", property.SetMethod!.Parameters.Select(
				_ => _.HasExplicitDefaultValue ? $"{_.Name}.Transform({_.ExplicitDefaultValue.GetDefaultValue()})" : _.Name));
			writer.WriteLine($"{newAdornments}(self.Add({memberIdentifier}, new List<Arg> {{ {parameters} }}));");
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