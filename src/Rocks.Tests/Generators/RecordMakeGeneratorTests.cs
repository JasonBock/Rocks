using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class RecordMakeGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public record RecordTest
				{
					public RecordTest() { }

					public virtual void Foo() { }
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Make<RecordTest>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class MakeExpectationsOfRecordTestExtensions
				{
					internal static global::MockTests.RecordTest Instance(this global::Rocks.MakeGeneration<global::MockTests.RecordTest> @self)
					{
						return new RockRecordTest();
					}
					internal static global::MockTests.RecordTest Instance(this global::Rocks.MakeGeneration<global::MockTests.RecordTest> @self, global::MockTests.RecordTest @original)
					{
						return new RockRecordTest(@original);
					}
					
					private sealed record RockRecordTest
						: global::MockTests.RecordTest
					{
						public RockRecordTest()
						{
						}
						public RockRecordTest(global::MockTests.RecordTest @original)
							: base(@original)
						{
						}
						
						public override void Foo()
						{
						}
						public override string ToString()
						{
							return default!;
						}
						protected override bool PrintMembers(global::System.Text.StringBuilder @builder)
						{
							return default!;
						}
						public override int GetHashCode()
						{
							return default!;
						}
						protected override global::System.Type EqualityContract
						{
							get => default!;
						}
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGeneratorV3>(code,
			new[] { (typeof(RockMakeGeneratorV3), "RecordTest_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}