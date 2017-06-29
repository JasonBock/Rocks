using Rocks.Options;
using System;
using System.Text;

namespace Rocks.Sketchpad
{
	public static class Program
	{
		static void Main(string[] args)
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
}
