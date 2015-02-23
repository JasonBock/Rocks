using System;
using System.Linq.Expressions;

namespace Rocks
{
	internal sealed class ArgumentExpectation<T>
	{
		private bool isAny;
		private bool isValue;
		private Delegate expression;
		private T value = default(T);

		internal ArgumentExpectation()
		{
			this.isAny = true;
		}

		internal ArgumentExpectation(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			this.isValue = true;
			this.value = value;
		}

		internal ArgumentExpectation(Expression expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}

			this.expression = Expression.Lambda(expression).Compile();
		}

		internal bool IsValid(T value)
		{
			return this.isAny ? true :
				this.isValue ? this.value.Equals(value) :
				((T)this.expression.DynamicInvoke()).Equals(value);
		}
	}
}
