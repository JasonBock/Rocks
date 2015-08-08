using System;
using System.Linq.Expressions;

namespace Rocks.Sketchpad
{
	public static class ExpressionEvaluation
	{
		public static void Evaluate<T, TReturn>(Expression<Func<T, TReturn>> handler)
		{
			var m = (handler.Body as MethodCallExpression).Method;
		}
	}

	public interface IExpressions
	{
		void TestMethod<U>(int a, string b, U c);
	}

	public class Data
	{
		public int Field;
		public int Property { get; set; }

		public int Value() { return Field + Property; }
	}
}
