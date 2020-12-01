﻿using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Builders
{
	internal static class MockIndexerBuilder
	{
		private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, bool raiseEvents, string explicitTypeName)
		{
			var method = result.Value.GetMethod!;
			var methodException =
				$"{method.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} {explicitTypeName}this[{string.Join(", ", method.Parameters.Select(_ => $"{{{_.Name}}}"))}";

			writer.WriteLine("get");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"if (this.handlers.TryGetValue({memberIdentifier}, out var methodHandlers))");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("foreach (var methodHandler in methodHandlers)");
			writer.WriteLine("{");
			writer.Indent++;

			for (var i = 0; i < method.Parameters.Length; i++)
			{
				var parameter = method.Parameters[i];

				if (i == 0)
				{
					writer.WriteLine(
						$"if (((methodHandler.Expectations[{i}] as Arg<{parameter.Type.GetName()}>)?.IsValid({parameter.Name}) ?? false){(i == method.Parameters.Length - 1 ? ")" : " &&")}");
				}
				else
				{
					if (i == 1)
					{
						writer.Indent++;
					}

					writer.WriteLine(
						$"((methodHandler.Expectations[{i}] as Arg<{parameter.Type.GetName()}>)?.IsValid({parameter.Name}) ?? false){(i == method.Parameters.Length - 1 ? ")" : " &&")}");

					if (i == method.Parameters.Length - 1)
					{
						writer.Indent--;
					}
				}
			}

			writer.WriteLine("{");
			writer.Indent++;

			MockMethodValueBuilder.BuildMethodHandler(writer, method, raiseEvents);
			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine();

			writer.WriteLine($@"throw new ExpectationException($""No handlers were found for {methodException})"");");
			writer.Indent--;
			writer.WriteLine("}");
		}

		private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, bool raiseEvents, string explicitTypeName)
		{
			var method = result.Value.SetMethod!;
			var methodException =
				$"{method.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} {explicitTypeName}this[{string.Join(", ", method.Parameters.Select(_ => $"{{{_.Name}}}"))}";

			writer.WriteLine("set");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine($"if (this.handlers.TryGetValue({memberIdentifier}, out var methodHandlers))");
			writer.WriteLine("{");
			writer.Indent++;

			writer.WriteLine("foreach (var methodHandler in methodHandlers)");
			writer.WriteLine("{");
			writer.Indent++;

			for (var i = 0; i < method.Parameters.Length; i++)
			{
				var parameter = method.Parameters[i];

				if (i == 0)
				{
					writer.WriteLine(
						$"if (((methodHandler.Expectations[{i}] as Arg<{parameter.Type.GetName()}>)?.IsValid({parameter.Name}) ?? false){(i == method.Parameters.Length - 1 ? ")" : " &&")}");
				}
				else
				{
					if (i == 1)
					{
						writer.Indent++;
					}

					writer.WriteLine(
						$"((methodHandler.Expectations[{i}] as Arg<{parameter.Type.GetName()}>)?.IsValid({parameter.Name}) ?? false){(i == method.Parameters.Length - 1 ? ")" : " &&")}");

					if (i == method.Parameters.Length - 1)
					{
						writer.Indent--;
					}
				}
			}

			writer.WriteLine("{");
			writer.Indent++;

			MockMethodVoidBuilder.BuildMethodHandler(writer, method, raiseEvents);
			writer.WriteLine("return;");
			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");

			writer.Indent--;
			writer.WriteLine("}");

			writer.WriteLine();

			writer.WriteLine($@"throw new ExpectationException($""No handlers were found for {methodException})"");");
			writer.Indent--;
			writer.WriteLine("}");
		}

		internal static void Build(IndentedTextWriter writer, PropertyMockableResult result, bool raiseEvents)
		{
			var attributes = result.Value.GetAttributes();
			var explicitTypeName = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				string.Empty : $"{result.Value.ContainingType.GetName(TypeNameOption.NoGenerics)}.";

			if (attributes.Length > 0)
			{
				writer.WriteLine(attributes.GetDescription());
			}

			var memberIdentifierAttribute = result.MemberIdentifier;

			if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine($@"[MemberIdentifier({memberIdentifierAttribute}, ""{explicitTypeName}{MockIndexerBuilder.GetSignature(result.Value.GetMethod!.Parameters)}"")]");
				memberIdentifierAttribute++;
			}

			if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet)
			{
				writer.WriteLine($@"[MemberIdentifier({memberIdentifierAttribute}, ""{explicitTypeName}{MockIndexerBuilder.GetSignature(result.Value.SetMethod!.Parameters)}"")]");
			}

			var visibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				"public " : string.Empty;
			var isOverriden = result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;
			var indexerSignature = $"{explicitTypeName}{MockIndexerBuilder.GetSignature(result.Value.Parameters)}";

			writer.WriteLine($"{visibility}{isOverriden}{result.Value.Type.GetName()} {indexerSignature}");
			writer.WriteLine("{");
			writer.Indent++;

			var memberIdentifier = result.MemberIdentifier;

			if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet)
			{
				MockIndexerBuilder.BuildGetter(writer, result, memberIdentifier, raiseEvents, explicitTypeName);
				memberIdentifier++;
			}

			if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.GetAndSet)
			{
				MockIndexerBuilder.BuildSetter(writer, result, memberIdentifier, raiseEvents, explicitTypeName);
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		private static string GetSignature(ImmutableArray<IParameterSymbol> parameters)
		{
			var methodParameters = string.Join(", ", parameters.Select(_ =>
			{
				var parameter = $"{_.Type.GetName()} {_.Name}";
				return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription()} " : string.Empty)}{parameter}";
			}));
			
			return $"this[{string.Join(", ", methodParameters)}]";
		}
	}
}