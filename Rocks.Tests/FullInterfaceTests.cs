using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class FullInterfaceTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IFullInterfaceTests>();
			rock.HandleAction(_ => _.Target());

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
