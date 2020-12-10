using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Make
{
	internal static class MockPropertyBuilder
	{
		internal static void Build(IndentedTextWriter writer, PropertyMockableResult result)
		{
			var property = result.Value;
			var attributes = property.GetAttributes();

			if(attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription());
			}

			var explicitTypeName = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				string.Empty : $"{property.ContainingType.GetName(TypeNameOption.NoGenerics)}.";

			var visibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				"public " : string.Empty;
			var isOverriden = result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;

			var returnByRef = property.ReturnsByRef ? "ref " : property.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
			writer.WriteLine($"{visibility}{isOverriden}{returnByRef}{property.Type.GetName()} {explicitTypeName}{property.Name}");
			writer.WriteLine("{");
			writer.Indent++;

			if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet)
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

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}