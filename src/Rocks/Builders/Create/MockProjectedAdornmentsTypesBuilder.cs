using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static partial class MockProjectedAdornmentsTypesBuilder
{
	internal static string GetProjectedAdornmentsName(TypeReferenceModel type) =>
		$"AdornmentsFor{type.FlattenedName}";

	internal static string GetProjectedAdornmentsFullyQualifiedNameName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var argForType = MockProjectedAdornmentsTypesBuilder.GetProjectedAdornmentsName(type);
		var typeArguments = typeToMock.TypeArguments.Length > 0 ?
			$"<{string.Join(", ", typeToMock.TypeArguments)}>" : string.Empty;
		return $"global::{(typeToMock.Namespace is null ? "" : $"{typeToMock.Namespace}.")}{typeToMock.FlattenedName}CreateExpectations{typeArguments}.Projections.{argForType}";
	}

	internal static string GetProjectedHandlerName(TypeReferenceModel type) =>
		$"HandlerFor{type.FlattenedName}";

	internal static string GetProjectedHandlerFullyQualifiedNameName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var argForType = MockProjectedAdornmentsTypesBuilder.GetProjectedHandlerName(type);
		var typeArguments = typeToMock.TypeArguments.Length > 0 ?
			$"<{string.Join(", ", typeToMock.TypeArguments)}>" : string.Empty;
		return $"global::{(typeToMock.Namespace is null ? "" : $"{typeToMock.Namespace}.")}{typeToMock.FlattenedName}CreateExpectations{typeArguments}.Projections.{argForType}";
	}

	internal static void Build(IndentedTextWriter writer, TypeMockModel type)
	{
		var adornmentTypes = new HashSet<TypeReferenceModel>();

		foreach (var method in type.Methods)
		{
			if (!method.ReturnsVoid && (method.ReturnType.TypeKind == TypeKind.FunctionPointer ||
				method.ReturnType.TypeKind == TypeKind.Pointer))
			{
				adornmentTypes.Add(method.ReturnType);
			}
		}

		foreach (var property in type.Properties)
		{
			if ((property.Accessors == PropertyAccessor.Get || property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) &&
				(property.Type.TypeKind == TypeKind.FunctionPointer || property.Type.TypeKind == TypeKind.Pointer))
			{
				adornmentTypes.Add(property.Type);
			}
		}

		if (adornmentTypes.Count > 0)
		{
			foreach (var adornmentType in adornmentTypes)
			{
				MockProjectedAdornmentsTypesBuilder.BuildTypes(writer, adornmentType);
			}
		}
	}

	private static void BuildTypes(IndentedTextWriter writer, TypeReferenceModel type)
	{
		var adornmentName = MockProjectedAdornmentsTypesBuilder.GetProjectedAdornmentsName(type);
		var handlerName = MockProjectedAdornmentsTypesBuilder.GetProjectedHandlerName(type);
		var returnType = type.FullyQualifiedName;

		// The reason why we don't derive from types that include the return value type
		// is that we're dealing with pointers or function pointers, and we can't declare that
		// with the type parameter.
		writer.WriteLines(
			$$"""
			internal unsafe class {{handlerName}}<TCallback>
				: global::Rocks.Handler<TCallback>
				where TCallback : global::System.Delegate
			{
				public {{returnType}} ReturnValue { get; set; }
			}

			internal unsafe class {{adornmentName}}<TAdornments, TCallback>
				: global::Rocks.Adornments<TAdornments, {{handlerName}}<TCallback>, TCallback>
				where TAdornments : {{adornmentName}}<TAdornments, TCallback>
				where TCallback : global::System.Delegate
			{
				internal {{adornmentName}}({{handlerName}}<TCallback> handler)
					: base(handler) { }
				
				internal {{adornmentName}}<TAdornments, TCallback> ReturnValue({{returnType}} returnValue)
				{
					this.handler.ReturnValue = returnValue;
					return this;
				}
			}
			""");
	}
}