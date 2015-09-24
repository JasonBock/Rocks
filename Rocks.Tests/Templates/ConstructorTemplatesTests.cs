using NUnit.Framework;
using Rocks.Templates;

namespace Rocks.Tests.Templates
{
	[TestFixture]
	public sealed class ConstructorTemplatesTests
	{
		[Test]
		public void GetConstructor()
		{
			Assert.AreEqual(
@"public a(SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>> handlersc)
	: base(b)
{
	this.handlers = handlers;
}", ConstructorTemplates.GetConstructor("a", "b", "c"));
		}

		[Test]
		public void GetConstructorNoArguments()
		{
			Assert.AreEqual(
@"public a() 
{ 
	this.handlers = new SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>>(
		new SCG.Dictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>>());
}", ConstructorTemplates.GetConstructorWithNoArguments("a"));
		}
	}
}
