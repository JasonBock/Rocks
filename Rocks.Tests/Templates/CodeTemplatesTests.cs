using NUnit.Framework;
using Rocks.Templates;

namespace Rocks.Tests.Templates
{
	[TestFixture]
	public sealed class CodeTemplatesTests
	{
		[Test]
		public void GetExpectation() =>
			Assert.That(CodeTemplates.GetExpectation("a", "b"), Is.EqualTo(
				"(methodHandler.Expectations[\"a\"] as R.ArgumentExpectation<b>).IsValid(a, \"a\")"));
	}
}
