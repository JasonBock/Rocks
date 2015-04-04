using Moq;
using System;

namespace Rocks.Sketchpad
{
	public static class MoqTests
	{
		public static void MultipleSetups()
		{
			var value = Guid.NewGuid();
			var foo = new Mock<IMoq>(MockBehavior.Strict);
			foo.Setup(_ => _.Foo("a", 4));
			foo.Setup(_ => _.Foo("b", value));

			var fooMock = foo.Object;
			fooMock.Foo("a", 4);
			fooMock.Foo("b", value);
			foo.VerifyAll();
		}
	}

	public interface IMoq
	{
		void Foo(string a, object b);
	}
}
