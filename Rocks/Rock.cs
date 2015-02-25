using Rocks.Exceptions;
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
			return Rock.Create<T>(new Options());
		}

		public static Rock<T> Create<T>(Options options)
			where T : class
		{
			var message = typeof(T).Validate();

			if (!string.IsNullOrWhiteSpace(message))
			{
				throw new ValidationException(message);
			}

			return new Rock<T>(options);
		}

		public static CreateResult<T> TryCreate<T>()
			where T : class
		{
			return Rock.TryCreate<T>(new Options());
		}

		public static CreateResult<T> TryCreate<T>(Options options)
			where T : class
		{
			var result = default(Rock<T>);
			var isSuccessful = false;

			var message = typeof(T).Validate();

			if (string.IsNullOrWhiteSpace(message))
			{
				result = new Rock<T>(options);
				isSuccessful = true;
			}

			return new CreateResult<T>(isSuccessful, result);
		}
	}

	public sealed class Rock<T>
		where T : class
	{
		private List<IRock> rocks = new List<IRock>();
		private Dictionary<string, HandlerInformation> handlers =
			new Dictionary<string, HandlerInformation>();
		private Options options;
		private SortedSet<string> namespaces = new SortedSet<string>();

		internal Rock()
			: this(new Options())
		{ }

		internal Rock(Options options)
		{
			this.options = options;
		}

		public void HandleAction(Expression<Action<T>> expression)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(method.GetArgumentExpectations());
		}

		public void HandleAction(Expression<Action<T>> expression, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(expectedCallCount, method.GetArgumentExpectations());
		}

		public void HandleAction(Expression<Action<T>> expression, Action handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler, method.GetArgumentExpectations());
		}

		public void HandleAction(Expression<Action<T>> expression, Action handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
		}

		public void HandleAction<T1>(Expression<Action<T>> expression, Action<T1> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler, method.GetArgumentExpectations());
		}

		public void HandleAction<T1>(Expression<Action<T>> expression, Action<T1> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
		}

		public void HandleAction<T1, T2>(Expression<Action<T>> expression, Action<T1, T2> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler, method.GetArgumentExpectations());
		}

		public void HandleAction<T1, T2>(Expression<Action<T>> expression, Action<T1, T2> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
		}

		public void HandleAction<T1, T2, T3>(Expression<Action<T>> expression, Action<T1, T2, T3> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler, method.GetArgumentExpectations());
		}

		public void HandleAction<T1, T2, T3>(Expression<Action<T>> expression, Action<T1, T2, T3> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
		}

		public void HandleAction<T1, T2, T3, T4>(Expression<Action<T>> expression, Action<T1, T2, T3, T4> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler, method.GetArgumentExpectations());
		}

		public void HandleAction<T1, T2, T3, T4>(Expression<Action<T>> expression, Action<T1, T2, T3, T4> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
		}

		public void HandleAction<T1, T2, T3, T4, T5>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler, method.GetArgumentExpectations());
		}

		public void HandleAction<T1, T2, T3, T4, T5>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
		}

		public ReturnValue<TResult> HandleFunc<TResult>(Expression<Func<T, TResult>> expression)
		{
			var method = ((MethodCallExpression)expression.Body);
			var handler = new HandlerInformation<TResult>(method.GetArgumentExpectations());
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] = handler;
				return new ReturnValue<TResult>(handler);
		}

		public ReturnValue<TResult> HandleFunc<TResult>(Expression<Func<T, TResult>> expression, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var handler = new HandlerInformation<TResult>(expectedCallCount, method.GetArgumentExpectations());
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] = handler;
			return new ReturnValue<TResult>(handler);
		}

		public void HandleFunc<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
		}

		public void HandleFunc<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
		}

		public void HandleFunc<T1, TResult>(Expression<Func<T, TResult>> expression, Func<T1, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
		}

		public void HandleFunc<T1, TResult>(Expression<Func<T, TResult>> expression, Func<T1, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
		}

		public void HandleFunc<T1, T2, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
		}

		public void HandleFunc<T1, T2, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
		}

		public void HandleFunc<T1, T2, T3, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
		}

		public void HandleFunc<T1, T2, T3, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
		}

		public void HandleFunc<T1, T2, T3, T4, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
		}

		public void HandleFunc<T1, T2, T3, T4, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			this.handlers[method.Method.GetMethodDescription(this.namespaces)] =
				new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
		}

		public T Make()
		{
			var tType = typeof(T);
			var readOnlyHandlers = new ReadOnlyDictionary<string, HandlerInformation>(this.handlers);
			var rockType = Rock.cache.GetOrAdd(tType,
				_ => new Maker(_, readOnlyHandlers, this.namespaces, this.options).Mock);
			var rock = Activator.CreateInstance(rockType, readOnlyHandlers);
			this.rocks.Add(rock as IRock);
			return rock as T;
		}

		public void Verify()
		{
			var failures = new List<string>();

			foreach (var rock in this.rocks)
			{
				foreach (var pair in rock.Handlers)
				{
					foreach (var failure in pair.Value.Verify())
					{
						failures.Add(string.Format(Constants.ErrorMessages.VerificationFailed,
							rock.GetType().FullName, pair.Key, failure));
					}
				}
			}

			if (failures.Count > 0)
			{
				throw new VerificationException(failures);
			}
		}
	}
}