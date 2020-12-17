using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create
{
	internal static class ExplicitPropertyExpectationsExtensionsPropertyBuilder
	{
		private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, string containingTypeName)
		{
			var property = result.Value;
			var propertyReturnValue = property.GetMethod!.ReturnType.GetName();
			var thisParameter = $"this ExplicitPropertyGetterExpectations<{result.MockType.GetName()}, {containingTypeName}> self";
			var mockTypeName = result.MockType.GetName();

			var delegateTypeName = property.GetMethod!.RequiresProjectedDelegate() ?
				MockProjectedDelegateBuilder.GetProjectedDelegateName(property.GetMethod!) :
				DelegateBuilder.Build(ImmutableArray<IParameterSymbol>.Empty, property.Type);
			var adornmentsType = property.Type.IsEsoteric() ?
				$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(property.Type, AdornmentType.Property, true)}<{mockTypeName}, {delegateTypeName}>" :
				$"PropertyAdornments<{mockTypeName}, {delegateTypeName}, {propertyReturnValue}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			writer.WriteLine($"internal static {returnValue} {property.Name}({thisParameter}) =>");
			writer.Indent++;

			var addMethod = property.Type.IsEsoteric() ?
				MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(property.Type) : $"Add<{propertyReturnValue}>";

			writer.WriteLine($"{newAdornments}(self.{addMethod}({memberIdentifier}, new List<Arg>()));");
			writer.Indent--;
		}

		private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, string containingTypeName)
		{
			var property = result.Value;
			var propertyParameterValue = property.SetMethod!.Parameters[0].Type.GetName();
			var thisParameter = $"this ExplicitPropertySetterExpectations<{result.MockType.GetName()}, {containingTypeName}> self";
			var mockTypeName = result.MockType.GetName();

			var delegateTypeName = property.SetMethod!.RequiresProjectedDelegate() ?
				MockProjectedDelegateBuilder.GetProjectedDelegateName(property.SetMethod!) :
				DelegateBuilder.Build(property.SetMethod!.Parameters);
			var adornmentsType = property.SetMethod!.RequiresProjectedDelegate() ?
				$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(property.Type, AdornmentType.Property, true)}<{mockTypeName}, {delegateTypeName}>" :
				$"PropertyAdornments<{mockTypeName}, {delegateTypeName}>";
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