using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Shim;

internal static class ShimIndexerBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel shimType)
	{
		foreach (var indexer in shimType.Properties
			.Where(_ => _.IsIndexer && !_.IsVirtual)
			.Select(_ => _))
		{
			writer.WriteLine();

			if (indexer.AttributesDescription.Length > 0)
			{
				writer.WriteLine(indexer.AttributesDescription);
			}

			var isUnsafe = indexer.IsUnsafe ? "unsafe " : string.Empty;
			var returnByRef = indexer.ReturnsByRef ? "ref " : indexer.ReturnsByRefReadOnly ? "ref readonly " : string.Empty;
			var (accessibility, explicitName, includeOptionalParameterValues) = 
				indexer.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
					("public ", string.Empty, true) : (string.Empty, $"{indexer.ContainingType.FullyQualifiedName}.", false);

			writer.WriteLine($"{accessibility}{isUnsafe}{returnByRef}{indexer.Type.FullyQualifiedName} {explicitName}{GetSignature(indexer.Parameters, includeOptionalParameterValues)}");
			writer.WriteLine("{");
			writer.Indent++;

			var parameters = string.Join(", ", indexer.Parameters.Select(_ =>
			{
				var direction = _.RefKind switch
				{
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}@{_.Name}";
			}));

			var accessors = indexer.Accessors;
			if (accessors == PropertyAccessor.Get || accessors == PropertyAccessor.GetAndInit ||
				accessors == PropertyAccessor.GetAndSet)
			{
				var refReturn = indexer.ReturnsByRef || indexer.ReturnsByRefReadOnly ? "ref " : string.Empty;
				writer.WriteLine($"get => {refReturn}(({shimType.Type.FullyQualifiedName})this.mock)[{parameters}]!;");
			}

			if (accessors == PropertyAccessor.Set || accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine($"set => (({shimType.Type.FullyQualifiedName})this.mock)[{parameters}] = value!;");
			}

			if (accessors == PropertyAccessor.Init || accessors == PropertyAccessor.GetAndInit)
			{
				writer.WriteLine($"init => (({shimType.Type.FullyQualifiedName})this.mock)[{parameters}] = value!;");
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		static string GetSignature(ImmutableArray<ParameterModel> parameters, bool includeOptionalParameterValues)
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
}