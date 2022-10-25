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
		var attributes = property.GetAllAttributes();

		if (attributes.Length > 0)
		{
			writer.WriteLine(attributes.GetDescription(compilation));
		}

		var explicitTypeName = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			string.Empty : $"{property.ContainingType.GetFullyQualifiedName()}.";

		var visibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{result.Value.DeclaredAccessibility.GetOverridingCodeValue()} " : string.Empty;
		var isUnsafe = property.IsUnsafe() ? "unsafe " : string.Empty;
		var isOverriden = result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;

		var returnByRef = property.ReturnsByRef ? "ref " : property.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
		writer.WriteLine($"{visibility}{isUnsafe}{isOverriden}{returnByRef}{property.Type.GetFullyQualifiedName()} {explicitTypeName}{property.Name}");
		writer.WriteLine("{");
		writer.Indent++;

		if ((result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet || result.Accessors == PropertyAccessor.GetAndInit) &&
			result.Value.GetMethod!.CanBeSeenByContainingAssembly(compilation.Assembly))
		{
			var getVisibility = result.Value.DeclaredAccessibility != result.Value.GetMethod!.DeclaredAccessibility ?
				$"{result.Value.GetMethod!.DeclaredAccessibility.GetOverridingCodeValue()} " : string.Empty;

			if (property.ReturnsByRef || property.ReturnsByRefReadonly)
			{
				writer.WriteLine($"{getVisibility}get => ref this.rr{result.MemberIdentifier};");
			}
			else
			{
				writer.WriteLine($"{getVisibility}get => default!;");
			}
		}

		if ((result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet) &&
			result.Value.SetMethod!.CanBeSeenByContainingAssembly(compilation.Assembly))
		{
			var setVisibility = result.Value.DeclaredAccessibility != result.Value.SetMethod!.DeclaredAccessibility ?
				$"{result.Value.SetMethod!.DeclaredAccessibility.GetOverridingCodeValue()} " : string.Empty;
			writer.WriteLine($"{setVisibility}set {{ }}");
		}
		else if ((result.Accessors == PropertyAccessor.Init || result.Accessors == PropertyAccessor.GetAndInit) && 
			result.Value.SetMethod!.CanBeSeenByContainingAssembly(compilation.Assembly))
		{
			var setVisibility = result.Value.DeclaredAccessibility != result.Value.SetMethod!.DeclaredAccessibility ?
				$"{result.Value.SetMethod!.DeclaredAccessibility.GetOverridingCodeValue()} " : string.Empty;
			writer.WriteLine($"{setVisibility}init {{ }}");
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}