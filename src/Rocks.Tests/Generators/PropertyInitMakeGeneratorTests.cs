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
			using Rocks;
			using Rocks.Exceptions;
			using System;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			#nullable enable
			namespace MockTests
			{
				internal static class MakeExpectationsOfITestExtensions
				{
					public sealed class ConstructorProperties
					{
						public int NonNullableValueType { get; init; }
						public int? NullableValueType { get; init; }
						public string? NonNullableReferenceType { get; init; }
						public string? NullableReferenceType { get; init; }
					}
					
					internal static ITest Instance(this MakeGeneration<ITest> self, ConstructorProperties? constructorProperties) =>
						constructorProperties is null ?
							new RockITest() :
							new RockITest()
							{
								NonNullableValueType = constructorProperties.NonNullableValueType,
								NullableValueType = constructorProperties.NullableValueType,
								NonNullableReferenceType = constructorProperties.NonNullableReferenceType!,
								NullableReferenceType = constructorProperties.NullableReferenceType,
							};
					
					private sealed class RockITest
						: ITest
					{
						public RockITest() { }
						
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

	//[Ignore("Currently cannot run this test until the required property feature is fully supported by the compiler.")]
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

			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "Test_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}