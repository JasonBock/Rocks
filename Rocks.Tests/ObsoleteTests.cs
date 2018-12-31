using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class ObsoleteTests
	{
		[Test]
		public static void CreateMockWithObsoleteMemberWithIsErrorSetToTrue()
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
