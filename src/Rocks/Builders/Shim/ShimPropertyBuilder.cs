using Microsoft.CodeAnalysis;
using Rocks.Builders.Create;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Shim;

internal static class ShimPropertyBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel shimType)
	{
		foreach (var property in shimType.Properties
			.Where(_ => !_.IsIndexer && !_.IsVirtual)
			.Select(_ => _))
		{
			writer.WriteLine();

			if (property.AttributesDescription.Length > 0)
			{
				writer.WriteLine(property.AttributesDescription);
			}

			var isUnsafe = property.IsUnsafe ? "unsafe " : string.Empty;
			var returnByRef = property.ReturnsByRef ? "ref " : property.ReturnsByRefReadOnly ? "ref readonly " : string.Empty;
			var (accessibility, explicitName) = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? 
				("public ", string.Empty) : (string.Empty, $"{property.ContainingType.FullyQualifiedName}.");

			writer.WriteLine($"{accessibility}{isUnsafe}{returnByRef}{property.Type.FullyQualifiedName} {explicitName}{property.Name}");
			writer.WriteLine("{");
			writer.Indent++;

			var accessors = property.Accessors;
			if (accessors == PropertyAccessor.Get || accessors == PropertyAccessor.GetAndInit ||
				accessors == PropertyAccessor.GetAndSet)
			{
				var refReturn = property.ReturnsByRef || property.ReturnsByRefReadOnly ? "ref " : string.Empty;
				writer.WriteLine($"get => {refReturn}(({shimType.Type.FullyQualifiedName})this.mock).{property.Name}!;");
			}

			if (accessors == PropertyAccessor.Set || accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine($"set => (({shimType.Type.FullyQualifiedName})this.mock).{property.Name} = value!;");
			}

			if (accessors == PropertyAccessor.Init || accessors == PropertyAccessor.GetAndInit)
			{
				writer.WriteLine($"init => (({shimType.Type.FullyQualifiedName})this.mock).{property.Name} = value!;");
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}