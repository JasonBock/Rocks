using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class PropertyMethodAdornmentsTests
	{
		[Test]
		public void Create()
		{
			var getterInfo = new HandlerInformation();
			var setterInfo = new HandlerInformation();

         var getter = new MethodAdornments(getterInfo);
			var setter = new MethodAdornments(setterInfo);

			var adornments = new PropertyMethodAdornments(getter, setter);
			adornments.RaisesOnGetter("a", EventArgs.Empty);
			adornments.RaisesOnSetter("b", EventArgs.Empty);

			Assert.AreEqual(1, getterInfo.GetRaiseEvents().Count);
			Assert.AreEqual(1, setterInfo.GetRaiseEvents().Count);
		}
	}
}
