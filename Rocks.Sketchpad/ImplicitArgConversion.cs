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
		}
	}

	public interface ITarget
	{
		void Foo(int a);
	}

	public static class IRockOfITargetExtensions
	{
		public static void Foo(this IRock<ITarget> @this, Argument<int> a)
		{
			Console.Out.WriteLine(a.Value);
		}
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
		public static implicit operator Argument<T>(T value)
		{
			return new Argument<T>(value);
		}

		public Argument(T value)
		{
			this.Value = value;
		}

		public T Value { get; }
	}
}
