using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static partial class MockProjectedTypesAdornmentsBuilderV3
{
	internal static string GetProjectedAdornmentName(TypeReferenceModel type, AdornmentType adornment, bool isExplicit) =>
		$"{(isExplicit ? "Explicit" : string.Empty)}{adornment}AdornmentsFor{type.FlattenedName}";

	internal static string GetProjectedAdornmentFullyQualifiedNameName(TypeReferenceModel type, TypeReferenceModel typeToMock, AdornmentType adornment, bool isExplicit)
	{
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.FlattenedName}";
		var argForType = MockProjectedTypesAdornmentsBuilderV3.GetProjectedAdornmentName(type, adornment, isExplicit);
		return $"global::{typeToMock.Namespace}{projectionsForNamespace}.{argForType}";
	}

	internal static string GetProjectedHandlerInformationName(TypeReferenceModel type) =>
		$"HandlerInformationFor{type.FlattenedName}";

	internal static string GetProjectedHandlerInformationFullyQualifiedNameName(TypeReferenceModel type, TypeReferenceModel typeToMock)
	{
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.FlattenedName}";
		var argForType = MockProjectedTypesAdornmentsBuilderV3.GetProjectedHandlerInformationName(type);
		return $"global::{typeToMock.Namespace}{projectionsForNamespace}.{argForType}";
	}

	internal static string GetProjectedAddExtensionMethodName(TypeReferenceModel type) =>
		$"AddFor{type.FlattenedName}";

	internal static string GetProjectedAddExtensionMethodFullyQualifiedName(ITypeSymbol type, ITypeSymbol typeToMock)
	{
		var containingNamespace = !typeToMock.ContainingNamespace?.IsGlobalNamespace ?? false ?
			$"{typeToMock.ContainingNamespace!.ToDisplayString()}." : string.Empty;
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.GetName(TypeNameOption.Flatten)}";
		var argForType = MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(type);
		return $"global::{containingNamespace}{projectionsForNamespace}.{argForType}";
	}

	internal static void Build(IndentedTextWriter writer, TypeMockModel type)
	{
		var adornmentTypes = new HashSet<(TypeReferenceModel type, AdornmentType adornment, bool isExplicit)>();

		foreach (var method in type.Methods)
		{
			if (!method.ReturnsVoid && method.ReturnType.IsPointer)
			{
				adornmentTypes.Add((method.ReturnType, AdornmentType.Method,
					method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes));
			}
		}

		foreach (var property in type.Properties)
		{
			if ((property.Accessors == PropertyAccessor.Get || property.Accessors == PropertyAccessor.GetAndSet) &&
				property.Type.IsPointer)
			{
				adornmentTypes.Add((property.Type, property.IsIndexer ? AdornmentType.Indexer : AdornmentType.Property,
					property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes));
			}
		}

		var types = adornmentTypes.Select(_ => _.type).ToList();

		if (types.Count > 0)
		{
			foreach (var handlerType in types.Distinct())
			{
				MockProjectedTypesAdornmentsBuilderV3.BuildHandlerInformationType(writer, handlerType);
			}

			MockProjectedTypesAdornmentsBuilderV3.BuildAddExtensionMethod(writer, type.Type, types);

			foreach (var (adornmentType, adornment, isExplicit) in adornmentTypes)
			{
				MockProjectedTypesAdornmentsBuilderV3.BuildAdornmentInformationType(writer, adornmentType, adornment, isExplicit);
			}
		}
	}

	private static void BuildAddExtensionMethod(IndentedTextWriter writer, TypeReferenceModel mockType, IEnumerable<TypeReferenceModel> types)
	{
		writer.WriteLine($"internal static class ExpectationsExtensions");
		writer.WriteLine("{");
		writer.Indent++;

		foreach (var type in types)
		{
			var handlerType = MockProjectedTypesAdornmentsBuilderV3.GetProjectedHandlerInformationName(type);
			var methodName = MockProjectedTypesAdornmentsBuilderV3.GetProjectedAddExtensionMethodName(type);
			writer.WriteLines(
				$$"""
				internal static {{handlerType}} {{methodName}}(this global::Rocks.Expectations.Expectations<{{mockType.FullyQualifiedName}}> @self, int @memberIdentifier, global::System.Collections.Generic.List<global::Rocks.Argument> @arguments)
				{
					var @information = new {{handlerType}}(@arguments.ToImmutableArray());
					@self.Handlers.AddOrUpdate(@memberIdentifier,
						() => new global::System.Collections.Generic.List<global::Rocks.HandlerInformation>(1) { @information }, _ => _.Add(@information));
					return @information;
				}
				""");
		}

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildHandlerInformationType(IndentedTextWriter writer, TypeReferenceModel type)
	{
		var handlerName = MockProjectedTypesAdornmentsBuilderV3.GetProjectedHandlerInformationName(type);
		var isUnsafe = type.IsPointer ? "unsafe " : string.Empty;
		writer.WriteLines(
			$$"""
			internal {{isUnsafe}}sealed class {{handlerName}}
				: global::Rocks.HandlerInformation
			{
				internal {{handlerName}}(global::System.Collections.Immutable.ImmutableArray<global::Rocks.Argument> @expectations)
					: base(null, @expectations) => this.ReturnValue = default;
				
				internal {{handlerName}}(global::System.Delegate? @method, global::System.Collections.Immutable.ImmutableArray<global::Rocks.Argument> @expectations)
					: base(@method, @expectations) => this.ReturnValue = default;
				
				internal {{type.FullyQualifiedName}} ReturnValue { get; set; }
			}
			""");
	}

	private static void BuildAdornmentInformationType(IndentedTextWriter writer, TypeReferenceModel adornmentType, 
		AdornmentType adornment, bool isExplicit)
	{
		var adornmentName = MockProjectedTypesAdornmentsBuilderV3.GetProjectedAdornmentName(adornmentType, adornment, isExplicit);
		var handlerName = MockProjectedTypesAdornmentsBuilderV3.GetProjectedHandlerInformationName(adornmentType);
		var isUnsafe = adornmentType.IsPointer ? "unsafe " : string.Empty;
		writer.WriteLines(
			$$"""
			internal sealed class {{adornmentName}}<T, TCallback>
				: global::Rocks.IAdornments<{{handlerName}}>
				where T : class
				where TCallback : global::System.Delegate
			{
				internal {{adornmentName}}({{handlerName}} @handler) =>
					this.Handler = @handler;
				
				internal {{adornmentName}}<T, TCallback> CallCount(uint @expectedCallCount)
				{
					this.Handler.SetExpectedCallCount(@expectedCallCount);
					return this;
				}
				
				internal {{adornmentName}}<T, TCallback> Callback(TCallback @callback)
				{
					this.Handler.SetCallback(@callback);
					return this;
				}
				
				internal {{isUnsafe}}{{adornmentName}}<T, TCallback> Returns({{adornmentType.FullyQualifiedName}} @returnValue)
				{
					this.Handler.ReturnValue = @returnValue;
					return this;
				}
				
				public {{handlerName}} Handler { get; }
			}
			""");
	}
}