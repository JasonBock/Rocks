using System;
using System.Linq.Expressions;

namespace Rocks
{
	internal sealed class ArgumentExpectation<T>
		: ArgumentExpectation
	{
		internal ArgumentExpectation()
		{
			this.IsAny = true;
		}

		internal ArgumentExpectation(T value)
		{
			this.IsValue = true;
			this.Value = value;
		}

		internal ArgumentExpectation(Func<T, bool> evaluation)
		{
			if (evaluation == null)
			{
				throw new ArgumentNullException(nameof(evaluation));
			}

			this.IsEvaluation = true;
			this.Evaluation = evaluation;
      }

		internal ArgumentExpectation(Expression expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}

			this.IsExpression = true;
			this.Expression = System.Linq.Expressions.Expression.Lambda(expression).Compile();
		}

		internal bool IsValid(T value)
		{
			if(this.IsAny)
			{
				return true;
			}
			else if(this.IsValue)
			{
				if(this.Value == null && value == null)
				{
					return true;
				}
				else if(this.Value != null && value != null)
				{
					return this.Value.Equals(value);
				}
				else
				{
					return false;
				}
			}
			else if(this.IsExpression)
			{
				return ((T)this.Expression.DynamicInvoke()).Equals(value);
			}
			else if (this.IsEvaluation)
			{
				return this.Evaluation(value);
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		internal Func<T, bool> Evaluation { get; private set; }
		internal Delegate Expression { get; private set; }
		internal bool IsAny { get; private set; }
		internal bool IsEvaluation { get; private set; }
		internal bool IsExpression { get; private set; }
		internal bool IsValue { get; private set; }
		internal T Value { get; private set; }
	}
}
