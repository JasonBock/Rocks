using Rocks.Options;
using System;

namespace Rocks.Sketchpad
{
	public static class Program
	{
		static void Main(string[] args)
		{
			IThingy chunk = null;
			var rock = Rock.Create<IThingy>(
				new RockOptions(level: OptimizationSetting.Debug,
					codeFile: CodeFileOptions.Create));
			rock.Handle(r => r.DoSomething(), () => chunk.Called = true);
			rock.Handle(nameof(IThingy.Called));
			chunk = rock.Make();
			chunk.DoSomething();
		}
	}

	public interface IThingy
	{
		bool Called { get; set; }
		void DoSomething();
		void DoNothing();
		int One();
		int Zero();
	}

	public interface IDoSomething
	{
		int GetValue();
	}
}
