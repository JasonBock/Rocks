using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ClassConstructorMakeGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public class BaseCtor
	{
		 public BaseCtor(int a, ref string b, out string c, params string[] d) { c = ""c""; }

		 public virtual void Foo() { }
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<BaseCtor>();
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
	internal static class MakeExpectationsOfBaseCtorExtensions
	{
		internal static BaseCtor Instance(this MakeGeneration<BaseCtor> self, int a, ref string b, out string c, params string[] d) =>
			new RockBaseCtor(a, ref b, out c, d);
		
		private sealed class RockBaseCtor
			: BaseCtor
		{
			public RockBaseCtor(int a, ref string b, out string c, params string[] d)
				: base(a, ref b, out c, d)
			{ }
			
			public override bool Equals(object? obj)
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
";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "BaseCtor_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}