#pragma warning disable CS0067
#pragma warning disable CS8618

namespace Rocks.CodeGenerationTest;

public static class MappedTypes
{
	public static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
		new()
		{
			{
				typeof(Csla.BusinessListBase<,>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.MappedBusinessListBase" },
					{ "C", "global::Rocks.CodeGenerationTest.MappedEditableListObject" },
				}
			},
			{
				typeof(Csla.BusinessBindingListBase<,>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.MappedBusinessBindingListBase" },
					{ "C", "global::Rocks.CodeGenerationTest.MappedEditableBusinessObject" },
				}
			},
			{
				typeof(Csla.ReadOnlyBindingListBase<,>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.MappedReadOnlyBindingListBase" },
					{ "C", "global::System.Object" },
				}
			},
			{
				typeof(Csla.ReadOnlyBase<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.MappedReadOnlyBase" },
				}
			},
			{
				typeof(Csla.DynamicBindingListBase<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.MappedBusinessBase" },
				}
			},
			{
				typeof(Csla.DynamicListBase<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.MappedBusinessBase" },
				}
			},
			{
				typeof(Csla.Rules.CommonRules.MaxValue<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.MappedComparable" },
				}
			},
			{
				typeof(Csla.Rules.CommonRules.MinValue<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.MappedComparable" },
				}
			},
			{
				typeof(Csla.CriteriaBase<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.MappedCriteriaBase" },
				}
			},
			{
				typeof(Csla.BusinessBase<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.MappedBusinessBase" },
				}
			},
			{
				typeof(Csla.CommandBase<>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.MappedCommandBase" },
				}
			},
			{
				typeof(Csla.ReadOnlyListBase<,>), new()
				{
					{ "T", "global::Rocks.CodeGenerationTest.MappedReadOnlyListBase" },
					{ "C", "global::System.Object" },
				}
			},
		};
}

#region Mapped Types
[System.Serializable]
public sealed class MappedReadOnlyListBase
	: Csla.ReadOnlyListBase<MappedReadOnlyListBase, object>
{ }

[System.Serializable]
public sealed class MappedCommandBase
	: Csla.CommandBase<MappedCommandBase>
{ }

[System.Serializable]
public sealed class MappedCriteriaBase
	: Csla.CriteriaBase<MappedCriteriaBase>
{ }

public sealed class MappedComparable
	: System.IComparable
{
   public int CompareTo(object? obj) => throw new NotImplementedException();
}

[System.Serializable]
public sealed class MappedBusinessBase
	: Csla.BusinessBase<MappedBusinessBase>
{ }

[System.Serializable]
public sealed class MappedDynamicBindingListBase
	: Csla.DynamicBindingListBase<MappedBusinessBase>
{ }

[System.Serializable]
public sealed class MappedReadOnlyBase
	: Csla.ReadOnlyBase<MappedReadOnlyBase>
{ }

[System.Serializable]
public sealed class MappedReadOnlyBindingListBase
	: Csla.ReadOnlyBindingListBase<MappedReadOnlyBindingListBase, object>
{ }

public sealed class MappedEditableBusinessObject
	: Csla.Core.IEditableBusinessObject
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

	public event Csla.Core.BusyChangedEventHandler BusyChanged;
	public event EventHandler<Csla.Core.ErrorEventArgs> UnhandledAsyncException;

	public void AcceptChanges(int parentEditLevel, bool parentBindingEdit) => throw new NotImplementedException();
	public void ApplyEdit() => throw new NotImplementedException();
	public void BeginEdit() => throw new NotImplementedException();
	public void CancelEdit() => throw new NotImplementedException();
	public void CopyState(int parentEditLevel, bool parentBindingEdit) => throw new NotImplementedException();
	public void Delete() => throw new NotImplementedException();
	public void DeleteChild() => throw new NotImplementedException();
	public void SetParent(Csla.Core.IParent parent) => throw new NotImplementedException();
	public void UndoChanges(int parentEditLevel, bool parentBindingEdit) => throw new NotImplementedException();
}

[System.Serializable]
public sealed class MappedBusinessBindingListBase
	: Csla.BusinessBindingListBase<MappedBusinessBindingListBase, MappedEditableBusinessObject>
{ }

public sealed class MappedEditableListObject
	: Csla.Core.IEditableBusinessObject
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

	public event Csla.Core.BusyChangedEventHandler BusyChanged;
	public event EventHandler<Csla.Core.ErrorEventArgs> UnhandledAsyncException;

	public void AcceptChanges(int parentEditLevel, bool parentBindingEdit) => throw new NotImplementedException();
	public void ApplyEdit() => throw new NotImplementedException();
	public void BeginEdit() => throw new NotImplementedException();
	public void CancelEdit() => throw new NotImplementedException();
	public void CopyState(int parentEditLevel, bool parentBindingEdit) => throw new NotImplementedException();
	public void Delete() => throw new NotImplementedException();
	public void DeleteChild() => throw new NotImplementedException();
	public void SetParent(Csla.Core.IParent parent) => throw new NotImplementedException();
	public void UndoChanges(int parentEditLevel, bool parentBindingEdit) => throw new NotImplementedException();
}

[System.Serializable]
public sealed class MappedBusinessListBase
	: Csla.BusinessListBase<MappedBusinessListBase, MappedEditableListObject>
{ }
#endregion

#pragma warning restore CS8618
#pragma warning restore CS0067