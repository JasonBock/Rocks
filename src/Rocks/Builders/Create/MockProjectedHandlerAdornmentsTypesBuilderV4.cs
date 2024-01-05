using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static partial class MockProjectedHandlerAdornmentsTypesBuilderV4
{
	internal static string GetProjectedAdornmentsName(TypeReferenceModel type) =>
		$"AdornmentsFor{type.FlattenedName}";

	internal static string GetProjectedAdornmentsFullyQualifiedNameName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.FlattenedName}";
		var argForType = MockProjectedHandlerAdornmentsTypesBuilderV4.GetProjectedAdornmentsName(type);
		return $"global::{(typeToMock.Namespace.Length == 0 ? "" : $"{typeToMock.Namespace}.")}{projectionsForNamespace}.{argForType}";
	}

	internal static string GetProjectedHandlerName(TypeReferenceModel type) =>
		$"HandlerFor{type.FlattenedName}";

	internal static string GetProjectedHandlerFullyQualifiedNameName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.FlattenedName}";
		var argForType = MockProjectedHandlerAdornmentsTypesBuilderV4.GetProjectedHandlerName(type);
		return $"global::{(typeToMock.Namespace.Length == 0 ? "" : $"{typeToMock.Namespace}.")}{projectionsForNamespace}.{argForType}";
	}

	internal static void Build(IndentedTextWriter writer, TypeMockModel type)
	{
		var adornmentTypes = new HashSet<(TypeReferenceModel type, string projectedCallbackName)>();

		foreach (var method in type.Methods)
		{
			if (!method.ReturnsVoid &&
				(method.ReturnType.TypeKind == TypeKind.FunctionPointer || method.ReturnType.IsRefLikeType))
			{
				adornmentTypes.Add((method.ReturnType,
					MockProjectedDelegateBuilderV4.GetProjectedCallbackDelegateFullyQualifiedName(method, method.MockType)));
			}
		}

		foreach (var property in type.Properties)
		{
			if ((property.Accessors == PropertyAccessor.Get || property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) &&
				(property.Type.TypeKind == TypeKind.FunctionPointer || property.Type.IsRefLikeType))
			{
				adornmentTypes.Add((property.Type,
					MockProjectedDelegateBuilderV4.GetProjectedCallbackDelegateFullyQualifiedName(property.GetMethod!, property.MockType)));
			}
		}

		var types = adornmentTypes.ToList();

		if (types.Count > 0)
		{
			foreach (var handlerType in types.Distinct())
			{
				MockProjectedHandlerAdornmentsTypesBuilderV4.BuildHandlerType(writer, handlerType.type, handlerType.projectedCallbackName);
			}

			foreach (var adornmentType in adornmentTypes)
			{
				MockProjectedHandlerAdornmentsTypesBuilderV4.BuildAdornmentsType(writer, adornmentType.type, adornmentType.projectedCallbackName);
			}
		}
	}

	private static void BuildHandlerType(IndentedTextWriter writer, TypeReferenceModel type, string projectedCallbackName)
	{
		var handlerName = MockProjectedHandlerAdornmentsTypesBuilderV4.GetProjectedHandlerName(type);
		var isUnsafe = type.IsPointer ? " unsafe" : string.Empty;

		writer.WriteLines(
			$$"""
			internal{{isUnsafe}} class {{handlerName}}
				: global::Rocks.HandlerV4<{{projectedCallbackName}}>
			{
				internal {{type.FullyQualifiedName}} ReturnValue { get; set; }
			}
			""");
	}

	private static void BuildAdornmentsType(IndentedTextWriter writer, TypeReferenceModel type, string projectedCallbackName)
	{
		var adornmentName = MockProjectedHandlerAdornmentsTypesBuilderV4.GetProjectedAdornmentsName(type);
		var handlerName = MockProjectedHandlerAdornmentsTypesBuilderV4.GetProjectedHandlerName(type);
		var isUnsafe = type.IsPointer ? " unsafe" : string.Empty;

		writer.WriteLines(
			$$"""
			internal{{isUnsafe}} sealed class {{adornmentName}}
				: global::Rocks.AdornmentsV4<{{handlerName}}, {{projectedCallbackName}}>
			{
				internal {{adornmentName}}({{handlerName}} handler) 
					: base(handler) 
				{ }

				internal {{adornmentName}} ReturnValue({{type.FullyQualifiedName}} returnValue)
				{
					this.handler.ReturnValue = returnValue;
					return this;
				}
			}
			""");
	}
}