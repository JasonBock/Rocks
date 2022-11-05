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
			$"{result.Value.GetOverridingCodeValue(compilation.Assembly)} " : string.Empty;
		var isUnsafe = property.IsUnsafe() ? "unsafe " : string.Empty;
		var isOverriden = result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;

		var returnByRef = property.ReturnsByRef ? "ref " : property.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
		writer.WriteLine($"{visibility}{isUnsafe}{isOverriden}{returnByRef}{property.Type.GetFullyQualifiedName()} {explicitTypeName}{property.Name}");
		writer.WriteLine("{");
		writer.Indent++;

		if ((result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet || result.Accessors == PropertyAccessor.GetAndInit) &&
			result.Value.GetMethod!.CanBeSeenByContainingAssembly(compilation.Assembly))
		{
			var methodVisibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? 
				$"{result.Value.GetMethod!.GetOverridingCodeValue(compilation.Assembly)} " : string.Empty;
			var getVisibility = visibility != methodVisibility ?
				methodVisibility : string.Empty;

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
			var methodVisibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? 
				$"{result.Value.SetMethod!.GetOverridingCodeValue(compilation.Assembly)} " : string.Empty;
			var setVisibility = visibility != methodVisibility ?
				methodVisibility : string.Empty;
			writer.WriteLine($"{setVisibility}set {{ }}");
		}
		else if ((result.Accessors == PropertyAccessor.Init || result.Accessors == PropertyAccessor.GetAndInit) && 
			result.Value.SetMethod!.CanBeSeenByContainingAssembly(compilation.Assembly))
		{
			var methodVisibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? 
				$"{result.Value.SetMethod!.GetOverridingCodeValue(compilation.Assembly)} " : string.Empty;
			var initVisibility = visibility != methodVisibility ? 
				methodVisibility : string.Empty;
			writer.WriteLine($"{initVisibility}init {{ }}");
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}