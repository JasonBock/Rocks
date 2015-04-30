using NUnit.Framework;
using Rocks.Construction;
using Rocks.Tests.Types;
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
	}
}
