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
		var adornmentTypes = new HashSet<(TypeReferenceModel type, MethodModel method)>();

		foreach (var method in type.Methods)
		{
			if (!method.ReturnsVoid &&
				(method.ReturnType.TypeKind == TypeKind.FunctionPointer || method.ReturnType.IsRefLikeType))
			{
				adornmentTypes.Add((method.ReturnType, method));
			}
		}

		foreach (var property in type.Properties)
		{
			if ((property.Accessors == PropertyAccessor.Get || property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) &&
				(property.Type.TypeKind == TypeKind.FunctionPointer || property.Type.IsRefLikeType))
			{
				adornmentTypes.Add((property.Type, property.GetMethod!));
			}
		}

		if (adornmentTypes.Count > 0)
		{
			foreach (var handlerType in adornmentTypes)
			{
				MockProjectedHandlerAdornmentsTypesBuilderV4.BuildHandlerType(writer, handlerType.type, handlerType.method);
			}

			foreach (var adornmentType in adornmentTypes)
			{
				MockProjectedHandlerAdornmentsTypesBuilderV4.BuildAdornmentsType(writer, adornmentType.type, adornmentType.method);
			}
		}
	}

	private static void BuildHandlerType(IndentedTextWriter writer, TypeReferenceModel type, MethodModel method)
	{
		var handlerName = MockProjectedHandlerAdornmentsTypesBuilderV4.GetProjectedHandlerName(type);
		var isUnsafe = type.IsPointer ? " unsafe" : string.Empty;
		var projectedCallbackDelegateName = MockProjectedDelegateBuilderV4.GetProjectedCallbackDelegateFullyQualifiedName(
			method, method.MockType);
		var returnType = 
			type.IsRefLikeType ? 
				$"{MockProjectedDelegateBuilderV4.GetProjectedReturnValueDelegateFullyQualifiedName(method, method.MockType)}?" : 
				type.FullyQualifiedName;

		writer.WriteLines(
			$$"""
			internal{{isUnsafe}} class {{handlerName}}
				: global::Rocks.HandlerV4<{{projectedCallbackDelegateName}}>
			{
				internal {{returnType}} ReturnValue { get; set; }
			}
			""");
	}

	private static void BuildAdornmentsType(IndentedTextWriter writer, TypeReferenceModel type, MethodModel method)
	{
		var adornmentName = MockProjectedHandlerAdornmentsTypesBuilderV4.GetProjectedAdornmentsName(type);
		var handlerName = MockProjectedHandlerAdornmentsTypesBuilderV4.GetProjectedHandlerName(type);
		var projectedCallbackDelegateName = MockProjectedDelegateBuilderV4.GetProjectedCallbackDelegateFullyQualifiedName(
			method, method.MockType);
		var returnType =
			type.IsRefLikeType ?
				MockProjectedDelegateBuilderV4.GetProjectedReturnValueDelegateFullyQualifiedName(method, method.MockType) :
				type.FullyQualifiedName;
		var isUnsafe = type.IsPointer ? " unsafe" : string.Empty;

		writer.WriteLines(
			$$"""
			internal{{isUnsafe}} sealed class {{adornmentName}}
				: global::Rocks.AdornmentsV4<{{handlerName}}, {{projectedCallbackDelegateName}}>
			{
				internal {{adornmentName}}({{handlerName}} handler) 
					: base(handler) 
				{ }

				internal {{adornmentName}} ReturnValue({{returnType}} returnValue)
				{
					this.handler.ReturnValue = returnValue;
					return this;
				}
			}
			""");
	}
}