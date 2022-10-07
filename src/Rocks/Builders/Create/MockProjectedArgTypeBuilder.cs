using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockProjectedArgTypeBuilder
{
	internal static void Build(IndentedTextWriter writer, MockInformation information)
	{
		foreach (var type in GetEsotericTypes(information))
		{
			if (type.IsPointer())
			{
				PointerArgTypeBuilder.Build(writer, type, information.TypeToMock!.Type);
			}
			else
			{
				RefLikeArgTypeBuilder.Build(writer, type, information.TypeToMock!.Type);
			}
		}
	}

	private static HashSet<ITypeSymbol> GetEsotericTypes(MockInformation information)
	{
		static void GetEsotericTypes(ImmutableArray<IParameterSymbol> parameters, ITypeSymbol? returnType, HashSet<ITypeSymbol> types)
		{
			foreach (var methodParameter in parameters)
			{
				if (methodParameter.Type.IsEsoteric())
				{
					types.Add(methodParameter.Type);
				}
			}

			if (returnType is not null && returnType.IsEsoteric())
			{
				types.Add(returnType);
			}
		}

		var types = new HashSet<ITypeSymbol>();

		foreach (var methodResult in information.Methods.Results)
		{
			var method = methodResult.Value;
			GetEsotericTypes(method.Parameters, method.ReturnsVoid ? null : method.ReturnType, types);
		}

		foreach (var propertyResult in information.Properties.Results)
		{
			var property = propertyResult.Value;

			if (property.IsIndexer)
			{
				GetEsotericTypes(property.Parameters, property.Type, types);
			}
			else
			{
				if (property.Type.IsEsoteric())
				{
					types.Add(property.Type);
				}
			}
		}

		return types;
	}
}