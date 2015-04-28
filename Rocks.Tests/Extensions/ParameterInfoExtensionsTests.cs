using NUnit.Framework;
using System.Runtime.InteropServices;
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

		[Test]
		public void GetModifierForInOutAttributes()
		{
			Assert.AreEqual(string.Empty,
				this.GetType().GetMethod(nameof(this.TargetWithInOutAttributesOnArgument)).GetParameters()[0].GetModifier());
		}

		[Test]
		public void GetModifierForOutAttribute()
		{
			Assert.AreEqual(string.Empty,
				this.GetType().GetMethod(nameof(this.TargetWithOutAttributeOnArgument)).GetParameters()[0].GetModifier());
		}

		public void TargetWithInOutAttributesOnArgument([In, Out] int[] a) { }
		public void TargetWithOutAttributeOnArgument([Out] int[] a) { }
		public void TargetWithByValArguments(int a) { }
		public void TargetWithOutArgument(out int a) { a = 0; }
		public void TargetWithRefArgument(ref int a) { }
		public void TargetWithParamsArgument(params string[] a) { }
	}
}
