using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockProjectedArgTypeBuilderV3
{
	internal static void Build(IndentedTextWriter writer, TypeModel type)
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

	private static HashSet<ITypeSymbol> GetEsotericTypes(TypeModel type)
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

		foreach (var method in type.Methods)
		{
			var method = method.Value;
			GetEsotericTypes(method.Parameters, method.ReturnsVoid ? null : method.ReturnType, types);
		}

		foreach (var propertyResult in type.Properties.Results)
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