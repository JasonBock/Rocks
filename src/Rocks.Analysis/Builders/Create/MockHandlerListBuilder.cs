﻿using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class MockHandlerListBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
	{
		MockHandlerListBuilder.BuildMethodHandlerTypes(writer, mockType, expectationsFullyQualifiedName);
		MockHandlerListBuilder.BuildPropertyHandlerTypes(writer, mockType, expectationsFullyQualifiedName);
	}

	private static void BuildHandler(IndentedTextWriter writer, MethodModel method, uint memberIdentifier, string expectationsFullyQualifiedName)
	{
		var typeArgumentsNamingContext = method.IsGenericMethod ?
			new TypeArgumentsNamingContext(method) :
			new TypeArgumentsNamingContext();
		var typeArguments = method.IsGenericMethod ?
			$"<{string.Join(", ", method.TypeArguments.Select(_ => _.BuildName(typeArgumentsNamingContext)))}>" : string.Empty;
		var callbackDelegateTypeName =
			method.RequiresProjectedDelegate ?
				$"Handler{memberIdentifier}{typeArguments}.CallbackForHandler" :
				DelegateBuilder.Build(method);

		string handlerBaseType;

		if (method.ReturnsVoid || method.ReturnType.RequiresProjectedArgument)
		{
			handlerBaseType = $"global::Rocks.Handler<{callbackDelegateTypeName}>";
		}
		else
		{
			if (method.ReturnType.IsRefLikeType || method.ReturnType.AllowsRefLikeType)
			{
				handlerBaseType = $"global::Rocks.Handler<{callbackDelegateTypeName}, global::System.Func<{method.ReturnType.BuildName(typeArgumentsNamingContext)}>>";
			}
			else
			{
				handlerBaseType = $"global::Rocks.Handler<{callbackDelegateTypeName}, {method.ReturnType.BuildName(typeArgumentsNamingContext)}>";
			}
		}

		writer.WriteLines(
			$$"""
			internal sealed class Handler{{memberIdentifier}}{{typeArguments}}
				: {{handlerBaseType}}
			""");

		if (method.Constraints.Length > 0)
		{
			writer.Indent++;
			writer.WriteLine(string.Join(" ", method.Constraints.Select(_ => _.ToString(typeArgumentsNamingContext, method))));
			writer.Indent--;
		}

		if (method.Parameters.Length > 0 || method.RequiresProjectedDelegate)
		{
			writer.WriteLine("{");
			writer.Indent++;

			if (method.RequiresProjectedDelegate)
			{
				writer.WriteLine(MockProjectedDelegateBuilder.GetProjectedDelegate(method));
			}

			var names = HandlerVariableNamingContext.Create();

			foreach (var parameter in method.Parameters)
			{
				var name = names[parameter.Name];

				var argumentTypeName = ProjectionBuilder.BuildArgument(parameter.Type, typeArgumentsNamingContext, parameter.RequiresNullableAnnotation);

				writer.WriteLine($"public {argumentTypeName} @{name} {{ get; set; }}");
			}

			if (method.ReturnType.RequiresProjectedArgument)
			{
				writer.WriteLine($"public {method.ReturnType.FullyQualifiedName} ReturnValue {{ get; set; }}");
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
		else
		{
			writer.WriteLine("{ }");
		}

		// Add the Handlers<> type.
		// If the method has open generics, we have to use the base Handler type -
		// we'll cast it later within the method implementation.
		var handlers = method.TypeArguments.Length == 0 ?
			$"private global::Rocks.Handlers<{expectationsFullyQualifiedName}.Handler{memberIdentifier}>? @handlers{memberIdentifier};" :
			$"private global::Rocks.Handlers<global::Rocks.Handler>? @handlers{memberIdentifier};";
		writer.WriteLine(handlers);
	}

	private static void BuildMethodHandlerTypes(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
	{
		foreach (var method in mockType.Methods)
		{
			MockHandlerListBuilder.BuildHandler(writer, method, method.MemberIdentifier, expectationsFullyQualifiedName);
		}
	}

	private static void BuildPropertyHandlerTypes(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
	{
		foreach (var property in mockType.Properties)
		{
			if (property.Accessors == PropertyAccessor.Get || property.Accessors == PropertyAccessor.Set || property.Accessors == PropertyAccessor.Init)
			{
				var method = property.Accessors == PropertyAccessor.Get ? property.GetMethod! : property.SetMethod!;
				MockHandlerListBuilder.BuildHandler(writer, method, property.MemberIdentifier, expectationsFullyQualifiedName);
			}
			else
			{
				var memberIdentifier = property.MemberIdentifier;

				if (property.GetCanBeSeenByContainingAssembly)
				{
					MockHandlerListBuilder.BuildHandler(writer, property.GetMethod!, memberIdentifier, expectationsFullyQualifiedName);
					memberIdentifier++;
				}

				if (property.SetCanBeSeenByContainingAssembly || property.InitCanBeSeenByContainingAssembly)
				{
					MockHandlerListBuilder.BuildHandler(writer, property.SetMethod!, memberIdentifier, expectationsFullyQualifiedName);
				}
			}
		}
	}
}