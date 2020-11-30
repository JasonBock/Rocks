using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Rocks.Builders
{
	internal static class EventExpectationsExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			if (information.Events.Length > 0)
			{
				if (information.Methods.Length > 0)
				{
					writer.WriteLine();
					EventExpectationsExtensionsBuilder.BuildAdornments(writer, information, "Method");
				}

				if (information.Properties.Length > 0)
				{
					if (information.Properties.Any(_ => !_.Value.IsIndexer))
					{
						writer.WriteLine();
						EventExpectationsExtensionsBuilder.BuildAdornments(writer, information, "Property");
					}
					if (information.Properties.Any(_ => _.Value.IsIndexer))
					{
						writer.WriteLine();
						EventExpectationsExtensionsBuilder.BuildAdornments(writer, information, "Indexer");
					}
				}
			}
		}

		private static void BuildAdornments(IndentedTextWriter writer, MockInformation information, string prefix)
		{
			static void BuildRaisesMethod(IndentedTextWriter writer, string extensionPrefix, string typeToMockName, EventMockableResult result,
				string argsType, bool hasReturn)
			{
				const string callbackName = "TCallback";
				const string returnName = "TReturn";

				var adornmentsTypes = new List<string> { typeToMockName, callbackName };
				var raisesTypes = new List<string> { callbackName };

				if (hasReturn)
				{
					adornmentsTypes.Add(returnName);
					raisesTypes.Add(returnName);
				}

				var adornments = string.Join(", ", adornmentsTypes);
				var raises = string.Join(", ", raisesTypes);
				var raisesOn = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? string.Empty :
					$"On{result.Value.ContainingType.GetName(TypeNameOption.FlattenGenerics)}";

				writer.WriteLine($"internal static {extensionPrefix}Adornments<{adornments}> Raises{result.Value.Name}{raisesOn}<{raises}>(this {extensionPrefix}Adornments<{adornments}> self, {argsType} args)");
				writer.Indent++;
				writer.WriteLine($"where {callbackName} : Delegate");
				writer.Indent--;

				writer.WriteLine("{");
				writer.Indent++;

				var fieldName = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? result.Value.Name :
					$"{result.Value.ContainingType.GetName(TypeNameOption.NoGenerics)}_{result.Value.Name}";

				writer.WriteLine($"self.Handler.AddRaiseEvent(new(\"{fieldName}\", args));");
				writer.WriteLine($"return self;");
				writer.Indent--;
				writer.WriteLine("}");
			}

			writer.WriteLine($"internal static class {prefix}AdornmentsOf{information.TypeToMock.GetName(TypeNameOption.FlattenGenerics)}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			var typeToMockName = information.TypeToMock.GetName();

			foreach (var result in information.Events)
			{
				var argsType = nameof(EventArgs);

				if (result.Value.Type is INamedTypeSymbol namedSymbol &&
					namedSymbol?.DelegateInvokeMethod?.Parameters is { Length: 2 })
				{
					argsType = namedSymbol.DelegateInvokeMethod.Parameters[1].Type.GetName();
				}

				if (information.Methods.Any(_ => !_.Value.ReturnsVoid) ||
					information.Properties.Any(_ => _.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet))
				{
					BuildRaisesMethod(writer, prefix, typeToMockName, result, argsType, true);
				}

				if (information.Methods.Any(_ => _.Value.ReturnsVoid) ||
					information.Properties.Any(_ => _.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet))
				{
					BuildRaisesMethod(writer, prefix, typeToMockName, result, argsType, false);
				}
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}