using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class MockProjectedDelegateBuilder
{
	internal static string GetProjectedCallbackDelegateName(IMethodSymbol method) =>
		method.GetName(extendedName: $"Callback_{(uint)method.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat).GetHashCode()}");

	internal static string GetProjectedReturnValueDelegateName(IMethodSymbol method) =>
		method.GetName(extendedName: $"ReturnValue_{(uint)method.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat).GetHashCode()}");

	internal static string GetProjectedDelegate(IMethodSymbol method, Compilation compilation)
	{
		var returnType = method.ReturnType.GetReferenceableName();
		var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
		{
			var direction = _.RefKind == RefKind.Ref ? "ref " : _.RefKind == RefKind.Out ? "out " : string.Empty;
			var parameter = $"{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.GetReferenceableName()} {_.Name}";
			return $"{(_.GetAttributes().Length > 0 ? $"{_.GetAttributes().GetDescription(compilation)} " : string.Empty)}{parameter}";
		}));
		var isUnsafe = method.IsUnsafe() ? "unsafe " : string.Empty;
		return $"internal {isUnsafe}delegate {returnType} {MockProjectedDelegateBuilder.GetProjectedCallbackDelegateName(method)}({methodParameters});";
	}

	internal static string GetProjectedReturnValueDelegate(IMethodSymbol method, Compilation compilation)
	{
		var returnType = method.ReturnType.GetReferenceableName();
		return $"internal delegate {returnType} {MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateName(method)}();";
	}

	internal static void Build(IndentedTextWriter writer, MockInformation information, Compilation compilation)
	{
		static void BuildDelegate(IndentedTextWriter writer, IMethodSymbol method, Compilation compilation)
		{
			writer.WriteLine(MockProjectedDelegateBuilder.GetProjectedDelegate(method, compilation));

			if(method.ReturnType.IsRefLikeType)
			{
				writer.WriteLine(MockProjectedDelegateBuilder.GetProjectedReturnValueDelegate(method, compilation));
			}
		}

		static void BuildDelegates(IndentedTextWriter writer, IEnumerable<IMethodSymbol> methods, Compilation compilation)
		{
			foreach (var method in methods)
			{
				BuildDelegate(writer, method, compilation);
			}
		}

		static void BuildProperties(IndentedTextWriter writer, MockInformation information, Compilation compilation)
		{
			var getPropertyMethods = information.Properties
				.Where(_ => _.Value.GetMethod is not null && _.Value.GetMethod.RequiresProjectedDelegate() &&
					_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
				.Select(_ => _.Value.GetMethod!);
			BuildDelegates(writer, getPropertyMethods, compilation);

			var setPropertyMethods = information.Properties
				.Where(_ => _.Value.SetMethod is not null && _.Value.SetMethod.RequiresProjectedDelegate() &&
					_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
				.Select(_ => _.Value.SetMethod!);
			BuildDelegates(writer, setPropertyMethods, compilation);

			var explicitGetPropertyMethodGroups = information.Properties
				.Where(_ => _.Value.GetMethod is not null && _.Value.GetMethod.RequiresProjectedDelegate() &&
					_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
				.GroupBy(_ => _.Value.ContainingType);

			foreach (var explicitGetPropertyMethodGroup in explicitGetPropertyMethodGroups)
			{
				BuildDelegate(writer, explicitGetPropertyMethodGroup.First().Value.GetMethod!, compilation);
			}

			var explicitSetPropertyMethodGroups = information.Properties
				.Where(_ => _.Value.SetMethod is not null && _.Value.SetMethod.RequiresProjectedDelegate() &&
					_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
				.GroupBy(_ => _.Value.ContainingType);

			foreach (var explicitSetPropertyMethodGroup in explicitSetPropertyMethodGroups)
			{
				BuildDelegate(writer, explicitSetPropertyMethodGroup.First().Value.SetMethod!, compilation);
			}
		}

		if (information.Methods.Length > 0)
		{
			var methods = information.Methods
				.Where(_ => _.Value.RequiresProjectedDelegate() &&
					_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
				.Select(_ => _.Value);
			BuildDelegates(writer, methods, compilation);

			var explicitMethodGroups = information.Methods
				.Where(_ => _.Value.RequiresProjectedDelegate() &&
					_.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
				.GroupBy(_ => _.Value.ContainingType);

			foreach (var explicitMethodGroup in explicitMethodGroups)
			{
				BuildDelegate(writer, explicitMethodGroup.First().Value, compilation);
			}
		}

		if (information.Properties.Length > 0)
		{
			BuildProperties(writer, information, compilation);
		}
	}
}