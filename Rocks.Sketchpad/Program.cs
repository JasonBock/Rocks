using Rocks.Options;
using System;

namespace Rocks.Sketchpad
{
	public static class Program
	{
		static void Main(string[] args)
		{
			var rock = Rock.Create<IDoSomething>(
				new RockOptions(
					level: OptimizationSetting.Debug,
					codeFile: CodeFileOptions.Create));
			rock.Handle(_ => _.GetValue()).Returns(22);

			var chunk = rock.Make();
			Console.Out.WriteLine(chunk.GetValue());

			rock.Verify();
		}
	}

	public interface IDoSomething
	{
		int GetValue();
	}
}
