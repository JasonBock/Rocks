using NUnit.Framework;
using System.Runtime.InteropServices;
using static Rocks.Extensions.ParameterInfoExtensions;

namespace Rocks.Tests.Extensions
{
	public sealed class ParameterInfoExtensionsTests
	{
		[Test]
		public void GetModifierForByVal() =>
			Assert.That(this.GetType().GetMethod(
				nameof(this.TargetWithByValArguments)).GetParameters()[0].GetModifier(), Is.Empty);

		[Test]
		public void GetModifierForOut() =>
			Assert.That(this.GetType().GetMethod(
				nameof(this.TargetWithOutArgument)).GetParameters()[0].GetModifier(), Is.EqualTo("out "));

		[Test]
		public void GetModifierForRef() =>
			Assert.That(this.GetType().GetMethod(
				nameof(this.TargetWithRefArgument)).GetParameters()[0].GetModifier(), Is.EqualTo("ref "));

		[Test]
		public void GetModifierForParams() =>
			Assert.That(this.GetType().GetMethod(
				nameof(this.TargetWithParamsArgument)).GetParameters()[0].GetModifier(), Is.EqualTo("params "));
		[Test]
		public void GetModifierForParamsIgnored() =>
			Assert.That(this.GetType().GetMethod(
				nameof(this.TargetWithParamsArgument)).GetParameters()[0].GetModifier(true), Is.Empty);

		[Test]
		public void GetModifierForInOutAttributes() =>
			Assert.That(this.GetType().GetMethod(
				nameof(this.TargetWithInOutAttributesOnArgument)).GetParameters()[0].GetModifier(), Is.Empty);

		[Test]
		public void GetModifierForOutAttribute() =>
			Assert.That(this.GetType().GetMethod(
				nameof(this.TargetWithOutAttributeOnArgument)).GetParameters()[0].GetModifier(), Is.Empty);

		public void TargetWithInOutAttributesOnArgument([In, Out] int[] a) { }
		public void TargetWithOutAttributeOnArgument([Out] int[] a) { }
		public void TargetWithByValArguments(int a) { }
		public void TargetWithOutArgument(out int a) => a = 0; 
		public void TargetWithRefArgument(ref int a) { }
		public void TargetWithParamsArgument(params string[] a) { }
	}
}