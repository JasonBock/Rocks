﻿using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Rocks.Builders
{
	/*
	
	Example: If we are given a method like: Foo(int a, string b)

	internal static MethodAdornments Foo(this MethodExpectations<IMockable> self, Arg<int> a, Arg<string> b) =>
		new MethodAdornments(self.Add(0, new Dictionary<int, Arg>
		{
			{ 0, a },
			{ 1, b },
		}));
	*/

	internal static class MethodExpectationsExtensionsMethodBuilder
	{
		internal static void Build(IndentedTextWriter writer, MethodMockableResult result, SortedSet<string> namespaces)
		{
			var method = result.Value;
			var thisParameter = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				$"this MethodExpectations<{result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}> self" :
				$"this ExplicitMethodExpectations<{result.Value.ContainingType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}, {result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}> self";
			var instanceParameters = method.Parameters.Length == 0 ? thisParameter :
				string.Join(", ", thisParameter,
					string.Join(", ", method.Parameters.Select(_ =>
					{
						if (!_.Type.ContainingNamespace?.IsGlobalNamespace ?? false)
						{
							namespaces.Add($"using {_.Type.ContainingNamespace!.ToDisplayString()};");
						}

						return $"Arg<{_.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}> {_.Name}";
					})));

			var adornmentsType = method.ReturnsVoid ?
				$"MethodAdornments<{result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>" :
				$"MethodAdornments<{result.MockType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}, {method.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			writer.WriteLine($"internal static {returnValue} {method.Name}({instanceParameters}) =>");
			writer.Indent++;

			var addReturnValue = method.ReturnsVoid ? string.Empty : $"<{method.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>";

			if(method.Parameters.Length == 0)
			{
				writer.WriteLine($"{newAdornments}(self.Add{addReturnValue}({result.MemberIdentifier}, new List<Arg>()));");
			}
			else
			{
				writer.WriteLine($"{newAdornments}(self.Add{addReturnValue}({result.MemberIdentifier}, new List<Arg> {{ {string.Join(", ", method.Parameters.Select(_ => _.Name))} }}));");
			}

			writer.Indent--;
		}
	}
}