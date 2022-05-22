using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class RecordMakeGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
@"using Rocks;
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
}";

		var generatedCode =
@"using MockTests;
using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

#nullable enable
namespace MockTests
{
	internal static class MakeExpectationsOfRecordTestExtensions
	{
		internal static RecordTest Instance(this MakeGeneration<RecordTest> self) =>
			new RockRecordTest();
		internal static RecordTest Instance(this MakeGeneration<RecordTest> self, RecordTest original) =>
			new RockRecordTest(original);
		
		private sealed record RockRecordTest
			: RecordTest
		{
			public RockRecordTest() { }
			public RockRecordTest(RecordTest original)
				: base(original)
			{ }
			
			public override void Foo()
			{
			}
			public override string ToString()
			{
				return default!;
			}
			protected override bool PrintMembers(StringBuilder builder)
			{
				return default!;
			}
			public override int GetHashCode()
			{
				return default!;
			}
			protected override Type EqualityContract
			{
				get => default!;
			}
		}
	}
}
";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "RecordTest_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}