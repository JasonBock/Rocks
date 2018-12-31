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
	}
}
