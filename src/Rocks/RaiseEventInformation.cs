namespace Rocks;

[Serializable]
public sealed class RaiseEventInformation
{
	public RaiseEventInformation(string fieldName, EventArgs args) =>
		(this.FieldName, this.Args) = (fieldName, args);

	public EventArgs Args { get; }
	public string FieldName { get; }
}