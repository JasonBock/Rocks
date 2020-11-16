using NUnit.Framework;
using System;

namespace Rocks.Tests
{
   public static class PropertyMethodAdornmentsTests
   {
		[Test]
		public static void Create()
		{
			var getterInfo = new HandlerInformation();
			var setterInfo = new HandlerInformation();

			var getter = new MethodAdornments(getterInfo);
			var setter = new MethodAdornments(setterInfo);

			var adornments = new PropertyMethodAdornments(getter, setter);
			adornments.RaisesOnGetter("a", EventArgs.Empty);
			adornments.RaisesOnSetter("b", EventArgs.Empty);

			Assert.That(getterInfo.GetRaiseEvents().Count, Is.EqualTo(1));
			Assert.That(setterInfo.GetRaiseEvents().Count, Is.EqualTo(1));
		}

		[Test]
		public static void CreateWithOnlyGetter()
		{
			var getterInfo = new HandlerInformation();
			var getter = new MethodAdornments(getterInfo);

			var adornments = new PropertyMethodAdornments(getter, null);
			adornments.RaisesOnGetter("a", EventArgs.Empty);
			adornments.RaisesOnSetter("b", EventArgs.Empty);

			Assert.That(getterInfo.GetRaiseEvents().Count, Is.EqualTo(1));
		}

		[Test]
		public static void CreateWithOnlySetter()
		{
			var setterInfo = new HandlerInformation();
			var setter = new MethodAdornments(setterInfo);

			var adornments = new PropertyMethodAdornments(null, setter);
			adornments.RaisesOnGetter("a", EventArgs.Empty);
			adornments.RaisesOnSetter("b", EventArgs.Empty);

			Assert.That(setterInfo.GetRaiseEvents().Count, Is.EqualTo(1));
		}
   }
}