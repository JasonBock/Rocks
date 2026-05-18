using Microsoft.CodeAnalysis;
using Rocks.Analysis.Builders.Create;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Models;

internal sealed record MethodModel
{
	internal MethodModel(IMethodSymbol method, ITypeReferenceModel mockType, ModelContext modelContext,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation,
		RequiresOverride requiresOverride, RequiresHiding requiresHiding, uint memberIdentifier)
	{
		var compilation = modelContext.SemanticModel.Compilation;

		(this.MockType, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.MemberIdentifier) =
			(mockType, requiresExplicitInterfaceImplementation, requiresOverride, memberIdentifier);

		this.ContainingType = modelContext.CreateTypeReference(method.ContainingType);

		if (requiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
		{
			this.OverridingCodeValue = method.GetAccessibilityValue(
				modelContext.SemanticModel.Compilation.Assembly);
		}

		this.IsMarkedWithDoesNotReturn = method.IsMarkedWithDoesNotReturn(compilation);
		this.ShouldThrowDoesNotReturnException = this.IsMarkedWithDoesNotReturn;

		this.IsAbstract = method.IsAbstract;
		this.IsVirtual = method.IsVirtual;
		this.IsGenericMethod = method.IsGenericMethod;
		this.IsUnsafe = method.IsUnsafe();

		this.RequiresHiding = requiresHiding;
		this.MethodKind = method.MethodKind;
		this.Constraints = method.GetConstraints(compilation);
		this.DefaultConstraints = method.GetDefaultConstraints();
		this.TypeArguments = [.. method.TypeArguments.Select(_ => modelContext.CreateTypeReference(_))];
		this.TypeParameters = [.. method.TypeParameters.Select(_ => modelContext.CreateTypeReference(_))];

		this.Name = method.Name;

		this.Parameters = [..method.Parameters.Select(
			_ => new ParameterModel(_, modelContext, requiresExplicitInterfaceImplementation: requiresExplicitInterfaceImplementation))];

		this.ReturnType = modelContext.CreateTypeReference(method.ReturnType);
		this.ReturnsVoid = method.ReturnsVoid;
		this.ReturnsByRef = method.ReturnsByRef;
		this.ReturnsByRefReadOnly = method.ReturnsByRefReadonly;

		this.AttributesDescription = method.GetAttributes().GetDescription(compilation);
		this.ReturnTypeAttributesDescription = method.GetReturnTypeAttributes().GetDescription(compilation, AttributeTargets.ReturnValue);
		this.RequiresProjectedDelegate = method.RequiresProjectedDelegate(compilation);

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

		this.ReturnTypeTypeArguments = method.ReturnType is INamedTypeSymbol returnType ?
			[.. returnType.TypeArguments.Select(_ => modelContext.CreateTypeReference(_))] : [];
		this.Hash = this.GetHash();
	}

	// This is primarily done to get a unique name
	// for the related adornments type.
	private string GetHash()
	{
		var typeArgumentsNamingContext = this.IsGenericMethod ?
			new TypeArgumentsNamingContext(this) :
			new TypeArgumentsNamingContext();

		// We add the containing type name if it needs explicit implementation
		// to prevent collisions between members on multiple interfaces.
		// See ExplicitInterfaceImplementationTests in IntegrationTests
		// for an example :)
		var fingerprint = string.Concat(
			this.Parameters.Select(parameter =>
				{
					var direction = parameter.RefKind == RefKind.Ref ?
						"ref" :
						parameter.RefKind == RefKind.Out ?
							"out" :
							string.Empty;
					var name = parameter.Type.FullyQualifiedName;
					return $"{name}{direction}";
				}).Concat(
					this.Constraints.Select(_ => _.ToString(typeArgumentsNamingContext, this)))
				.Concat(
					[
						this.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
							this.ContainingType.FullyQualifiedName :
							""
					]));

		// Lifted from: https://andrewlock.net/why-is-string-gethashcode-different-each-time-i-run-my-program-in-net-core/
		unchecked
		{
			var hash1 = (5381 << 16) + 5381;
			var hash2 = hash1;

			for (var i = 0; i < fingerprint.Length; i += 2)
			{
				hash1 = ((hash1 << 5) + hash1) ^ fingerprint[i];

				if (i == fingerprint.Length - 1)
				{
					break;
				}

				hash2 = ((hash2 << 5) + hash2) ^ fingerprint[i + 1];
			}

			return (hash1 + (hash2 * 1566083941)).ToString("X8");
		}
	}

	internal string AttributesDescription { get; }
	internal EquatableArray<Constraints> Constraints { get; }
	internal ITypeReferenceModel ContainingType { get; }
	internal EquatableArray<Constraints> DefaultConstraints { get; }
	internal string Hash { get; }
	internal bool IsAbstract { get; }
	internal bool IsGenericMethod { get; }
	internal bool IsMarkedWithDoesNotReturn { get; }
	internal bool IsUnsafe { get; }
	internal bool IsVirtual { get; }
	internal uint MemberIdentifier { get; }
	internal MethodKind MethodKind { get; }
	internal ITypeReferenceModel MockType { get; }
	internal string Name { get; }
	internal string? OverridingCodeValue { get; }
	internal EquatableArray<ParameterModel> Parameters { get; }
	internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
	internal RequiresHiding RequiresHiding { get; }
	internal RequiresOverride RequiresOverride { get; }
	internal bool RequiresProjectedDelegate { get; }
	internal ITypeReferenceModel ReturnType { get; }
	internal string ReturnTypeAttributesDescription { get; }
	internal bool ReturnTypeIsTaskOfTType { get; }
	internal bool ReturnTypeIsTaskOfTTypeAndIsNullForgiving { get; }
	internal bool ReturnTypeIsTaskType { get; }
	internal bool ReturnTypeIsValueTaskOfTType { get; }
	internal bool ReturnTypeIsValueTaskOfTTypeAndIsNullForgiving { get; }
	internal bool ReturnTypeIsValueTaskType { get; }
	internal EquatableArray<ITypeReferenceModel> ReturnTypeTypeArguments { get; }
	internal bool ReturnsVoid { get; }
	internal bool ReturnsByRef { get; }
	internal bool ReturnsByRefReadOnly { get; }
	internal bool ShouldThrowDoesNotReturnException { get; }
	internal EquatableArray<ITypeReferenceModel> TypeArguments { get; }
	internal EquatableArray<ITypeReferenceModel> TypeParameters { get; }
}