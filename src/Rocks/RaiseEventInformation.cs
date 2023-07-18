namespace Rocks;

/// <summary>
/// Defines event information from a field on a mock.
/// </summary>
[Serializable]
public sealed class RaiseEventInformation
{
	/// <summary>
	/// Creates a new <see cref="RaiseEventInformation"/> instance.
	/// </summary>
	/// <param name="fieldName">The field name.</param>
	/// <param name="args">The event arguments.</param>
	public RaiseEventInformation(string fieldName, object args) =>
		(this.FieldName, this.Args) = (fieldName, args);

	/// <summary>
	/// Gets the event arguments.
	/// </summary>
	public object Args { get; }
	/// <summary>
	/// Gets the field name.
	/// </summary>
	public string FieldName { get; }
}