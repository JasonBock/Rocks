using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockProjectedArgTypeBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type)
	{
		foreach (var esotericType in GetEsotericTypes(type))
		{
			if (esotericType.IsPointer)
			{
				PointerArgTypeBuilder.Build(writer, esotericType, type);
			}
			else
			{
				RefLikeArgTypeBuilder.Build(writer, esotericType, type);
			}
		}
	}

	private static HashSet<TypeReferenceModel> GetEsotericTypes(TypeMockModel type)
	{
		static void GetEsotericTypes(ImmutableArray<ParameterModel> parameters, TypeReferenceModel? returnType, HashSet<TypeReferenceModel> types)
		{
			foreach (var methodParameter in parameters)
			{
				if (methodParameter.Type.IsEsoteric)
				{
					types.Add(methodParameter.Type);
				}
			}

			if (returnType is not null && returnType.IsEsoteric)
			{
				types.Add(returnType);
			}
		}

		var types = new HashSet<TypeReferenceModel>();

		foreach (var method in type.Methods)
		{
			GetEsotericTypes(method.Parameters, method.ReturnsVoid ? null : method.ReturnType, types);
		}

		foreach (var property in type.Properties)
		{
			if (property.IsIndexer)
			{
				GetEsotericTypes(property.Parameters, property.Type, types);
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