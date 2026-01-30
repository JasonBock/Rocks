using Microsoft.CodeAnalysis;
using Rocks.Analysis.Builders.Shim;
using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Analysis.Builders.Create;

internal static class MockIndexerBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, TypeMockModel type,
		PropertyModel indexer, string indexerVisibility,
		uint memberIdentifier, bool raiseEvents)
	{
		var method = indexer.GetMethod!;
		var shouldThrowDoesNotReturnException = method.IsMarkedWithDoesNotReturn;
		var methodVisibility = indexer.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{method.OverridingCodeValue} " : string.Empty;
		var visibility = methodVisibility != indexerVisibility ?
			methodVisibility : string.Empty;
		var namingContext = new VariablesNamingContext(method);

		writer.WriteLines(
			$$"""
			[global::Rocks.MemberIdentifier({{memberIdentifier}})]
			{{visibility}}get
			{
				if (this.Expectations.handlers{{memberIdentifier}} is not null)
				{
					foreach (var @{{namingContext["handler"]}} in this.Expectations.handlers{{memberIdentifier}})
					{
			""");

		writer.Indent += 3;

		var handlerNamingContext = HandlerVariableNamingContext.Create();

		for (var i = 0; i < method.Parameters.Length; i++)
		{
			var parameter = method.Parameters[i];

			if (i == 0)
			{
				writer.WriteLine(
					$"if (@{namingContext["handler"]}.@{handlerNamingContext[parameter.Name]}.IsValid(@{parameter.Name}!){(i == method.Parameters.Length - 1 ? ")" : " &&")}");
			}
			else
			{
				if (i == 1)
				{
					writer.Indent++;
				}

				writer.WriteLine(
					$"@{namingContext["handler"]}.@{handlerNamingContext[parameter.Name]}.IsValid(@{parameter.Name}!){(i == method.Parameters.Length - 1 ? ")" : " &&")}");

				if (i == method.Parameters.Length - 1)
				{
					writer.Indent--;
				}
			}
		}

		writer.WriteLine("{");
		writer.Indent++;
		MockMethodValueBuilder.BuildMethodHandler(
			writer, method, namingContext,
			raiseEvents, shouldThrowDoesNotReturnException, indexer.MemberIdentifier);
		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");

		writer.WriteLine();
		ExpectationExceptionBuilder.Build(
			writer, method, "No handlers match for", memberIdentifier, type.ExpectationsPropertyName);

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

				return $"@{_.Name}: {direction}@{_.Name}!";
			}));
			var refReturn = indexer.ReturnsByRef || indexer.ReturnsByRefReadOnly ? "ref " : string.Empty;
			var target = indexer.ContainingType.TypeKind == TypeKind.Interface ?
				$"this.shimFor{ShimBuilder.GetShimName(indexer.ContainingType)}" : "base";
			writer.WriteLine($"return {refReturn}{target}[{parameters}];");

			writer.Indent--;
			writer.WriteLine("}");
		}
		else
		{
			writer.WriteLine();
			ExpectationExceptionBuilder.Build(
				writer, method, "No handlers were found for", memberIdentifier, type.ExpectationsPropertyName);
		}

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildSetter(IndentedTextWriter writer, TypeMockModel type,
		PropertyModel indexer, string indexerVisibility,
		uint memberIdentifier, bool raiseEvents)
	{
		var method = indexer.SetMethod!;
		var shouldThrowDoesNotReturnException = method.IsMarkedWithDoesNotReturn;
		var methodVisibility = indexer.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{method.OverridingCodeValue} " : string.Empty;
		var visibility = methodVisibility != indexerVisibility ?
			methodVisibility : string.Empty;
		var namingContext = new VariablesNamingContext(method);
		var accessor = indexer.Accessors == PropertyAccessor.Init || indexer.Accessors == PropertyAccessor.GetAndInit ?
			"init" : "set";

		writer.WriteLines(
			$$"""
			[global::Rocks.MemberIdentifier({{memberIdentifier}})]
			{{visibility}}{{accessor}}
			{
				if (this.Expectations.handlers{{memberIdentifier}} is not null)
				{
					foreach (var @{{namingContext["handler"]}} in this.Expectations.handlers{{memberIdentifier}})
					{
			""");

		writer.Indent += 3;

		var handlerNamingContext = HandlerVariableNamingContext.Create();

		for (var i = 0; i < method.Parameters.Length; i++)
		{
			var parameter = method.Parameters[i];

			if (i == 0)
			{
				writer.WriteLine(
					$"if (@{namingContext["handler"]}.@{handlerNamingContext[parameter.Name]}.IsValid(@{parameter.Name}!){(i == method.Parameters.Length - 1 ? ")" : " &&")}");
			}
			else
			{
				if (i == 1)
				{
					writer.Indent++;
				}

				writer.WriteLine(
					$"@{namingContext["handler"]}.@{handlerNamingContext[parameter.Name]}.IsValid(@{parameter.Name}!){(i == method.Parameters.Length - 1 ? ")" : " &&")}");

				if (i == method.Parameters.Length - 1)
				{
					writer.Indent--;
				}
			}
		}

		writer.WriteLine("{");
		writer.Indent++;
		MockMethodVoidBuilder.BuildMethodHandler(writer, method, namingContext, raiseEvents);

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
		ExpectationExceptionBuilder.Build(
			writer, method, "No handlers match for", memberIdentifier, type.ExpectationsPropertyName);

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

				return $"@{_.Name}: {direction}@{_.Name}!";
			}));
			var target = indexer.ContainingType.TypeKind == TypeKind.Interface ?
				$"this.shimFor{ShimBuilder.GetShimName(indexer.ContainingType)}" : "base";
			writer.WriteLine($"{target}[{parameters}] = @value!;");

			writer.Indent--;
			writer.WriteLine("}");
		}
		else
		{
			writer.WriteLine();
			ExpectationExceptionBuilder.Build(
				writer, method, "No handlers were found for", memberIdentifier, type.ExpectationsPropertyName);
		}

		writer.Indent--;
		writer.WriteLine("}");
	}

	internal static void Build(IndentedTextWriter writer, TypeMockModel type,
		PropertyModel indexer, bool raiseEvents)
	{
		var explicitTypeName = indexer.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			string.Empty : $"{indexer.ContainingType.FullyQualifiedName}.";
		var isGetterVisible = false;
		var isSetterVisible = false;

		if (indexer.AttributesDescription.Length > 0)
		{
			writer.WriteLine(indexer.AttributesDescription);
		}

		var memberIdentifierAttribute = indexer.MemberIdentifier;
		var includeOptionalParameterValues = indexer.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No;
		var signature = GetSignature(indexer.Parameters, includeOptionalParameterValues);

		if (indexer.Accessors == PropertyAccessor.Get || indexer.Accessors == PropertyAccessor.GetAndSet ||
			indexer.Accessors == PropertyAccessor.GetAndInit)
		{
			isGetterVisible = indexer.GetCanBeSeenByContainingAssembly;
		}

		if (indexer.Accessors == PropertyAccessor.Set || indexer.Accessors == PropertyAccessor.Init || indexer.Accessors == PropertyAccessor.GetAndSet || indexer.Accessors == PropertyAccessor.GetAndInit)
		{
			isSetterVisible = indexer.SetCanBeSeenByContainingAssembly || indexer.InitCanBeSeenByContainingAssembly;
		}

		var visibility = indexer.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{indexer.OverridingCodeValue} " : string.Empty;
		var isOverriden = indexer.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty;
		var isUnsafe = indexer.IsUnsafe ? "unsafe " : string.Empty;
		var indexerSignature = $"{explicitTypeName}{signature}";

		var returnByRef = indexer.ReturnsByRef ? "ref " : indexer.ReturnsByRefReadOnly ? "ref readonly " : string.Empty;
		writer.WriteLine($"{visibility}{isUnsafe}{isOverriden}{returnByRef}{indexer.Type.FullyQualifiedName} {indexerSignature}");
		writer.WriteLine("{");
		writer.Indent++;

		var memberIdentifier = indexer.MemberIdentifier;

		if (isGetterVisible)
		{
		 BuildGetter(writer, type, indexer, visibility, memberIdentifier, raiseEvents);
			memberIdentifier++;
		}

		if (isSetterVisible)
		{
		 BuildSetter(writer, type, indexer, visibility, memberIdentifier, raiseEvents);
		}

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static string GetSignature(ImmutableArray<ParameterModel> parameters, bool includeOptionalParameterValues)
	{
		var methodParameters = string.Join(", ", parameters.Select(_ =>
		{
			var defaultValue = includeOptionalParameterValues && _.HasExplicitDefaultValue ?
				_.AttributesDescription.Contains("Optional") ?
					string.Empty :
					$" = {_.ExplicitDefaultValue}" :
				string.Empty;

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