using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodBaseExtensionTests
	{
		[Test]
		public void GetArgumentNameList()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithArguments));
			Assert.AreEqual("a, c", target.GetArgumentNameList());
		}

		[Test]
		public void GetArgumentNameListWithParams()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithParamsArgument));
			Assert.AreEqual("a", target.GetArgumentNameList());
		}

		public void TargetWithArguments(int a, string c) { }

		public void TargetWithParamsArgument(params int[] a) { }
	}
}
