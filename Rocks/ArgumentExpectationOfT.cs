using System;
using System.Linq.Expressions;

namespace Rocks
{
	internal sealed class ArgumentExpectation<T>
		: ArgumentExpectation
	{
		private bool isAny;
		private bool isEvaluation;
		private bool isExpression;
		private bool isValue;
		private Func<T, bool> evaluation;
		private Delegate expression;
		private T value = default(T);

		internal ArgumentExpectation()
		{
			this.isAny = true;
		}

		internal ArgumentExpectation(T value)
		{
			this.isValue = true;
			this.value = value;
		}

		internal ArgumentExpectation(Func<T, bool> evaluation)
		{
			if (evaluation == null)
			{
				throw new ArgumentNullException(nameof(evaluation));
			}

			this.isEvaluation = true;
			this.evaluation = evaluation;
      }

		internal ArgumentExpectation(Expression expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}

			this.isExpression = true;
			this.expression = Expression.Lambda(expression).Compile();
		}

		internal bool IsValid(T value)
		{
			if(this.isAny)
			{
				return true;
			}
			else if(this.isValue)
			{
				if(this.value == null && value == null)
				{
					return true;
				}
				else if(this.value != null && value != null)
				{
					return this.value.Equals(value);
				}
				else
				{
					return false;
				}
			}
			else if(this.isExpression)
			{
				return ((T)this.expression.DynamicInvoke()).Equals(value);
			}
			else if (this.isEvaluation)
			{
				return this.evaluation(value);
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}
