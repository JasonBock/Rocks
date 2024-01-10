﻿using Microsoft.CodeAnalysis;
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
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.FlattenedName}";
		var argForType = MockProjectedAdornmentsTypesBuilder.GetProjectedAdornmentsName(type);
		return $"global::{(typeToMock.Namespace.Length == 0 ? "" : $"{typeToMock.Namespace}.")}{projectionsForNamespace}.{argForType}";
	}

	internal static string GetProjectedHandlerName(TypeReferenceModel type) =>
		$"HandlerFor{type.FlattenedName}";

	internal static string GetProjectedHandlerFullyQualifiedNameName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.FlattenedName}";
		var argForType = MockProjectedAdornmentsTypesBuilder.GetProjectedHandlerName(type);
		return $"global::{(typeToMock.Namespace.Length == 0 ? "" : $"{typeToMock.Namespace}.")}{projectionsForNamespace}.{argForType}";
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

		writer.WriteLines(
			$$"""
			internal unsafe class {{handlerName}}<TCallback>
				: global::Rocks.Handler<TCallback>
				where TCallback : global::System.Delegate
			{
				public {{returnType}} ReturnValue { get; set; }
			}

			internal unsafe sealed class {{adornmentName}}<TCallback>
				: global::Rocks.Adornments<{{handlerName}}<TCallback>, TCallback>
				where TCallback : global::System.Delegate
			{
				internal {{adornmentName}}({{handlerName}}<TCallback> handler) 
					: base(handler) 
				{ }

				internal {{adornmentName}}<TCallback> ReturnValue({{returnType}} returnValue)
				{
					this.handler.ReturnValue = returnValue;
					return this;
				}
			}
			""");
	}
}