using Rocks.Options;
using System;

namespace Rocks.Sketchpad
{
	public interface ITest
	{
		void Run();
	}

	public static class Quickstart
	{
		public static void Run()
		{
			var rock = Rock.Create<ITest>(
				new RockOptions(OptimizationSetting.Debug, CodeFileOptions.Create));
			rock.Handle(_ => _.Run());

			var chunk = rock.Make();
			chunk.Run();

			rock.Verify();
		}

		public delegate void RefTargetDelegate(ref int a);

		private static void RefTarget(ref int a) { }

		private static void RunWithMake()
		{
			var value = Rock.Make<IValue>();

			var producer = Rock.Create<IProduceValue>();
			producer.Handle(_ => _.Produce()).Returns(value);

			var uses = new UsesProducer(producer.Make());

			var producedValue = uses.GetValue();

			// producedValue and value are the same references.
		}

		private static void RunWithEvent()
		{
			var rock = Rock.Create<IHaveAnEvent>();
			rock.Handle(_ => _.Target(1))
				.Raises(nameof(IHaveAnEvent.TargetEvent), EventArgs.Empty);

#pragma warning disable CS0219 // Variable is assigned but its value is never used
			var wasEventRaised = false;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => wasEventRaised = true;
			chunk.Target(1);

			rock.Verify();
		}

		private static void RunWithIndexer()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer1SetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IHaveIndexer>();
			rock.Handle(() => indexer1, _ => returnValue, (i1, value) => setValue = value);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1];
			chunk[indexer1] = indexer1SetValue;

			rock.Verify();
		}

		private static void RunWithProperties()
		{
			var rock = Rock.Create<IHaveAProperty>();
			rock.Handle(nameof(IHaveAProperty.GetterAndSetter));

			var chunk = rock.Make();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			var value = chunk.GetterAndSetter;

			rock.Verify();
		}

		private static void RunWithRefs()
		{
			var a = 1;

			var rock = Rock.Create<IHaveRefs>();
			rock.Handle(_ => _.Target(ref a), 
				new RefTargetDelegate(Quickstart.RefTarget));

			var chunk = rock.Make();
			chunk.Target(ref a);

			rock.Verify();
		}

		private static void RunWithGenerics()
		{
			var rock = Rock.Create<IHaveGenerics<string>>();
			rock.Handle(_ => _.Target<int>("a", 44));

			var chunk = rock.Make();
			chunk.Target("a", 44);

			rock.Verify();
		}

		private static void RunClassWithParametersInConstructor()
		{
			var rock = Rock.Create<MockedClass>();
			rock.Handle(_ => _.Target());

			var chunk = rock.Make(new object[] { 44 });
			chunk.Target();

			rock.Verify();
		}

		private static void RunMethodReturns()
		{
			var rock = Rock.Create<IAmSimple>();
			rock.Handle<int>(_ => _.TargetFunc()).Returns(44);

			var chunk = rock.Make();
			var x = chunk.TargetFunc();

			rock.Verify();
		}

		private static void RunMethodCapture()
		{
			var value = 0;

			var rock = Rock.Create<IHaveParameterExpectations>();
			rock.Handle<int>(_ => _.Target(Arg.Is<int>(i => i > 10)), 
				a => value = a);

			var chunk = rock.Make();
			chunk.Target(44);

			rock.Verify();
		}

		private static void RunMultipleCallCounts()
		{
			var rock = Rock.Create<IHaveParameterExpectations>();
			rock.Handle(_ => _.Target(44), 2);
			rock.Handle(_ => _.Target(22), 3);

			var chunk = rock.Make();
			chunk.Target(22);
			chunk.Target(44);
			chunk.Target(22);
			chunk.Target(44);
			chunk.Target(22);

			rock.Verify();
		}

		private static void RunParameterExpectations()
		{
			var rock = Rock.Create<IHaveParameterExpectations>();
			rock.Handle(_ => _.Target(Arg.Is<int>(a => a > 20 && a < 50)));
			rock.Handle(_ => _.Target(10));

			var chunk = rock.Make();
			chunk.Target(44);
			chunk.Target(10);

			rock.Verify();
		}

		private static void RunSimple()
		{
			var rock = Rock.Create<IAmSimple>();
			rock.Handle(_ => _.TargetAction());
			rock.Handle(_ => _.TargetFunc()).Returns(44);

			var chunk = rock.Make();
			chunk.TargetAction();
			var result = chunk.TargetFunc();

			rock.Verify();
		}
	}

	public interface IAmSimple
	{
		void TargetAction();
		int TargetFunc();
	}

	public interface IHaveParameterExpectations
	{
		void Target(int a);
	}

	public interface IHaveRefs
	{
		void Target(ref int a);
	}

	public interface IHaveGenerics<T>
	{
		void Target<Q>(T a, Q b);
	}

	public class MockedClass
	{
		public MockedClass(int value) { }

		public virtual void Target() { }
	}

	public interface IHaveAProperty
	{
		string GetterAndSetter { get; set; }
	}

	public interface IHaveIndexer
	{
		string this[string a] { get; set; }
	}

	public interface IHaveAnEvent
	{
		event EventHandler TargetEvent;
		void Target(int a);
	}

	public interface IValue { }

	public interface IProduceValue
	{
		IValue Produce();
	}

	public class UsesProducer
	{
		private readonly IProduceValue producer;

		public UsesProducer(IProduceValue producer)
		{
			this.producer = producer;
		}

		public IValue GetValue()
		{
			return this.producer.Produce();
		}
	}
}