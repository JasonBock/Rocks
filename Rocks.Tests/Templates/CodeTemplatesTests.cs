using NUnit.Framework;
using Rocks.Templates;

namespace Rocks.Tests.Templates
{
	public static class CodeTemplatesTests
	{
		[Test]
		public static void GetExpectation() =>
			Assert.That(CodeTemplates.GetExpectation("a", "b"), Is.EqualTo(
				"(methodHandler.Expectations[\"a\"] as R.ArgumentExpectation<b>).IsValid(a)"));
	}
}
