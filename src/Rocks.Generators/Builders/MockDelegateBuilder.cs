using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Rocks.Builders
{
	internal static class MockDelegateBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			static void BuildDelegate(IndentedTextWriter writer, IMethodSymbol method)
			{
				var returnType = method.ReturnType.GetName();
				var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
				{
					var direction = _.RefKind == RefKind.Ref ? "ref " : _.RefKind == RefKind.Out ? "out " : string.Empty;
					var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetName()} {_.Name}";
					return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription()} " : string.Empty)}{parameter}";
				}));
				var methodSignature = $"public delegate {returnType} {method.GetName(extendedName: "Callback")}({methodParameters});";
				writer.WriteLine(methodSignature);
			}

			static void BuildDelegates(IndentedTextWriter writer, IEnumerable<IMethodSymbol> methods)
			{
				foreach (var method in methods)
				{
					BuildDelegate(writer, method);
				}
			}

			if(information.Methods.Length > 0)
			{
				var methods = information.Methods
					.Where(_ => _.Value.Parameters.Any(_ => _.RefKind == RefKind.Ref || _.RefKind == RefKind.Out) &&
						_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
					.Select(_ => _.Value);
				BuildDelegates(writer, methods);

				var explicitMethodGroups = information.Methods
					.Where(_ => _.Value.Parameters.Any(_ => _.RefKind == RefKind.Ref || _.RefKind == RefKind.Out) &&
						_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
					.GroupBy(_ => _.Value.ContainingType);

				foreach (var explicitMethodGroup in explicitMethodGroups)
				{
					BuildDelegate(writer, explicitMethodGroup.First().Value);
				}
			}
		}
	}
}