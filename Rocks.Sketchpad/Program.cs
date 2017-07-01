using Rocks.Options;
using Rocks.Tests;
using System;
using System.Text;

namespace Rocks.Sketchpad
{
	public static class Program
	{
		static void Main(string[] args)
		{
			var test = new EventsTests();
			test.CreateWhenEventsHaveGenericEventArgs();
		}

		private static void UnicodeTest()
		{
			var rock = Rock.Create<UnicodeEncoding>(
				new RockOptions(
					level: OptimizationSetting.Debug,
					codeFile: CodeFileOptions.Create));
			rock.Handle(_ => _.GetHashCode()).Returns(1);

			var chunk = rock.Make();

			Console.Out.WriteLine(chunk.GetHashCode());

			rock.Verify();
		}
	}

	public interface IHavePrimitives
	{
		char DoSomething(int x);
	}
}
