using Rocks.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Rocks
{
	public static class Rock
	{
		public static Rock<T> Create<T>()
			where T : class
		{
			var message = typeof(T).Validate();

			if (!string.IsNullOrWhiteSpace(message))
			{
				throw new RockException(message);
			}

			return new Rock<T>();
		}

		public static bool TryCreate<T>(out Rock<T> result)
			where T : class
		{
			result = default(Rock<T>);

			var message = typeof(T).Validate();

			if (!string.IsNullOrWhiteSpace(message))
			{
				return false;
			}

			result = new Rock<T>();
			return true;
      }
	}

	public sealed class Rock<T>
		where T : class
	{
		private Dictionary<string, Delegate> handlers = 
			new Dictionary<string, Delegate>();

		internal Rock() { }

		public void Handle(Expression<Action<T>> expression,
			Action handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void Handle<T1>(Expression<Action<T>> expression,
			Action<T1> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void Handle<T1, T2>(Expression<Action<T>> expression,
			Action<T1, T2> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void Handle<T1, T2, T3>(Expression<Action<T>> expression,
			Action<T1, T2, T3> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void Handle<T1, T2, T3, T4>(Expression<Action<T>> expression,
			Action<T1, T2, T3, T4> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void Handle<TResult>(Expression<Func<T, TResult>> expression,
			Func<TResult> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void Handle<TResult, T1>(Expression<Func<T, TResult>> expression,
			Func<T1, TResult> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void Handle<TResult, T1, T2>(Expression<Func<T, TResult>> expression,
			Func<T1, T2, TResult> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void Handle<TResult, T1, T2, T3>(Expression<Func<T, TResult>> expression,
			Func<T1, T2, T3, TResult> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription()] = handler;
		}

		public void Handle<TResult, T1, T2, T3, T4>(Expression<Func<T, TResult>> expression,
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