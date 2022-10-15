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
		};
}

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
#pragma warning restore CS8618
#pragma warning restore CS0067