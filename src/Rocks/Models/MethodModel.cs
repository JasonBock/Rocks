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

		this.IsMarkedWithDoesNotReturn = method.IsMarkedWithDoesNotReturn();
		this.ShouldThrowDoesNotReturnException = this.IsMarkedWithDoesNotReturn;

		this.IsAbstract = method.IsAbstract;
		this.IsVirtual = method.IsVirtual;
		this.IsGenericMethod = method.IsGenericMethod;
		this.IsUnsafe = method.IsUnsafe();

		this.MethodKind = method.MethodKind;
		this.Constraints = method.GetConstraints();
		this.DefaultConstraints = method.GetDefaultConstraints();
		this.Name = method.GetName();

		this.Parameters = method.Parameters.Select(_ =>
		{
			return new ParameterModel(_, this.MockType, compilation);
		}).ToImmutableArray();

		this.ReturnType = new TypeReferenceModel(method.ReturnType, compilation);
		this.ReturnsVoid = method.ReturnsVoid;
		this.ReturnsByRef = method.ReturnsByRef;
		this.ReturnsByRefReadOnly = method.ReturnsByRefReadonly;

		this.AttributesDescription = method.GetAttributes().GetDescription(compilation);
		this.ReturnTypeAttributesDescription = method.GetReturnTypeAttributes().GetDescription(compilation, AttributeTargets.ReturnValue);
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
			this.ReturnTypeTypeArguments = returnType.TypeArguments.Select(_ => new TypeReferenceModel(_, compilation)).ToImmutableArray();
		}
		else
		{
			this.ReturnTypeTypeArguments = ImmutableArray<TypeReferenceModel>.Empty;
		}
	}

	internal string AttributesDescription { get; }
	internal EquatableArray<string> Constraints { get; }
	internal TypeReferenceModel ContainingType { get; }
	internal EquatableArray<string> DefaultConstraints { get; }
	internal bool IsAbstract { get; }
	internal bool IsGenericMethod { get; }
	internal bool IsMarkedWithDoesNotReturn { get; }
	internal bool IsUnsafe { get; }
	internal bool IsVirtual { get; }
	internal uint MemberIdentifier { get; }
	internal MethodKind MethodKind { get; }
	internal TypeReferenceModel MockType { get; }
	internal string Name { get; }
	internal string? OverridingCodeValue { get; }
	internal EquatableArray<ParameterModel> Parameters { get; }
	internal string? ProjectedCallbackDelegateName { get; }
	internal string? ProjectedReturnValueDelegateName { get; }
	internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
	internal RequiresOverride RequiresOverride { get; }
	internal bool RequiresProjectedDelegate { get; }
	internal TypeReferenceModel ReturnType { get; }
	internal string ReturnTypeAttributesDescription { get; }
	internal bool ReturnTypeIsTaskOfTType { get; }
	internal bool ReturnTypeIsTaskOfTTypeAndIsNullForgiving { get; }
	internal bool ReturnTypeIsTaskType { get; }
	internal bool ReturnTypeIsValueTaskOfTType { get; }
	internal bool ReturnTypeIsValueTaskOfTTypeAndIsNullForgiving { get; }
	internal bool ReturnTypeIsValueTaskType { get; }
	internal EquatableArray<TypeReferenceModel> ReturnTypeTypeArguments { get; }
	internal bool ReturnsVoid { get; }
	internal bool ReturnsByRef { get; }
	internal bool ReturnsByRefReadOnly { get; }
	internal bool ShouldThrowDoesNotReturnException { get; }
}