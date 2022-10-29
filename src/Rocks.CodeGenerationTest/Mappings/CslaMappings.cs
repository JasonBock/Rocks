using Csla;
using Csla.Core;
using Csla.Rules.CommonRules;

namespace Rocks.CodeGenerationTest.Mappings;

internal static class CslaMappings
{
	internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
		new()
		{
			{
				typeof(BusinessListBase<,>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.Mappings.MappedBusinessListBase" },
					{ "C", "global::Rocks.CodeGenerationTest.Mappings.MappedEditableListObject" },
				}
			},
			{
				typeof(BusinessBindingListBase<,>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.Mappings.MappedBusinessBindingListBase" },
					{ "C", "global::Rocks.CodeGenerationTest.Mappings.MappedEditableBusinessObject" },
				}
			},
			{
				typeof(ReadOnlyBindingListBase<,>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.Mappings.MappedReadOnlyBindingListBase" },
					{ "C", "global::System.Object" },
				}
			},
			{
				typeof(ReadOnlyBase<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.Mappings.MappedReadOnlyBase" },
				}
			},
			{
				typeof(DynamicBindingListBase<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.Mappings.MappedBusinessBase" },
				}
			},
			{
				typeof(DynamicListBase<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.Mappings.MappedBusinessBase" },
				}
			},
			{
				typeof(MaxValue<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.Mappings.MappedComparable" },
				}
			},
			{
				typeof(MinValue<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.Mappings.MappedComparable" },
				}
			},
			{
				typeof(CriteriaBase<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.Mappings.MappedCriteriaBase" },
				}
			},
			{
				typeof(BusinessBase<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.Mappings.MappedBusinessBase" },
				}
			},
			{
				typeof(CommandBase<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.Mappings.MappedCommandBase" },
				}
			},
			{
				typeof(ReadOnlyListBase<,>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.Mappings.MappedReadOnlyListBase" },
					{ "C", "global::System.Object" },
				}
			},
		};
}

[Serializable]
public sealed class MappedReadOnlyListBase
	: ReadOnlyListBase<MappedReadOnlyListBase, object>
{ }

[Serializable]
public sealed class MappedCommandBase
	: CommandBase<MappedCommandBase>
{ }

[Serializable]
public sealed class MappedCriteriaBase
	: CriteriaBase<MappedCriteriaBase>
{ }

public sealed class MappedComparable
	: IComparable
{
	public int CompareTo(object? obj) => throw new NotImplementedException();
}

[Serializable]
public sealed class MappedBusinessBase
	: BusinessBase<MappedBusinessBase>
{ }

[Serializable]
public sealed class MappedDynamicBindingListBase
	: DynamicBindingListBase<MappedBusinessBase>
{ }

[Serializable]
public sealed class MappedReadOnlyBase
	: ReadOnlyBase<MappedReadOnlyBase>
{ }

[Serializable]
public sealed class MappedReadOnlyBindingListBase
	: ReadOnlyBindingListBase<MappedReadOnlyBindingListBase, object>
{ }

public sealed class MappedEditableBusinessObject
	: IEditableBusinessObject
{
	public int EditLevelAdded { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	public int Identity => throw new NotImplementedException();

	public int EditLevel => throw new NotImplementedException();

	public bool IsValid => throw new NotImplementedException();

	public bool IsSelfValid => throw new NotImplementedException();

	public bool IsDirty => throw new NotImplementedException();

	public bool IsSelfDirty => throw new NotImplementedException();

	public bool IsDeleted => throw new NotImplementedException();

	public bool IsNew => throw new NotImplementedException();

	public bool IsSavable => throw new NotImplementedException();

	public bool IsChild => throw new NotImplementedException();

	public bool IsBusy => throw new NotImplementedException();

	public bool IsSelfBusy => throw new NotImplementedException();

#pragma warning disable CS0067
	public event BusyChangedEventHandler? BusyChanged;
	public event EventHandler<Csla.Core.ErrorEventArgs>? UnhandledAsyncException;
#pragma warning restore CS0067

	public void AcceptChanges(int parentEditLevel, bool parentBindingEdit) => throw new NotImplementedException();
	public void ApplyEdit() => throw new NotImplementedException();
	public void BeginEdit() => throw new NotImplementedException();
	public void CancelEdit() => throw new NotImplementedException();
	public void CopyState(int parentEditLevel, bool parentBindingEdit) => throw new NotImplementedException();
	public void Delete() => throw new NotImplementedException();
	public void DeleteChild() => throw new NotImplementedException();
	public void SetParent(IParent parent) => throw new NotImplementedException();
	public void UndoChanges(int parentEditLevel, bool parentBindingEdit) => throw new NotImplementedException();
}

[Serializable]
public sealed class MappedBusinessBindingListBase
	: BusinessBindingListBase<MappedBusinessBindingListBase, MappedEditableBusinessObject>
{ }

public sealed class MappedEditableListObject
	: IEditableBusinessObject
{
	public int EditLevelAdded { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	public int Identity => throw new NotImplementedException();

	public int EditLevel => throw new NotImplementedException();

	public bool IsValid => throw new NotImplementedException();

	public bool IsSelfValid => throw new NotImplementedException();

	public bool IsDirty => throw new NotImplementedException();

	public bool IsSelfDirty => throw new NotImplementedException();

	public bool IsDeleted => throw new NotImplementedException();

	public bool IsNew => throw new NotImplementedException();

	public bool IsSavable => throw new NotImplementedException();

	public bool IsChild => throw new NotImplementedException();

	public bool IsBusy => throw new NotImplementedException();

	public bool IsSelfBusy => throw new NotImplementedException();

#pragma warning disable CS0067
	public event BusyChangedEventHandler? BusyChanged;
	public event EventHandler<Csla.Core.ErrorEventArgs>? UnhandledAsyncException;
#pragma warning restore CS0067

	public void AcceptChanges(int parentEditLevel, bool parentBindingEdit) => throw new NotImplementedException();
	public void ApplyEdit() => throw new NotImplementedException();
	public void BeginEdit() => throw new NotImplementedException();
	public void CancelEdit() => throw new NotImplementedException();
	public void CopyState(int parentEditLevel, bool parentBindingEdit) => throw new NotImplementedException();
	public void Delete() => throw new NotImplementedException();
	public void DeleteChild() => throw new NotImplementedException();
	public void SetParent(IParent parent) => throw new NotImplementedException();
	public void UndoChanges(int parentEditLevel, bool parentBindingEdit) => throw new NotImplementedException();
}

[Serializable]
public sealed class MappedBusinessListBase
	: BusinessListBase<MappedBusinessListBase, MappedEditableListObject>
{ }