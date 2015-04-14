using System;
using System.Collections.Generic;

namespace Rocks.Sketchpad
{
	public static class InAndOut
	{
		public static void Test()
		{
			IInAndOut<SubCC, SubCC> x = new BaseInAndOut<SubCC>();
			Console.Out.WriteLine($"{(x as IIn<SubCC>) == null}");
			Console.Out.WriteLine($"{(x as IIn<BaseCC>) == null}");
			Console.Out.WriteLine($"{(x as IOut<SubCC>) == null}");
			Console.Out.WriteLine($"{(x as IOut<BaseCC>) == null}");
		}
	}

	public interface IIn<in T>
	{
		void Bar(T a);
	}

	public interface IOut<out T>
	{
		T Foo();
	}

	public interface IInAndOut<in TIn, out TOut>
		: IIn<TIn>, IOut<TOut>
	{
	}

	public class BaseInAndOut<T>
		: IInAndOut<T, T>
	{
		public virtual void Bar(T a) { }

		public virtual T Foo() { return default(T); }
	}

	public class BaseCC { }

	public class SubCC : BaseCC { }
}
