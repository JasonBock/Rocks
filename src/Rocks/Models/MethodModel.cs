using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

internal sealed record MethodModel
{
	internal MethodModel(IMethodSymbol method, TypeReferenceModel mockType, Compilation compilation,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation,
		RequiresOverride requiresOverride, uint memberIdentifier)
	{
		(this.MockType, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.MemberIdentifier) =
			 (mockType, requiresExplicitInterfaceImplementation, requiresOverride, memberIdentifier);

		this.ContainingType = new TypeReferenceModel(method.ContainingType, compilation);

		if (requiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
		{
			this.OverridingCodeValue = method.GetOverridingCodeValue(compilation.Assembly);
		}

		this.IsMarkedWithDoesNotReturn = method.IsMarkedWithDoesNotReturn(compilation);
		this.IsAbstract = method.IsAbstract;
		this.IsVirtual = method.IsVirtual;
		this.IsGenericMethod = method.IsGenericMethod;
		this.MethodKind = method.MethodKind;
		this.Constraints = method.GetConstraints();
		this.DefaultConstraints = method.GetDefaultConstraints();
		this.ContainingTypeKind = method.ContainingType.TypeKind;
		this.Name = method.GetName();
		this.IsUnsafe = method.IsUnsafe();
		this.ShouldThrowDoesNotReturnException = method.IsMarkedWithDoesNotReturn(compilation);
		this.Parameters = method.Parameters.Select(_ => new ParameterModel(_, this.MockType, compilation)).ToImmutableArray();

		this.ReturnType = new TypeReferenceModel(method.ReturnType, compilation);
		this.ReturnsVoid = method.ReturnsVoid;
		this.ReturnsByRef = method.ReturnsByRef;
		this.ReturnsByRefReadOnly = method.ReturnsByRefReadonly;

		this.AttributesDescription = method.GetAttributes().GetDescription(compilation);
		this.ReturnTypeAttributesDescription = method.GetReturnTypeAttributes().GetDescription(compilation);
		this.RequiresProjectedDelegate = method.RequiresProjectedDelegate();

		if (this.RequiresProjectedDelegate)
		{
			this.ProjectedCallbackDelegateName = method.GetName(extendedName: $"Callback_{method.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat).GetHash()}");
		}

		if (this.ReturnType.IsRefLikeType)
		{
			this.ProjectedReturnValueDelegateName = method.GetName(extendedName: $"ReturnValue_{method.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat).GetHash()}");
		}

		if (!this.ReturnsVoid)
		{
			var taskType = compilation.GetTypeByMetadataName(typeof(Task).FullName);
			var taskOfTType = compilation.GetTypeByMetadataName(typeof(Task<>).FullName);
			var valueTaskType = compilation.GetTypeByMetadataName(typeof(ValueTask).FullName);
			var valueTaskOfTType = compilation.GetTypeByMetadataName(typeof(ValueTask<>).FullName);

			if (method.ReturnType.Equals(taskType))
			{
				this.ReturnTypeIsTaskType = true;
			}
			else if (method.ReturnType.Equals(valueTaskType))
			{
				this.ReturnTypeIsValueTaskType = true;
			}
			else if (method.ReturnType.OriginalDefinition.Equals(taskOfTType))
			{
				this.ReturnTypeIsTaskOfTType = true;
				var taskReturnType = (method.ReturnType as INamedTypeSymbol)!;
				this.ReturnTypeIsTaskOfTTypeAndIsNullForgiving = taskReturnType.TypeArgumentNullableAnnotations[0] == NullableAnnotation.Annotated;
			}
			else if (method.ReturnType.OriginalDefinition.Equals(valueTaskOfTType))
			{
				this.ReturnTypeIsValueTaskOfTType = true;
				var taskReturnType = (method.ReturnType as INamedTypeSymbol)!;
				this.ReturnTypeIsValueTaskOfTTypeAndIsNullForgiving = taskReturnType.TypeArgumentNullableAnnotations[0] == NullableAnnotation.Annotated;
			}
		}

		if (method.ReturnType is INamedTypeSymbol returnType)
		{
			this.TypeArguments = returnType.TypeArguments.Select(_ => new TypeReferenceModel(_, compilation)).ToImmutableArray();
		}
		else
		{
			this.TypeArguments = ImmutableArray<TypeReferenceModel>.Empty;
		}
	}

	internal EquatableArray<TypeReferenceModel> TypeArguments { get; }
	internal bool ReturnTypeIsValueTaskOfTTypeAndIsNullForgiving { get; }
   internal bool ReturnTypeIsTaskOfTTypeAndIsNullForgiving { get; }
   internal bool ReturnTypeIsTaskType { get; }
	internal bool ReturnTypeIsValueTaskType { get; }
	internal bool ReturnTypeIsTaskOfTType { get; }
	internal bool ReturnTypeIsValueTaskOfTType { get; }
	internal bool IsMarkedWithDoesNotReturn { get; }
	/// <summary>
	/// Gets the type that contains the method.
	/// </summary>
	internal TypeReferenceModel ContainingType { get; }
	/// <summary>
	/// Gets the mock type.
	/// </summary>
	internal TypeReferenceModel MockType { get; }
	internal string? ProjectedReturnValueDelegateName { get; }
	internal string? ProjectedCallbackDelegateName { get; }
	internal bool RequiresProjectedDelegate { get; }
	internal bool IsUnsafe { get; }
	internal bool IsAbstract { get; }
	internal bool IsVirtual { get; }
	internal bool IsGenericMethod { get; }
	internal MethodKind MethodKind { get; }
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
	internal string AttributesDescription { get; }
	internal string ReturnTypeAttributesDescription { get; }
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
