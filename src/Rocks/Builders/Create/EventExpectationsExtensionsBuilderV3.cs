using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class EventExpectationsExtensionsBuilderV3
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		if (mockType.Events.Length > 0)
		{
			if (mockType.Methods.Length > 0)
			{
				writer.WriteLine();
				EventExpectationsExtensionsBuilderV3.BuildAdornments(writer, mockType, "Method");
			}

			if (mockType.Properties.Length > 0)
			{
				if (mockType.Properties.Any(_ => !_.IsIndexer))
				{
					writer.WriteLine();
					EventExpectationsExtensionsBuilderV3.BuildAdornments(writer, mockType, "Property");
				}
				if (mockType.Properties.Any(_ => _.IsIndexer))
				{
					writer.WriteLine();
					EventExpectationsExtensionsBuilderV3.BuildAdornments(writer, mockType, "Indexer");
				}
			}
		}
	}

	private static void BuildAdornments(IndentedTextWriter writer, TypeMockModel mockType, string prefix)
	{
		static void BuildRaisesMethod(IndentedTextWriter writer, string extensionPrefix, string typeToMockName, EventModel @event,
			string argsType, bool hasReturn)
		{
			const string callbackName = "TCallback";
			const string returnName = "TReturn";

			var adornmentsTypes = hasReturn ? new string[] { typeToMockName, callbackName, returnName } :
				new string[] { typeToMockName, callbackName };
			var raisesTypes = hasReturn ? new string[] { callbackName, returnName } :
				new string[] { callbackName };

			var adornments = string.Join(", ", adornmentsTypes);
			var raises = string.Join(", ", raisesTypes);
			var raisesOn = @event.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? string.Empty :
				$"On{@event.ContainingType.FlattenedName}";

			writer.WriteLine($"internal static global::Rocks.{extensionPrefix}Adornments<{adornments}> Raises{@event.Name}{raisesOn}<{raises}>(this global::Rocks.{extensionPrefix}Adornments<{adornments}> @self, {argsType} @args)");
			writer.Indent++;
			writer.WriteLine($"where {callbackName} : global::System.Delegate");
			writer.Indent--;

			writer.WriteLine("{");
			writer.Indent++;

			var fieldName = @event.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? @event.Name :
				$"{@event.ContainingType.NoGenericsName}_{@event.Name}";

			writer.WriteLine($"@self.Handler.AddRaiseEvent(new(\"{fieldName}\", @args));");
			writer.WriteLine($"return @self;");
			writer.Indent--;
			writer.WriteLine("}");
		}

		writer.WriteLine($"internal static class {prefix}AdornmentsOf{mockType.Type.FlattenedName}Extensions");
		writer.WriteLine("{");
		writer.Indent++;

		var typeToMockName = mockType.Type.FullyQualifiedName;

		foreach (var @event in mockType.Events)
		{
			if (mockType.Methods.Any(_ => !_.ReturnsVoid) ||
				mockType.Properties.Any(_ => _.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet))
			{
				BuildRaisesMethod(writer, prefix, typeToMockName, @event, @event.ArgsType, true);
			}

			if (mockType.Methods.Any(_ => _.ReturnsVoid) ||
				mockType.Properties.Any(_ => _.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet))
			{
				BuildRaisesMethod(writer, prefix, typeToMockName, @event, @event.ArgsType, false);
			}
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}