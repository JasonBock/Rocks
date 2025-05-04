#define NEVER

using NUnit.Framework;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Rocks.Analysis.IntegrationTests.AttributeTestTypes;

public class NotNullIfNotCases
{
	[return: NotNullIfNotNull(nameof(node))]
	public virtual object? VisitMethod(object? node) => default;
}

public class ConventionDispatcher
{
	[Conditional("NEVER")]
	public virtual void AssertNoScope() { }
}

public static class AttributeTests
{
	[Test]
	public static void CreateWithNotNullIfNotNull()
	{
		var node = new object();
		var result = new object();

		using var context = new RockContext(); 
		var expectations = context.Create<NotNullIfNotCasesCreateExpectations>();
		expectations.Methods.VisitMethod(node).ReturnValue(result);

		var mock = expectations.Instance();
		var mockResult = mock.VisitMethod(node);

		Assert.That(mockResult, Is.SameAs(result));
	}

	[Test]
	public static void CreateWithConditional()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ConventionDispatcherCreateExpectations>();
		expectations.Methods.AssertNoScope();

		var mock = expectations.Instance();
		mock.AssertNoScope();
	}
}