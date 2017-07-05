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
			var type = typeof(SubEvents);
			foreach(var @event in type.GetTypeInfo().GetEvents(ReflectionValues.PublicNonPublicInstance))
			{
				Console.Out.WriteLine(@event.Name);
			}

			var x = new Rocks.Tests.Extensions.TypeExtensionsGetMockableEventsTests();
			x.GetMockableEventsFromSubClass();
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
	public class BaseEvents
	{
		public event EventHandler BaseEvent;
	}

	public class SubEvents : BaseEvents
	{
		public event EventHandler SubEvent;
	}
#pragma warning restore CS0067

	public interface IHavePrimitives
	{
		char DoSomething(int x);
	}
}
