using Rocks.Options;
using System;

namespace Rocks.Sketchpad
{
	public static class Program
	{
		static void Main(string[] args)
		{
			//IThingy chunk = null;
			//var calledValue = false;

			//var rock = Rock.Create<IThingy>(
			//	new RockOptions(level: OptimizationSetting.Debug,
			//		codeFile: CodeFileOptions.Create));
			//rock.Handle(nameof(IThingy.Called), () => calledValue, _ => calledValue = _);
			//rock.Handle(r => r.DoSomething(), () => chunk.Called = true);

			//chunk = rock.Make();
			//chunk.DoSomething();

			//Console.Out.WriteLine(chunk.Called);
			//rock.Verify();


			var rock = Rock.Create<IThingy>();
			rock.Handle(_ => _.DoSomething());
			rock.Make();
			rock.Verify();
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
