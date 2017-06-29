using Rocks.Extensions;
using Rocks.Options;
using System;
using System.Text;

namespace Rocks.Sketchpad
{
	public static class Program
	{
		static void Main(string[] args)
		{
			Console.Out.WriteLine(typeof(byte*[]).GetSafeName());
			Console.ReadLine();

			//var rock = Rock.Create<IHavePrimitives>(
			//	new RockOptions(
			//		level: OptimizationSetting.Debug,
			//		codeFile: CodeFileOptions.Create));
			//rock.Handle(_ => _.DoSomething(3)).Returns('c');

			//var chunk = rock.Make();
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
