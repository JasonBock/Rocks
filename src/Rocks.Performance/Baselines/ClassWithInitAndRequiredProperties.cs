using BenchmarkDotNet.Attributes;

namespace Rocks.Performance.Baselines;

[MemoryDiagnoser]
public class ClassWithInitAndRequiredProperties
	: BaselineTest
{
	public ClassWithInitAndRequiredProperties()
		: base(
			"""
			#nullable enable

			using Rocks;
			
			[assembly: Rock(typeof(InitAndRequiredProperties), BuildType.Create)]
			
			public class InitAndRequiredProperties
			{
				public virtual void Work() { }

				public int NonNullableInitValueType { get; init; }
				public int? NullableInitValueType { get; init; }
				public string? NullableInitReferenceType { get; init; }
			
				public required int NonNullableRequiredValueType { get; init; }
				public required int? NullableRequiredValueType { get; init; }
				public required string NonNullableRequiredReferenceType { get; init; }
				public required string? NullableRequiredReferenceType { get; init; }
			}			
			""")
	{ }
}