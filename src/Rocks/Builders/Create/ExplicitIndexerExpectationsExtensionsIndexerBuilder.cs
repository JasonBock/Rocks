using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders.Create
{
	internal static class ExplicitIndexerExpectationsExtensionsIndexerBuilder
	{
		private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, string containingTypeName)
		{
			var property = result.Value;
			var propertyReturnValue = property.Type.GetName();
			var mockTypeName = result.MockType.GetName();
			var thisTypeName = $"{WellKnownNames.Explicit}{WellKnownNames.Indexer}{WellKnownNames.Getter}{WellKnownNames.Expectations}";
			var thisParameter = $"this {thisTypeName}<{mockTypeName}, {containingTypeName}> self";

			var delegateTypeName = property.GetMethod!.RequiresProjectedDelegate() ?
				MockProjectedDelegateBuilder.GetProjectedDelegateName(property.GetMethod!) :
				DelegateBuilder.Build(property.Parameters, property.Type);
			var adornmentsType = property.GetMethod!.RequiresProjectedDelegate() ?
				$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(property.Type, AdornmentType.Indexer, true)}<{mockTypeName}, {delegateTypeName}>" :
				$"{WellKnownNames.Indexer}{WellKnownNames.Adornments}<{mockTypeName}, {delegateTypeName}, {propertyReturnValue}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			var instanceParameters = string.Join(", ", thisParameter,
				string.Join(", ", property.GetMethod!.Parameters.Select(_ =>
				{
					return $"{nameof(Argument)}<{_.Type.GetName()}> {_.Name}";
				})));

			writer.WriteLine($"internal static {returnValue} {WellKnownNames.This}({instanceParameters}) =>");
			writer.Indent++;

			var parameters = string.Join(", ", property.GetMethod!.Parameters.Select(
				_ => _.HasExplicitDefaultValue ? $"{_.Name}.{WellKnownNames.Transform}({_.ExplicitDefaultValue.GetDefaultValue(_.Type.IsValueType)})" : _.Name));
			var addMethod = property.Type.IsEsoteric() ?
				MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(property.Type) : $"Add<{propertyReturnValue}>";
			writer.WriteLine($"{newAdornments}(self.{addMethod}({memberIdentifier}, new List<{nameof(Argument)}>({property.GetMethod!.Parameters.Length}) {{ {parameters} }}));");
			writer.Indent--;
		}

		private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, string containingTypeName)
		{
			var property = result.Value;
			var mockTypeName = result.MockType.GetName();
			var thisTypeName = $"{WellKnownNames.Explicit}{WellKnownNames.Indexer}{WellKnownNames.Setter}{WellKnownNames.Expectations}";
			var thisParameter = $"this {thisTypeName}<{mockTypeName}, {containingTypeName}> self";

			var delegateTypeName = property.SetMethod!.RequiresProjectedDelegate() ?
				MockProjectedDelegateBuilder.GetProjectedDelegateName(property.SetMethod!) :
				DelegateBuilder.Build(property.SetMethod!.Parameters);
			var adornmentsType = property.SetMethod!.RequiresProjectedDelegate() ?
				$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(property.Type, AdornmentType.Indexer, true)}<{mockTypeName}, {delegateTypeName}>" :
				$"{WellKnownNames.Indexer}{WellKnownNames.Adornments}<{mockTypeName}, {delegateTypeName}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			var instanceParameters = string.Join(", ", thisParameter,
				string.Join(", ", property.SetMethod!.Parameters.Select(_ =>
				{
					return $"{nameof(Argument)}<{_.Type.GetName()}> {_.Name}";
				})));

			writer.WriteLine($"internal static {returnValue} {WellKnownNames.This}({instanceParameters}) =>");
			writer.Indent++;

			var parameters = string.Join(", ", property.SetMethod!.Parameters.Select(
				_ => _.HasExplicitDefaultValue ? $"{_.Name}.{WellKnownNames.Transform}({_.ExplicitDefaultValue.GetDefaultValue(_.Type.IsValueType)})" : _.Name));
			writer.WriteLine($"{newAdornments}(self.Add({memberIdentifier}, new List<{nameof(Argument)}>({property.SetMethod!.Parameters.Length}) {{ {parameters} }}));");
			writer.Indent--;
		}

		internal static void Build(IndentedTextWriter writer, PropertyMockableResult result, PropertyAccessor accessor, string containingTypeName)
		{
			var memberIdentifier = result.MemberIdentifier;

			if(accessor == PropertyAccessor.Get)
			{
				ExplicitIndexerExpectationsExtensionsIndexerBuilder.BuildGetter(writer, result, memberIdentifier, containingTypeName);
			}
			else
			{
				if (result.Accessors == PropertyAccessor.GetAndSet)
				{
					memberIdentifier++;
				}

				ExplicitIndexerExpectationsExtensionsIndexerBuilder.BuildSetter(writer, result, memberIdentifier, containingTypeName);
			}
		}
	}
}