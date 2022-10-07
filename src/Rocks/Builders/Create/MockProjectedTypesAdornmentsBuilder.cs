using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static partial class MockProjectedTypesAdornmentsBuilder
{
	internal static string GetProjectedAdornmentName(ITypeSymbol type, AdornmentType adornment, bool isExplicit) =>
		$"{(isExplicit ? "Explicit" : string.Empty)}{adornment}AdornmentsFor{type.GetName(TypeNameOption.Flatten)}";

	internal static string GetProjectedAdornmentFullyQualifiedNameName(ITypeSymbol type, ITypeSymbol typeToMock, AdornmentType adornment, bool isExplicit)
	{
		var containingNamespace = !typeToMock.ContainingNamespace?.IsGlobalNamespace ?? false ?
			$"{typeToMock.ContainingNamespace!.ToDisplayString()}." : string.Empty;
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.GetName(TypeNameOption.Flatten)}";
		var argForType = MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(type, adornment, isExplicit);
		return $"global::{containingNamespace}{projectionsForNamespace}.{argForType}";
	}

	internal static string GetProjectedHandlerInformationName(ITypeSymbol type) =>
		$"HandlerInformationFor{type.GetName(TypeNameOption.Flatten)}";

	internal static string GetProjectedHandlerInformationFullyQualifiedNameName(ITypeSymbol type, ITypeSymbol typeToMock)
	{
		var containingNamespace = !typeToMock.ContainingNamespace?.IsGlobalNamespace ?? false ?
			$"{typeToMock.ContainingNamespace!.ToDisplayString()}." : string.Empty;
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.GetName(TypeNameOption.Flatten)}";
		var argForType = MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationName(type);
		return $"global::{containingNamespace}{projectionsForNamespace}.{argForType}";
	}

	internal static string GetProjectedAddExtensionMethodName(ITypeSymbol type) =>
		$"AddFor{type.GetName(TypeNameOption.Flatten)}";

	internal static string GetProjectedAddExtensionMethodFullyQualifiedName(ITypeSymbol type, ITypeSymbol typeToMock)
	{
		var containingNamespace = !typeToMock.ContainingNamespace?.IsGlobalNamespace ?? false ?
			$"{typeToMock.ContainingNamespace!.ToDisplayString()}." : string.Empty;
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.GetName(TypeNameOption.Flatten)}";
		var argForType = MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(type);
		return $"global::{containingNamespace}{projectionsForNamespace}.{argForType}";
	}

	internal static void Build(IndentedTextWriter writer, MockInformation information)
	{
		var adornmentTypes = new HashSet<(ITypeSymbol type, AdornmentType adornment, bool isExplicit)>();

		foreach (var methodResult in information.Methods.Results)
		{
			var method = methodResult.Value;

			if (!method.ReturnsVoid && method.ReturnType.IsPointer())
			{
				adornmentTypes.Add((method.ReturnType, AdornmentType.Method,
					methodResult.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes));
			}
		}

		foreach (var propertyResult in information.Properties.Results)
		{
			var property = propertyResult.Value;

			if ((propertyResult.Accessors == PropertyAccessor.Get || propertyResult.Accessors == PropertyAccessor.GetAndSet) &&
				property.Type.IsPointer())
			{
				adornmentTypes.Add((property.Type, property.IsIndexer ? AdornmentType.Indexer : AdornmentType.Property,
					propertyResult.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes));
			}
		}

		var types = adornmentTypes.Select(_ => _.type).ToList();

		if (types.Count > 0)
		{
			foreach (var handlerType in types.Distinct())
			{
				BuildHandlerInformationType(writer, handlerType);
			}

			BuildAddExtensionMethod(writer, information.TypeToMock!, types);

			foreach (var (type, adornment, isExplicit) in adornmentTypes)
			{
				BuildAdornmentInformationType(writer, type, adornment, isExplicit);
			}
		}
	}

	private static void BuildAddExtensionMethod(IndentedTextWriter writer, MockedType typeToMock, IEnumerable<ITypeSymbol> types)
	{
		writer.WriteLine($"internal static class ExpectationsExtensions");
		writer.WriteLine("{");
		writer.Indent++;

		foreach (var type in types)
		{
			var handlerType = MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationName(type);
			var methodName = MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(type);
			writer.WriteLines(
				$$"""
				internal static {{handlerType}} {{methodName}}(this global::Rocks.Expectations.Expectations<{{typeToMock.ReferenceableName}}> @self, int @memberIdentifier, global::System.Collections.Generic.List<global::Rocks.Argument> @arguments)
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

	private static void BuildHandlerInformationType(IndentedTextWriter writer, ITypeSymbol type)
	{
		var handlerName = MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationName(type);
		var isUnsafe = type.IsPointer() ? "unsafe " : string.Empty;
		writer.WriteLines(
			$$"""
			internal {{isUnsafe}}sealed class {{handlerName}}
				: global::Rocks.HandlerInformation
			{
				internal {{handlerName}}(global::System.Collections.Immutable.ImmutableArray<global::Rocks.Argument> @expectations)
					: base(null, @expectations) => this.ReturnValue = default;
				
				internal {{handlerName}}(global::System.Delegate? @method, global::System.Collections.Immutable.ImmutableArray<global::Rocks.Argument> @expectations)
					: base(@method, @expectations) => this.ReturnValue = default;
				
				internal {{type.GetReferenceableName()}} ReturnValue { get; set; }
			}
			""");
	}

	private static void BuildAdornmentInformationType(IndentedTextWriter writer, ITypeSymbol type, AdornmentType adornment, bool isExplicit)
	{
		var adornmentName = MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(type, adornment, isExplicit);
		var handlerName = MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationName(type);
		var isUnsafe = type.IsPointer() ? "unsafe " : string.Empty;
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
				
				internal {{isUnsafe}}{{adornmentName}}<T, TCallback> Returns({{type.GetReferenceableName()}} @returnValue)
				{
					this.Handler.ReturnValue = @returnValue;
					return this;
				}
				
				public {{handlerName}} Handler { get; }
			}
			""");
	}
}