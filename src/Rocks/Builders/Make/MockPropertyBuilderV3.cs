using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Make;

internal static class MockPropertyBuilderV3
{
	internal static void Build(IndentedTextWriter writer, Models.PropertyModel property)
	{
		if (property.AttributesDescription.Length > 0)
		{
			writer.WriteLine(property.AttributesDescription);
		}

		var explicitTypeName = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			string.Empty : $"{property.ContainingType.FullyQualifiedName}.";

		var visibility = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{property.OverridingCodeValue} " : string.Empty;
		var isUnsafe = property.IsUnsafe ? "unsafe " : string.Empty;
		var isOverriden = property.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;

		var returnByRef = property.ReturnsByRef ? "ref " : property.ReturnsByRefReadOnly ? "ref readonly " : string.Empty;
		writer.WriteLine($"{visibility}{isUnsafe}{isOverriden}{returnByRef}{property.Type.FullyQualifiedName} {explicitTypeName}{property.Name}");
		writer.WriteLine("{");
		writer.Indent++;

		if ((property.Accessors == PropertyAccessor.Get || property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) &&
			property.GetCanBeSeenByContainingAssembly)
		{
			var methodVisibility = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? 
				$"{property.GetMethod!.OverridingCodeValue} " : string.Empty;
			var getVisibility = visibility != methodVisibility ?
				methodVisibility : string.Empty;

			if (property.ReturnsByRef || property.ReturnsByRefReadOnly)
			{
				writer.WriteLine($"{getVisibility}get => ref this.rr{property.MemberIdentifier};");
			}
			else
			{
				writer.WriteLine($"{getVisibility}get => default!;");
			}
		}

		if ((property.Accessors == PropertyAccessor.Set || property.Accessors == PropertyAccessor.GetAndSet) &&
			property.SetCanBeSeenByContainingAssembly)
		{
			var methodVisibility = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? 
				$"{property.SetMethod!.OverridingCodeValue} " : string.Empty;
			var setVisibility = visibility != methodVisibility ?
				methodVisibility : string.Empty;
			writer.WriteLine($"{setVisibility}set {{ }}");
		}
		else if ((property.Accessors == PropertyAccessor.Init || property.Accessors == PropertyAccessor.GetAndInit) && 
			property.InitCanBeSeenByContainingAssembly)
		{
			var methodVisibility = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? 
				$"{property.SetMethod!.OverridingCodeValue} " : string.Empty;
			var initVisibility = visibility != methodVisibility ? 
				methodVisibility : string.Empty;
			writer.WriteLine($"{initVisibility}init {{ }}");
		}

		writer.Indent--;
		writer.WriteLine("}");
	}
}