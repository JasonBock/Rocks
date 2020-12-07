﻿using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders.Create
{
	internal static class MockExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information, ImmutableHashSet<INamespaceSymbol>.Builder namespaces)
		{
			MockDelegateBuilder.Build(writer, information);

			writer.WriteLine($"internal static class CreateExpectationsOf{information.TypeToMock.GetName(TypeNameOption.FlattenGenerics)}Extensions");
			writer.WriteLine("{");
			writer.Indent++;

			namespaces.AddRange(information.Methods.SelectMany(_ => _.Value.GetNamespaces()));
			namespaces.AddRange(information.Constructors.SelectMany(_ => _.GetNamespaces()));
			namespaces.AddRange(information.Properties.SelectMany(_ => _.Value.GetNamespaces()));
			namespaces.AddRange(information.Events.SelectMany(_ => _.Value.GetNamespaces()));

			MockMethodExtensionsBuilder.Build(writer, information);
			MockPropertyExtensionsBuilder.Build(writer, information);
			MockConstructorExtensionsBuilder.Build(writer, information);
			writer.WriteLine();
			MockCreateBuilder.Build(writer, information);

			writer.Indent--;
			writer.WriteLine("}");

			MethodExpectationsExtensionsBuilder.Build(writer, information);
			PropertyExpectationsExtensionsBuilder.Build(writer, information);
			EventExpectationsExtensionsBuilder.Build(writer, information);
		}
	}
}