using System;
using System.Linq.Expressions;

namespace Rocks
{
	public partial interface IRock<T> 
		where T : class
	{
		MethodAdornments Handle(Expression<Action<T>> expression, Delegate handler);
		MethodAdornments Handle(Expression<Action<T>> expression, Delegate handler, uint expectedCallCount);
	}
}