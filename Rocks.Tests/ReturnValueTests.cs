using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class ReturnValueTests
	{
		[Test]
		public void Create()
		{
			var handler = new HandlerInformation<string>();
			var returnValue = new ReturnValue<string>(handler);
			Assert.AreEqual(default(string), handler.ReturnValue, nameof(handler.ReturnValue));
		}

		[Test]
		public void SetReturnValue()
		{
			var newReturnValue = "a";
			var handler = new HandlerInformation<string>();
			var returnValue = new ReturnValue<string>(handler);
			returnValue.Returns(newReturnValue);
			Assert.AreEqual(newReturnValue, handler.ReturnValue, nameof(handler.ReturnValue));
		}
	}
}
