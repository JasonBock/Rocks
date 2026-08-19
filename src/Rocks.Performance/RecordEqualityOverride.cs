using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis;

namespace Rocks.Performance;

[MemoryDiagnoser]
public class RecordEqualityOverride
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	private DefaultEquality defaultEquality1;
	private DefaultEquality defaultEquality2;
	private SpecificEquality specificEquality1;
	private SpecificEquality specificEquality2;

	[GlobalSetup]
	public void GlobalSetup()
	{
		var listType = typeof(List<string>);

		this.defaultEquality1 = new DefaultEquality
		{
			AttributesDescription = "DefaultMember",
			FlattenedName = "global::System.Collections.Generic.List_string",
			FullyQualifiedName = listType.FullName!,
			FullyQualifiedNameNoGenerics = listType.FullName!,
			Name = listType.Name,
			Namespace = listType.Namespace,
		};

		this.defaultEquality2 = new DefaultEquality
		{
			AttributesDescription = "DefaultMember",
			FlattenedName = "global::System.Collections.Generic.List_string",
			FullyQualifiedName = listType.FullName!,
			FullyQualifiedNameNoGenerics = listType.FullName!,
			Name = listType.Name,
			Namespace = listType.Namespace,
		};

		this.specificEquality1 = new SpecificEquality
		{
			AttributesDescription = "DefaultMember",
			FlattenedName = "global::System.Collections.Generic.List_string",
			FullyQualifiedName = listType.FullName!,
			FullyQualifiedNameNoGenerics = listType.FullName!,
			Name = listType.Name,
			Namespace = listType.Namespace,
		};

		this.specificEquality2 = new SpecificEquality
		{
			AttributesDescription = "DefaultMember",
			FlattenedName = "global::System.Collections.Generic.List_string",
			FullyQualifiedName = listType.FullName!,
			FullyQualifiedNameNoGenerics = listType.FullName!,
			Name = listType.Name,
			Namespace = listType.Namespace,
		};
	}

	[Benchmark(Baseline = true)]
	public bool GetDefaultEquality() =>
		this.defaultEquality1.Equals(this.defaultEquality2);

	[Benchmark]
	public bool GetSpecificEquality() =>
		this.specificEquality1.Equals(this.specificEquality2);
}

public sealed record DefaultEquality
{
	public bool AllowsRefLikeType { get; }
	public required string AttributesDescription { get; init; }
	public required string FlattenedName { get; init; }
	public required string FullyQualifiedName { get; init; }
	public required string FullyQualifiedNameNoGenerics { get; init; }
	public bool IsBasedOnTypeParameter { get; }
	public bool IsGenericType { get; }
	public bool IsOpenGeneric { get; }
	public bool IsPointer { get; }
	public bool IsRecord { get; }
	public bool IsReferenceType { get; }
	public bool IsRefLikeType { get; }
	public bool IsTupleType { get; }
	public required string Name { get; init; }
	public string? Namespace { get; init; }
	public bool RequiresProjectedArgument { get; }
	public NullableAnnotation NullableAnnotation { get; }
	public uint PointedAtCount { get; }
	public string? PointerNames { get; }
	public SpecialType SpecialType { get; }
	public TypeKind TypeKind { get; }
}

public sealed record SpecificEquality
{
	public bool Equals(SpecificEquality? other) =>
		this.FullyQualifiedName == other?.FullyQualifiedName;

	public override int GetHashCode() =>
		this.FullyQualifiedName.GetHashCode(StringComparison.InvariantCulture);

	public bool AllowsRefLikeType { get; }
	public required string AttributesDescription { get; init; }
	public required string FlattenedName { get; init; }
	public required string FullyQualifiedName { get; init; }
	public required string FullyQualifiedNameNoGenerics { get; init; }
	public bool IsBasedOnTypeParameter { get; }
	public bool IsGenericType { get; }
	public bool IsOpenGeneric { get; }
	public bool IsPointer { get; }
	public bool IsRecord { get; }
	public bool IsReferenceType { get; }
	public bool IsRefLikeType { get; }
	public bool IsTupleType { get; }
	public required string Name { get; init; }
	public string? Namespace { get; init; }
	public bool RequiresProjectedArgument { get; }
	public NullableAnnotation NullableAnnotation { get; }
	public uint PointedAtCount { get; }
	public string? PointerNames { get; }
	public SpecialType SpecialType { get; }
	public TypeKind TypeKind { get; }
}