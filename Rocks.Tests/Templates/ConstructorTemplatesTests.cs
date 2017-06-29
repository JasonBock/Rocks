using NUnit.Framework;
using Rocks.Templates;

namespace Rocks.Tests.Templates
{
	[TestFixture]
	public sealed class ConstructorTemplatesTests
	{
		[Test]
		public void GetConstructor() =>
			Assert.That(ConstructorTemplates.GetConstructor("a", "b", "c"), Is.EqualTo(
@"public a(SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>> handlersc)
	: base(b) =>
	this.handlers = handlers;"));

		[Test]
		public void GetConstructorNoArguments() =>
			Assert.That(ConstructorTemplates.GetConstructorWithNoArguments("a"), Is.EqualTo(
@"public a() =>
	this.handlers = new SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>>(
		new SCG.Dictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>>());"));
	}
}
