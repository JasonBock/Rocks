﻿using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Linq;

namespace Rocks.Builders.Create
{
	internal static class MethodExpectationsExtensionsBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			if (information.Methods.Length > 0)
			{
				writer.WriteLine();
				var typeToMock = information.TypeToMock.GetName(TypeNameOption.Flatten);

				if (information.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
				{
					writer.WriteLine($"internal static class {WellKnownNames.Method}{WellKnownNames.Expectations}Of{typeToMock}{WellKnownNames.Extensions}");
					writer.WriteLine("{");
					writer.Indent++;

					foreach (var result in information.Methods.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No))
					{
						MethodExpectationsExtensionsMethodBuilder.Build(writer, result);
					}

					writer.Indent--;
					writer.WriteLine("}");
				}

				if (information.Methods.Any(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes))
				{
					foreach (var typeGroup in information.Methods
						.Where(_ => _.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
						.GroupBy(_ => _.Value.ContainingType))
					{
						var containingTypeName = typeGroup.Key.GetName(TypeNameOption.Flatten);
						writer.WriteLine($"internal static class {WellKnownNames.Explicit}{WellKnownNames.Method}{WellKnownNames.Expectations}Of{typeToMock}For{containingTypeName}{WellKnownNames.Extensions}");
						writer.WriteLine("{");
						writer.Indent++;

						foreach (var result in typeGroup)
						{
							MethodExpectationsExtensionsMethodBuilder.Build(writer, result);
						}

						writer.Indent--;
						writer.WriteLine("}");
					}
				}
			}
		}
	}
}