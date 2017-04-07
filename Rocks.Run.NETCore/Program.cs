using Rocks.Options;
using System;

namespace Rocks.Run.NETCore
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Start");
			var rock = Rock.Create<IAmSimple>(new RockOptions(codeFile: CodeFileOptions.Create));
			rock.Handle(_ => _.TargetAction());
			rock.Handle(_ => _.TargetFunc()).Returns(44);

			var chunk = rock.Make();
			Console.WriteLine("Finished");
			//chunk.TargetAction();
			//Console.WriteLine(chunk.TargetFunc());

			//rock.Verify();
		}
	}

	public interface IAmSimple
	{
		void TargetAction();
		int TargetFunc();
	}
}