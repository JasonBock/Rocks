using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockHandlerListBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType, string expectationsFullyQualifiedName)
	{
		var hasParameters = mockType.Methods.Any(_ => _.Parameters.Length > 0) ||
			mockType.Properties.Any(_ => (_.GetMethod?.Parameters.Length > 0) || (_.SetMethod?.Parameters.Length > 0));

		if (hasParameters)
		{
			// CS8618 - Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
			// We know we're going to set this and we have control over that, so we emit the pragma to shut the compiler up.
			writer.WriteLine("#pragma warning disable CS8618");
		}

		BuildMethodHandlerTypes(writer, mockType);
		BuildPropertyHandlerTypes(writer, mockType);

		if (hasParameters)
		{
			// CS8618 - Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
			// We know we're going to set this and we have control over that, so we emit the pragma to shut the compiler up.
			writer.WriteLine("#pragma warning restore CS8618");
		}
	}

	private static void BuildHandler(IndentedTextWriter writer, MethodModel method, uint memberIdentifier)
	{
		var callbackDelegateTypeName =
			method.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(method, method.MockType) :
				method.ReturnsVoid ?
					DelegateBuilder.Build(method.Parameters) :
					DelegateBuilder.Build(method.Parameters, method.ReturnType);

		string handlerBaseType;

		if (method.ReturnType.TypeKind == TypeKind.FunctionPointer ||
			method.ReturnType.TypeKind == TypeKind.Pointer)
		{
			handlerBaseType = $"{MockProjectedAdornmentsTypesBuilder.GetProjectedHandlerFullyQualifiedNameName(method.ReturnType, method.MockType)}<{callbackDelegateTypeName}>";
		}
		else
		{
			var returnTypeName =
				method.ReturnsVoid ?
					string.Empty :
					method.ReturnType.IsRefLikeType ?
						MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(method, method.MockType) :
						method.ReturnType.FullyQualifiedName;
			returnTypeName = returnTypeName == string.Empty ? string.Empty : $", {returnTypeName}";
			handlerBaseType = $"global::Rocks.Handler<{callbackDelegateTypeName}{returnTypeName}>";
		}

		var typeArguments = method.TypeArguments.Length > 0 ?
			$"<{string.Join(", ", method.TypeArguments)}>" : string.Empty;

		writer.WriteLines(
			$$"""
			internal sealed class Handler{{memberIdentifier}}{{typeArguments}}
				: {{handlerBaseType}}
			""");

		if (method.Constraints.Length > 0)
		{
			writer.Indent++;
			writer.WriteLine(string.Join(" ", method.Constraints));
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
				var argumentTypeName =
					parameter.Type.IsEsoteric ?
						parameter.Type.IsPointer ?
							$"public {PointerArgTypeBuilder.GetProjectedFullyQualifiedName(parameter.Type, method.MockType)}" :
							$"public {RefLikeArgTypeBuilder.GetProjectedFullyQualifiedName(parameter.Type, method.MockType)}" : 
						$"public global::Rocks.Argument<{parameter.Type.FullyQualifiedName}{requiresNullable}>";

				writer.WriteLine($"{argumentTypeName} @{name} {{ get; set; }}");
			}

			writer.Indent--;
			writer.WriteLine("}");
		}
		else
		{
			writer.WriteLine("{ }");
		}

		writer.WriteLine();

		// Add the Handlers<> type.
		// If the method has open generics, we have to use the base Handler type -
		// we'll cast it later within the method implementation.
		var handlersBaseType = $"global::Rocks.Handlers<{handlerBaseType}>";
		var handlers = method.TypeArguments.Length == 0 ?
			$"private {handlersBaseType}? @handlers{method.MemberIdentifier};" :
			$"private global::Rocks.Handlers<global::Rocks.Handler>? @handlers{method.MemberIdentifier};";
		writer.WriteLine(handlers);
		writer.WriteLine();
	}

	private static void BuildMethodHandlerTypes(IndentedTextWriter writer, TypeMockModel mockType)
	{
		foreach (var method in mockType.Methods)
		{
			MockHandlerListBuilder.BuildHandler(writer, method, method.MemberIdentifier);
		}
	}

	private static void BuildPropertyHandlerTypes(IndentedTextWriter writer, TypeMockModel mockType)
	{
		foreach (var property in mockType.Properties)
		{
			if (property.Accessors == PropertyAccessor.Get || property.Accessors == PropertyAccessor.Set || property.Accessors == PropertyAccessor.Init)
			{
				var method = property.Accessors == PropertyAccessor.Get ? property.GetMethod! : property.SetMethod!;
				MockHandlerListBuilder.BuildHandler(writer, method, property.MemberIdentifier);
			}
			else
			{
				var memberIdentifier = property.MemberIdentifier;

				if (property.GetCanBeSeenByContainingAssembly)
				{
					MockHandlerListBuilder.BuildHandler(writer, property.GetMethod!, memberIdentifier);
					memberIdentifier++;
				}

				if (property.SetCanBeSeenByContainingAssembly || property.InitCanBeSeenByContainingAssembly)
				{
					MockHandlerListBuilder.BuildHandler(writer, property.SetMethod!, memberIdentifier);
				}
			}
		}
	}
}