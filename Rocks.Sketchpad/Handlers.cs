using System;
using System.Linq.Expressions;

namespace Rocks.Sketchpad
{
	public sealed class ArgumentExpectation<T>
	{
		private bool isAny;
		private bool isExpression;
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

			this.isExpression = true;
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
		public static void Evaluate<T>(Expression<Action<T>> handler)
		{
			var argumentIndex = 0;
			var expressionMethod = (handler.Body as MethodCallExpression);
			var expressionMethodArguments = expressionMethod.Method.GetParameters();

			foreach (var argument in expressionMethod.Arguments)
			{
				var expressionMethodArgument = expressionMethodArguments[argumentIndex];

				switch (argument.NodeType)
				{
					case ExpressionType.Constant:
						Console.Out.WriteLine($"{nameof(ExpressionType.Constant)} for {expressionMethodArgument.Name}: {(argument as ConstantExpression).Value}");
						break;
					case ExpressionType.Call:
						var method = (argument as MethodCallExpression).Method;
						var validMethod = typeof(Arg).GetMethod(nameof(Arg.IsAny));

						if (method.Name == validMethod.Name && method.DeclaringType == validMethod.DeclaringType)
						{
							Console.Out.WriteLine($"{nameof(ExpressionType.Call)} for {expressionMethodArgument.Name}: {method.Name}");
						}
						else
						{
							Console.Out.WriteLine($"{nameof(ExpressionType.Call)} captured for {expressionMethodArgument.Name}: {argument.ToString()}");
							var callInvoker = Expression.Lambda(argument).Compile();
							Console.Out.WriteLine($"{nameof(ExpressionType.Call)} value for {expressionMethodArgument.Name}: {callInvoker.DynamicInvoke()}");
						}
						break;
					default:
						Console.Out.WriteLine($"{argument.NodeType.ToString()} captured for {expressionMethodArgument.Name}: {argument.ToString()}");
						var defaultInvoker = Expression.Lambda(argument).Compile();
						Console.Out.WriteLine($"{argument.NodeType.ToString()} value for {expressionMethodArgument.Name}: {defaultInvoker.DynamicInvoke()}");
						break;
				}

				argumentIndex++;
			}
		}

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
