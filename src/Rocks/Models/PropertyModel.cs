using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

internal sealed record PropertyModel
{
	internal PropertyModel(IPropertySymbol property, ITypeReferenceModel mockType, ModelContext modelContext,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation, RequiresOverride requiresOverride,
		PropertyAccessor accessors, uint memberIdentifier)
	{
		var compilation = modelContext.SemanticModel.Compilation;

		(this.Type, this.MockType, this.RequiresExplicitInterfaceImplementation, this.RequiresOverride, this.Accessors, this.MemberIdentifier) =
			(modelContext.CreateTypeReference(property.Type), mockType, requiresExplicitInterfaceImplementation, requiresOverride, accessors, memberIdentifier);

		this.ContainingType = modelContext.CreateTypeReference(property.ContainingType);
		this.Name = property.Name;
		this.IsVirtual = property.IsVirtual;
		this.IsAbstract = property.IsAbstract;
		this.IsIndexer = property.IsIndexer;
		this.IsUnsafe = property.IsUnsafe();
		this.Parameters = [..property.Parameters.Select(
			_ => new ParameterModel(_, modelContext, requiresExplicitInterfaceImplementation: requiresExplicitInterfaceImplementation))];

		var allAttributes = property.GetAllAttributes();

		this.AttributesDescription = property.GetAttributes().GetDescription(compilation);
		this.AllAttributesDescription = allAttributes.GetDescription(compilation);

		this.ReturnsByRef = property.ReturnsByRef;
		this.ReturnsByRefReadOnly = property.ReturnsByRefReadonly;

		if (this.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
		{
			this.OverridingCodeValue = property.GetAccessibilityValue(compilation.Assembly);
		}

		if (this.Accessors == PropertyAccessor.Get || this.Accessors == PropertyAccessor.GetAndSet || this.Accessors == PropertyAccessor.GetAndInit)
		{
			this.GetMethod = new MethodModel(property.GetMethod!, mockType, modelContext,
				requiresExplicitInterfaceImplementation, requiresOverride, RequiresHiding.No, memberIdentifier);
			this.GetCanBeSeenByContainingAssembly = property.GetMethod!.CanBeSeenByContainingAssembly(compilation.Assembly, compilation);
		}

		if (this.Accessors == PropertyAccessor.Set || this.Accessors == PropertyAccessor.GetAndSet)
		{
			this.SetMethod = new MethodModel(property.SetMethod!, mockType, modelContext,
				requiresExplicitInterfaceImplementation, requiresOverride, RequiresHiding.No, memberIdentifier + 1);
			this.SetCanBeSeenByContainingAssembly = property.SetMethod!.CanBeSeenByContainingAssembly(compilation.Assembly, compilation);
		}

		if (this.Accessors == PropertyAccessor.Init || this.Accessors == PropertyAccessor.GetAndInit)
		{
			this.SetMethod ??= new MethodModel(property.SetMethod!, mockType, modelContext,
				requiresExplicitInterfaceImplementation, requiresOverride, RequiresHiding.No, memberIdentifier + 1);

			this.InitCanBeSeenByContainingAssembly = property.SetMethod!.CanBeSeenByContainingAssembly(compilation.Assembly, compilation);
		}
	}

	internal PropertyAccessor Accessors { get; }
	internal string AllAttributesDescription { get; }
	internal string AttributesDescription { get; }
	internal ITypeReferenceModel ContainingType { get; }
	internal bool GetCanBeSeenByContainingAssembly { get; }
	internal MethodModel? GetMethod { get; }
	internal bool InitCanBeSeenByContainingAssembly { get; }
	internal bool IsAbstract { get; }
	internal bool IsIndexer { get; }
	internal bool IsUnsafe { get; }
	internal bool IsVirtual { get; }
	internal uint MemberIdentifier { get; }
	internal ITypeReferenceModel MockType { get; }
	internal string Name { get; }
	internal string? OverridingCodeValue { get; }
	internal EquatableArray<ParameterModel> Parameters { get; }
	internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
	internal RequiresOverride RequiresOverride { get; }
	internal bool ReturnsByRef { get; }
	internal bool ReturnsByRefReadOnly { get; }
	internal bool SetCanBeSeenByContainingAssembly { get; }
	internal MethodModel? SetMethod { get; }
	internal ITypeReferenceModel Type { get; }
}