using NUnit.Framework;
using Rocks.Templates;

namespace Rocks.Tests.Templates
{
	[TestFixture]
	public sealed class EventTemplatesTests
	{
		[Test]
		public void GetEvent()
		{
			Assert.AreEqual("public a event b c;", EventTemplates.GetEvent("a", "b", "c"));
		}
	}
}
