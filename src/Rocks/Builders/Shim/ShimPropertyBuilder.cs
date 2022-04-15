using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Shim;

internal static class ShimPropertyBuilder
{
	internal static void Build(IndentedTextWriter writer, ITypeSymbol shimType, Compilation compilation)
	{
		foreach (var property in shimType.GetMembers().OfType<IPropertySymbol>()
			.Where(_ => !_.IsIndexer && !_.IsVirtual))
		{
			writer.WriteLine();

			var attributes = property.GetAttributes();

			if (attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription(compilation));
			}

			var isUnsafe = property.IsUnsafe() ? "unsafe " : string.Empty;

			var returnByRef = property.ReturnsByRef ? "ref " : property.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
			writer.WriteLine($"public {isUnsafe}{returnByRef}{property.Type.GetName()} {property.Name}");
			writer.WriteLine("{");
			writer.Indent++;

			var accessors = property.GetAccessors();
			if (accessors == PropertyAccessor.Get || accessors == PropertyAccessor.GetAndInit ||
				accessors == PropertyAccessor.GetAndSet)
			{
				var refReturn = property.ReturnsByRef || property.ReturnsByRefReadonly ? "ref " : string.Empty;
				writer.WriteLine($"get => {refReturn}this.mock.{property.Name};");
			}

			if (accessors == PropertyAccessor.Set || accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine($"set => this.mock.{property.Name} = value;");
			}

			if (accessors == PropertyAccessor.Init || accessors == PropertyAccessor.GetAndInit)
			{
				writer.WriteLine($"init => this.mock.{property.Name} = value;");
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}