using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Shim;

internal static class ShimPropertyBuilderV3
{
	internal static void Build(IndentedTextWriter writer, Compilation compilation, MockInformation shimInformation)
	{
		foreach (var property in shimInformation.Properties.Results
			.Where(_ => !_.Value.IsIndexer && !_.Value.IsVirtual)
			.Select(_ => _.Value))
		{
			writer.WriteLine();

			var attributes = property.GetAttributes();

			if (attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription(compilation));
			}

			var isUnsafe = property.IsUnsafe() ? "unsafe " : string.Empty;

			var returnByRef = property.ReturnsByRef ? "ref " : property.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
			writer.WriteLine($"public {isUnsafe}{returnByRef}{property.Type.GetFullyQualifiedName()} {property.Name}");
			writer.WriteLine("{");
			writer.Indent++;

			var accessors = property.GetAccessors();
			if (accessors == PropertyAccessor.Get || accessors == PropertyAccessor.GetAndInit ||
				accessors == PropertyAccessor.GetAndSet)
			{
				var refReturn = property.ReturnsByRef || property.ReturnsByRefReadonly ? "ref " : string.Empty;
				writer.WriteLine($"get => {refReturn}global::System.Runtime.CompilerServices.Unsafe.As<{shimInformation.TypeToMock!.Type.GetFullyQualifiedName()}>(this.mock).{property.Name};");
			}

			if (accessors == PropertyAccessor.Set || accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine($"set => global::System.Runtime.CompilerServices.Unsafe.As<{shimInformation.TypeToMock!.Type.GetFullyQualifiedName()}>(this.mock).{property.Name} = value;");
			}

			if (accessors == PropertyAccessor.Init || accessors == PropertyAccessor.GetAndInit)
			{
				writer.WriteLine($"init => global::System.Runtime.CompilerServices.Unsafe.As<{shimInformation.TypeToMock!.Type.GetFullyQualifiedName()}>(this.mock).{property.Name} = value;");
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}