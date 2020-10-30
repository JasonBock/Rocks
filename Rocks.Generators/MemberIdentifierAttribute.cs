using System;

namespace Rocks
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class MemberIdentifierAttribute
		: Attribute
	{
		public MemberIdentifierAttribute(int value, string description) =>
			(this.Value, this.Description) = (value, description);

		public string Description { get; }
		public int Value { get; }
	}
}