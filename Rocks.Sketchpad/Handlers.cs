using System;
using System.Linq.Expressions;

namespace Rocks.Sketchpad
{
	public sealed class ArgumentExpectation<T>
	{
		private bool isAny;
		private bool isValue;
		private Delegate expression;
		private T value = default(T);

		public ArgumentExpectation()
		{
			this.isAny = true;
		}

		public ArgumentExpectation(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			this.isValue = true;
			this.value = value;
		}

		public ArgumentExpectation(Expression expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}

			this.expression = Expression.Lambda(expression).Compile();
		}

		public bool IsValid(T value)
		{
			return this.isAny ? true :
				this.isValue ? this.value.Equals(value) :
				((T)this.expression.DynamicInvoke()).Equals(value);
		}
	}

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

	public static class Arg
	{
		public static T Is<T>(Func<T, bool> evaluation) { return default(T); }
		public static T IsAny<T>() { return default(T); }
	}
}
