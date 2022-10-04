using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class NullableAnnotationTests
{
	[Test]
	public static async Task GenerateWithMethodsWhenParameterWithNullDefaultIsNotAnnotatedAsync()
	{
		var code =
			"""
			using Rocks;

			public class NeedNullable
			{
			    public virtual int IntReturn(object initializationData = null) => 0;
			    public virtual void VoidReturn(object initializationData = null) { }
			}
			
			public static class Test
			{
				public static void Generate()
				{
					var rock = Rock.Create<NeedNullable>();
				}
			}
			""";

		var generatedCode = "";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "NeedNullable_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}