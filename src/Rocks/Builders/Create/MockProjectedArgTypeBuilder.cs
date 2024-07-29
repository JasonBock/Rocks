using Microsoft.CodeAnalysis;
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
		static void GetEsotericTypes(ImmutableArray<ParameterModel> parameters, HashSet<TypeReferenceModel> types)
		{
			foreach (var parameter in parameters)
			{
				if (parameter.Type.TypeKind == TypeKind.FunctionPointer ||
					parameter.Type.TypeKind == TypeKind.Pointer)
				{
					types.Add(parameter.Type);
				}
			}
		}

		var types = new HashSet<TypeReferenceModel>(new RefLikeTypeEqualityComparer());

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

			if (property.Type.TypeKind == TypeKind.FunctionPointer ||
				property.Type.TypeKind == TypeKind.Pointer)
			{
				types.Add(property.Type);
			}
		}

		return types;
	}

	// TODO: This can probably be removed...
	private sealed class RefLikeTypeEqualityComparer
		: EqualityComparer<TypeReferenceModel>
	{
		private static string GetName(TypeReferenceModel type)
		{
			string genericValues;

			if (!type.IsGenericType)
			{
				genericValues = string.Empty;
			}
			else
			{
				genericValues = $"<{string.Join(", ", type.TypeArguments.Select(_ => _.TypeKind == TypeKind.TypeParameter ? string.Empty : _.FullyQualifiedName))}>";
			}
	
			return $"{type.FullyQualifiedNameNoGenerics}{genericValues}";
		}

		public override bool Equals(TypeReferenceModel x, TypeReferenceModel y) =>
			 RefLikeTypeEqualityComparer.GetName(x) == RefLikeTypeEqualityComparer.GetName(y);

		public override int GetHashCode(TypeReferenceModel obj) =>
			RefLikeTypeEqualityComparer.GetName(obj).GetHashCode();
	}
}