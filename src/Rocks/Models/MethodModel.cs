using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

/// <summary>
/// Defines a method that can be mocked.
/// </summary>
internal record MethodModel
{
	/// <summary>
	/// Creates a new <see cref="MethodMockableResult"/> instance.
	/// </summary>
	/// <param name="method">The <see cref="IMethodSymbol"/> to obtain information from.</param>
	/// <param name="requiresExplicitInterfaceImplementation">Specifies if <paramref name="method"/> requires explicit implementation.</param>
	/// <param name="requiresOverride">Specifies if <paramref name="method"/> requires an override.</param>
	/// <param name="memberIdentifier">The member identifier.</param>
	/// <param name="compilation">The compilation.</param>
	internal MethodModel(IMethodSymbol method, Compilation compilation,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation,
		RequiresOverride requiresOverride, uint memberIdentifier)
	{
		(this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.MemberIdentifier) =
			 (requiresExplicitInterfaceImplementation, requiresOverride, memberIdentifier);

		if (requiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes)
		{
			this.ContainingTypeFullyQualifiedName = method.ContainingType.GetFullyQualifiedName();
			this.ContainingTypeFlattenedName = method.ContainingType.GetName(TypeNameOption.Flatten);
		}
		else
		{
			this.OverridingCodeValue = method.GetOverridingCodeValue(compilation.Assembly);
		}

		this.IsAbstract = method.IsAbstract;
		this.Constraints = method.GetConstraints();
		this.DefaultConstraints = method.GetDefaultConstraints();
		this.ContainingTypeKind = method.ContainingType.TypeKind;
		this.Name = method.GetName();
		this.IsUnsafe = method.IsUnsafe();
		this.ShouldThrowDoesNotReturnException = method.IsMarkedWithDoesNotReturn(compilation);
		this.Parameters = method.Parameters.Select(_ => new ParameterModel(_, compilation)).ToImmutableArray();

		this.ReturnType = new TypeReferenceModel(method.ReturnType, compilation);
		this.ReturnsVoid = method.ReturnsVoid;
		this.ReturnsByRef = method.ReturnsByRef;
		this.ReturnsByRefReadOnly = method.ReturnsByRefReadonly;

		this.AttributesDescription = method.GetAttributes().GetDescription(compilation);
		this.RequiresProjectedDelegate = method.RequiresProjectedDelegate();

		if (this.RequiresProjectedDelegate)
		{
			this.ProjectedCallbackDelegateName = method.GetName(extendedName: $"Callback_{method.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat).GetHash()}");
		}

		this.ReturnTypeIsRefLikeType = method.ReturnType.IsRefLikeType;

		if (this.ReturnTypeIsRefLikeType)
		{
			this.ProjectedReturnValueDelegateName = method.GetName(extendedName: $"ReturnValue_{method.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat).GetHash()}");
		}
	}

   internal string? ProjectedReturnValueDelegateName { get; }
   internal string? ProjectedCallbackDelegateName { get; }
   internal bool RequiresProjectedDelegate { get; }
	internal bool IsUnsafe { get; }
	internal string? ContainingTypeFlattenedName { get; }
	internal string? ContainingTypeFullyQualifiedName { get; }
	internal bool IsAbstract { get; }
	internal EquatableArray<string> Constraints { get; }
	internal EquatableArray<string> DefaultConstraints { get; }
	internal TypeKind ContainingTypeKind { get; }
	internal string Name { get; }
	internal bool ShouldThrowDoesNotReturnException { get; }
	internal EquatableArray<ParameterModel> Parameters { get; }
   internal TypeReferenceModel ReturnType { get; }
   internal bool ReturnsVoid { get; }
	internal bool ReturnsByRef { get; }
	internal bool ReturnsByRefReadOnly { get; }
	internal bool ReturnTypeIsRefLikeType { get; }
	internal string AttributesDescription { get; }
	internal string? OverridingCodeValue { get; }

	/// <summary>
	/// Gets the member identifier.
	/// </summary>
	internal uint MemberIdentifier { get; }

	/// <summary>
	/// Gets the <see cref="RequiresExplicitInterfaceImplementation"/> value that specifies if this result
	/// needs explicit implementation.
	/// </summary>
	internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }

	/// <summary>
	/// Gets the <see cref="RequiresOverride"/> value that specifies if this result
	/// needs an override.
	/// </summary>
	internal RequiresOverride RequiresOverride { get; }
}
