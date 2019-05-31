using System;
using SLE = System.Linq.Expressions;

namespace Rocks
{
	[Serializable]
   public sealed class ArgumentIsExpressionExpectation<T>
	   : ArgumentExpectation<T>
   {
		internal ArgumentIsExpressionExpectation(SLE.Expression expression) => 
			this.Expression = SLE.Expression.Lambda(expression).Compile() ?? 
				throw new ArgumentNullException(nameof(expression));

		public override bool IsValid(T value) => 
			ObjectEquality.AreEqual((T)this.Expression.DynamicInvoke(), value);

		internal Delegate Expression { get; }
   }
}