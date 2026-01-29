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
			static string GetOptionalParameter(ParameterModel parameter, ParameterModel lastParameter,
				string typeName, string requiresNullable) =>
					lastParameter.IsParams && lastParameter.Type.IsRefLikeType ? 
						$"[global::System.Runtime.InteropServices.Optional, global::System.Runtime.InteropServices.DefaultParameterValue({parameter.ExplicitDefaultValue})] {typeName}{requiresNullable} @{parameter.Name}" :
						parameter.AttributesDescription.Contains("Optional") ?
							$"[global::System.Runtime.InteropServices.Optional, global::System.Runtime.InteropServices.DefaultParameterValue({parameter.ExplicitDefaultValue})] {typeName}{requiresNullable} @{parameter.Name}" :
							$"{typeName}{requiresNullable} @{parameter.Name} = {parameter.ExplicitDefaultValue}";

			var needsGenerationWithDefaults = false;
			var typeArgumentsNamingContext = method.IsGenericMethod ?
				new TypeArgumentsNamingContext(method) :
				new TypeArgumentsNamingContext();

			var instanceParameters = method.Parameters.Length == 0 ? string.Empty :
				string.Join(", ", method.Parameters.Select(parameter =>
				{
					var argumentTypeName = ProjectionBuilder.BuildArgument(
						parameter.Type, typeArgumentsNamingContext, parameter.RequiresNullableAnnotation);

					if (parameter.Type.IsPointer)
					{
						return $"{argumentTypeName} @{parameter.Name}";
					}
					else
					{
						var requiresNullable = parameter.RequiresNullableAnnotation ? "?" : string.Empty;
						// TODO: Why are we doing BuildName? Maybe because
						// of the type arguments and trying to close the generic?
						var typeName = parameter.Type.BuildName(typeArgumentsNamingContext);

						if (isGeneratedWithDefaults)
						{
							return parameter.HasExplicitDefaultValue ?
								GetOptionalParameter(parameter, method.Parameters[method.Parameters.Length - 1], typeName, requiresNullable) :
								parameter.IsParams ?
									parameter.Type.IsRefLikeType ?
										$"global::Rocks.RefStructArgument<{parameter.Type.FullyQualifiedName}> @{parameter.Name}" :
										$"params {typeName}{requiresNullable} @{parameter.Name}" :
									$"{argumentTypeName} @{parameter.Name}";
						}

						if (!isGeneratedWithDefaults)
						{
							// Only set this flag if we're currently not generating with defaults.
							needsGenerationWithDefaults |= parameter.HasExplicitDefaultValue ||
								(parameter.IsParams && !parameter.Type.IsRefLikeType);
						}

						return $"{argumentTypeName} @{parameter.Name}";
					}
				}));

			var typeArguments = method.IsGenericMethod ?
				$"<{string.Join(", ", method.TypeArguments.Select(_ => _.BuildName(typeArgumentsNamingContext)))}>" : string.Empty;
			var callbackDelegateTypeName = method.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(method, method.MockType, expectationsFullyQualifiedName, method.MemberIdentifier) :
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
				writer.WriteLine($"internal {hiding}{expectationsFullyQualifiedName}.Adornments.AdornmentsForHandler{method.MemberIdentifier}{typeArguments} {method.Name}{typeArguments}({instanceParameters}){constraints}");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLine($"global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);");

				foreach (var parameter in method.Parameters)
				{
					writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
				}

				writer.WriteLine();
				writer.WriteLines(
					$$"""
					var @{{handlerContext["handler"]}} = new {{expectationsFullyQualifiedName}}.Handler{{method.MemberIdentifier}}{{typeArguments}}
					{
					""");
				writer.Indent++;

				var handlerNamingContext = HandlerVariableNamingContext.Create();

				foreach (var parameter in method.Parameters)
				{
					if (parameter.HasExplicitDefaultValue && method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
					{
						writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = @{parameter.Name}.Transform({parameter.ExplicitDefaultValue}),");
					}
					else if (parameter.RefKind == RefKind.Out)
					{
						writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = global::Rocks.Arg.Any<{parameter.Type.BuildName(typeArgumentsNamingContext)}{(parameter.RequiresNullableAnnotation ? "?" : string.Empty)}>(),");
					}
					else
					{
						writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = @{parameter.Name},");
					}
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
}