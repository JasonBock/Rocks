using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class MockMethodVoidBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type, MethodModel method, 
		bool raiseEvents, string expectationsFullyQualifiedName)
	{
		var shouldThrowDoesNotReturnException = method.ShouldThrowDoesNotReturnException;
		var typeArgumentsNamingContext = method.IsGenericMethod ?
			new TypeArgumentsNamingContext(method) :
			new TypeArgumentsNamingContext();

		var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
		{
			var requiresNullable = _.RequiresNullableAnnotation ? "?" : string.Empty;
			var defaultValue = _.HasExplicitDefaultValue && method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				$" = {_.ExplicitDefaultValue}" : string.Empty;
			var scoped = _.IsParams ? string.Empty :
				_.IsScoped ? "scoped " : string.Empty;
			var direction = _.RefKind switch
			{
				RefKind.Ref => "ref ",
				RefKind.Out => "out ",
				RefKind.In => "in ",
				RefKind.RefReadOnlyParameter => "ref readonly ",
				_ => string.Empty
			};

			var typeName = _.Type.BuildName(typeArgumentsNamingContext);
			var parameter = $"{scoped}{direction}{(_.IsParams ? "params " : string.Empty)}{typeName}{requiresNullable} @{_.Name}{defaultValue}";
			var attributes = _.AttributesDescription;
			return $"{(attributes.Length > 0 ? $"{attributes} " : string.Empty)}{parameter}";
		}));

		var explicitTypeNameDescription = method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
			$"{method.ContainingType.FullyQualifiedName}." : string.Empty;
		var isUnsafe = method.IsUnsafe ? "unsafe " : string.Empty;

		var typeArguments = method.IsGenericMethod ?
			$"<{string.Join(", ", method.TypeArguments.Select(_ => !method.MockType.TypeArguments.Any(m => m.FullyQualifiedName == _.FullyQualifiedName) ? _.FullyQualifiedName : _.BuildName(typeArgumentsNamingContext)))}>" : string.Empty;

		var methodSignature =
			$"{isUnsafe}void {explicitTypeNameDescription}{method.Name}{typeArguments}({methodParameters})";

		if (method.AttributesDescription.Length > 0)
		{
			writer.WriteLine(method.AttributesDescription);
		}

		writer.WriteLine($"[global::Rocks.Runtime.MemberIdentifier({method.MemberIdentifier})]");
		var isPublic = method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{method.OverridingCodeValue} " : string.Empty;
		writer.WriteLine($"{isPublic}{(method.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty)}{methodSignature}");

		var constraints = new List<Constraints>();

		if (method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No &&
			method.ContainingType.TypeKind == TypeKind.Interface)
		{
			constraints.AddRange(method.Constraints);
		}

		if (method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ||
			method.RequiresOverride == RequiresOverride.Yes)
		{
			constraints.AddRange(method.DefaultConstraints);
		}

		if (constraints.Count > 0)
		{
			writer.Indent++;

			foreach (var constraint in constraints)
			{
				writer.WriteLine(constraint.ToString(typeArgumentsNamingContext, method));
			}

			writer.Indent--;
		}

		writer.WriteLine("{");
		writer.Indent++;

		foreach (var outParameter in method.Parameters.Where(_ => _.RefKind == RefKind.Out))
		{
			writer.WriteLine($"{outParameter.Name} = default!;");
		}

		var namingContext = new VariablesNamingContext(method);

		writer.WriteLine($"if (this.{type.ExpectationsPropertyName}.handlers{method.MemberIdentifier} is not null)");
		writer.WriteLine("{");
		writer.Indent++;

		if (method.Parameters.Length > 0)
		{
			MockMethodVoidBuilder.BuildMethodValidationHandlerWithParameters(
				writer, type, method, namingContext, typeArgumentsNamingContext,
				raiseEvents, shouldThrowDoesNotReturnException,
				expectationsFullyQualifiedName);
		}
		else
		{
			MockMethodVoidBuilder.BuildMethodValidationHandlerNoParameters(
				writer, type, method, namingContext, typeArgumentsNamingContext,
				raiseEvents, shouldThrowDoesNotReturnException,
				expectationsFullyQualifiedName);
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
			var passedParameter = string.Join(", ", method.Parameters.Select(_ =>
			{
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};

				return $"@{_.Name}: {direction}@{_.Name}!";
			}));
			var target = method.ContainingType.TypeKind == TypeKind.Interface ?
				$"this.shimFor{method.ContainingType.FlattenedName}" : "base";

			if (shouldThrowDoesNotReturnException)
			{
				writer.WriteLine($"{target}.{method.Name}{typeArguments}({passedParameter});");
				writer.WriteLine("throw new global::Rocks.Runtime.Exceptions.DoesNotReturnException();");
			}
			else
			{
				writer.WriteLine($"{target}.{method.Name}{typeArguments}({passedParameter});");
			}
		}
		else
		{
			ExpectationExceptionBuilder.Build(
				writer, method, "No handlers were found for", method.MemberIdentifier);
		}

		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");
		writer.WriteLine();
	}

	private static void BuildMethodValidationHandlerNoParameters(IndentedTextWriter writer, TypeMockModel type, MethodModel method, 
		VariablesNamingContext namingContext, TypeArgumentsNamingContext typeArgumentsNamingContext,
		bool raiseEvents, bool shouldThrowDoesNotReturnException, string expectationsFullyQualifiedName)
	{
		var foreachHandlerName = method.IsGenericMethod ?
			namingContext["genericHandler"] : namingContext["handler"];

		writer.WriteLine($"var @{foreachHandlerName} = this.{type.ExpectationsPropertyName}.handlers{method.MemberIdentifier}.First;");

		if (method.IsGenericMethod)
		{
			// We'll cast the handler to ensure the general handler type is of the
			// closed generic handler type.
			writer.WriteLine($"if (@{foreachHandlerName} is {expectationsFullyQualifiedName}.Handler{method.MemberIdentifier}<{string.Join(", ", method.TypeArguments.Select(_ => _.BuildName(typeArgumentsNamingContext)))}> @{namingContext["handler"]})");
			writer.WriteLine("{");
			writer.Indent++;
		}

		MockMethodVoidBuilder.BuildMethodHandler(writer, method, namingContext, raiseEvents);

		if (method.IsGenericMethod)
		{
			writer.Indent--;
			writer.WriteLines(
				$$"""
				}
				else
				{
				""");

			ExpectationExceptionBuilder.Build(
				writer, method, "The provided handler does not match for", method.MemberIdentifier);

			writer.WriteLine("}");
		}

		if (shouldThrowDoesNotReturnException)
		{
			writer.WriteLine($"throw new global::Rocks.Runtime.Exceptions.DoesNotReturnException();");
		}
	}

	private static void BuildMethodValidationHandlerWithParameters(IndentedTextWriter writer, TypeMockModel type, MethodModel method,
		VariablesNamingContext namingContext, TypeArgumentsNamingContext typeArgumentsNamingContext,
		bool raiseEvents, bool shouldThrowDoesNotReturnException, string expectationsFullyQualifiedName)
	{
		writer.WriteLine($"var @{namingContext["foundMatch"]} = false;");
		writer.WriteLine();

		var foreachHandlerName = method.IsGenericMethod ?
			namingContext["genericHandler"] : namingContext["handler"];

		writer.WriteLine($"foreach (var @{foreachHandlerName} in this.{type.ExpectationsPropertyName}.handlers{method.MemberIdentifier})");

		if (method.Parameters.Length > 0)
		{
			writer.WriteLine("{");
			writer.Indent++;
		}

		if (method.IsGenericMethod)
		{
			// We'll cast the handler to ensure the general handler type is of the
			// closed generic handler type.
			writer.WriteLine($"if (@{foreachHandlerName} is {expectationsFullyQualifiedName}.Handler{method.MemberIdentifier}<{string.Join(", ", method.TypeArguments.Select(_ => _.BuildName(typeArgumentsNamingContext)))}> @{namingContext["handler"]})");
			writer.WriteLine("{");
			writer.Indent++;
		}

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
		writer.WriteLine($"@{namingContext["foundMatch"]} = true;");
		MockMethodVoidBuilder.BuildMethodHandler(writer, method, namingContext, raiseEvents);
		writer.WriteLine("break;");
		writer.Indent--;
		writer.WriteLine("}");

		if (method.IsGenericMethod)
		{
			writer.Indent--;
			writer.WriteLine("}");
		}

		if (method.Parameters.Length > 0)
		{
			writer.Indent--;
			writer.WriteLine("}");
		}

		writer.WriteLine();
		writer.WriteLine($"if (!@{namingContext["foundMatch"]})");
		writer.WriteLine("{");
		writer.Indent++;
		ExpectationExceptionBuilder.Build(
			writer, method, "No handlers match for", method.MemberIdentifier);
		writer.Indent--;
		writer.WriteLine("}");

		if (shouldThrowDoesNotReturnException)
		{
			writer.WriteLine();
			writer.WriteLine("throw new global::Rocks.Runtime.Exceptions.DoesNotReturnException();");
		}
	}

	internal static void BuildMethodHandler(IndentedTextWriter writer, MethodModel method,
		VariablesNamingContext namingContext, bool raiseEvents)
	{
		writer.WriteLine($"@{namingContext["handler"]}.CallCount++;");

		var methodArguments = method.Parameters.Length == 0 ? string.Empty :
			string.Join(", ", method.Parameters.Select(
				_ => _.RefKind == RefKind.Ref || _.RefKind == RefKind.Out ? $"{(_.RefKind == RefKind.Ref ? "ref" : "out")} @{_.Name}!" : $"@{_.Name}!"));
		writer.WriteLine($"@{namingContext["handler"]}.Callback?.Invoke({methodArguments});");

		if (raiseEvents)
		{
			writer.WriteLine($"@{namingContext["handler"]}.RaiseEvents(this);");
		}
	}
}