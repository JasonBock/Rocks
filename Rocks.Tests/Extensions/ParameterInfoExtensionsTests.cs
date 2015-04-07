using NUnit.Framework;
using static Rocks.Extensions.ParameterInfoExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class ParameterInfoExtensionsTests
	{
		[Test]
		public void GetModifierForByVal()
		{
			Assert.AreEqual(string.Empty, 
				this.GetType().GetMethod(nameof(this.TargetWithByValArguments)).GetParameters()[0].GetModifier());
		}

		[Test]
		public void GetModifierForOut()
		{
			Assert.AreEqual("out ",
				this.GetType().GetMethod(nameof(this.TargetWithOutArgument)).GetParameters()[0].GetModifier());
		}

		[Test]
		public void GetModifierForRef()
		{
			Assert.AreEqual("ref ",
				this.GetType().GetMethod(nameof(this.TargetWithRefArgument)).GetParameters()[0].GetModifier());
		}

		[Test]
		public void GetModifierForParams()
		{
			Assert.AreEqual("params ",
				this.GetType().GetMethod(nameof(this.TargetWithParamsArgument)).GetParameters()[0].GetModifier());
		}

		[Test]
		public void GetModifierForParamsIgnored()
		{
			Assert.AreEqual(string.Empty,
				this.GetType().GetMethod(nameof(this.TargetWithParamsArgument)).GetParameters()[0].GetModifier(true));
		}

		public void TargetWithByValArguments(int a) { }
		public void TargetWithOutArgument(out int a) { a = 0; }
		public void TargetWithRefArgument(ref int a) { }
		public void TargetWithParamsArgument(params string[] a) { }
	}
}
