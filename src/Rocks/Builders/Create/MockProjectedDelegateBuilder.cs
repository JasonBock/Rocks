using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockProjectedDelegateBuilder
{
	internal static string GetProjectedCallbackDelegateFullyQualifiedName(MethodModel method, TypeReferenceModel typeToMock)
	{
		var delegateName = method.ProjectedCallbackDelegateName;
		var typeArgumentsNamingContext = method.IsGenericMethod ?
			new TypeArgumentsNamingContext(typeToMock) :
			new TypeArgumentsNamingContext();

		var typeArguments = typeToMock.IsOpenGeneric ?
			$"<{string.Join(", ", typeToMock.TypeArguments)}>" : string.Empty;
		var methodArguments = method.IsGenericMethod ?
			$"<{string.Join(", ", method.TypeArguments.Select(_ => _.BuildName(typeArgumentsNamingContext)))}>" : string.Empty;
		return $"global::{(typeToMock.Namespace is null ? "" : $"{typeToMock.Namespace}.")}{typeToMock.FlattenedName}CreateExpectations{typeArguments}.Projections.{delegateName}{methodArguments}";
	}

	internal static string GetProjectedReturnValueDelegateFullyQualifiedName(MethodModel method, TypeReferenceModel typeToMock)
	{
		var delegateName = method.ProjectedReturnValueDelegateName;
		var typeArguments = typeToMock.IsOpenGeneric ?
			$"<{string.Join(", ", typeToMock.TypeArguments)}>" : string.Empty;
		return $"global::{(typeToMock.Namespace is null ? "" : $"{typeToMock.Namespace}.")}{typeToMock.FlattenedName}CreateExpectations{typeArguments}.Projections.{delegateName}";
	}

	internal static string GetProjectedDelegate(MethodModel method)
	{
		var returnType = method.ReturnType.FullyQualifiedName;
		var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
		{
			var scoped = _.IsScoped ? "scoped " : string.Empty;
			var direction = _.RefKind == RefKind.Ref ? "ref " : _.RefKind == RefKind.Out ? "out " : string.Empty;
			var parameter = $"{scoped}{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.FullyQualifiedName} @{_.Name}";
			return $"{_.AttributesDescription}{parameter}";
		}));
		var isUnsafe = method.IsUnsafe ? "unsafe " : string.Empty;
		var constraints = method.Constraints;
		var methodConstraints = constraints.Length > 0 ?
			$" {string.Join(" ", constraints)}" : string.Empty;
		var typeArguments = method.IsGenericMethod ?
			$"<{string.Join(", ", method.TypeArguments)}>" : string.Empty;

		return $"internal {isUnsafe}delegate {returnType} {method.ProjectedCallbackDelegateName}{typeArguments}({methodParameters}){methodConstraints};";
	}

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