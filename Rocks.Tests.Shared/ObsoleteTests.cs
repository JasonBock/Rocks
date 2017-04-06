using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class ObsoleteTests
	{
		[Test]
		public void CreateMockWithObsoleteMemberWithIsErrorSetToTrue()
		{
			var rock = Rock.Create<IHaveAnObsoleteMember>();
			rock.Handle(_ => _.Method());

			var mock = rock.Make();
			mock.Method();

			rock.Verify();
		}
	}

	public interface IHaveAnObsoleteMember
	{
		[Obsolete("a", true)]
		void ObsoleteMethod();

		void Method();
	}
}
