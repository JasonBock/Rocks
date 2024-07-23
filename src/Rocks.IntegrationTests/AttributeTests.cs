#define NEVER

using NUnit.Framework;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Rocks.IntegrationTests.AttributeTestTypes;

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

		var expectations = new NotNullIfNotCasesCreateExpectations();
		expectations.Methods.VisitMethod(node).ReturnValue(result);

		var mock = expectations.Instance();
		var mockResult = mock.VisitMethod(node);

		Assert.That(mockResult, Is.SameAs(result));

		expectations.Verify();
	}

	[Test]
	public static void CreateWithConditional()
	{
		var expectations = new ConventionDispatcherCreateExpectations();
		expectations.Methods.AssertNoScope();

		var mock = expectations.Instance();
		mock.AssertNoScope();

		expectations.Verify();
	}
}