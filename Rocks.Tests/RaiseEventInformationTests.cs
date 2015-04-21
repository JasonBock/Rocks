using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class RaiseEventInformationTests
	{
		[Test]
		public void Create()
		{
			var args = new EventArgs();
			var information = new RaiseEventInformation("a", args);

			Assert.AreEqual("a", information.Name, nameof(information.Name));
			Assert.AreSame(args, information.Args, nameof(information.Args));
		}
	}
}
