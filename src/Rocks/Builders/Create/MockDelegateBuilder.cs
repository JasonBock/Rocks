using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Rocks.Builders.Create
{
	internal static class MockDelegateBuilder
	{
		internal static void Build(IndentedTextWriter writer, MockInformation information)
		{
			static void BuildDelegate(IndentedTextWriter writer, IMethodSymbol method, bool forIndexer)
			{
				static string BuildIndexerName(IMethodSymbol method) =>
					$"For{string.Join("_", method.Parameters.Select(_ => _.Type.GetName(TypeNameOption.Flatten)))}";

				var returnType = method.ReturnType.GetName();
				var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
				{
					var direction = _.RefKind == RefKind.Ref ? "ref " : _.RefKind == RefKind.Out ? "out " : string.Empty;
					var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetName()} {_.Name}";
					return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription()} " : string.Empty)}{parameter}";
				}));
				var isUnsafe = method.IsUnsafe() ? "unsafe " : string.Empty;
				var extendedName = $"{(forIndexer ? BuildIndexerName(method) : string.Empty)}Callback";
				var methodSignature = $"public {isUnsafe}delegate {returnType} {method.GetName(extendedName: extendedName)}({methodParameters});";
				writer.WriteLine(methodSignature);
			}

			static void BuildDelegates(IndentedTextWriter writer, IEnumerable<IMethodSymbol> methods, bool forIndexers)
			{
				foreach (var method in methods)
				{
					BuildDelegate(writer, method, forIndexers);
				}
			}

			static void BuildProperties(IndentedTextWriter writer, MockInformation information, bool forIndexers)
			{
				var getPropertyMethods = information.Properties
					.Where(_ => _.Value.GetMethod is not null && _.Value.Type.IsPointer() && _.Value.IsIndexer == forIndexers &&
						_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
					.Select(_ => _.Value.GetMethod!);
				BuildDelegates(writer, getPropertyMethods, forIndexers);

				var setPropertyMethods = information.Properties
					.Where(_ => _.Value.SetMethod is not null && _.Value.Type.IsPointer() && _.Value.IsIndexer == forIndexers &&
						_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
					.Select(_ => _.Value.SetMethod!);
				BuildDelegates(writer, setPropertyMethods, forIndexers);

				var explicitGetPropertyMethodGroups = information.Properties
					.Where(_ => _.Value.GetMethod is not null && _.Value.Type.IsPointer() && _.Value.IsIndexer == forIndexers &&
						_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
					.GroupBy(_ => _.Value.ContainingType);

				foreach (var explicitGetPropertyMethodGroup in explicitGetPropertyMethodGroups)
				{
					BuildDelegate(writer, explicitGetPropertyMethodGroup.First().Value.GetMethod!, forIndexers);
				}

				var explicitSetPropertyMethodGroups = information.Properties
					.Where(_ => _.Value.SetMethod is not null && _.Value.Type.IsPointer() && _.Value.IsIndexer == forIndexers &&
						_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
					.GroupBy(_ => _.Value.ContainingType);

				foreach (var explicitSetPropertyMethodGroup in explicitSetPropertyMethodGroups)
				{
					BuildDelegate(writer, explicitSetPropertyMethodGroup.First().Value.SetMethod!, forIndexers);
				}
			}

			if (information.Methods.Length > 0)
			{
				var methods = information.Methods
					.Where(_ => _.Value.Parameters.Any(_ => _.RefKind == RefKind.Ref || _.RefKind == RefKind.Out || _.Type.IsPointer()) &&
						_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
					.Select(_ => _.Value);
				BuildDelegates(writer, methods, false);

				var explicitMethodGroups = information.Methods
					.Where(_ => _.Value.Parameters.Any(_ => _.RefKind == RefKind.Ref || _.RefKind == RefKind.Out || _.Type.IsPointer()) &&
						_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
					.GroupBy(_ => _.Value.ContainingType);

				foreach (var explicitMethodGroup in explicitMethodGroups)
				{
					BuildDelegate(writer, explicitMethodGroup.First().Value, false);
				}
			}

			if (information.Properties.Length > 0)
			{
				BuildProperties(writer, information, false);
				BuildProperties(writer, information, true);
			}
		}
	}
}