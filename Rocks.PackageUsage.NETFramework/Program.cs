using System;

namespace Rocks.PackageUsage.NETFramework
{
	class Program
	{
		static void Main(string[] args)
		{
			var rock = Rock.Create<IDoSomething>();
			rock.Handle(_ => _.DoIt()).Returns(20);

			var chunk = rock.Make();
			Console.Out.WriteLine(chunk.DoIt());

			rock.Verify();
		}
	}

	public interface IDoSomething
	{
		int DoIt();
	}
}
