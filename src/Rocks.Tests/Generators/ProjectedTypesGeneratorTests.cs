using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ProjectedTypesGeneratorTests
{
	[Test]
	public static async Task GenerateWithPointersAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			#nullable enable
			namespace MockTests
			{
				public unsafe interface IHavePointers
				{
					void DelegatePointerParameter(delegate*<int, void> value);
					delegate*<int, void> DelegatePointerReturn();
					void PointerParameter(int* value);
					int* PointerReturn();
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IHavePointers>();
					}
				}
			}
			""";

		var generatedCode = "";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IHavePointers_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithRefStructAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			#nullable enable
			namespace MockTests
			{
				public interface IHaveInAndOutSpan
				{
					Span<int> Foo(Span<int> values);
					Span<byte> Values { get; set; }
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IHaveInAndOutSpan>();
					}
				}
			}
			""";

		var generatedCode = "";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IHaveInAndOutSpan_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}