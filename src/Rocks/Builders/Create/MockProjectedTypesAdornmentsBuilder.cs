using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static partial class MockProjectedTypesAdornmentsBuilder
{
	internal static string GetProjectedAdornmentName(ITypeSymbol type, AdornmentType adornment, bool isExplicit) =>
		$"{(isExplicit ? WellKnownNames.Explicit : string.Empty)}{adornment}{WellKnownNames.Adornments}For{type.GetName(TypeNameOption.Flatten)}";

	internal static string GetProjectedHandlerInformationName(ITypeSymbol type) =>
		$"{nameof(HandlerInformation)}For{type.GetName(TypeNameOption.Flatten)}";

	internal static string GetProjectedAddExtensionMethodName(ITypeSymbol type) =>
		$"AddFor{type.GetName(TypeNameOption.Flatten)}";

	internal static void Build(IndentedTextWriter writer, MockInformation information, NamespaceGatherer namespaces)
	{
		var adornmentTypes = new HashSet<(ITypeSymbol type, AdornmentType adornment, bool isExplicit)>();

		foreach (var methodResult in information.Methods)
		{
			var method = methodResult.Value;

			if (!method.ReturnsVoid && method.ReturnType.IsPointer())
			{
				adornmentTypes.Add((method.ReturnType, AdornmentType.Method,
					methodResult.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes));
			}
		}

		foreach (var propertyResult in information.Properties)
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

			BuildAddExtensionMethod(writer, information.TypeToMock!, types, namespaces);

			foreach (var (type, adornment, isExplicit) in adornmentTypes)
			{
				BuildAdornmentInformationType(writer, type, adornment, isExplicit);
			}
		}
	}

	private static void BuildAddExtensionMethod(IndentedTextWriter writer, MockedType typeToMock, IEnumerable<ITypeSymbol> types,
		NamespaceGatherer namespaces)
	{
		writer.WriteLine($"internal static class {WellKnownNames.Expectations}{WellKnownNames.Wrapper}{WellKnownNames.Extensions}");
		writer.WriteLine("{");
		writer.Indent++;

		foreach (var type in types)
		{
			var handlerType = MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationName(type);
			var methodName = MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(type);

			writer.WriteLine($"internal static {handlerType} {methodName}(this {WellKnownNames.Expectations}{WellKnownNames.Wrapper}<{typeToMock.GenericName}> self, int memberIdentifier, List<{nameof(Argument)}> arguments)");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"var information = new {handlerType}(arguments.ToImmutableArray());");
			writer.WriteLine($"self.{WellKnownNames.Expectations}.Handlers.AddOrUpdate(memberIdentifier,");
			writer.Indent++;
			writer.WriteLine("() => new List<HandlerInformation>(1) { information }, _ => _.Add(information));");
			writer.Indent--;
			writer.WriteLine("return information;");

			writer.Indent--;
			writer.WriteLine("}");
		}

		writer.Indent--;
		writer.WriteLine("}");

		namespaces.Add(typeof(IDictionaryOfTKeyTValueExtensions));
	}

	private static void BuildHandlerInformationType(IndentedTextWriter writer, ITypeSymbol type)
	{
		var handlerName = MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationName(type);
		var isUnsafe = type.IsPointer() ? "unsafe " : string.Empty;

		writer.WriteLine($"internal {isUnsafe}sealed class {handlerName}");
		writer.Indent++;
		writer.WriteLine(": HandlerInformation");
		writer.Indent--;
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"internal {handlerName}(ImmutableArray<{nameof(Argument)}> expectations)");
		writer.Indent++;
		writer.WriteLine(": base(null, expectations) => this.ReturnValue = default;");
		writer.Indent--;

		writer.WriteLine();

		writer.WriteLine($"internal {handlerName}({nameof(Delegate)}? method, ImmutableArray<{nameof(Argument)}> expectations)");
		writer.Indent++;
		writer.WriteLine(": base(method, expectations) => this.ReturnValue = default;");
		writer.Indent--;

		writer.WriteLine();
		writer.WriteLine($"internal {type.GetName()} ReturnValue {{ get; set; }}");

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildAdornmentInformationType(IndentedTextWriter writer, ITypeSymbol type, AdornmentType adornment, bool isExplicit)
	{
		var adornmentName = MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(type, adornment, isExplicit);
		var handlerName = MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationName(type);

		writer.WriteLine($"internal sealed class {adornmentName}<T, TCallback>");
		writer.Indent++;
		writer.WriteLine($": IAdornments<{handlerName}>");
		writer.WriteLine("where T : class");
		writer.WriteLine("where TCallback : Delegate");
		writer.Indent--;

		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"internal {adornmentName}({handlerName} handler) =>");
		writer.Indent++;
		writer.WriteLine("this.Handler = handler;");
		writer.Indent--;

		writer.WriteLine();
		writer.WriteLine($"internal {adornmentName}<T, TCallback> CallCount(uint expectedCallCount)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine($"this.Handler.{nameof(HandlerInformation.SetExpectedCallCount)}(expectedCallCount);");
		writer.WriteLine("return this;");
		writer.Indent--;
		writer.WriteLine("}");

		writer.WriteLine();

		writer.WriteLine($"internal {adornmentName}<T, TCallback> Callback(TCallback callback)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine($"this.Handler.{nameof(HandlerInformation.SetCallback)}(callback);");
		writer.WriteLine("return this;");
		writer.Indent--;
		writer.WriteLine("}");

		writer.WriteLine();

		var isUnsafe = type.IsPointer() ? "unsafe " : string.Empty;
		writer.WriteLine($"internal {isUnsafe}{adornmentName}<T, TCallback> Returns({type.GetName()} returnValue)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("this.Handler.ReturnValue = returnValue;");
		writer.WriteLine("return this;");
		writer.Indent--;
		writer.WriteLine("}");

		writer.WriteLine();

		writer.WriteLine($"public {handlerName} Handler {{ get; }}");

		writer.Indent--;
		writer.WriteLine("}");
	}
}