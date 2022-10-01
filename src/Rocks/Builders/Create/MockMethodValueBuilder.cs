using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockMethodValueBuilder
{
	internal static void Build(IndentedTextWriter writer, MethodMockableResult result, bool raiseEvents,
		Compilation compilation)
	{
		var method = result.Value;
		var shouldThrowDoesNotReturnException = method.IsMarkedWithDoesNotReturn(compilation);

		var returnByRef = method.ReturnsByRef ? "ref " : method.ReturnsByRefReadonly ? "ref readonly " : string.Empty;
		var returnType = $"{returnByRef}{method.ReturnType.GetReferenceableName()}";
		var parametersDescription = string.Join(", ", method.Parameters.Select(_ =>
		{
			var direction = _.RefKind switch
			{
				RefKind.Ref => "ref ",
				RefKind.Out => "out ",
				RefKind.In => "in ",
				_ => string.Empty
			};
			return $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetReferenceableName()} {_.Name}";
		}));
		var explicitTypeNameDescription = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
			$"{method.ContainingType.GetReferenceableName()}." : string.Empty;
		var methodDescription = $"{returnType} {explicitTypeNameDescription}{method.GetName()}({parametersDescription})";

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
			var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetReferenceableName()} {_.Name}{defaultValue}";
			return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription(compilation)} " : string.Empty)}{parameter}";
		}));
		var isUnsafe = method.IsUnsafe() ? "unsafe " : string.Empty;
		var methodSignature =
			$"{isUnsafe}{returnType} {explicitTypeNameDescription}{method.GetName()}({methodParameters})";
		var methodException =
			$"{returnType} {explicitTypeNameDescription}{method.GetName()}({string.Join(", ", method.Parameters.Select(_ => $"{{{_.Name}}}"))})";

		var attributes = method.GetAttributes();

		if (attributes.Length > 0)
		{
			writer.WriteLine(attributes.GetDescription(compilation));
		}

		var returnAttributes = method.GetReturnTypeAttributes();

		if (returnAttributes.Length > 0)
		{
			writer.WriteLine(returnAttributes.GetDescription(compilation, AttributeTargets.ReturnValue));
		}

		writer.WriteLine($@"[global::Rocks.MemberIdentifier({result.MemberIdentifier}, ""{methodDescription}"")]");
		var isPublic = result.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{result.Value.DeclaredAccessibility.GetOverridingCodeValue()} " : string.Empty;
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

		var namingContext = new VariableNamingContext(method);

		writer.WriteLine($"if (this.handlers.TryGetValue({result.MemberIdentifier}, out var {namingContext["methodHandlers"]}))");
		writer.WriteLine("{");
		writer.Indent++;

		if (method.Parameters.Length > 0)
		{
			MockMethodValueBuilder.BuildMethodValidationHandlerWithParameters(
				writer, method, result.MockType, namingContext,
				raiseEvents, shouldThrowDoesNotReturnException, result.MemberIdentifier);
		}
		else
		{
			MockMethodValueBuilder.BuildMethodValidationHandlerNoParameters(
				writer, method, result.MockType, namingContext,
				raiseEvents, shouldThrowDoesNotReturnException, result.MemberIdentifier);
		}

		if (method.Parameters.Length > 0)
		{
			writer.WriteLine();
			writer.WriteLine($"throw new global::Rocks.Exceptions.ExpectationException(\"No handlers match for {methodSignature.Replace("\"", "\\\"")}\");");
		}

		writer.Indent--;
		writer.WriteLine("}");

		if (!method.IsAbstract)
		{
			writer.WriteLine("else");
			writer.WriteLine("{");
			writer.Indent++;

			// We'll call the base implementation if an expectation wasn't provided.
			// We'll do this as well for interfaces with a DIM through a shim.
			// If something like this is added in the future, then I'll revisit this:
			// https://github.com/dotnet/csharplang/issues/2337
			// Note that if the method has [DoesNotReturn], calling the base method
			// and returning its' value should not trip a compiler warning,
			// as the base method is responsible for not returning.
			var passedParameter = string.Join(", ", method.Parameters.Select(_ =>
			{
				var direction = _.RefKind switch
				{
					RefKind.Ref => "ref ",
					RefKind.Out => "out ",
					RefKind.In => "in ",
					_ => string.Empty
				};
				return $"{direction}{_.Name}";
			}));
			var target = method.ContainingType.TypeKind == TypeKind.Interface ?
				$"this.shimFor{method.ContainingType.GetName(TypeNameOption.Flatten)}" : "base";
			writer.WriteLine($"return {target}.{method.GetName()}({passedParameter});");

			writer.Indent--;
			writer.WriteLine("}");
		}
		else
		{
			writer.WriteLine();
			writer.WriteLine($"throw new global::Rocks.Exceptions.ExpectationException(\"No handlers were found for {methodSignature.Replace("\"", "\\\"")}\");");
		}

		writer.Indent--;
		writer.WriteLine("}");
		writer.WriteLine();
	}

	internal static void BuildMethodHandler(IndentedTextWriter writer, IMethodSymbol method, ITypeSymbol typeToMock,
		VariableNamingContext namingContext, bool raiseEvents, bool shouldThrowDoesNotReturnException, uint memberIndentifier)
	{
		var methodCast = method.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(method, typeToMock) :
			DelegateBuilder.Build(method.Parameters, method.ReturnType);

		if (method.ReturnsByRef || method.ReturnsByRefReadonly)
		{
			writer.WriteLine(
				method.ReturnType.TypeKind != TypeKind.TypeParameter ?
					$"this.rr{memberIndentifier} = {namingContext["methodHandler"]}.Method is not null ?" :
					$"this.rr{memberIndentifier} = {namingContext["methodHandler"]}.Method is not null && {namingContext["methodHandler"]}.Method is {methodCast} {namingContext["methodReturn"]} ? ");
		}
		else
		{
			if (shouldThrowDoesNotReturnException)
			{
				writer.WriteLine(
					method.ReturnType.TypeKind != TypeKind.TypeParameter ?
						$"_ = {namingContext["methodHandler"]}.Method is not null ?" :
						$"_ = {namingContext["methodHandler"]}.Method is not null && {namingContext["methodHandler"]}.Method is {methodCast} {namingContext["methodReturn"]} ?");
			}
			else
			{
				writer.WriteLine(
					method.ReturnType.TypeKind != TypeKind.TypeParameter ?
						$"var {namingContext["result"]} = {namingContext["methodHandler"]}.Method is not null ?" :
						$"var {namingContext["result"]} = {namingContext["methodHandler"]}.Method is not null && {namingContext["methodHandler"]}.Method is {methodCast} {namingContext["methodReturn"]} ?");
			}
		}

		writer.Indent++;

		var methodArguments = method.Parameters.Length == 0 ? string.Empty :
			string.Join(", ", method.Parameters.Select(
				_ => _.RefKind == RefKind.Ref || _.RefKind == RefKind.Out ? $"{(_.RefKind == RefKind.Ref ? "ref" : "out")} {_.Name}" : _.Name));
		var methodReturnType = method.ReturnType.IsRefLikeType ?
			MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(method, typeToMock) : method.ReturnType.GetReferenceableName();
		var handlerName = method.ReturnType.IsPointer() ?
			MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationFullyQualifiedNameName(method.ReturnType, typeToMock) :
			$"global::Rocks.HandlerInformation<{methodReturnType}>";

		writer.WriteLine(
			method.ReturnType.TypeKind != TypeKind.TypeParameter ?
				$"global::System.Runtime.CompilerServices.Unsafe.As<{methodCast}>({namingContext["methodHandler"]}.Method)({methodArguments}) :" :
				$"{namingContext["methodReturn"]}({methodArguments}) :");

		if (method.ReturnType.IsPointer() || !method.ReturnType.IsRefLikeType)
		{
			if (method.ReturnType.TypeKind != TypeKind.TypeParameter)
			{
				writer.WriteLine($"global::System.Runtime.CompilerServices.Unsafe.As<{handlerName}>({namingContext["methodHandler"]}).ReturnValue;");
			}
			else
			{
				writer.WriteLines(
					$$"""
					{{namingContext["methodHandler"]}} is {{handlerName}} {{namingContext["returnValue"]}} ?
						{{namingContext["returnValue"]}}.ReturnValue :
						throw new global::Rocks.Exceptions.MockException($"No return value could be obtained for {{method.ReturnType.Name}} of type {typeof({{method.ReturnType.Name}}).FullName}.");
					"""
				);
			}
		}
		else
		{
			if (method.ReturnType.TypeKind != TypeKind.TypeParameter)
			{
				writer.WriteLine($"global::System.Runtime.CompilerServices.Unsafe.As<{handlerName}>({namingContext["methodHandler"]}).ReturnValue!.Invoke();");
			}
			else
			{
				writer.WriteLines(
					$$"""
					{{namingContext["methodHandler"]}} is {{handlerName}} {{namingContext["returnValue"]}} ?
						{{namingContext["returnValue"]}}.ReturnValue!.Invoke() :
						throw new global::Rocks.Exceptions.MockException($"No return value could be obtained for {{method.ReturnType.Name}} of type {typeof({{method.ReturnType.Name}}).FullName}.");
					"""
				);
			}
		}

		writer.Indent--;

		if (raiseEvents)
		{
			writer.WriteLine($"{namingContext["methodHandler"]}.RaiseEvents(this);");
		}

		writer.WriteLine($"{namingContext["methodHandler"]}.IncrementCallCount();");

		if (shouldThrowDoesNotReturnException)
		{
			writer.WriteLine($"throw new global::Rocks.Exceptions.DoesNotReturnException();");
		}
		else
		{
			if (method.ReturnsByRef || method.ReturnsByRefReadonly)
			{
				writer.WriteLine($"return ref this.rr{memberIndentifier};");
			}
			else
			{
				writer.WriteLine($"return {namingContext["result"]}!;");
			}
		}
	}

	private static void BuildMethodValidationHandlerWithParameters(IndentedTextWriter writer,
		IMethodSymbol method, ITypeSymbol typeToMock, VariableNamingContext namingContext, 
		bool raiseEvents, bool shouldThrowDoesNotReturnException, uint memberIdentifier)
	{
		writer.WriteLine($"foreach (var {namingContext["methodHandler"]} in {namingContext["methodHandlers"]})");
		writer.WriteLine("{");
		writer.Indent++;

		for (var i = 0; i < method.Parameters.Length; i++)
		{
			var parameter = method.Parameters[i];
			var argType = parameter.Type.IsPointer() ?
				PointerArgTypeBuilder.GetProjectedFullyQualifiedName(parameter.Type, typeToMock) :
					parameter.Type.IsRefLikeType ?
						RefLikeArgTypeBuilder.GetProjectedFullyQualifiedName(parameter.Type, typeToMock) :
						$"global::Rocks.Argument<{parameter.Type.GetReferenceableName()}>";

			if (i == 0)
			{
				writer.WriteLine(
					parameter.Type.TypeKind != TypeKind.TypeParameter ?
						$"if (global::System.Runtime.CompilerServices.Unsafe.As<{argType}>({namingContext["methodHandler"]}.Expectations[{i}]).IsValid({parameter.Name}){(i == method.Parameters.Length - 1 ? ")" : " &&")}" :
						$"if ((({namingContext["methodHandler"]}.Expectations[{i}] as {argType})?.IsValid({parameter.Name}) ?? false){(i == method.Parameters.Length - 1 ? ")" : " &&")}");
			}
			else
			{
				if (i == 1)
				{
					writer.Indent++;
				}

				writer.WriteLine(
					parameter.Type.TypeKind != TypeKind.TypeParameter ?
						$"global::System.Runtime.CompilerServices.Unsafe.As<{argType}>({namingContext["methodHandler"]}.Expectations[{i}]).IsValid({parameter.Name}){(i == method.Parameters.Length - 1 ? ")" : " &&")}" :
						$"(({namingContext["methodHandler"]}.Expectations[{i}] as {argType})?.IsValid({parameter.Name}) ?? false){(i == method.Parameters.Length - 1 ? ")" : " &&")}");

				if (i == method.Parameters.Length - 1)
				{
					writer.Indent--;
				}
			}
		}

		writer.WriteLine("{");
		writer.Indent++;

		MockMethodValueBuilder.BuildMethodHandler(
			writer, method, typeToMock, namingContext, raiseEvents, shouldThrowDoesNotReturnException, memberIdentifier);
		writer.Indent--;
		writer.WriteLine("}");

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildMethodValidationHandlerNoParameters(IndentedTextWriter writer, IMethodSymbol method,
		ITypeSymbol typeToMock, VariableNamingContext namingContext, bool raiseEvents, bool shouldThrowDoesNotReturnException, uint memberIdentifier)
	{
		writer.WriteLine($"var {namingContext["methodHandler"]} = {namingContext["methodHandlers"]}[0];");
		MockMethodValueBuilder.BuildMethodHandler(
			writer, method, typeToMock, namingContext, raiseEvents, shouldThrowDoesNotReturnException, memberIdentifier);
	}
}