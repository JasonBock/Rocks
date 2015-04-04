using Rocks.Exceptions;
using System;
using System.Linq.Expressions;

namespace Rocks
{
	public sealed class ArgumentExpectation<T>
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
		
		public bool IsValid(T value, string parameter)
		{
			var isValid = true;

			if (!this.IsAny)
			{
				if (this.IsValue)
				{
					if(!ObjectEquality.AreEqual(this.Value, value))
					{
						isValid = false;
               }
				}
				else if (this.IsExpression)
				{
					if(!ObjectEquality.AreEqual(((T)this.Expression.DynamicInvoke()), value))
					{
						isValid = false;
					}
				}
				else // Must be this.IsEvaluation
				{
					if(!this.Evaluation(value))
					{
						isValid = false;
					}
				}
			}

			return isValid;
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
