using System;
using System.Linq.Expressions;

namespace Rocks
{
	internal sealed class ArgumentExpectation<T>
		: ArgumentExpectation
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
			else
			{
				return ((T)this.expression.DynamicInvoke()).Equals(value);
			}
		}
	}
}
