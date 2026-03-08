using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class MethodExpectationsMethodBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type, MethodModel method,
		string expectationsFullyQualifiedName,
		Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		// isGeneratedWithDefaults really means, "do we need to generate an overload
		// for the given method because it has optional and/or params parameters".
		static void BuildImplementation(IndentedTextWriter writer, TypeMockModel type, MethodModel method,
			bool isGeneratedWithDefaults, string expectationsFullyQualifiedName,
			Action<AdornmentsPipeline> adornmentsFQNsPipeline)
		{
			var needsGenerationWithDefaults = false;
			var typeArgumentsNamingContext = method.IsGenericMethod ?
				new TypeArgumentsNamingContext(method) :
				new TypeArgumentsNamingContext();

			var instanceParameters = method.Parameters.Length == 0 ? string.Empty :
				string.Join(", ", method.Parameters.Select(parameter =>
			{
				var (expectationParameter, needs) = parameter.GetExpectationParameter(
					isGeneratedWithDefaults, method.Parameters[method.Parameters.Length - 1], typeArgumentsNamingContext);
				needsGenerationWithDefaults |= needs;
				return expectationParameter;
			}));

			var typeArguments = method.IsGenericMethod ?
				$"<{string.Join(", ", method.TypeArguments.Select(_ => _.BuildName(typeArgumentsNamingContext)))}>" : string.Empty;
			var callbackDelegateTypeName = method.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(method, expectationsFullyQualifiedName, method.MemberIdentifier) :
				DelegateBuilder.Build(method);

			string adornmentsType;

			var handlerTypeName = $"{expectationsFullyQualifiedName}.Handler{method.MemberIdentifier}{typeArguments}";

			if (method.ReturnType.IsPointer)
			{
				adornmentsType = $"global::Rocks.Adornments<AdornmentsForHandler{method.MemberIdentifier}{typeArguments}, {handlerTypeName}, {callbackDelegateTypeName}>";
			}
			else
			{
				var returnType =
					method.ReturnsVoid ?
						string.Empty :
						method.ReturnType.IsRefLikeType || method.ReturnType.AllowsRefLikeType ?
							$"global::System.Func<{method.ReturnType.BuildName(typeArgumentsNamingContext)}>" :
							method.ReturnType.BuildName(typeArgumentsNamingContext);

				adornmentsType = method.ReturnsVoid ?
					$"global::Rocks.Adornments<AdornmentsForHandler{method.MemberIdentifier}{typeArguments}, {handlerTypeName}, {callbackDelegateTypeName}>" :
					$"global::Rocks.Adornments<AdornmentsForHandler{method.MemberIdentifier}{typeArguments}, {handlerTypeName}, {callbackDelegateTypeName}, {returnType}>";
			}

			var constraints = method.Constraints.Length > 0 ?
				$" {string.Join(" ", method.Constraints.Select(_ => _.ToString(typeArgumentsNamingContext, method)))}" : "";
			var hiding = method.RequiresHiding == RequiresHiding.Yes ? "new " : string.Empty;

			adornmentsFQNsPipeline(new(adornmentsType, typeArguments, constraints, method, method.MemberIdentifier));

			if (isGeneratedWithDefaults)
			{
				var parameterValues = string.Join(", ", method.Parameters.Select(
					parameter =>
						parameter.HasExplicitDefaultValue || parameter.IsParams ?
							parameter.Type.IsRefLikeType ?
								$"@{parameter.Name}" :
								$"global::Rocks.Arg.Is(@{parameter.Name})" :
							$"@{parameter.Name}"));

				writer.WriteLines(
					$$"""
					internal {{hiding}}{{expectationsFullyQualifiedName}}.Adornments.AdornmentsForHandler{{method.MemberIdentifier}}{{typeArguments}} {{method.Name}}{{typeArguments}}({{instanceParameters}}){{constraints}} =>
						this.{{method.Name}}{{typeArguments}}({{parameterValues}});
					""");
			}
			else if (method.Parameters.Length == 0)
			{
				writer.WriteLines(
					$$"""
					internal {{hiding}}{{expectationsFullyQualifiedName}}.Adornments.AdornmentsForHandler{{method.MemberIdentifier}}{{typeArguments}} {{method.Name}}{{typeArguments}}({{instanceParameters}}){{constraints}}
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
						var handler = new {{expectationsFullyQualifiedName}}.Handler{{method.MemberIdentifier}}{{typeArguments}}();
						if (this.parent.handlers{{method.MemberIdentifier}} is null) { this.parent.handlers{{method.MemberIdentifier}} = new(handler); }
						else { this.parent.handlers{{method.MemberIdentifier}}.Add(handler); }
						return new(handler);
					}
					""");
			}
			else
			{
				var handlerContext = new VariablesNamingContext(method);
				writer.WriteLines(
					$$"""
					internal {{hiding}}{{expectationsFullyQualifiedName}}.Adornments.AdornmentsForHandler{{method.MemberIdentifier}}{{typeArguments}} {{method.Name}}{{typeArguments}}({{instanceParameters}}){{constraints}}
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
					""");

				writer.Indent++;
				foreach (var parameter in method.Parameters)
				{
					writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
				}

				writer.WriteLines(
					$$"""

					var @{{handlerContext["handler"]}} = new {{expectationsFullyQualifiedName}}.Handler{{method.MemberIdentifier}}{{typeArguments}}
					{
					""");
				writer.Indent++;

				var handlerNamingContext = HandlerVariableNamingContext.Create();

				foreach (var parameter in method.Parameters)
				{
					MethodExpectationsMethodBuilder.BuildParameterSetting(
						writer, method, typeArgumentsNamingContext, handlerNamingContext, parameter);
				}

				writer.Indent--;
				writer.WriteLines(
					$$"""
					};

					if (this.parent.handlers{{method.MemberIdentifier}} is null) { this.parent.handlers{{method.MemberIdentifier}} = new(@{{handlerContext["handler"]}}); }
					else { this.parent.handlers{{method.MemberIdentifier}}.Add(@{{handlerContext["handler"]}}); }
					return new(@{{handlerContext["handler"]}});
					""");

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (needsGenerationWithDefaults)
			{
				writer.WriteLine();
				BuildImplementation(writer, type, method, true, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
			}
		}

		BuildImplementation(writer, type, method, false, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
	}

	private static void BuildParameterSetting(
		IndentedTextWriter writer, MethodModel method, TypeArgumentsNamingContext typeArgumentsNamingContext,
		VariablesNamingContext handlerNamingContext, ParameterModel parameter)
	{
		if (parameter.HasExplicitDefaultValue && method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
		{
			writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = @{parameter.Name}.Transform({parameter.ExplicitDefaultValue}),");
		}
		else if (parameter.RefKind == RefKind.Out)
		{
			if (parameter.Type.IsRefLikeType)
			{
				writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = new(),");
			}
			else
			{
				writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = global::Rocks.Arg.Any<{parameter.Type.BuildName(typeArgumentsNamingContext)}{(parameter.RequiresNullableAnnotation ? "?" : string.Empty)}>(),");
			}
		}
		else
		{
			writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = @{parameter.Name},");
		}
	}
}