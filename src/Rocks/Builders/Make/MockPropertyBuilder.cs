using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Make;

internal static class MockPropertyBuilder
{
	internal static void Build(IndentedTextWriter writer, PropertyMockableResult result,
		Compilation compilation)
	{
		var property = result.Value;
		var attributes = property.GetAttributes();

		if (attributes.Length > 0)
		{
			writer.WriteLine(attributes.GetDescription(compilation));
		}

		var explicitTypeName = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			string.Empty : $"{property.ContainingType.GetReferenceableName()}.";

		var visibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{result.Value.DeclaredAccessibility.GetOverridingCodeValue()} " : string.Empty;
		var isUnsafe = property.IsUnsafe() ? "unsafe " : string.Empty;
		var isOverriden = result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;

		var returnByRef = property.ReturnsByRef ? "ref " : property.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
		writer.WriteLine($"{visibility}{isUnsafe}{isOverriden}{returnByRef}{property.Type.GetReferenceableName()} {explicitTypeName}{property.Name}");
		writer.WriteLine("{");
		writer.Indent++;

		if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet ||
			result.Accessors == PropertyAccessor.GetAndInit)
		{
			if (property.ReturnsByRef || property.ReturnsByRefReadonly)
			{
				writer.WriteLine($"get => ref this.rr{result.MemberIdentifier};");
			}
			else
			{
				writer.WriteLine("get => default!;");
			}
		}

		if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet)
		{
			writer.WriteLine("set { }");
		}
		else if (result.Accessors == PropertyAccessor.Init || result.Accessors == PropertyAccessor.GetAndInit)
		{
			writer.WriteLine("init { }");
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}