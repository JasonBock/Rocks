using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class MockHandlerListBuilderV4
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel mockType)
	{
		BuildMethodHandlerTypes(writer, mockType);

		if (mockType.Methods.Length > 0)
		{
			writer.WriteLine();
		}

		BuildPropertyHandlerTypes(writer, mockType);

		if (mockType.Properties.Length > 0)
		{
			writer.WriteLine();
		}

		BuildHandlerListFields(writer, mockType);
	}

	private static void BuildHandlerListFields(IndentedTextWriter writer, TypeMockModel mockType)
	{
		foreach (var method in mockType.Methods)
		{
			writer.WriteLine($"private readonly global::System.Collections.Generic.List<global::Rocks.Handler{method.MemberIdentifier}> @handlers{method.MemberIdentifier} = new();");
		}

		foreach (var property in mockType.Properties)
		{
			writer.WriteLine($"private readonly global::System.Collections.Generic.List<global::Rocks.Handler{property.MemberIdentifier}> @handlers{property.MemberIdentifier} = new();");
		}
	}

	private static void BuildMethodHandlerTypes(IndentedTextWriter writer, TypeMockModel mockType)
	{
		foreach (var method in mockType.Methods)
		{
			var callbackDelegateTypeName = method.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(method, method.MockType) :
				method.ReturnsVoid ?
					DelegateBuilder.Build(method.Parameters) :
					DelegateBuilder.Build(method.Parameters, method.ReturnType);
			var returnTypeName = method.ReturnsVoid ? string.Empty :
				method.ReturnType.IsRefLikeType ?
					MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(method, method.MockType) :
					method.ReturnType.FullyQualifiedName;
			returnTypeName = returnTypeName == string.Empty ? string.Empty : $", {returnTypeName}";

			writer.WriteLines(
				$$"""
				internal sealed class Handler{{method.MemberIdentifier}}
					: HandlerV4<{{callbackDelegateTypeName}}{{returnTypeName}}>
				{
				""");

			writer.Indent++;

			// CS8618 - Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
			// We know we're going to set this and we have control over that, so we emit the pragma to shut the compiler up.
			writer.WriteLine("#pragma warning disable CS8618");

			// TODO: I should consider putting this in MethodModel as well as properties.
			var names = new VariableNamingContextV4(new[] { "CallCount", "ExpectedCallCount", "Callback", "ReturnValue" }.ToImmutableArray());

			foreach (var parameter in method.Parameters)
			{
				var requiresNullable = parameter.RequiresNullableAnnotation ? "?" : string.Empty;
				var name = names[parameter.Name];
				writer.WriteLine($"public global::Rocks.Argument<{parameter.Type.FullyQualifiedName}{requiresNullable}> {name} {{ get; set; }}");
			}

			writer.WriteLine("#pragma warning restore CS8618");

			writer.Indent--;
			writer.WriteLine("}");
		}
	}

	private static void BuildPropertyHandlerTypes(IndentedTextWriter writer, TypeMockModel mockType)
	{
		foreach (var property in mockType.Properties)
		{
			var parameters = property.GetMethod is not null ?
				property.GetMethod.Parameters :
				property.SetMethod!.Parameters;
			var callbackDelegateTypeName = DelegateBuilder.Build(parameters);
			var returnTypeName = property.GetMethod is null ? string.Empty :
				property.GetMethod.ReturnType.IsRefLikeType ?
					MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(property.GetMethod, property.MockType) :
					property.GetMethod.ReturnType.FullyQualifiedName;
			returnTypeName = returnTypeName == string.Empty ? string.Empty : $", {returnTypeName}";

			writer.WriteLines(
				$$"""
				internal sealed class Handler{{property.MemberIdentifier}}
					: HandlerV4<{{callbackDelegateTypeName}}{{returnTypeName}}>
				{
				""");

			writer.Indent++;

			// CS8618 - Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
			// We know we're going to set this and we have control over that, so we emit the pragma to shut the compiler up.
			writer.WriteLine("#pragma warning disable CS8618");

			// TODO: I should consider putting this in MethodModel as well as properties.
			var names = new VariableNamingContextV4(new[] { "CallCount", "ExpectedCallCount", "Callback", "ReturnValue" }.ToImmutableArray());

			foreach (var parameter in parameters)
			{
				var requiresNullable = parameter.RequiresNullableAnnotation ? "?" : string.Empty;
				var name = names[parameter.Name];
				writer.WriteLine($"public global::Rocks.Argument<{parameter.Type.FullyQualifiedName}{requiresNullable}> {name} {{ get; set; }}");
			}

			writer.WriteLine("#pragma warning restore CS8618");

			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}