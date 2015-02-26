using NUnit.Framework;
using Rocks.Extensions;

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

		public void TargetWithByValArguments(int a) { }
		public void TargetWithOutArgument(out int a) { a = 0; }
		public void TargetWithRefArgument(ref int a) { }
	}
}
