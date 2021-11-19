namespace Rocks;

// This attribute is emitted into members on the mock type
// so the verification call can get a meaningful stringified description
// of the member that failed.
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
public sealed class MemberIdentifierAttribute
	 : Attribute
{
	public MemberIdentifierAttribute(int value, string description) =>
		(this.Value, this.Description) = (value, description);

	public string Description { get; }
	public int Value { get; }
}