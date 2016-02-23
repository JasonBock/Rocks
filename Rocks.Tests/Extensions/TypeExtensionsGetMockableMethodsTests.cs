using NUnit.Framework;
using Rocks.Construction.InMemory;
using Rocks.Tests.Types;
using System;
using System.Linq;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsGetMockableMethodsTests
	{
		[Test]
		public void GetMockableMethodsWhenTypeHasProtectedInternalAbstractMethod()
		{
			var methods = typeof(HasProtectedInternalAbstractMethod).GetMockableMethods(new InMemoryNameGenerator());
			Assert.IsTrue(methods.Where(_ => _.Value.Name == "Target").Any());
		}

		[Test]
		public void GetMockableMethodsWhenTypeIsInterfaceAndHasObjectMethods()
		{
			var methods = typeof(IHaveObjectMethods).GetMockableMethods(new InMemoryNameGenerator());
			Assert.AreEqual(1, methods.Count);
			Assert.IsTrue(methods[0].Value.Name == "Target");
		}
	}

	public interface IHaveObjectMethods
	{
		Type GetType();
		string ToString();
		void Target();
	}
}
