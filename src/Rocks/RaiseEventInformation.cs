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
	/// <remarks>
	/// The caller needs to ensure that the <paramref name="fieldName"/> matches the event name
	/// on the type to mock, and that the type of the object passed into <paramref name="args"/> is correct.
	/// Use <c>nameof()</c> to extract the event name correctly.
	/// </remarks>
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