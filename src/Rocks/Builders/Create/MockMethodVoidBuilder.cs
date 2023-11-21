using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockMethodVoidBuilder
{
	internal static void Build(IndentedTextWriter writer, MethodModel method, bool raiseEvents)
	{
		var shouldThrowDoesNotReturnException = method.ShouldThrowDoesNotReturnException;
		var parametersDescription = string.Join(", ", method.Parameters.Select(_ =>
		{
			var requiresNullable = _.RequiresNullableAnnotation ? "?" : string.Empty;
			var direction = _.RefKind switch
			{
				RefKind.Ref => "ref ",
				RefKind.Out => "out ",
				RefKind.In => "in ",
				_ => string.Empty
			};
			return $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.FullyQualifiedName}{requiresNullable} @{_.Name}";
		}));
		var explicitTypeNameDescription = method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
			$"{method.ContainingType.FullyQualifiedName}." : string.Empty;
		var methodDescription = $"void {explicitTypeNameDescription}{method.Name}({parametersDescription})";

		var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
		{
			var requiresNullable = _.RequiresNullableAnnotation ? "?" : string.Empty;
			var defaultValue = _.HasExplicitDefaultValue && method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				$" = {_.ExplicitDefaultValue}" : string.Empty;
			var direction = _.RefKind switch
			{
				RefKind.Ref => "ref ",
				RefKind.Out => "out ",
				RefKind.In => "in ",
				_ => string.Empty
			};
			var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.FullyQualifiedName}{requiresNullable} @{_.Name}{defaultValue}";
			var attributes = _.AttributesDescription;
			return $"{(attributes.Length > 0 ? $"{attributes} " : string.Empty)}{parameter}";
		}));
		var isUnsafe = method.IsUnsafe ? "unsafe " : string.Empty;
		var methodSignature =
			$"{isUnsafe}void {explicitTypeNameDescription}{method.Name}({methodParameters})";
		var methodException =
			$"void {explicitTypeNameDescription}{method.Name}({string.Join(", ", method.Parameters.Select(_ => $"{{@{_.Name}}}"))})";

		if (method.AttributesDescription.Length > 0)
		{
			writer.WriteLine(method.AttributesDescription);
		}

		writer.WriteLine($$"""[global::Rocks.MemberIdentifier({{method.MemberIdentifier}}, "{{methodDescription}}")]""");
		var isPublic = method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{method.OverridingCodeValue} " : string.Empty;
		writer.WriteLine($"{isPublic}{(method.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty)}{methodSignature}");

		var constraints = ImmutableArray<string>.Empty;

		if (method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			method.ContainingType.TypeKind == TypeKind.Interface)
		{
			constraints = method.Constraints;
		}

		if (method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ||
			method.RequiresOverride == RequiresOverride.Yes)
		{
			constraints = constraints.AddRange(method.DefaultConstraints);
		}

		if (constraints.Length > 0)
		{
			writer.Indent++;

			foreach (var constraint in constraints)
			{
				writer.WriteLine(constraint);
			}

			writer.Indent--;
		}

		writer.WriteLine("{");
		writer.Indent++;

		foreach (var outParameter in method.Parameters.Where(_ => _.RefKind == RefKind.Out))
		{
			writer.WriteLine($"{outParameter.Name} = default!;");
		}

		var namingContext = new VariableNamingContext(method);

		writer.WriteLine($"if (this.handlers.TryGetValue({method.MemberIdentifier}, out var @{namingContext["methodHandlers"]}))");
		writer.WriteLine("{");
		writer.Indent++;

		if (method.Parameters.Length > 0)
		{
			MockMethodVoidBuilder.BuildMethodValidationHandlerWithParameters(
				writer, method, method.MockType, namingContext,
				methodSignature, raiseEvents, shouldThrowDoesNotReturnException);
		}
		else
		{
			MockMethodVoidBuilder.BuildMethodValidationHandlerNoParameters(
				writer, method, method.MockType, namingContext,
				raiseEvents, shouldThrowDoesNotReturnException);
		}

		writer.Indent--;
		writer.WriteLine("}");

		writer.WriteLine("else");
		writer.WriteLine("{");
		writer.Indent++;

		if (!method.IsAbstract)
		{
			// We'll call the base implementation if an expectation wasn't provided.
			// We'll do this as well for interfaces with a DIM through a shim.
			// If something like this is added in the future, then I'll revisit this:
			// https://github.com/dotnet/csharplang/issues/2337
			// Note that if the method has [DoesNotReturn], we'll throw DoesNotReturnException
			// if the base method didn't throw an exception.
			var index = -1;
			var passedParameter = string.Join(", ", method.Parameters.Select(_ =>
			{
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};

				index++;
				return $"{(method.OverriddenMethod is not null ? method.OverriddenMethod.Parameters[index].Name : _.Name)}: {direction}@{_.Name}!";
			}));
			var target = method.ContainingType.TypeKind == TypeKind.Interface ?
				$"this.shimFor{method.ContainingType.FlattenedName}" : "base";

			if (shouldThrowDoesNotReturnException)
			{
				writer.WriteLine($"{target}.{method.Name}({passedParameter});");
				writer.WriteLine("throw new global::Rocks.Exceptions.DoesNotReturnException();");
			}
			else
			{
				writer.WriteLine($"{target}.{method.Name}({passedParameter});");
			}
		}
		else
		{
			writer.WriteLine($"throw new global::Rocks.Exceptions.ExpectationException(\"No handlers were found for {methodSignature.Replace("\"", "\\\"")}\");");
		}

		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");
		writer.WriteLine();
	}

	private static void BuildMethodValidationHandlerNoParameters(IndentedTextWriter writer,
		MethodModel method, TypeReferenceModel typeToMock, VariableNamingContext namingContext,
		bool raiseEvents, bool shouldThrowDoesNotReturnException)
	{
		writer.WriteLine($"var @{namingContext["methodHandler"]} = @{namingContext["methodHandlers"]}[0];");
		MockMethodVoidBuilder.BuildMethodHandler(writer, method, typeToMock, namingContext, raiseEvents);

		if (shouldThrowDoesNotReturnException)
		{
			writer.WriteLine($"throw new global::Rocks.Exceptions.DoesNotReturnException();");
		}
	}

	private static void BuildMethodValidationHandlerWithParameters(IndentedTextWriter writer, MethodModel method,
		TypeReferenceModel typeToMock, VariableNamingContext namingContext,
		string methodSignature, bool raiseEvents, bool shouldThrowDoesNotReturnException)
	{
		writer.WriteLine($"var @{namingContext["foundMatch"]} = false;");
		writer.WriteLine();
		writer.WriteLine($"foreach (var @{namingContext["methodHandler"]} in @{namingContext["methodHandlers"]})");

		if (method.Parameters.Length > 0)
		{
			writer.WriteLine("{");
			writer.Indent++;
		}

		for (var i = 0; i < method.Parameters.Length; i++)
		{
			var parameter = method.Parameters[i];
			var requiresNullable = parameter.RequiresNullableAnnotation ? "?" : string.Empty;
			var argType = parameter.Type.IsPointer ?
				PointerArgTypeBuilder.GetProjectedFullyQualifiedName(parameter.Type, typeToMock) :
					parameter.Type.IsRefLikeType ?
						RefLikeArgTypeBuilder.GetProjectedFullyQualifiedName(parameter.Type, typeToMock) :
						$"global::Rocks.Argument<{parameter.Type.FullyQualifiedName}{requiresNullable}>";

			if (i == 0)
			{
				writer.WriteLine(
					!parameter.Type.IsBasedOnTypeParameter ?
						$"if ((({argType})@{namingContext["methodHandler"]}.Expectations[{i}]).IsValid(@{parameter.Name}!){(i == method.Parameters.Length - 1 ? ")" : " &&")}" :
						$"if (((@{namingContext["methodHandler"]}.Expectations[{i}] as {argType})?.IsValid(@{parameter.Name}!) ?? false){(i == method.Parameters.Length - 1 ? ")" : " &&")}");
			}
			else
			{
				if (i == 1)
				{
					writer.Indent++;
				}

				writer.WriteLine(
					!parameter.Type.IsBasedOnTypeParameter ?
						$"(({argType})@{namingContext["methodHandler"]}.Expectations[{i}]).IsValid(@{parameter.Name}!){(i == method.Parameters.Length - 1 ? ")" : " &&")}" :
						$"((@{namingContext["methodHandler"]}.Expectations[{i}] as {argType})?.IsValid(@{parameter.Name}!) ?? false){(i == method.Parameters.Length - 1 ? ")" : " &&")}");

				if (i == method.Parameters.Length - 1)
				{
					writer.Indent--;
				}
			}
		}

		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine($"@{namingContext["foundMatch"]} = true;");
		writer.WriteLine();
		MockMethodVoidBuilder.BuildMethodHandler(writer, method, typeToMock, namingContext, raiseEvents);
		writer.WriteLine("break;");
		writer.Indent--;
		writer.WriteLine("}");

		if (method.Parameters.Length > 0)
		{
			writer.Indent--;
			writer.WriteLine("}");
		}

		writer.WriteLine();
		writer.WriteLine($"if (!@{namingContext["foundMatch"]})");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine($"throw new global::Rocks.Exceptions.ExpectationException(\"No handlers match for {methodSignature.Replace("\"", "\\\"")}\");");
		writer.Indent--;
		writer.WriteLine("}");

		if (shouldThrowDoesNotReturnException)
		{
			writer.WriteLine();
			writer.WriteLine("throw new global::Rocks.Exceptions.DoesNotReturnException();");
		}
	}

	internal static void BuildMethodHandler(IndentedTextWriter writer, MethodModel method, TypeReferenceModel typeToMock,
		VariableNamingContext namingContext, bool raiseEvents)
	{
		writer.WriteLine($"@{namingContext["methodHandler"]}.IncrementCallCount();");

		var methodCast = method.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(method, typeToMock) :
			DelegateBuilder.Build(method.Parameters);

		writer.WriteLine(!method.IsGenericMethod ?
			$"if (@{namingContext["methodHandler"]}.Method is not null)" :
			$"if (@{namingContext["methodHandler"]}.Method is not null && @{namingContext["methodHandler"]}.Method is {methodCast} @{namingContext["method"]})");
		writer.WriteLine("{");
		writer.Indent++;

		var methodArguments = method.Parameters.Length == 0 ? string.Empty :
			string.Join(", ", method.Parameters.Select(
				_ => _.RefKind == RefKind.Ref || _.RefKind == RefKind.Out ? $"{(_.RefKind == RefKind.Ref ? "ref" : "out")} @{_.Name}!" : $"@{_.Name}!"));
		writer.WriteLine(!method.IsGenericMethod ?
			$"(({methodCast})@{namingContext["methodHandler"]}.Method)({methodArguments});" :
			$"@{namingContext["method"]}({methodArguments});");

		writer.Indent--;
		writer.WriteLine("}");

		if (raiseEvents)
		{
			writer.WriteLine();
			writer.WriteLine($"@{namingContext["methodHandler"]}.RaiseEvents(this);");
		}
	}
}