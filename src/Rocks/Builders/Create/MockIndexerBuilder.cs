using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockIndexerBuilder
{
	private static void BuildGetter(IndentedTextWriter writer,
		PropertyModelOLD result, Compilation compilation, string indexerVisibility, 
		uint memberIdentifier, bool raiseEvents, string signature)
	{
		var indexer = result.Value;
		var method = indexer.GetMethod!;
		var shouldThrowDoesNotReturnException = method.IsMarkedWithDoesNotReturn(compilation);
		var methodVisibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? 
			$"{method.GetOverridingCodeValue(compilation.Assembly)} " : string.Empty;
		var visibility = methodVisibility != indexerVisibility ?
			methodVisibility : string.Empty;
		var namingContext = new VariableNamingContext(method);

		writer.WriteLine($"{visibility}get");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"if (this.handlers.TryGetValue({memberIdentifier}, out var @{namingContext["methodHandlers"]}))");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"foreach (var @{namingContext["methodHandler"]} in @{namingContext["methodHandlers"]})");
		writer.WriteLine("{");
		writer.Indent++;

		for (var i = 0; i < method.Parameters.Length; i++)
		{
			var parameter = method.Parameters[i];

			if (i == 0)
			{
				writer.WriteLine(
					$"if (((global::Rocks.Argument<{parameter.Type.GetFullyQualifiedName()}>)@{namingContext["methodHandler"]}.Expectations[{i}]).IsValid(@{parameter.Name}){(i == method.Parameters.Length - 1 ? ")" : " &&")}");
			}
			else
			{
				if (i == 1)
				{
					writer.Indent++;
				}

				writer.WriteLine(
					$"((global::Rocks.Argument<{parameter.Type.GetFullyQualifiedName()}>)@{namingContext["methodHandler"]}.Expectations[{i}]).IsValid(@{parameter.Name}){(i == method.Parameters.Length - 1 ? ")" : " &&")}");

				if (i == method.Parameters.Length - 1)
				{
					writer.Indent--;
				}
			}
		}

		writer.WriteLine("{");
		writer.Indent++;

		MockMethodValueBuilder.BuildMethodHandler(
			writer, method, result.MockType, namingContext,
			raiseEvents, shouldThrowDoesNotReturnException, result.MemberIdentifier);
		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");

		writer.WriteLine();
		writer.WriteLine($"throw new global::Rocks.Exceptions.ExpectationException(\"No handlers match for {signature.Replace("\"", "\\\"")}\");");

		writer.Indent--;
		writer.WriteLine("}");

		if (!indexer.IsAbstract)
		{
			writer.WriteLine("else");
			writer.WriteLine("{");
			writer.Indent++;

			// We'll call the base implementation if an expectation wasn't provided.
			// We'll do this as well for interfaces with a DIM through a shim.
			// If something like this is added in the future, then I'll revisit this:
			// https://github.com/dotnet/csharplang/issues/2337
			var parameters = string.Join(", ", indexer.Parameters.Select(_ =>
			{
				var direction = _.RefKind switch
				{
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}@{_.Name}";
			}));
			var refReturn = indexer.ReturnsByRef || indexer.ReturnsByRefReadonly ? "ref " : string.Empty;
			var target = indexer.ContainingType.TypeKind == TypeKind.Interface ?
				$"this.shimFor{indexer.ContainingType.GetName(TypeNameOption.Flatten)}" : "base";
			writer.WriteLine($"return {refReturn}{target}[{parameters}];");

			writer.Indent--;
			writer.WriteLine("}");
		}
		else
		{
			writer.WriteLine();
			writer.WriteLine($"throw new global::Rocks.Exceptions.ExpectationException(\"No handlers were found for {signature})\");");
		}

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildSetter(IndentedTextWriter writer,
		PropertyModelOLD result, Compilation compilation, string indexerVisibility, 
		uint memberIdentifier, bool raiseEvents, string signature)
	{
		var indexer = result.Value;
		var method = indexer.SetMethod!;
		var shouldThrowDoesNotReturnException = method.IsMarkedWithDoesNotReturn(compilation);
		var methodVisibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? 
			$"{method.GetOverridingCodeValue(compilation.Assembly)} " : string.Empty;
		var visibility = methodVisibility != indexerVisibility ?
			methodVisibility : string.Empty;
		var namingContext = new VariableNamingContext(method);
		var accessor = result.Accessors == PropertyAccessor.Init || result.Accessors == PropertyAccessor.GetAndInit ?
			"init" : "set";

		writer.WriteLine($"{visibility}{accessor}");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"if (this.handlers.TryGetValue({memberIdentifier}, out var @{namingContext["methodHandlers"]}))");
		writer.WriteLine("{");
		writer.Indent++;

		writer.WriteLine($"foreach (var @{namingContext["methodHandler"]} in @{namingContext["methodHandlers"]})");
		writer.WriteLine("{");
		writer.Indent++;

		for (var i = 0; i < method.Parameters.Length; i++)
		{
			var parameter = method.Parameters[i];

			if (i == 0)
			{
				writer.WriteLine(
					$"if (((global::Rocks.Argument<{parameter.Type.GetFullyQualifiedName()}>)@{namingContext["methodHandler"]}.Expectations[{i}]).IsValid(@{parameter.Name}){(i == method.Parameters.Length - 1 ? ")" : " &&")}");
			}
			else
			{
				if (i == 1)
				{
					writer.Indent++;
				}

				writer.WriteLine(
					$"((global::Rocks.Argument<{parameter.Type.GetFullyQualifiedName()}>)@{namingContext["methodHandler"]}.Expectations[{i}]).IsValid(@{parameter.Name}){(i == method.Parameters.Length - 1 ? ")" : " &&")}");

				if (i == method.Parameters.Length - 1)
				{
					writer.Indent--;
				}
			}
		}

		writer.WriteLine("{");
		writer.Indent++;

		MockMethodVoidBuilder.BuildMethodHandler(writer, method, result.MockType, namingContext, raiseEvents);

		if (shouldThrowDoesNotReturnException)
		{
			writer.WriteLine($"throw new global::Rocks.Exceptions.DoesNotReturnException();");
		}
		else
		{
			writer.WriteLine("return;");
		}

		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");

		writer.WriteLine();
		writer.WriteLine($"throw new global::Rocks.Exceptions.ExpectationException(\"No handlers match for {signature.Replace("\"", "\\\"")}\");");

		writer.Indent--;
		writer.WriteLine("}");

		if (!indexer.IsAbstract)
		{
			writer.WriteLine("else");
			writer.WriteLine("{");
			writer.Indent++;

			// We'll call the base implementation if an expectation wasn't provided.
			// We'll do this as well for interfaces with a DIM through a shim.
			// If something like this is added in the future, then I'll revisit this:
			// https://github.com/dotnet/csharplang/issues/2337
			var parameters = string.Join(", ", indexer.Parameters.Select(_ =>
			{
				var direction = _.RefKind switch
				{
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}@{_.Name}";
			}));
			var target = indexer.ContainingType.TypeKind == TypeKind.Interface ?
				$"this.shimFor{indexer.ContainingType.GetName(TypeNameOption.Flatten)}" : "base";
			writer.WriteLine($"{target}[{parameters}] = value;");

			writer.Indent--;
			writer.WriteLine("}");
		}
		else
		{
			writer.WriteLine();
			writer.WriteLine($"throw new global::Rocks.Exceptions.ExpectationException(\"No handlers were found for {signature.Replace("\"", "\\\"")})\");");
		}

		writer.Indent--;
		writer.WriteLine("}");
	}

	internal static void Build(IndentedTextWriter writer,
		PropertyModelOLD result, bool raiseEvents, Compilation compilation)
	{
		var indexer = result.Value;
		var attributes = indexer.GetAttributes();
		var explicitTypeName = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			string.Empty : $"{indexer.ContainingType.GetFullyQualifiedName()}.";
		var isGetterVisible = false;
		var isSetterVisible = false;

		if (attributes.Length > 0)
		{
			writer.WriteLine(attributes.GetDescription(compilation));
		}

		var memberIdentifierAttribute = result.MemberIdentifier;
		var signature = MockIndexerBuilder.GetSignature(indexer.Parameters, false, compilation);

		if (result.Accessors == PropertyAccessor.Get || result.Accessors == PropertyAccessor.GetAndSet ||
			result.Accessors == PropertyAccessor.GetAndInit)
		{
			isGetterVisible = result.Value.GetMethod!.CanBeSeenByContainingAssembly(compilation.Assembly);

			if (isGetterVisible)
			{
				writer.WriteLine($$"""[global::Rocks.MemberIdentifier({{memberIdentifierAttribute}}, "{{explicitTypeName}}{{signature}}")]""");
				memberIdentifierAttribute++;
			}
		}

		if (result.Accessors == PropertyAccessor.Set || result.Accessors == PropertyAccessor.Init ||
			result.Accessors == PropertyAccessor.GetAndSet || result.Accessors == PropertyAccessor.GetAndInit)
		{
			isSetterVisible = result.Value.SetMethod!.CanBeSeenByContainingAssembly(compilation.Assembly);

			if (isSetterVisible)
			{
				writer.WriteLine($$"""[global::Rocks.MemberIdentifier({{memberIdentifierAttribute}}, "{{explicitTypeName}}{{signature}}")]""");
			}
		}

		var visibility = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{result.Value.GetOverridingCodeValue(compilation.Assembly)} " : string.Empty;
		var isOverriden = result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;
		var isUnsafe = indexer.IsUnsafe() ? "unsafe " : string.Empty;
		var indexerSignature = $"{explicitTypeName}{MockIndexerBuilder.GetSignature(indexer.Parameters, true, compilation)}";

		var returnByRef = indexer.ReturnsByRef ? "ref " : indexer.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
		writer.WriteLine($"{visibility}{isUnsafe}{isOverriden}{returnByRef}{indexer.Type.GetFullyQualifiedName()} {indexerSignature}");
		writer.WriteLine("{");
		writer.Indent++;

		var memberIdentifier = result.MemberIdentifier;

		if (isGetterVisible)
		{
			MockIndexerBuilder.BuildGetter(writer, result, compilation,
				visibility, memberIdentifier, raiseEvents, signature);
			memberIdentifier++;
		}

		if (isSetterVisible)
		{
			MockIndexerBuilder.BuildSetter(writer, result, compilation,
				visibility, memberIdentifier, raiseEvents, signature);
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
					 $" = {_.ExplicitDefaultValue.GetDefaultValue(_.Type)}" : string.Empty;
			var direction = _.RefKind switch
			{
				RefKind.In => "in ",
				_ => string.Empty
			};
			var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetFullyQualifiedName()} @{_.Name}{defaultValue}";
			return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription(compilation)} " : string.Empty)}{parameter}";
		}));

		return $"this[{string.Join(", ", methodParameters)}]";
	}
}