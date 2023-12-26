using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockMethodValueBuilderV4
{
	internal static void Build(IndentedTextWriter writer, MethodModel method, bool raiseEvents)
	{
		var shouldThrowDoesNotReturnException = method.ShouldThrowDoesNotReturnException;

		var returnByRef = method.ReturnsByRef ? "ref " : method.ReturnsByRefReadOnly ? "ref readonly " : string.Empty;
		var returnType = $"{returnByRef}{method.ReturnType.FullyQualifiedName}";
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
		var methodDescription = $"{returnType} {explicitTypeNameDescription}{method.Name}({parametersDescription})";

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
			$"{isUnsafe}{returnType} {explicitTypeNameDescription}{method.Name}({methodParameters})";
		var methodException =
			$"{returnType} {explicitTypeNameDescription}{method.Name}({string.Join(", ", method.Parameters.Select(_ => $"{{@{_.Name}}}"))})";

		var attributes = method.AttributesDescription;

		if (method.AttributesDescription.Length > 0)
		{
			writer.WriteLine(method.AttributesDescription);
		}

		if (method.ReturnTypeAttributesDescription.Length > 0)
		{
			writer.WriteLine(method.ReturnTypeAttributesDescription);
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
			writer.WriteLine($"@{outParameter.Name} = default!;");
		}

		var namingContext = new VariableNamingContext(method);

		writer.WriteLine($"if (this.expectations.handlers{method.MemberIdentifier}.Count > 0)");
		writer.WriteLine("{");
		writer.Indent++;

		if (method.Parameters.Length > 0 || method.IsGenericMethod)
		{
			MockMethodValueBuilderV4.BuildMethodValidationHandlerWithParameters(
				writer, method, method.MockType, namingContext,
				raiseEvents, shouldThrowDoesNotReturnException, method.MemberIdentifier);
		}
		else
		{
			MockMethodValueBuilderV4.BuildMethodValidationHandlerNoParameters(
				writer, method, method.MockType, namingContext,
				raiseEvents, shouldThrowDoesNotReturnException, method.MemberIdentifier);
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
			// Note that if the method has [DoesNotReturn], we'll disregard
			// the return value and throw DoesNotReturnException
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

				return $"{_.Name}: {direction}@{_.Name}!";
			}));
			var target = method.ContainingType.TypeKind == TypeKind.Interface ?
				$"this.shimFor{method.ContainingType.FlattenedName}" : "base";

			if (shouldThrowDoesNotReturnException)
			{
				writer.WriteLine($"_ = {target}.{method.Name}({passedParameter});");
				writer.WriteLine("throw new global::Rocks.Exceptions.DoesNotReturnException();");
			}
			else
			{
				writer.WriteLine($"return {target}.{method.Name}({passedParameter});");
			}

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

	internal static void BuildMethodHandler(IndentedTextWriter writer, MethodModel method, TypeReferenceModel typeToMock,
		VariableNamingContext namingContext, bool raiseEvents, bool shouldThrowDoesNotReturnException, uint memberIndentifier)
	{
		writer.WriteLine($"@{namingContext["handler"]}.CallCount++;");

		var methodCast = method.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(method, typeToMock) :
			DelegateBuilder.Build(method.Parameters, method.ReturnType);

		if (method.ReturnsByRef || method.ReturnsByRefReadOnly)
		{
			writer.WriteLine(
				!method.ReturnType.IsBasedOnTypeParameter ?
					$"this.rr{memberIndentifier} = @{namingContext["handler"]}.Callback is not null ?" :
					$"this.rr{memberIndentifier} = @{namingContext["handler"]}.Callback is not null && @{namingContext["handler"]}.Method is {methodCast} @{namingContext["methodReturn"]} ? ");
		}
		else
		{
			if (shouldThrowDoesNotReturnException)
			{
				writer.WriteLine(
					!method.ReturnType.IsBasedOnTypeParameter ?
						$"_ = @{namingContext["handler"]}.Callback is not null ?" :
						$"_ = @{namingContext["handler"]}.Callback is not null && @{namingContext["handler"]}.Method is {methodCast} @{namingContext["methodReturn"]} ?");
			}
			else
			{
				writer.WriteLine(
					!method.ReturnType.IsBasedOnTypeParameter ?
						$"var @{namingContext["result"]} = @{namingContext["handler"]}.Callback is not null ?" :
						$"var @{namingContext["result"]} = @{namingContext["handler"]}.Callback is not null && @{namingContext["handler"]}.Method is {methodCast} @{namingContext["methodReturn"]} ?");
			}
		}

		writer.Indent++;

		var methodArguments = method.Parameters.Length == 0 ? string.Empty :
			string.Join(", ", method.Parameters.Select(
				_ => _.RefKind == RefKind.Ref || _.RefKind == RefKind.Out ? $"{(_.RefKind == RefKind.Ref ? "ref" : "out")} @{_.Name}!" : $"@{_.Name}!"));
		var methodReturnType = method.ReturnType.IsRefLikeType ?
			MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(method, typeToMock) : method.ReturnType.FullyQualifiedName;
		var handlerName = method.ReturnType.IsPointer ?
			MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationFullyQualifiedNameName(method.ReturnType, typeToMock) :
			$"global::Rocks.HandlerInformation<{methodReturnType}>";

		writer.WriteLine(
			!method.ReturnType.IsBasedOnTypeParameter ?
				$"@{namingContext["handler"]}.Callback({methodArguments}) :" :
				$"@{namingContext["methodReturn"]}({methodArguments}) :");

		if (method.ReturnType.IsPointer || !method.ReturnType.IsRefLikeType)
		{
			if (!method.ReturnType.IsBasedOnTypeParameter)
			{
				writer.WriteLine($"@{namingContext["handler"]}.ReturnValue;");
			}
			else
			{
				writer.WriteLines(
					$$"""
					@{{namingContext["handler"]}} is {{handlerName}} @{{namingContext["returnValue"]}} ?
						@{{namingContext["returnValue"]}}.ReturnValue :
						throw new global::Rocks.Exceptions.NoReturnValueException("No return value could be obtained for {{method.ReturnType.FullyQualifiedName}}.");
					"""
				);
			}
		}
		else
		{
			if (!method.ReturnType.IsBasedOnTypeParameter)
			{
				writer.WriteLine($"@{namingContext["handler"]}.ReturnValue!.Invoke();");
			}
			else
			{
				writer.WriteLines(
					$$"""
					@{{namingContext["handler"]}} is {{handlerName}} @{{namingContext["returnValue"]}} ?
						@{{namingContext["returnValue"]}}.ReturnValue!.Invoke() :
						throw new global::Rocks.Exceptions.NoReturnValueException("No return value could be obtained for {{method.ReturnType.FullyQualifiedName}}.");
					"""
				);
			}
		}

		writer.Indent--;

		if (raiseEvents)
		{
			writer.WriteLine($"@{namingContext["handler"]}.RaiseEvents(this);");
		}

		if (shouldThrowDoesNotReturnException)
		{
			writer.WriteLine($"throw new global::Rocks.Exceptions.DoesNotReturnException();");
		}
		else
		{
			if (method.ReturnsByRef || method.ReturnsByRefReadOnly)
			{
				writer.WriteLine($"return ref this.rr{memberIndentifier};");
			}
			else
			{
				writer.WriteLine($"return @{namingContext["result"]}!;");
			}
		}
	}

	private static void BuildMethodValidationHandlerWithParameters(IndentedTextWriter writer,
		MethodModel method, TypeReferenceModel typeToMock, VariableNamingContext namingContext,
		bool raiseEvents, bool shouldThrowDoesNotReturnException, uint memberIdentifier)
	{
		writer.WriteLine($"foreach (var @{namingContext["handler"]} in this.expectations.handlers{method.MemberIdentifier})");
		writer.WriteLine("{");
		writer.Indent++;

		if (method.IsGenericMethod)
		{
			var methodCast = method.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(method, typeToMock) :
				DelegateBuilder.Build(method.Parameters, method.ReturnType);
			var methodReturnType = method.ReturnType.IsRefLikeType ?
				MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(method, typeToMock) : method.ReturnType.FullyQualifiedName;
			var handlerName = method.ReturnType.IsPointer ?
				MockProjectedTypesAdornmentsBuilder.GetProjectedHandlerInformationFullyQualifiedNameName(method.ReturnType, typeToMock) :
				$"global::Rocks.HandlerInformation<{methodReturnType}>";
			writer.WriteLine($"if ((@{namingContext["handler"]}.Method is not null && @{namingContext["handler"]}.Callback is {methodCast}) || @{namingContext["handler"]} is {handlerName})");
			writer.WriteLine("{");
			writer.Indent++;
		}

		var handlerNamingContext = HandlerVariableNamingContextV4.Create();

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
						$"if ((@{namingContext["handler"]}.{handlerNamingContext[parameter.Name]}.IsValid(@{parameter.Name}!){(i == method.Parameters.Length - 1 ? ")" : " &&")}" :
						$"if (((@{namingContext["handler"]}.{handlerNamingContext[parameter.Name]} as {argType})?.IsValid(@{parameter.Name}!) ?? false){(i == method.Parameters.Length - 1 ? ")" : " &&")}");
			}
			else
			{
				if (i == 1)
				{
					writer.Indent++;
				}

				writer.WriteLine(
					!parameter.Type.IsBasedOnTypeParameter ?
						$"@{namingContext["handler"]}.{handlerNamingContext[parameter.Name]}.IsValid(@{parameter.Name}!){(i == method.Parameters.Length - 1 ? ")" : " &&")}" :
						$"((@{namingContext["handler"]}.{handlerNamingContext[parameter.Name]} as {argType})?.IsValid(@{parameter.Name}!) ?? false){(i == method.Parameters.Length - 1 ? ")" : " &&")}");

				if (i == method.Parameters.Length - 1)
				{
					writer.Indent--;
				}
			}
		}

		writer.WriteLine("{");
		writer.Indent++;

		MockMethodValueBuilderV4.BuildMethodHandler(
			writer, method, typeToMock, namingContext, raiseEvents, shouldThrowDoesNotReturnException, memberIdentifier);
		writer.Indent--;
		writer.WriteLine("}");

		if (method.IsGenericMethod)
		{
			writer.Indent--;
			writer.WriteLine("}");
		}

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildMethodValidationHandlerNoParameters(IndentedTextWriter writer, MethodModel method,
		TypeReferenceModel typeToMock, VariableNamingContext namingContext,
		bool raiseEvents, bool shouldThrowDoesNotReturnException, uint memberIdentifier)
	{
		writer.WriteLine($"var @{namingContext["handler"]} = this.expectations.handlers{method.MemberIdentifier}[0];");
		MockMethodValueBuilderV4.BuildMethodHandler(
			writer, method, typeToMock, namingContext, raiseEvents, shouldThrowDoesNotReturnException, memberIdentifier);
	}
}