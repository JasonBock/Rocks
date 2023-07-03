using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class PropertyInitMakeGeneratorTests
{
	[Test]
	public static async Task GenerateWithInitAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			#nullable enable
			namespace MockTests
			{
				public interface ITest
				{
					int NonNullableValueType { get; init; }
					int? NullableValueType { get; init; }
					string NonNullableReferenceType { get; init; }
					string? NullableReferenceType { get; init; }
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Make<ITest>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class MakeExpectationsOfITestExtensions
				{
					internal sealed class ConstructorProperties
					{
						internal int NonNullableValueType { get; init; }
						internal int? NullableValueType { get; init; }
						internal string? NonNullableReferenceType { get; init; }
						internal string? NullableReferenceType { get; init; }
					}
					
					internal static global::MockTests.ITest Instance(this global::Rocks.MakeGeneration<global::MockTests.ITest> @self, ConstructorProperties? @constructorProperties)
					{
						return new RockITest(@constructorProperties);
					}
					
					private sealed class RockITest
						: global::MockTests.ITest
					{
						public RockITest(ConstructorProperties? @constructorProperties)
						{
							if (@constructorProperties is not null)
							{
								this.NonNullableValueType = @constructorProperties.NonNullableValueType;
								this.NullableValueType = @constructorProperties.NullableValueType;
								this.NonNullableReferenceType = @constructorProperties.NonNullableReferenceType!;
								this.NullableReferenceType = @constructorProperties.NullableReferenceType;
							}
						}
						
						public int NonNullableValueType
						{
							get => default!;
							init { }
						}
						public int? NullableValueType
						{
							get => default!;
							init { }
						}
						public string NonNullableReferenceType
						{
							get => default!;
							init { }
						}
						public string? NullableReferenceType
						{
							get => default!;
							init { }
						}
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "ITest_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithRequiredAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			#nullable enable
			namespace MockTests
			{
				public class Test
				{
					public virtual void Foo() { }
					public required int NonNullableValueType { get; init; }
					public required int? NullableValueType { get; init; }
					public required string NonNullableReferenceType { get; init; }
					public required string? NullableReferenceType { get; init; }
				}
			
				public static class MockTest
				{
					public static void Generate()
					{
						var rock = Rock.Make<Test>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class MakeExpectationsOfTestExtensions
				{
					internal sealed class ConstructorProperties
					{
						internal required int NonNullableValueType { get; init; }
						internal required int? NullableValueType { get; init; }
						internal required string? NonNullableReferenceType { get; init; }
						internal required string? NullableReferenceType { get; init; }
					}
					
					internal static global::MockTests.Test Instance(this global::Rocks.MakeGeneration<global::MockTests.Test> @self, ConstructorProperties @constructorProperties)
					{
						if (@constructorProperties is null)
						{
							throw new global::System.ArgumentNullException(nameof(@constructorProperties));
						}
						return new RockTest(@constructorProperties);
					}
					
					private sealed class RockTest
						: global::MockTests.Test
					{
						[global::System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
						public RockTest(ConstructorProperties @constructorProperties)
						{
							this.NonNullableValueType = @constructorProperties.NonNullableValueType;
							this.NullableValueType = @constructorProperties.NullableValueType;
							this.NonNullableReferenceType = @constructorProperties.NonNullableReferenceType!;
							this.NullableReferenceType = @constructorProperties.NullableReferenceType;
						}
						
						public override bool Equals(object? @obj)
						{
							return default!;
						}
						public override int GetHashCode()
						{
							return default!;
						}
						public override string? ToString()
						{
							return default!;
						}
						public override void Foo()
						{
						}
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "Test_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}