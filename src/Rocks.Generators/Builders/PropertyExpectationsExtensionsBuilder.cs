using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders
{
	internal static class PropertyExpectationsExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			if (information.Properties.Any(_ => !_.Value.IsIndexer))
			{
				if (information.Properties.Any(_ => !_.Value.IsIndexer &&
					 (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet)))
				{
					writer.WriteLine($"internal static class PropertyGetterExpectationsOf{information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}Extensions");
					writer.WriteLine("{");
					writer.Indent++;

					foreach (var result in information.Properties.Where(_ => !_.Value.IsIndexer &&
						(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet)))
					{
						PropertyExpectationsExtensionsPropertyBuilder.Build(writer, result, PropertyAccessor.Get);
					}

					writer.Indent--;
					writer.WriteLine("}");
				}

				if (information.Properties.Any(_ => !_.Value.IsIndexer &&
					 (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
				{
					writer.WriteLine($"internal static class PropertySetterExpectationsOf{information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}Extensions");
					writer.WriteLine("{");
					writer.Indent++;

					foreach (var result in information.Properties.Where(_ => !_.Value.IsIndexer &&
						(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
					{
						PropertyExpectationsExtensionsPropertyBuilder.Build(writer, result, PropertyAccessor.Set);
					}

					writer.Indent--;
					writer.WriteLine("}");
				}
			}

			if (information.Properties.Any(_ => _.Value.IsIndexer))
			{
				if (information.Properties.Any(_ => _.Value.IsIndexer &&
					 (_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet)))
				{
					writer.WriteLine($"internal static class IndexerGetterExpectationsOf{information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}Extensions");
					writer.WriteLine("{");
					writer.Indent++;

					foreach (var result in information.Properties.Where(_ => _.Value.IsIndexer &&
						(_.Accessors == PropertyAccessor.Get || _.Accessors == PropertyAccessor.GetAndSet)))
					{
						IndexerExpectationsExtensionsIndexerBuilder.Build(writer, result, PropertyAccessor.Get);
					}

					writer.Indent--;
					writer.WriteLine("}");
				}

				if (information.Properties.Any(_ => _.Value.IsIndexer &&
					 (_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
				{
					writer.WriteLine($"internal static class IndexerSetterExpectationsOf{information.TypeToMock.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}Extensions");
					writer.WriteLine("{");
					writer.Indent++;

					foreach (var result in information.Properties.Where(_ => _.Value.IsIndexer &&
						(_.Accessors == PropertyAccessor.Set || _.Accessors == PropertyAccessor.GetAndSet)))
					{
						IndexerExpectationsExtensionsIndexerBuilder.Build(writer, result, PropertyAccessor.Set);
					}

					writer.Indent--;
					writer.WriteLine("}");
				}
			}
		}
	}
}