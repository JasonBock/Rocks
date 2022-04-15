using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Shim;

internal static class ShimIndexerBuilder
{
	internal static void Build(IndentedTextWriter writer, ITypeSymbol shimType, Compilation compilation)
	{
		foreach (var indexer in shimType.GetMembers().OfType<IPropertySymbol>()
			.Where(_ => _.IsIndexer && !_.IsVirtual))
		{
			writer.WriteLine();

			var attributes = indexer.GetAttributes();

			if (attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription(compilation));
			}

			var isUnsafe = indexer.IsUnsafe() ? "unsafe " : string.Empty;

			var returnByRef = indexer.ReturnsByRef ? "ref " : indexer.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
			writer.WriteLine($"public {isUnsafe}{returnByRef}{indexer.Type.GetName()} {GetSignature(indexer.Parameters, true, compilation)}");
			writer.WriteLine("{");
			writer.Indent++;

			var parameters = string.Join(", ", indexer.Parameters.Select(_ =>
			{
				var direction = _.RefKind switch
				{
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}{_.Name}";
			}));

			var accessors = indexer.GetAccessors();
			if (accessors == PropertyAccessor.Get || accessors == PropertyAccessor.GetAndInit ||
				accessors == PropertyAccessor.GetAndSet)
			{
				var refReturn = indexer.ReturnsByRef || indexer.ReturnsByRefReadonly ? "ref " : string.Empty;
				writer.WriteLine($"get => {refReturn}this.mock[{parameters}];");
			}

			if (accessors == PropertyAccessor.Set || accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine($"set => this.mock[{parameters}] = value;");
			}

			if (accessors == PropertyAccessor.Init || accessors == PropertyAccessor.GetAndInit)
			{
				writer.WriteLine($"init => this.mock[{parameters}] = value;");
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		static string GetSignature(ImmutableArray<IParameterSymbol> parameters, bool includeOptionalParameterValues,
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
				var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetName()} {_.Name}{defaultValue}";
				return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription(compilation)} " : string.Empty)}{parameter}";
			}));

			return $"this[{string.Join(", ", methodParameters)}]";
		}
	}
}