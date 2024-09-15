namespace Rocks.Models;

internal sealed record ProjectedModelInformation
{
	internal ProjectedModelInformation(TypeReferenceModel type)
	{
		if (type.IsPointer)
		{
			this.PointerCount = (uint)type.FullyQualifiedName.Count(c => c == '*');
			this.PointerNames = string.Concat(Enumerable.Repeat("Pointer", (int)this.PointerCount));
		}
		else
		{
			this.Type = type;
		}
	}

	internal uint PointerCount { get; init; }
	internal string? PointerNames { get; init; }
	internal TypeReferenceModel? Type { get; init; }
}