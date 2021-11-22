using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class PropertyInitMakeGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public interface ITest
	{
		int Bar { get; init; }
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<ITest>();
		}
	}
}";

		var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

#nullable enable
namespace MockTests
{
	internal static class MakeExpectationsOfITestExtensions
	{
		internal static ITest Instance(this MakeGeneration<ITest> self) =>
			new RockITest();
		
		private sealed class RockITest
			: ITest
		{
			public RockITest() { }
			
			public int Bar
			{
				get => default!;
				init { }
			}
		}
	}
}
";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "ITest_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}