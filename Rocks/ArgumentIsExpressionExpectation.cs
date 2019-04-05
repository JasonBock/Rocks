using System;
using System.Linq.Expressions;

namespace Rocks
{
	[Serializable]
   public sealed class ArgumentIsExpressionExpectation<T>
		: ArgumentExpectation
   {
		internal ArgumentIsExpressionExpectation(Expression expression) => 
			this.Expression = System.Linq.Expressions.Expression.Lambda(expression).Compile() ?? 
				throw new ArgumentNullException(nameof(expression));

		public bool IsValid(T value) => 
			ObjectEquality.AreEqual(((T)this.Expression.DynamicInvoke()), value);

		internal Delegate Expression { get; }
   }
}