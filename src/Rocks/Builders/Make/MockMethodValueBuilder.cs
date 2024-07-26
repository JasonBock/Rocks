using Microsoft.CodeAnalysis;
using Rocks.Builders.Create;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Make;

internal static class MockMethodValueBuilder
{
	internal static void Build(IndentedTextWriter writer, MethodModel method)
	{
		var shouldThrowDoesNotReturnException = method.IsMarkedWithDoesNotReturn;
		var typeArgumentsNamingContext = method.IsGenericMethod ?
			new TypeArgumentsNamingContext(method) :
			new TypeArgumentsNamingContext();

		var returnByRef = method.ReturnsByRef ? "ref " : method.ReturnsByRefReadOnly ? "ref readonly " : string.Empty;
		var returnTypeValue = method.ReturnType.BuildName(typeArgumentsNamingContext);
		var returnType = $"{returnByRef}{returnTypeValue}";
		var explicitTypeNameDescription = method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
			$"{method.ContainingType.FullyQualifiedName}." : string.Empty;

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

		var typeArguments = method.IsGenericMethod ?
			$"<{string.Join(", ", method.TypeArguments.Select(_ => _.BuildName(typeArgumentsNamingContext)))}>" : string.Empty;
		var methodSignature = $"{returnType} {explicitTypeNameDescription}{method.Name}{typeArguments}({methodParameters})";

		if (method.AttributesDescription.Length > 0)
		{
			writer.WriteLine(method.AttributesDescription);
		}

		if (method.ReturnTypeAttributesDescription.Length > 0)
		{
			writer.WriteLine(method.ReturnTypeAttributesDescription);
		}

		var isUnsafe = method.IsUnsafe ? "unsafe " : string.Empty;
		var isPublic = method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
			$"{method.OverridingCodeValue} " : string.Empty;
		writer.WriteLine($"{isPublic}{isUnsafe}{(method.RequiresOverride == RequiresOverride.Yes ? "override " : string.Empty)}{methodSignature}");

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
			writer.WriteLine($"@{outParameter.Name} = default!;");
		}

		if(shouldThrowDoesNotReturnException)
		{
			writer.WriteLine("throw new global::Rocks.Exceptions.DoesNotReturnException();");
		}
		else
		{
			if (method.ReturnTypeIsTaskType)
			{
				writer.WriteLine("return global::System.Threading.Tasks.Task.CompletedTask;");
			}
			else if (method.ReturnTypeIsValueTaskType)
			{
				writer.WriteLine("return new global::System.Threading.Tasks.ValueTask();");
			}
			else if (method.ReturnTypeIsTaskOfTType)
			{
				var isNullForgiving = method.ReturnTypeIsTaskOfTTypeAndIsNullForgiving ? string.Empty : "!";
				writer.WriteLine($"return global::System.Threading.Tasks.Task.FromResult(default({method.ReturnTypeTypeArguments[0].BuildName(typeArgumentsNamingContext)}){isNullForgiving});");
			}
			else if (method.ReturnTypeIsValueTaskOfTType)
			{
				var isNullForgiving = method.ReturnTypeIsValueTaskOfTTypeAndIsNullForgiving ? string.Empty : "!";
				var valueTaskReturnTypeName = method.ReturnTypeTypeArguments[0].BuildName(typeArgumentsNamingContext);
				writer.WriteLine($"return new global::System.Threading.Tasks.ValueTask<{valueTaskReturnTypeName}>(default({valueTaskReturnTypeName}){isNullForgiving});");
			}
			else
			{
				if (method.ReturnsByRef || method.ReturnsByRefReadOnly)
				{
					writer.WriteLine($"return ref this.rr{method.MemberIdentifier};");
				}
				else
				{
					writer.WriteLine("return default!;");
				}
			}
		}
		writer.Indent--;
		writer.WriteLine("}");
	}
}