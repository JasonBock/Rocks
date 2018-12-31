using NUnit.Framework;
using Rocks.Templates;

namespace Rocks.Tests.Templates
{
	public static class EventTemplatesTests
	{
		[Test]
		public static void GetEvent() =>
			Assert.That(EventTemplates.GetEvent("a", "b", "c"), Is.EqualTo("public a event b c;"));
	}
}
