using System;
using SLE = System.Linq.Expressions;

namespace Rocks
{
	[Serializable]
	public sealed class ArgumentIsExpressionExpectation<T>
		: ArgumentExpectation<T>
	{
		internal ArgumentIsExpressionExpectation(SLE.Expression expression) =>
			this.Expression = SLE.Expression.Lambda(expression, Array.Empty<SLE.ParameterExpression>()).Compile() ??
				throw new ArgumentNullException(nameof(expression));

		public override bool IsValid(T value) =>
			ObjectEquality.AreEqual((T)this.Expression.DynamicInvoke(Array.Empty<object>()), value);

		internal Delegate Expression { get; }
	}
}