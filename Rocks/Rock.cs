using Rocks.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Rocks
{
	public sealed class Rock<T>
		where T : class
	{
		private Dictionary<string, Delegate> handlers = 
			new Dictionary<string, Delegate>();

		public Rock()
		{
			var message = typeof(T).Validate();

			if (!string.IsNullOrWhiteSpace(message))
			{
				throw new RockException(message);
			}
		}

		public void HandleAction(Expression<Action<T>> expression,
			Action handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void HandleAction<T1>(Expression<Action<T>> expression,
			Action<T1> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void HandleAction<T1, T2>(Expression<Action<T>> expression,
			Action<T1, T2> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void HandleAction<T1, T2, T3>(Expression<Action<T>> expression,
			Action<T1, T2, T3> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void HandleFunction<T1, T2, T3, T4>(Expression<Action<T>> expression,
			Action<T1, T2, T3, T4> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void HandleFunction<TResult>(Expression<Func<T, TResult>> expression,
			Func<TResult> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void HandleFunction<TResult, T1>(Expression<Func<T, TResult>> expression,
			Func<T1, TResult> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void HandleFunction<TResult, T1, T2>(Expression<Func<T, TResult>> expression,
			Func<T1, T2, TResult> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void HandleFunction<TResult, T1, T2, T3>(Expression<Func<T, TResult>> expression,
			Func<T1, T2, T3, TResult> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void HandleFunction<TResult, T1, T2, T3, T4>(Expression<Func<T, TResult>> expression,
			Func<T1, T2, T3, T4, TResult> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}
		public T Make()
		{
			return RockMaker.Make<T>(this.handlers);
		}

		public T Make<T1>(T1 arg1)
		{
			var x = this.handlers.ToImmutableDictionary();
			return default(T);
		}
	}
}