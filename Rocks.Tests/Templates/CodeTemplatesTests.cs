using NUnit.Framework;
using Rocks.Templates;

namespace Rocks.Tests.Templates
{
	[TestFixture]
	public sealed class CodeTemplatesTests
	{
		[Test]
		public void GetExpectation()
		{
			Assert.AreEqual("(methodHandler.Expectations[\"a\"] as ArgumentExpectation<b>).IsValid(a, \"a\")", CodeTemplates.GetExpectation("a", "b"));
		}
	}
}
