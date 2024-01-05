using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockProjectedArgTypeBuilderV4
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type)
	{
		foreach (var esotericType in GetEsotericTypes(type))
		{
			ProjectedArgTypeBuilderV4.Build(writer, esotericType, type);
		}
	}

	private static HashSet<TypeReferenceModel> GetEsotericTypes(TypeMockModel type)
	{
		static void GetEsotericTypes(ImmutableArray<ParameterModel> parameters, HashSet<TypeReferenceModel> types)
		{
			foreach (var methodParameter in parameters)
			{
				if (methodParameter.Type.IsRefLikeType || methodParameter.Type.TypeKind == TypeKind.FunctionPointer)
				{
					types.Add(methodParameter.Type);
				}
			}
		}

		var types = new HashSet<TypeReferenceModel>();

		foreach (var method in type.Methods)
		{
			GetEsotericTypes(method.Parameters, types);
		}

		foreach (var property in type.Properties)
		{
			if (property.IsIndexer)
			{
				GetEsotericTypes(property.Parameters, types);
			}
			else
			{
				if (property.Type.IsEsoteric)
				{
					types.Add(property.Type);
				}
			}
		}

		return types;
	}
}