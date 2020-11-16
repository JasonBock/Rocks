using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using Rocks.Extensions;
using Rocks.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Rocks.Sketchpad
{
	public static class Program
	{
		static void Main() =>
			//BenchmarkRunner.Run<GenericArgumentsTests>();
			Demo.Demonstrate();
			//Program.HandleVirtualOnClass();
			//Program.HandleFoo();
			//Program.SpanTypeTests();

		//static async Task Main() => 
		//	await ExpressionEvaluation.RunEvaluationsAsync();

		public struct MyThing<T> { }

		private static void SpanTypeTests()
		{
			var s = typeof(Span<byte>);
			var g = s.GetGenericTypeDefinition();

			Console.Out.WriteLine(typeof(MyThing<>).IsAssignableFrom(typeof(MyThing<byte>)));
			Console.Out.WriteLine(typeof(Span<>).IsAssignableFrom(typeof(Span<byte>)));
			Console.Out.WriteLine(typeof(Span<>).IsAssignableFrom(g));
			Console.Out.WriteLine(typeof(Span<>) == typeof(Span<byte>));
		}

		private static void RunBenchmark() =>
			BenchmarkRunner.Run<MetadataReferenceCacheBenchmark>();

		public class IFoo
		{
			public virtual int Bar() => 42;
		}

		public static void HandleFoo()
		{
			var rock = Rock.Create<IFoo>();
			//rock.Handle(_ => _.Bar(3));
			var chunk = rock.Make();
			chunk.Bar();
			rock.Verify();
		}

		public class Virtualized
		{
			public virtual void Foo() => Console.Out.WriteLine("Call me!");
		}

		private static void HandleVirtualOnClass()
		{
			var rock = Rock.Create<Virtualized>();
			rock.Handle(_ => _.Foo()); 
			var chunk = rock.Make();
			chunk.Foo();
			rock.Verify();
		}

		private static void UnicodeTest()
		{
			var rock = Rock.Create<UnicodeEncoding>(
				new RockOptions(
					level: OptimizationSetting.Debug,
					codeFile: CodeFileOption.Create));
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