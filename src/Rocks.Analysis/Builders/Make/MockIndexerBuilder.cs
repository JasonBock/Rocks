using Microsoft.CodeAnalysis;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Analysis.Builders.Make;

internal static class MockIndexerBuilder
{
	internal static void Build(IndentedTextWriter writer, PropertyModel indexer)
	{
		var explicitTypeName = indexer.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			string.Empty : $"{indexer.ContainingType.FullyQualifiedName}.";

		if (indexer.AttributesDescription.Length > 0)
		{
			writer.WriteLine(indexer.AttributesDescription);
		}

		var visibility = indexer.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{indexer.OverridingCodeValue} " : string.Empty;
		var isUnsafe = indexer.IsUnsafe ? "unsafe " : string.Empty;
		var isOverriden = indexer.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;
		var includeOptionalParameterValues = indexer.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No;
		var indexerSignature = $"{explicitTypeName}{GetSignature(indexer.Parameters, includeOptionalParameterValues)}";

		var returnByRef = indexer.ReturnsByRef ? "ref " : indexer.ReturnsByRefReadOnly ? "ref readonly " : string.Empty;
		writer.WriteLine($"{visibility}{isUnsafe}{isOverriden}{returnByRef}{indexer.Type.FullyQualifiedName} {indexerSignature}");
		writer.WriteLine("{");
		writer.Indent++;

		if ((indexer.Accessors == PropertyAccessor.Get || indexer.Accessors == PropertyAccessor.GetAndSet || indexer.Accessors == PropertyAccessor.GetAndInit) && 
			indexer.GetCanBeSeenByContainingAssembly)
		{
			var methodVisibility = indexer.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				$"{indexer.GetMethod!.OverridingCodeValue} " : string.Empty;
			var getVisibility = visibility != methodVisibility ?
				methodVisibility : string.Empty;

			if (indexer.ReturnsByRef || indexer.ReturnsByRefReadOnly)
			{
				writer.WriteLine($"{getVisibility}get => ref this.rr{indexer.MemberIdentifier};");
			}
			else
			{
				writer.WriteLine($"{getVisibility}get => default!;");
			}
		}

		if ((indexer.Accessors == PropertyAccessor.Set || indexer.Accessors == PropertyAccessor.GetAndSet) && 
			indexer.SetCanBeSeenByContainingAssembly)
		{
			var methodVisibility = indexer.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				$"{indexer.SetMethod!.OverridingCodeValue} " : string.Empty;
			var setVisibility = visibility != methodVisibility ?
				methodVisibility : string.Empty;
			writer.WriteLine($"{setVisibility}set {{ }}");
		}
		else if ((indexer.Accessors == PropertyAccessor.Init || indexer.Accessors == PropertyAccessor.GetAndInit) && 
			indexer.InitCanBeSeenByContainingAssembly)
		{
			var methodVisibility = indexer.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				$"{indexer.SetMethod!.OverridingCodeValue} " : string.Empty;
			var initVisibility = visibility != methodVisibility ?
				methodVisibility : string.Empty;
			writer.WriteLine($"{initVisibility}init {{ }}");
		}

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static string GetSignature(ImmutableArray<ParameterModel> parameters, bool includeOptionalParameterValues)
	{
		var methodParameters = string.Join(", ", parameters.Select(_ =>
		{
			var defaultValue = includeOptionalParameterValues && _.HasExplicitDefaultValue ?
				$" = {_.ExplicitDefaultValue}" : string.Empty;
			var direction = _.RefKind switch
			{
				RefKind.In => "in ",
				_ => string.Empty
			};
			var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.FullyQualifiedName} @{_.Name}{defaultValue}";
			return $"{(_.AttributesDescription.Length > 0 ? $"{_.AttributesDescription} " : string.Empty)}{parameter}";
		}));

		return $"this[{string.Join(", ", methodParameters)}]";
	}
}