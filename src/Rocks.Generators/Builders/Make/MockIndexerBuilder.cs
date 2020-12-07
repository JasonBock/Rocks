using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders.Make
{
	internal static class MockIndexerBuilder
	{
		internal static void Build(IndentedTextWriter writer, PropertyMockableResult result)
		{
			var indexer = result.Value;
			var attributes = indexer.GetAttributes();
			var explicitTypeName = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				string.Empty : $"{indexer.ContainingType.GetName(TypeNameOption.NoGenerics)}.";

			if (attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription());
			}

			var visibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				"public " : string.Empty;
			var isOverriden = result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;
			var indexerSignature = $"{explicitTypeName}{MockIndexerBuilder.GetSignature(indexer.Parameters, true)}";

			var returnByRef = indexer.ReturnsByRef ? "ref " : indexer.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
			writer.WriteLine($"{visibility}{isOverriden}{returnByRef}{indexer.Type.GetName()} {indexerSignature}");
			writer.WriteLine("{");
			writer.Indent++;

			if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine("get => default!;");
			}

			if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine("set { }");
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		private static string GetSignature(ImmutableArray<IParameterSymbol> parameters, bool includeOptionalParameterValues)
		{
			var methodParameters = string.Join(", ", parameters.Select(_ =>
			{
				var defaultValue = includeOptionalParameterValues && _.HasExplicitDefaultValue ? 
					$" = {_.ExplicitDefaultValue.GetDefaultValue()}" : string.Empty;
				var direction = _.RefKind switch
				{
					RefKind.In => "in ",
					_ => string.Empty
				};
				var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetName()} {_.Name}{defaultValue}";
				return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription()} " : string.Empty)}{parameter}";
			}));
			
			return $"this[{string.Join(", ", methodParameters)}]";
		}
	}
}