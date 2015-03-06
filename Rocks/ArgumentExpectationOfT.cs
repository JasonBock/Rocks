using Rocks.Exceptions;
using System;
using System.Collections.Generic;
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
		
		public void Validate(T value, string parameter)
		{
			if (!this.IsAny)
			{
				if (this.IsValue)
				{
					if(!this.AreEqual(this.Value, value, typeof(T)))
					{
						throw new ExpectationException($"Expectation on parameter {parameter} failed.");
               }
				}
				else if (this.IsExpression)
				{
					if(!this.AreEqual(((T)this.Expression.DynamicInvoke()), value, typeof(T)))
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

		private bool AreEqual(object value1, object value2, Type valueType)
		{
			if (value1 == null && value2 == null)
			{
				return true;
			}
			else if(value1 != null && value2 != null)
         {
				if (!valueType.IsArray)
				{
					return value1.Equals(value2);
				}
				else
				{
					var array1 = value1 as Array;
					var array2 = value2 as Array;

					if (array1.Length != array2.Length)
					{
						return false;
					}

					for (int i = 0; i < array1.Length; i++)
					{
						if (!this.AreEqual(array1.GetValue(i), array2.GetValue(i), array1.GetValue(i).GetType()))
						{
							return false;
						}
					}

					return true;
				}
			}
			else
			{
				return false;
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
