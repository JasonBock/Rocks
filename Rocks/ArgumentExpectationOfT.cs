using System;
using System.Linq.Expressions;

namespace Rocks
{
#if !NETCOREAPP1_1
	[Serializable]
#endif
	public sealed class ArgumentExpectation<T>
		: ArgumentExpectation
	{
		internal ArgumentExpectation() => this.IsAny = true;

		internal ArgumentExpectation(T value)
		{
			this.IsValue = true;
			this.Value = value;
		}

		internal ArgumentExpectation(Func<T, bool> evaluation)
		{
			this.IsEvaluation = true;
			this.Evaluation = evaluation ?? throw new ArgumentNullException(nameof(evaluation));
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
					isValid = ObjectEquality.AreEqual(this.Value, value);
				}
				else if (this.IsExpression)
				{
					isValid = ObjectEquality.AreEqual(((T)this.Expression.DynamicInvoke()), value);
				}
				else // Must be this.IsEvaluation
				{
					isValid = this.Evaluation(value);
				}
			}

			return isValid;
		}

		internal Func<T, bool> Evaluation { get; }
		internal Delegate Expression { get; }
		internal bool IsAny { get; }
		internal bool IsEvaluation { get; }
		internal bool IsExpression { get; }
		internal bool IsValue { get; }
		internal T Value { get; }
	}
}
