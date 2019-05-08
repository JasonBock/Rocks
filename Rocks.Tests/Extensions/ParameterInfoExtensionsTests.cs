using NUnit.Framework;
using System.Reflection;
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

		[Test]
		public static void IsNullableReferenceForValueTypeParameter()
		{
			var parameter = typeof(ParameterInfoExtensionsTests).GetMethod(
				nameof(ParameterInfoExtensionsTests.ValueTypeParameter), BindingFlags.NonPublic | BindingFlags.Static)
				.GetParameters()[0];
			Assert.That(parameter.IsNullableReference(), Is.False);
		}

		[Test]
		public static void IsNullableReferenceForNonNullableReferenceParameter()
		{
			var parameter = typeof(ParameterInfoExtensionsTests).GetMethod(
				nameof(ParameterInfoExtensionsTests.NonNullableReferenceTypeParameter), BindingFlags.NonPublic | BindingFlags.Static)
				.GetParameters()[0];
			Assert.That(parameter.IsNullableReference(), Is.False);
		}

		[Test]
		public static void IsNullableReferenceForNullableReferenceParameter()
		{
			var parameter = typeof(ParameterInfoExtensionsTests).GetMethod(
				nameof(ParameterInfoExtensionsTests.NullableReferenceTypeParameter), BindingFlags.NonPublic | BindingFlags.Static)
				.GetParameters()[0];
			Assert.That(parameter.IsNullableReference(), Is.True);
		}

		[Test]
		public static void IsNullableReferenceForVoidReturn()
		{
			var parameter = typeof(ParameterInfoExtensionsTests).GetMethod(
				nameof(ParameterInfoExtensionsTests.VoidReturn), BindingFlags.NonPublic | BindingFlags.Static)
				.ReturnParameter;
			Assert.That(parameter.IsNullableReference(), Is.False);
		}

		[Test]
		public static void IsNullableReferenceForValueTypeReturn()
		{
			var parameter = typeof(ParameterInfoExtensionsTests).GetMethod(
				nameof(ParameterInfoExtensionsTests.ValueTypeReturn), BindingFlags.NonPublic | BindingFlags.Static)
				.ReturnParameter;
			Assert.That(parameter.IsNullableReference(), Is.False);
		}

		[Test]
		public static void IsNullableReferenceForNonNullableReferenceTypeReturn()
		{
			var parameter = typeof(ParameterInfoExtensionsTests).GetMethod(
				nameof(ParameterInfoExtensionsTests.NonNullableReferenceTypeReturn), BindingFlags.NonPublic | BindingFlags.Static)
				.ReturnParameter;
			Assert.That(parameter.IsNullableReference(), Is.False);
		}

		[Test]
		public static void IsNullableReferenceForNullableReferenceTypeReturn()
		{
			var parameter = typeof(ParameterInfoExtensionsTests).GetMethod(
				nameof(ParameterInfoExtensionsTests.NullableReferenceTypeReturn), BindingFlags.NonPublic | BindingFlags.Static)
				.ReturnParameter;
			Assert.That(parameter.IsNullableReference(), Is.True);
		}

		private static void ValueTypeParameter(int a) { }
		private static void NonNullableReferenceTypeParameter(string a) { }
		private static void NullableReferenceTypeParameter(string? a) { }

		private static void VoidReturn() { }
		private static int ValueTypeReturn() => 1;
		private static string NonNullableReferenceTypeReturn() => string.Empty;
		private static string? NullableReferenceTypeReturn() => null;

		public void TargetWithInOutAttributesOnArgument([In, Out] int[] a) { }
		public void TargetWithOutAttributeOnArgument([Out] int[] a) { }
		public void TargetWithByValArguments(int a) { }
		public void TargetWithOutArgument(out int a) => a = 0; 
		public void TargetWithRefArgument(ref int a) { }
		public void TargetWithParamsArgument(params string[] a) { }
	}
}