using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockProjectedDelegateBuilderV3
{
	internal static string GetProjectedCallbackDelegateFullyQualifiedName(IMethodSymbol method, ITypeSymbol typeToMock)
	{
		var containingNamespace = !typeToMock.ContainingNamespace?.IsGlobalNamespace ?? false ?
			$"{typeToMock.ContainingNamespace!.ToDisplayString()}." : string.Empty;
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.GetName(TypeNameOption.Flatten)}";
		var delegateName = MockProjectedDelegateBuilder.GetProjectedCallbackDelegateName(method);
		return $"global::{containingNamespace}{projectionsForNamespace}.{delegateName}";
	}

	internal static string GetProjectedReturnValueDelegateFullyQualifiedName(IMethodSymbol method, ITypeSymbol typeToMock)
	{
		var containingNamespace = !typeToMock.ContainingNamespace?.IsGlobalNamespace ?? false ?
			$"{typeToMock.ContainingNamespace!.ToDisplayString()}." : string.Empty;
		var projectionsForNamespace = $"ProjectionsFor{typeToMock.GetName(TypeNameOption.Flatten)}";
		var delegateName = MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateName(method);
		return $"global::{containingNamespace}{projectionsForNamespace}.{delegateName}";
	}

	// TODO: this could go on the MethodModel itself.
	internal static string GetProjectedDelegate(MethodModel method)
	{
		var returnType = method.ReturnTypeFullyQualifiedName;
		var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
		{
			var direction = _.RefKind == RefKind.Ref ? "ref " : _.RefKind == RefKind.Out ? "out " : string.Empty;
			var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.TypeFullyQualifiedName} @{_.Name}";
			return $"{_.AttributesDescription}{parameter}";
		}));
		var isUnsafe = method.IsUnsafe ? "unsafe " : string.Empty;
		var constraints = method.Constraints;
		var methodConstraints = constraints.Length > 0 ?
			$" {string.Join(" ", constraints)}" : string.Empty;

		return $"internal {isUnsafe}delegate {returnType} {method.ProjectedCallbackDelegateName}({methodParameters}){methodConstraints};";
	}

	// TODO: this could go on the MethodModel itself.
   internal static string GetProjectedReturnValueDelegate(MethodModel method) => 
		$"internal delegate {method.ReturnTypeFullyQualifiedName} {method.ProjectedReturnValueDelegateName}();";

   internal static void Build(IndentedTextWriter writer, TypeModel type)
	{
		static void BuildDelegate(IndentedTextWriter writer, MethodModel method)
		{
			writer.WriteLine(MockProjectedDelegateBuilderV3.GetProjectedDelegate(method));

			if(method.ReturnTypeIsRefLikeType)
			{
				writer.WriteLine(MockProjectedDelegateBuilderV3.GetProjectedReturnValueDelegate(method));
			}
		}

		static void BuildDelegates(IndentedTextWriter writer, IEnumerable<MethodModel> methods)
		{
			foreach (var method in methods)
			{
				BuildDelegate(writer, method);
			}
		}

		// TODO: Come back for properties
		//static void BuildProperties(IndentedTextWriter writer, MockInformation information, Compilation compilation)
		//{
		//	var getPropertyMethods = information.Properties.Results
		//		.Where(_ => _.Value.GetMethod is not null && _.Value.GetMethod.RequiresProjectedDelegate() &&
		//			_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
		//		.Select(_ => _.Value.GetMethod!);
		//	BuildDelegates(writer, getPropertyMethods, compilation);

		//	var setPropertyMethods = information.Properties.Results
		//		.Where(_ => _.Value.SetMethod is not null && _.Value.SetMethod.RequiresProjectedDelegate() &&
		//			_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
		//		.Select(_ => _.Value.SetMethod!);
		//	BuildDelegates(writer, setPropertyMethods, compilation);

		//	var explicitGetPropertyMethodGroups = information.Properties.Results
		//		.Where(_ => _.Value.GetMethod is not null && _.Value.GetMethod.RequiresProjectedDelegate() &&
		//			_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
		//		.GroupBy(_ => _.Value.ContainingType);

		//	foreach (var explicitGetPropertyMethodGroup in explicitGetPropertyMethodGroups)
		//	{
		//		BuildDelegate(writer, explicitGetPropertyMethodGroup.First().Value.GetMethod!, compilation);
		//	}

		//	var explicitSetPropertyMethodGroups = information.Properties.Results
		//		.Where(_ => _.Value.SetMethod is not null && _.Value.SetMethod.RequiresProjectedDelegate() &&
		//			_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
		//		.GroupBy(_ => _.Value.ContainingType);

		//	foreach (var explicitSetPropertyMethodGroup in explicitSetPropertyMethodGroups)
		//	{
		//		BuildDelegate(writer, explicitSetPropertyMethodGroup.First().Value.SetMethod!, compilation);
		//	}
		//}

		if (type.Methods.Length > 0)
		{
			var methods = type.Methods
				.Where(_ => _.RequiresProjectedDelegate &&
					_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No);
			BuildDelegates(writer, methods);

			var explicitMethodGroups = type.Methods
				.Where(_ => _.RequiresProjectedDelegate &&
					_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
				.GroupBy(_ => _.ContainingTypeFullyQualifiedName);

			foreach (var explicitMethodGroup in explicitMethodGroups)
			{
				BuildDelegate(writer, explicitMethodGroup.First());
			}
		}

		// TODO: Come back for properties
		//if (type.Properties.Length > 0)
		//{
		//	BuildProperties(writer, type, compilation);
		//}
	}
}