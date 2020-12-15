using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Rocks.Builders.Create
{
	internal static partial class MockProjectedTypesAdornmentsBuilder
	{
		internal static string GetProjectedAdornmentName(ITypeSymbol type, AdornmentType adornment) => 
			$"{adornment}AdornmentsFor{type.GetName(TypeNameOption.Flatten)}";

		internal static string GetProjectedHandlerInformationName(ITypeSymbol type) =>
			$"HandlerInformationFor{type.GetName(TypeNameOption.Flatten)}";

		internal static string GetProjectedAddExtensionMethodName(ITypeSymbol type) => 
			$"AddFor{type.GetName(TypeNameOption.Flatten)}";

		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			var adornmentTypes = new HashSet<(ITypeSymbol type, AdornmentType adornment)>();

			foreach(var methodResult in information.Methods)
			{
				var method = methodResult.Value;

				if(!method.ReturnsVoid && method.ReturnType.IsEsoteric())
				{
					adornmentTypes.Add((method.ReturnType, AdornmentType.Method));
				}
			}

			foreach(var propertyResult in information.Properties)
			{
				var property = propertyResult.Value;

				if((propertyResult.Accessors == PropertyAccessor.Get || propertyResult.Accessors == PropertyAccessor.GetAndSet) &&
					property.Type.IsEsoteric())
				{
					adornmentTypes.Add((property.Type, property.IsIndexer ? AdornmentType.Indexer : AdornmentType.Property));
				}
			}

			foreach(var handlerType in adornmentTypes.Select(_ => _.type).Distinct())
			{
				BuildHandlerInformationType(writer, handlerType);
			}

			foreach (var (type, adornment) in adornmentTypes)
			{
				BuildAdornmentInformationType(writer, type, adornment);
				BuildAddExtensionMethod(writer, type, adornment);
			}
		}

		private static void BuildAddExtensionMethod(IndentedTextWriter writer, ITypeSymbol type, AdornmentType adornment)
		{
			var handlerType = MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationName(type);
			var methodName = MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(type);
			var expectationsName = $"{adornment}Expectations<T>";

			writer.WriteLine($"internal static {handlerType} {methodName}(this {expectationsName} self, int memberIdentifier, List<Arg> arguments)");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"var information = new {handlerType}(arguments.ToImmutableArray());");
			writer.WriteLine("this.Expectations.Handlers.AddOrUpdate(memberIdentifier,");
			writer.Indent++;
			writer.WriteLine("() => new List<HandlerInformation> { information }, _ => _.Add(information));");
			writer.Indent--;
			writer.WriteLine("return information;");

			writer.Indent--;
			writer.WriteLine("}");
		}

		private static void BuildHandlerInformationType(IndentedTextWriter writer, ITypeSymbol type)
		{
			var handlerName = MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationName(type);

			writer.WriteLine("[Serializable]");
			writer.WriteLine($"public sealed class {handlerName}");
			writer.Indent++;
			writer.WriteLine(": HandlerInformation");
			writer.Indent--;
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"internal {handlerName}(ImmutableArray<Arg> expectations)");
			writer.Indent++;
			writer.WriteLine(": base(null, expectations) => this.ReturnValue = default;");
			writer.Indent--;

			writer.WriteLine();

			writer.WriteLine($"internal {handlerName}(Delegate? method, ImmutableArray<Arg> expectations)");
			writer.Indent++;
			writer.WriteLine(": base(method, expectations) => this.ReturnValue = default;");
			writer.Indent--;

			writer.WriteLine();
			var isUnsafe = type.IsPointer() ? "unsafe " : string.Empty;
			writer.WriteLine($"public {isUnsafe}{type.GetName()} ReturnValue {{ get; internal set; }}");

			writer.Indent--;
			writer.WriteLine("}");
		}

		private static void BuildAdornmentInformationType(IndentedTextWriter writer, ITypeSymbol type, AdornmentType adornment)
		{
			var adornmentName = MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(type, adornment);
			var handlerName = MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationName(type);

			writer.WriteLine($"public sealed class {adornmentName}<T, TCallback>");
			writer.Indent++;
			writer.WriteLine($": IAdornments<{handlerName}>");
			writer.WriteLine("where T : class");
			writer.WriteLine("where TCallback : Delegate");
			writer.Indent--;

			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"public {adornmentName}({handlerName} handler) =>");
			writer.Indent++;
			writer.WriteLine("this.Handler = handler;");
			writer.Indent--;

			writer.WriteLine();
			writer.WriteLine($"public {adornmentName}<T, TCallback> CallCount(uint expectedCallCount)");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine("this.Handler.SetExpectedCallCount(expectedCallCount);");
			writer.WriteLine("return this;");
			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine();

			writer.WriteLine($"public {adornmentName}<T, TCallback> Callback(TCallback callback)");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine("this.Handler.SetCallback(callback);");
			writer.WriteLine("return this;");
			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine();

			writer.WriteLine($"public {handlerName} Handler {{ get; }}");

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}