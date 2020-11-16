using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class FullInterfaceTests
	{
		[Test]
		public static void Make()
		{
			var rock = Rock.Create<IFullInterfaceTests>();
			rock.Handle(_ => _.Target());

			var chunk = rock.Make();
			chunk.Target();

			rock.Verify();
		}
	}

	public interface IFullInterfaceTests
	{
		void Target();
		int Property { get; set; }
		event EventHandler Event;
	}
}
