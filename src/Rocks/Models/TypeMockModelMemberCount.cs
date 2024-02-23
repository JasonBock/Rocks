namespace Rocks.Models;

// Note that the PropertyCount is expected to be the sum
// of the property's accessors.
// That is, if a property has just a "get" or "set" or "init", that counts as 1.
// If the property has a "get/set", that counts as 2.
internal sealed record TypeMockModelMemberCount
{
	internal TypeMockModelMemberCount(int methodCount, int propertyCount) =>
		(this.MethodCount, this.PropertyCount, this.TotalCount) =
			(methodCount, propertyCount, methodCount + propertyCount);

	internal int MethodCount { get; }
	internal int PropertyCount { get; }
	internal int TotalCount { get; }
};