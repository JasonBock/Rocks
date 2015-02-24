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
		
		internal void Validate(T value, string parameter)
		{
			if (!this.IsAny)
			{
				if (this.IsValue)
				{
					if (this.Value == null && value == null)
					{
					}
					else if (this.Value != null && value != null)
					{
						if(!this.Value.Equals(value))
						{
							throw new ExpectationException($"Expectation on parameter {parameter} failed.");
                  }
					}
					else
					{
						throw new ExpectationException($"Expectation on parameter {parameter} failed.");
					}
				}
				else if (this.IsExpression)
				{
					if(!((T)this.Expression.DynamicInvoke()).Equals(value))
					{
						throw new ExpectationException($"Expectation on parameter {parameter} failed.");
					}
				}
				else if (this.IsEvaluation)
				{
					if(!this.Evaluation(value))
					{
						throw new ExpectationException($"Expectation on parameter {parameter} failed.");
					}
				}
				else
				{
					throw new NotImplementedException();
				}
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
