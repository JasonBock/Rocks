using NUnit.Framework;
using Rocks.Templates;

namespace Rocks.Tests.Templates
{
	[TestFixture]
	public sealed class EventTemplatesTests
	{
		[Test]
		public void GetEvent() =>
			Assert.That(EventTemplates.GetEvent("a", "b", "c"), Is.EqualTo("public a event b c;"));
	}
}
