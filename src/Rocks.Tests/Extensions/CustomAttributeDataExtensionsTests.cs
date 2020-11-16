using NUnit.Framework;
using System.Reflection;
using static Rocks.Extensions.CustomAttributeDataExtensions;

namespace Rocks.Tests.Extensions
{
	public sealed class CustomAttributeDataExtensionsTests
	{
		[Test]
		public void DoesNullableAttributeExistForValueType()
		{
			var parameter = typeof(CustomAttributeDataExtensionsTests).GetMethod(
				nameof(CustomAttributeDataExtensionsTests.ValueTypeParameter), 
				BindingFlags.NonPublic | BindingFlags.Static)!.GetParameters()[0];
			var hasNullable = false;
			foreach(var attribute in parameter.GetCustomAttributesData())
			{
				hasNullable |= attribute.IsNullableAttribute();
			}

			Assert.That(hasNullable, Is.False);
		}

		[Test]
		public void DoesNullableAttributeExistForReferenceType()
		{
			var parameter = typeof(CustomAttributeDataExtensionsTests).GetMethod(
				nameof(CustomAttributeDataExtensionsTests.ReferenceTypeParameter), 
				BindingFlags.NonPublic | BindingFlags.Static)!.GetParameters()[0];
			var hasNullable = false;
			foreach (var attribute in parameter.GetCustomAttributesData())
			{
				hasNullable |= attribute.IsNullableAttribute();
			}

			Assert.That(hasNullable, Is.False);
		}

		private static void ValueTypeParameter(int a) { }
		private static void ReferenceTypeParameter(string a) { }
	}
}