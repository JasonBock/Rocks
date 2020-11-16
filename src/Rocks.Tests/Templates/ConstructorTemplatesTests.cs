using NUnit.Framework;
using Rocks.Templates;

namespace Rocks.Tests.Templates
{
	public static class ConstructorTemplatesTests
	{
		[Test]
		public static void GetConstructor() =>
			Assert.That(ConstructorTemplates.GetConstructor("a", "b", "c"), Is.EqualTo(
@"public a(SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>> handlersc)
	: base(b) =>
	this.handlers = handlers;"));

		[Test]
		public static void GetConstructorNoArguments() =>
			Assert.That(ConstructorTemplates.GetConstructorWithNoArguments("a"), Is.EqualTo(
@"public a() =>
	this.handlers = new SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>>(
		new SCG.Dictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>>());"));
	}
}
