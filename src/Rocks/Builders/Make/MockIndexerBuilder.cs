using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Make;

internal static class MockIndexerBuilder
{
	internal static void Build(IndentedTextWriter writer, PropertyMockableResult result, Compilation compilation)
	{
		var indexer = result.Value;
		var attributes = indexer.GetAttributes();
		var explicitTypeName = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			string.Empty : $"{indexer.ContainingType.GetName(TypeNameOption.IncludeGenerics)}.";

		if (attributes.Length > 0)
		{
			writer.WriteLine(attributes.GetDescription(compilation));
		}

		var visibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{result.Value.DeclaredAccessibility.GetOverridingCodeValue()} " : string.Empty;
		var isUnsafe = indexer.IsUnsafe() ? "unsafe " : string.Empty;
		var isOverriden = result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;
		var indexerSignature = $"{explicitTypeName}{MockIndexerBuilder.GetSignature(indexer.Parameters, true, compilation)}";

		var returnByRef = indexer.ReturnsByRef ? "ref " : indexer.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
		writer.WriteLine($"{visibility}{isUnsafe}{isOverriden}{returnByRef}{indexer.Type.GetReferenceableName()} {indexerSignature}");
		writer.WriteLine("{");
		writer.Indent++;

		if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet ||
			result.Accessors == PropertyAccessor.GetAndInit)
		{
			if (indexer.ReturnsByRef || indexer.ReturnsByRefReadonly)
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

	private static string GetSignature(ImmutableArray<IParameterSymbol> parameters, bool includeOptionalParameterValues,
		Compilation compilation)
	{
		var methodParameters = string.Join(", ", parameters.Select(_ =>
		{
			var defaultValue = includeOptionalParameterValues && _.HasExplicitDefaultValue ?
					 $" = {_.ExplicitDefaultValue.GetDefaultValue(_.Type.IsValueType)}" : string.Empty;
			var direction = _.RefKind switch
			{
				RefKind.In => "in ",
				_ => string.Empty
			};
			var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetReferenceableName()} {_.Name}{defaultValue}";
			return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription(compilation)} " : string.Empty)}{parameter}";
		}));

		return $"this[{string.Join(", ", methodParameters)}]";
	}
}