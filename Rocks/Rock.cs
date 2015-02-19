using Rocks.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Rocks
{
	public static class Rock
	{
		internal static ConcurrentDictionary<Type, Type> cache = new ConcurrentDictionary<Type, Type>();

		public static Rock<T> Create<T>()
			where T : class
		{
			return Rock.Create<T>(new RockOptions());
		}

		public static Rock<T> Create<T>(RockOptions options)
			where T : class
		{
			var message = typeof(T).Validate();

			if (!string.IsNullOrWhiteSpace(message))
			{
				throw new RockValidationException(message);
			}

			return new Rock<T>(options);
		}

		public static bool TryCreate<T>(out Rock<T> result)
			where T : class
		{
			return Rock.TryCreate<T>(new RockOptions(), out result);
		}

		public static bool TryCreate<T>(RockOptions options, out Rock<T> result)
			where T : class
		{
			result = default(Rock<T>);

			var message = typeof(T).Validate();

			if (!string.IsNullOrWhiteSpace(message))
			{
				return false;
			}

			result = new Rock<T>(options);
			return true;
		}
	}

	public sealed class Rock<T>
		where T : class
	{
		private List<IRock> rocks = new List<IRock>();
		private Dictionary<string, HandlerInformation> handlers = 
			new Dictionary<string, HandlerInformation>();
		private RockOptions options;
		private SortedSet<string> namespaces = new SortedSet<string>();

		internal Rock()
			: this(new RockOptions())
		{ }

		internal Rock(RockOptions options)
		{
			this.options = options;
		}

		public void Handle(Expression<Action<T>> expression,
			Action handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription(this.namespaces)] = 
				new HandlerInformation(handler);
		}

		public void Handle<T1>(Expression<Action<T>> expression,
			Action<T1> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler);
		}

		public void Handle<T1, T2>(Expression<Action<T>> expression,
			Action<T1, T2> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler);
		}

		public void Handle<T1, T2, T3>(Expression<Action<T>> expression,
			Action<T1, T2, T3> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler);
		}

		public void Handle<T1, T2, T3, T4>(Expression<Action<T>> expression,
			Action<T1, T2, T3, T4> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler);
		}

		public void Handle<TResult>(Expression<Func<T, TResult>> expression,
			Func<TResult> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler);
		}

		public void Handle<TResult, T1>(Expression<Func<T, TResult>> expression,
			Func<T1, TResult> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler);
		}

		public void Handle<TResult, T1, T2>(Expression<Func<T, TResult>> expression,
			Func<T1, T2, TResult> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler);
		}

		public void Handle<TResult, T1, T2, T3>(Expression<Func<T, TResult>> expression,
			Func<T1, T2, T3, TResult> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler);
		}

		public void Handle<TResult, T1, T2, T3, T4>(Expression<Func<T, TResult>> expression,
			Func<T1, T2, T3, T4, TResult> handler)
		{
			this.handlers[((MethodCallExpression)expression.Body).Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler);
		}

		public T Make()
		{
			var tType = typeof(T);
			var readOnlyHandlers = new ReadOnlyDictionary<string, HandlerInformation>(this.handlers);
			var rockType = Rock.cache.GetOrAdd(tType,
				_ => new RockMaker(_, readOnlyHandlers, this.namespaces, this.options).Mock);
			var rock = Activator.CreateInstance(rockType, readOnlyHandlers);
			this.rocks.Add(rock as IRock);
         return rock as T;
		}

		public void Verify()
		{
			var failures = new List<string>();

			foreach(var rock in this.rocks)
			{
				foreach(var pair in rock.Handlers)
				{
					foreach(var failure in pair.Value.Verify())
					{
						failures.Add(string.Format(Constants.ErrorMessages.VerificationFailed,
							rock.GetType().FullName, pair.Key, failure));
					}
				}
			}

			if(failures.Count > 0)
			{
				throw new RockVerificationException(failures);
			}
		}
	}
}