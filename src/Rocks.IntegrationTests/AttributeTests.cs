using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Rocks.IntegrationTests;

public class NotNullIfNotCases
{
	[return: NotNullIfNotNull(nameof(node))]
	public virtual object? VisitMethod(object? node) => default;
}

public static class AttributeTests
{
	[Test]
	public static void CreateWithNotNullIfNotNull()
	{
		var node = new object();
		var result = new object();

		var expectations = Rock.Create<NotNullIfNotCases>();
		expectations.Methods().VisitMethod(node).Returns(result);

		var mock = expectations.Instance();
		var mockResult = mock.VisitMethod(node);

		Assert.That(mockResult, Is.SameAs(result));

		expectations.Verify();
	}
}
