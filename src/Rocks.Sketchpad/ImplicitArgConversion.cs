using System;

namespace Rocks.Sketchpad
{
	public static class ImplicitArgConversion
	{
		public static void Test()
		{
			var ua = new UsesArguments();
			ua.Foo(44, Guid.NewGuid(), "a");
			(null as IRock<ITarget>).Foo(22);

			var rock = Rock.Create<ITarget>();
			rock.Foo(44);
			rock.Handle(_ => _.Foo(Arg.Is<int>(v => v % 2 == 0)));
		}
	}

	public interface ITarget
	{
		void Foo(int a);
	}

	public static class IRockOfITargetExtensions
	{
		public static void Foo(this IRock<ITarget> @this, Argument<int> a) =>
			@this.Handle(_ => _.Foo(a.Value));
	}

	public class UsesArguments
	{
		public void Foo(Argument<int> a, Argument<Guid> b, Argument<string> c)
		{
			Console.Out.WriteLine(a.Value);
			Console.Out.WriteLine(b.Value);
			Console.Out.WriteLine(c.Value);
		}
	}

	public class Argument<T>
	{
		public static implicit operator Argument<T>(T value) =>
			new Argument<T>(value);

		public Argument(T value) => this.Value = value;

		public T Value { get; }
	}
}
