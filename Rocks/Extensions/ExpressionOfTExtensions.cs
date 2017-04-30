using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Rocks.Extensions
{
	internal static class ExpressionOfTExtensions
	{
		internal static ArgumentExpectation<TProperty> GetExpectationForSetter<TProperty>(this Expression<Func<TProperty>> @this)
		{
			return @this.Body.Create() as ArgumentExpectation<TProperty>;
		}

		internal static ReadOnlyCollection<Expression> ParseForPropertyIndexers(this Expression<Func<object[]>> @this)
		{
			var expressions = (@this.Body as NewArrayExpression);
			var indexerExpressions = new List<Expression>();

			foreach (var expression in expressions.Expressions)
			{
				switch (expression.NodeType)
				{
					case ExpressionType.Convert:
						indexerExpressions.Add((expression as UnaryExpression).Operand);
						break;
					default:
						indexerExpressions.Add(expression);
						break;
				}
			}

			return indexerExpressions.AsReadOnly();
		}
	}
}
