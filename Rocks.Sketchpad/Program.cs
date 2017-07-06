using Rocks.Options;
using Rocks.Tests;
using System;
using System.Reflection;
using System.Text;

namespace Rocks.Sketchpad
{
	public static class Program
	{
		static void Main(string[] args)
		{
			var test = new HandleProperty1IndexerTests();
			test.MakeWithGetAndSetIndexerProperty();
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

#pragma warning disable CS0067
	public interface IStupid
	{
		void Foo();
	}

	public interface IBaseEvents
	{
		event EventHandler BaseEvent;
	}

	public interface ISubEvents : IBaseEvents
	{
		event EventHandler SubEvent;
	}

	public class BaseEvents
	{
		public event EventHandler BaseEvent;
	}

	public class SubEvents : BaseEvents
	{
		public event EventHandler SubEvent;
	}

	public abstract class HandleISubEvents : ISubEvents
	{
		public abstract event EventHandler SubEvent;
		public abstract event EventHandler BaseEvent;
	}
#pragma warning restore CS0067

	public interface IHavePrimitives
	{
		char DoSomething(int x);
	}
}
