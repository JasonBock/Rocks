using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockHandlerListBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
	{
		MockHandlerListBuilder.BuildMethodHandlerTypes(writer, mockType, expectationsFullyQualifiedName);
		MockHandlerListBuilder.BuildPropertyHandlerTypes(writer, mockType, expectationsFullyQualifiedName);
	}

	private static void BuildHandler(IndentedTextWriter writer, MethodModel method, uint memberIdentifier, string expectationsFullyQualifiedName)
	{
		var callbackDelegateTypeName =
			method.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(method, method.MockType) :
				DelegateBuilder.Build(method);
		var typeArgumentsNamingContext = method.IsGenericMethod ?
			new TypeArgumentsNamingContext(method) :
			new TypeArgumentsNamingContext();
		var typeArguments = method.IsGenericMethod ?
			$"<{string.Join(", ", method.TypeArguments.Select(_ => _.BuildName(typeArgumentsNamingContext)))}>" : string.Empty;

		string handlerBaseType;

		if (method.ReturnType.TypeKind == TypeKind.FunctionPointer ||
			method.ReturnType.TypeKind == TypeKind.Pointer)
		{
			handlerBaseType = $"{MockProjectedAdornmentsTypesBuilder.GetProjectedHandlerFullyQualifiedNameName(method.ReturnType, method.MockType)}<{callbackDelegateTypeName}>";
		}
		else
		{
			string returnTypeName;

			if (method.ReturnsVoid)
			{
				returnTypeName = string.Empty;
			}
			else if (method.ReturnType.IsRefLikeType || method.ReturnType.AllowsRefLikeType)
			{
				returnTypeName = $"global::System.Func<{method.ReturnType.BuildName(typeArgumentsNamingContext)}>";
			}
			else
			{
				returnTypeName = method.ReturnType.BuildName(typeArgumentsNamingContext);
			}

			returnTypeName = returnTypeName == string.Empty ? string.Empty : $", {returnTypeName}";
			handlerBaseType = $"global::Rocks.Handler<{callbackDelegateTypeName}{returnTypeName}>";
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

		if (method.Parameters.Length > 0)
		{
			writer.WriteLine("{");
			writer.Indent++;

			var names = HandlerVariableNamingContext.Create();

			foreach (var parameter in method.Parameters)
			{
				var requiresNullable = parameter.RequiresNullableAnnotation ? "?" : string.Empty;
				var name = names[parameter.Name];

				string argumentTypeName;

				if (parameter.Type.IsPointer)
				{
					argumentTypeName = $"public {PointerArgTypeBuilder.GetProjectedFullyQualifiedName(parameter.Type, method.MockType)}";
				}
				else
				{
					argumentTypeName = parameter.Type.IsRefLikeType || parameter.Type.AllowsRefLikeType ?
						$"public global::Rocks.RefStructArgument<{parameter.Type.BuildName(typeArgumentsNamingContext)}{requiresNullable}>" :
						$"public global::Rocks.Argument<{parameter.Type.BuildName(typeArgumentsNamingContext)}{requiresNullable}>";
				}

				writer.WriteLine($"{argumentTypeName} @{name} {{ get; set; }}");
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