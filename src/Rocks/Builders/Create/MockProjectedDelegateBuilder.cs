using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockProjectedDelegateBuilder
{
	internal static string GetProjectedCallbackDelegateFullyQualifiedName(MethodModel method, TypeReferenceModel typeToMock)
	{
		var projectionsForName = $"ProjectionsFor{typeToMock.FlattenedName}";
		var delegateName = method.ProjectedCallbackDelegateName;
		var typeArguments = method.TypeArguments.Length > 0 ?
			$"<{string.Join(", ", method.TypeArguments)}>" : string.Empty;

		return $"global::{(typeToMock.Namespace.Length == 0 ? "" : $"{typeToMock.Namespace}.")}{typeToMock.FlattenedName}CreateExpectations.{projectionsForName}.{delegateName}{typeArguments}";
	}

	internal static string GetProjectedReturnValueDelegateFullyQualifiedName(MethodModel method, TypeReferenceModel typeToMock)
	{
		var projectionsForName = $"ProjectionsFor{typeToMock.FlattenedName}";
		var delegateName = method.ProjectedReturnValueDelegateName;
		return $"global::{(typeToMock.Namespace.Length == 0 ? "" : $"{typeToMock.Namespace}.")}{typeToMock.FlattenedName}CreateExpectations.{projectionsForName}.{delegateName}";
	}

	// TODO: this could go on the MethodModel itself.
	internal static string GetProjectedDelegate(MethodModel method)
	{
		var returnType = method.ReturnType.FullyQualifiedName;
		var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
		{
			var direction = _.RefKind == RefKind.Ref ? "ref " : _.RefKind == RefKind.Out ? "out " : string.Empty;
			var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.FullyQualifiedName} @{_.Name}";
			return $"{_.AttributesDescription}{parameter}";
		}));
		var isUnsafe = method.IsUnsafe ? "unsafe " : string.Empty;
		var constraints = method.Constraints;
		var methodConstraints = constraints.Length > 0 ?
			$" {string.Join(" ", constraints)}" : string.Empty;
		var typeArguments = method.TypeArguments.Length > 0 ?
			$"<{string.Join(", ", method.TypeArguments)}>" : string.Empty;

		return $"internal {isUnsafe}delegate {returnType} {method.ProjectedCallbackDelegateName}{typeArguments}({methodParameters}){methodConstraints};";
	}

	// TODO: this could go on the MethodModel itself.
	internal static string GetProjectedReturnValueDelegate(MethodModel method) =>
		$"internal {(method.IsUnsafe ? "unsafe " : string.Empty)}delegate {method.ReturnType.FullyQualifiedName} {method.ProjectedReturnValueDelegateName}();";

	internal static void Build(IndentedTextWriter writer, TypeMockModel type)
	{
		static void BuildDelegate(IndentedTextWriter writer, MethodModel method, HashSet<string> generatedDelegates)
		{
			if (generatedDelegates.Add(method.ProjectedCallbackDelegateName!))
			{
				writer.WriteLine(MockProjectedDelegateBuilder.GetProjectedDelegate(method));
			}

			if (method.ReturnType.IsRefLikeType &&
				generatedDelegates.Add(method.ProjectedReturnValueDelegateName!))
			{
				writer.WriteLine(MockProjectedDelegateBuilder.GetProjectedReturnValueDelegate(method));
			}
		}

		static void BuildDelegates(IndentedTextWriter writer, IEnumerable<MethodModel> methods, HashSet<string> generatedDelegates)
		{
			foreach (var method in methods)
			{
				BuildDelegate(writer, method, generatedDelegates);
			}
		}

		static void BuildProperties(IndentedTextWriter writer, TypeMockModel mockType, HashSet<string> generatedDelegates)
		{
			var getPropertyMethods = mockType.Properties
				.Where(_ => _.GetMethod is not null && _.GetMethod.RequiresProjectedDelegate &&
					_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
				.Select(_ => _.GetMethod!);
			BuildDelegates(writer, getPropertyMethods, generatedDelegates);

			var setPropertyMethods = mockType.Properties
				.Where(_ => _.SetMethod is not null && _.SetMethod.RequiresProjectedDelegate &&
					_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
				.Select(_ => _.SetMethod!);
			BuildDelegates(writer, setPropertyMethods, generatedDelegates);

			var explicitGetPropertyMethodGroups = mockType.Properties
				.Where(_ => _.GetMethod is not null && _.GetMethod.RequiresProjectedDelegate &&
					_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
				.GroupBy(_ => _.ContainingType);

			foreach (var explicitGetPropertyMethodGroup in explicitGetPropertyMethodGroups)
			{
				BuildDelegate(writer, explicitGetPropertyMethodGroup.First().GetMethod!, generatedDelegates);
			}

			var explicitSetPropertyMethodGroups = mockType.Properties
				.Where(_ => _.SetMethod is not null && _.SetMethod.RequiresProjectedDelegate &&
					_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
				.GroupBy(_ => _.ContainingType);

			foreach (var explicitSetPropertyMethodGroup in explicitSetPropertyMethodGroups)
			{
				BuildDelegate(writer, explicitSetPropertyMethodGroup.First().SetMethod!, generatedDelegates);
			}
		}

		var generatedDelegates = new HashSet<string>();

		if (type.Methods.Length > 0)
		{
			var methods = type.Methods
				.Where(_ => _.RequiresProjectedDelegate &&
					_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No);
			BuildDelegates(writer, methods, generatedDelegates);

			var explicitMethodGroups = type.Methods
				.Where(_ => _.RequiresProjectedDelegate &&
					_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
				.GroupBy(_ => _.ContainingType);

			foreach (var explicitMethodGroup in explicitMethodGroups)
			{
				BuildDelegate(writer, explicitMethodGroup.First(), generatedDelegates);
			}
		}

		if (type.Properties.Length > 0)
		{
			BuildProperties(writer, type, generatedDelegates);
		}
	}
}