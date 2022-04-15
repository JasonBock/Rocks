using Microsoft.CodeAnalysis;
using Rocks.Exceptions;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockMethodVoidBuilder
{
	internal static void Build(IndentedTextWriter writer, MethodMockableResult result, bool raiseEvents,
		Compilation compilation)
	{
		var method = result.Value;
		var parametersDescription = string.Join(", ", method.Parameters.Select(_ =>
		{
			var direction = _.RefKind switch
			{
				RefKind.Ref => "ref ",
				RefKind.Out => "out ",
				RefKind.In => "in ",
				_ => string.Empty
			};
			return $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetName()} {_.Name}";
		}));
		var explicitTypeNameDescription = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
			$"{method.ContainingType.GetName(TypeNameOption.NoGenerics)}." : string.Empty;
		var methodDescription = $"void {explicitTypeNameDescription}{method.GetName()}({parametersDescription})";

		var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
		{
			var defaultValue = _.HasExplicitDefaultValue ? $" = {_.ExplicitDefaultValue.GetDefaultValue(_.Type.IsValueType)}" : string.Empty;
			var direction = _.RefKind switch
			{
				RefKind.Ref => "ref ",
				RefKind.Out => "out ",
				RefKind.In => "in ",
				_ => string.Empty
			};
			var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetName()} {_.Name}{defaultValue}";
			return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription(compilation)} " : string.Empty)}{parameter}";
		}));
		var isUnsafe = method.IsUnsafe() ? "unsafe " : string.Empty;
		var methodSignature =
			$"{isUnsafe}void {explicitTypeNameDescription}{method.GetName()}({methodParameters})";
		var methodException =
			$"void {explicitTypeNameDescription}{method.GetName()}({string.Join(", ", method.Parameters.Select(_ => $"{{{_.Name}}}"))})";

		var attributes = method.GetAttributes();

		if (attributes.Length > 0)
		{
			writer.WriteLine(attributes.GetDescription(compilation));
		}

		writer.WriteLine($@"[MemberIdentifier({result.MemberIdentifier}, ""{methodDescription}"")]");
		var isPublic = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			"public " : string.Empty;
		writer.WriteLine($"{isPublic}{(result.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty)}{methodSignature}");

		var constraints = method.GetConstraints();

		if (result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ||
			result.RequiresOverride == RequiresOverride.Yes)
		{
			constraints = constraints.AddRange(method.GetDefaultConstraints());
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

		writer.WriteLine($"if (this.handlers.TryGetValue({result.MemberIdentifier}, out var methodHandlers))");
		writer.WriteLine("{");
		writer.Indent++;

		if (method.Parameters.Length > 0)
		{
			MockMethodVoidBuilder.BuildMethodValidationHandlerWithParameters(writer, method, methodSignature, raiseEvents);
		}
		else
		{
			MockMethodVoidBuilder.BuildMethodValidationHandlerNoParameters(writer, method, raiseEvents);
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
			var passedParameter = string.Join(", ", method.Parameters.Select(_ =>
			{
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Name}";
			}));
			var target = method.ContainingType.TypeKind == TypeKind.Interface ?
				$"this.shimFor{method.ContainingType.GetName(TypeNameOption.Flatten)}" : "base";
			writer.WriteLine($"{target}.{method.GetName()}({passedParameter});");
		}
		else
		{
			writer.WriteLine($"throw new {nameof(ExpectationException)}(\"No handlers were found for {methodSignature.Replace("\"", "\\\"")}\");");
		}

		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");
		writer.WriteLine();
	}

	private static void BuildMethodValidationHandlerNoParameters(IndentedTextWriter writer, IMethodSymbol method, bool raiseEvents)
	{
		writer.WriteLine("var methodHandler = methodHandlers[0];");
		MockMethodVoidBuilder.BuildMethodHandler(writer, method, raiseEvents);
	}

	private static void BuildMethodValidationHandlerWithParameters(IndentedTextWriter writer, IMethodSymbol method,
		string methodSignature, bool raiseEvents)
	{
		writer.WriteLine("var foundMatch = false;");
		writer.WriteLine();
		writer.WriteLine("foreach (var methodHandler in methodHandlers)");
		writer.WriteLine("{");
		writer.Indent++;

		for (var i = 0; i < method.Parameters.Length; i++)
		{
			var parameter = method.Parameters[i];
			var argType = parameter.Type.IsPointer() ? PointerArgTypeBuilder.GetProjectedName(parameter.Type) :
				parameter.Type.IsRefLikeType ? RefLikeArgTypeBuilder.GetProjectedName(parameter.Type) :
				$"{nameof(Argument)}<{parameter.Type.GetName()}>";

			if (i == 0)
			{
				writer.WriteLine(
					$"if (((methodHandler.{WellKnownNames.Expectations}[{i}] as {argType})?.IsValid({parameter.Name}) ?? false){(i == method.Parameters.Length - 1 ? ")" : " &&")}");
			}
			else
			{
				if (i == 1)
				{
					writer.Indent++;
				}

				writer.WriteLine(
					$"((methodHandler.{WellKnownNames.Expectations}[{i}] as {argType})?.IsValid({parameter.Name}) ?? false){(i == method.Parameters.Length - 1 ? ")" : " &&")}");

				if (i == method.Parameters.Length - 1)
				{
					writer.Indent--;
				}
			}
		}

		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("foundMatch = true;");
		writer.WriteLine();
		MockMethodVoidBuilder.BuildMethodHandler(writer, method, raiseEvents);
		writer.WriteLine("break;");
		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");

		writer.WriteLine();
		writer.WriteLine("if (!foundMatch)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine($"throw new {nameof(ExpectationException)}(\"No handlers match for {methodSignature.Replace("\"", "\\\"")})\");");
		writer.Indent--;
		writer.WriteLine("}");
	}

	internal static void BuildMethodHandler(IndentedTextWriter writer, IMethodSymbol method, bool raiseEvents)
	{
		writer.WriteLine("if (methodHandler.Method is not null)");
		writer.WriteLine("{");
		writer.Indent++;

		var methodCast = method.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedDelegateName(method) :
			DelegateBuilder.Build(method.Parameters);
		var methodArguments = method.Parameters.Length == 0 ? string.Empty :
			string.Join(", ", method.Parameters.Select(
				_ => _.RefKind == RefKind.Ref || _.RefKind == RefKind.Out ? $"{(_.RefKind == RefKind.Ref ? "ref" : "out")} {_.Name}" : _.Name));
		writer.WriteLine($"(({methodCast})methodHandler.Method)({methodArguments});");

		writer.Indent--;
		writer.WriteLine("}");
		writer.WriteLine();

		if (raiseEvents)
		{
			writer.WriteLine($"methodHandler.{nameof(HandlerInformation.RaiseEvents)}(this);");
		}

		writer.WriteLine($"methodHandler.{nameof(HandlerInformation.IncrementCallCount)}();");
	}
}