using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Rocks.Extensions
{
	internal static class ExpressionOfTExtensions
	{
		private static void HandleCall(MethodCallExpression expression, List<Type> indexerTypes)
		{
			var argumentMethod = expression.Method;

			if (argumentMethod.DeclaringType == typeof(Arg))
			{
				indexerTypes.Add(argumentMethod.GetGenericArguments()[0]);
			}
			else
			{
				indexerTypes.Add(argumentMethod.ReturnType);
			}
		}

		private static void HandleConstant(ConstantExpression expression, List<Type> indexerTypes)
		{
			indexerTypes.Add(expression.Value.GetType());
		}

		internal static ArgumentExpectation GetExpectationForSetter<TPropertyValue>(this Expression<Func<TPropertyValue>> @this)
		{
			return @this.Body.Create(typeof(TPropertyValue));
		}

		internal static Tuple<ReadOnlyCollection<Type>, ReadOnlyCollection<Expression>> ParseForPropertyIndexers(this Expression<Func<object[]>> @this)
		{
			var expressions = (@this.Body as NewArrayExpression);
			var indexerTypes = new List<Type>();
			var indexerExpressions = new List<Expression>();

			foreach (var expression in expressions.Expressions)
			{
				switch (expression.NodeType)
				{
					case ExpressionType.Constant:
						ExpressionOfTExtensions.HandleConstant(expression as ConstantExpression, indexerTypes);
						indexerExpressions.Add(expression);
						break;
					case ExpressionType.Call:
						ExpressionOfTExtensions.HandleCall(expression as MethodCallExpression, indexerTypes);
						indexerExpressions.Add(expression);
						break;
					case ExpressionType.Convert:
						var operand = (expression as UnaryExpression).Operand;

						if (operand.NodeType == ExpressionType.Constant)
						{
							ExpressionOfTExtensions.HandleConstant(operand as ConstantExpression, indexerTypes);
						}
						else if (operand.NodeType == ExpressionType.Call)
						{
							ExpressionOfTExtensions.HandleCall(operand as MethodCallExpression, indexerTypes);
						}
						else
						{
							throw new NotSupportedException();
						}

						indexerExpressions.Add(operand);
						break;
					default:
						throw new NotSupportedException();
				}
			}

			return new Tuple<ReadOnlyCollection<Type>, ReadOnlyCollection<Expression>>(
				indexerTypes.AsReadOnly(), indexerExpressions.AsReadOnly());
		}

		internal static Tuple<ReadOnlyCollection<Type>, ReadOnlyCollection<ArgumentExpectation>> ParseForPropertyIndexersAndSetter<TPropertyValue>(
			this Expression<Func<object[]>> @this, Expression<Func<TPropertyValue>> setter)
		{
			return null;
		}
	}
}
