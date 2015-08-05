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
@"public a(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlersc)
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
	this.handlers = new ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>(
		new System.Collections.Generic.Dictionary<int, ReadOnlyCollection<HandlerInformation>>());
}", ConstructorTemplates.GetConstructorWithNoArguments("a"));
		}
	}
}
