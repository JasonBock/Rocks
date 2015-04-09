using Rocks.Construction;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using static Rocks.Extensions.ExpressionOfTExtensions;
using static Rocks.Extensions.IDictionaryOfTKeyTValueExtensions;
using static Rocks.Extensions.IRockExtensions;
using static Rocks.Extensions.MethodCallExpressionExtensions;
using static Rocks.Extensions.MethodInfoExtensions;
using static Rocks.Extensions.PropertyInfoExtensions;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks
{
	public static class Rock
	{
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


		internal static AssemblyBinder Binder { get; } = new AssemblyBinder();
      internal static Dictionary<CacheKey, Type> Cache { get; } = new Dictionary<CacheKey, Type>();
      internal static object CacheLock { get; } = new object();
   }

	public sealed class Rock<T>
		where T : class
	{
		private List<IRock> rocks = new List<IRock>();
		private Dictionary<string, List<HandlerInformation>> handlers =
			new Dictionary<string, List<HandlerInformation>>();
		private Options options;
		private SortedSet<string> namespaces = new SortedSet<string>();

		internal Rock(Options options)
		{
			this.options = options;
		}

		public void HandleDelegate(Expression<Action<T>> expression, Delegate handler)
		{
			this.namespaces.Add(handler.GetType().Namespace);
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleDelegate(Expression<Action<T>> expression, Delegate handler, uint expectedCallCount)
		{
			this.namespaces.Add(handler.GetType().Namespace);
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction(Expression<Action<T>> expression)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction(Expression<Action<T>> expression, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction(Expression<Action<T>> expression, Action handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction(Expression<Action<T>> expression, Action handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1>(Expression<Action<T>> expression, Action<T1> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1>(Expression<Action<T>> expression, Action<T1> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2>(Expression<Action<T>> expression, Action<T1, T2> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2>(Expression<Action<T>> expression, Action<T1, T2> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3>(Expression<Action<T>> expression, Action<T1, T2, T3> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3>(Expression<Action<T>> expression, Action<T1, T2, T3> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4>(Expression<Action<T>> expression, Action<T1, T2, T3, T4> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4>(Expression<Action<T>> expression, Action<T1, T2, T3, T4> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public ReturnValue<TResult> HandleFunc<TResult>(Expression<Func<T, TResult>> expression)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new ReturnValue<TResult>(info);
		}

		public ReturnValue<TResult> HandleFunc<TResult>(Expression<Func<T, TResult>> expression, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new ReturnValue<TResult>(info);
		}

		public void HandleFunc<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, TResult>(Expression<Func<T, TResult>> expression, Func<T1, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, TResult>(Expression<Func<T, TResult>> expression, Func<T1, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> handler)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> handler, uint expectedCallCount)
		{
			var method = ((MethodCallExpression)expression.Body);
			var info = new HandlerInformation<TResult>(handler, expectedCallCount, method.GetArgumentExpectations());
			this.handlers.AddOrUpdate(method.Method.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty(string name)
		{
			var property = typeof(T).FindProperty(name);

			if (property.CanRead)
			{
				var info = property.GetGetterHandler();
				this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
					() => new List<HandlerInformation> { info }, _ => _.Add(info));
			}

			if (property.CanWrite)
			{
				var info = property.GetSetterHandler();
				this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
					() => new List<HandlerInformation> { info }, _ => _.Add(info));
			}
		}

		// TODO, probably need to rename method so I don't get ambiguous calls :(
		//public void HandleProperty<TPropertyValue>(string name, Expression<Func<TPropertyValue>> setterExpectation)
		//{
		//	var property = typeof(T).FindProperty(name);

		//	if (property.CanRead)
		//	{
		//		this.handlers[property.GetMethod.GetMethodDescription(this.namespaces)] = property.GetGetterHandler();
		//	}

		//	if (property.CanWrite)
		//	{
		//		this.handlers[property.SetMethod.GetMethodDescription(this.namespaces)] = new HandlerInformation(
		//			property.CreateDefaultSetterExpectation());
		//	}
		//}

		public void HandleProperty(string name, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(name);

			if (property.CanRead)
			{
				var info = property.GetGetterHandler(expectedCallCount);
				this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
					() => new List<HandlerInformation> { info }, _ => _.Add(info));
			}

			if (property.CanWrite)
			{
				var info = property.GetSetterHandler(expectedCallCount);
				this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
					() => new List<HandlerInformation> { info }, _ => _.Add(info));
			}
		}

		public void HandleProperty<TPropertyValue>(string name, Func<TPropertyValue> getter)
		{
			var property = typeof(T).FindProperty(name, PropertyAccessors.Get);
			var info = property.GetGetterHandler(getter);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<TPropertyValue>(string name, Func<TPropertyValue> getter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(name, PropertyAccessors.Get);
			var info = property.GetGetterHandler(getter, expectedCallCount);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<TPropertyValue>(string name, Action<TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(name, PropertyAccessors.Set);
			var info = property.GetSetterHandler(setter);
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<TPropertyValue>(string name, Action<TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(name, PropertyAccessors.Set);
			var info = property.GetSetterHandler(setter, expectedCallCount);
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<TPropertyValue>(string name, Func<TPropertyValue> getter, Action<TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(name, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(getter);
			var setInfo = property.GetSetterHandler(setter);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
		}

		public void HandleProperty<TPropertyValue>(string name, Func<TPropertyValue> getter, Action<TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(name, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(getter, expectedCallCount);
			var setInfo = property.GetSetterHandler(setter, expectedCallCount);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
		}

		public void HandleProperty(Expression<Func<object[]>> indexers)
		{
			var indexerExpressions = indexers.ParseForPropertyIndexers();
			var property = typeof(T).FindProperty(indexerExpressions.Select(_ => _.Type).ToArray());

			if (property.CanRead)
			{
				var info = property.GetGetterHandler(indexerExpressions);
				this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
					() => new List<HandlerInformation> { info }, _ => _.Add(info));
			}

			if (property.CanWrite)
			{
				var info = property.GetSetterHandler(indexerExpressions);
				this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
					() => new List<HandlerInformation> { info }, _ => _.Add(info));
			}
		}

		public void HandleProperty(Expression<Func<object[]>> indexers, uint expectedCallCount)
		{
			var indexerExpressions = indexers.ParseForPropertyIndexers();
			var property = typeof(T).FindProperty(indexerExpressions.Select(_ => _.Type).ToArray());

			if (property.CanRead)
			{
				var info = property.GetGetterHandler(indexerExpressions);
				this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
					() => new List<HandlerInformation> { info }, _ => _.Add(info));
			}

			if (property.CanWrite)
			{
				var info = property.GetSetterHandler(indexerExpressions);
				this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
					() => new List<HandlerInformation> { info }, _ => _.Add(info));
			}
		}

		public void HandleProperty<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Func<T1, TPropertyValue> getter)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), getter);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Func<T1, TPropertyValue> getter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), getter, expectedCallCount);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Action<T1, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), setter);
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Action<T1, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), setter, expectedCallCount);
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Func<T1, TPropertyValue> getter, Action<T1, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), getter);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), setter);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
		}

		public void HandleProperty<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Func<T1, TPropertyValue> getter, Action<T1, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), getter, expectedCallCount);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), setter, expectedCallCount);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
		}

		public void HandleProperty<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Func<T1, T2, TPropertyValue> getter)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), getter);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Func<T1, T2, TPropertyValue> getter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), getter, expectedCallCount);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Action<T1, T2, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), setter);
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Action<T1, T2, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), setter, expectedCallCount);
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Func<T1, T2, TPropertyValue> getter, Action<T1, T2, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), getter);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), setter);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
		}

		public void HandleProperty<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Func<T1, T2, TPropertyValue> getter, Action<T1, T2, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), getter, expectedCallCount);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), setter, expectedCallCount);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
		}

		public void HandleProperty<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Func<T1, T2, T3, TPropertyValue> getter)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type, indexer3.Body.Type }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), getter);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Func<T1, T2, T3, TPropertyValue> getter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type, indexer3.Body.Type }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), getter, expectedCallCount);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Action<T1, T2, T3, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type, indexer3.Body.Type }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), setter);
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Action<T1, T2, T3, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type, indexer3.Body.Type }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), setter, expectedCallCount);
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Func<T1, T2, T3, TPropertyValue> getter, Action<T1, T2, T3, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type, indexer3.Body.Type }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), getter);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), setter);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
		}

		public void HandleProperty<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Func<T1, T2, T3, TPropertyValue> getter, Action<T1, T2, T3, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type, indexer3.Body.Type }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), getter, expectedCallCount);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), setter, expectedCallCount);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
		}

		public void HandleProperty<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Func<T1, T2, T3, T4, TPropertyValue> getter)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type, indexer3.Body.Type, indexer4.Body.Type }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), getter);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Func<T1, T2, T3, T4, TPropertyValue> getter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type, indexer3.Body.Type, indexer4.Body.Type }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), getter, expectedCallCount);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Action<T1, T2, T3, T4, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type, indexer3.Body.Type, indexer4.Body.Type }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), setter);
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Action<T1, T2, T3, T4, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type, indexer3.Body.Type, indexer4.Body.Type }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), setter, expectedCallCount);
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
		}

		public void HandleProperty<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Func<T1, T2, T3, T4, TPropertyValue> getter, Action<T1, T2, T3, T4, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type, indexer3.Body.Type, indexer4.Body.Type }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), getter);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), setter);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
		}

		public void HandleProperty<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Func<T1, T2, T3, T4, TPropertyValue> getter, Action<T1, T2, T3, T4, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { indexer1.Body.Type, indexer2.Body.Type, indexer3.Body.Type, indexer4.Body.Type }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), getter, expectedCallCount);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), setter, expectedCallCount);
			this.handlers.AddOrUpdate(property.GetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.handlers.AddOrUpdate(property.SetMethod.GetMethodDescription(this.namespaces),
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
		}

		public T Make()
		{
			var readOnlyHandlers = new ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>>(
				this.handlers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.AsReadOnly()));
			var rockType = this.GetMockType(readOnlyHandlers);

			var rock = Activator.CreateInstance(rockType, readOnlyHandlers);
			this.rocks.Add(rock as IRock);
			return rock as T;
		}

		public T Make(object[] constructorArguments)
		{
			var readOnlyHandlers = new ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>>(
				this.handlers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.AsReadOnly()));
			var rockType = this.GetMockType(readOnlyHandlers);

			var arguments = new List<object> { readOnlyHandlers };
			arguments.AddRange(constructorArguments);

			var rock = Activator.CreateInstance(rockType, arguments.ToArray(), null);
			this.rocks.Add(rock as IRock);
			return rock as T;
		}

		private Type GetMockType(ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>> readOnlyHandlers)
		{
			var tType = typeof(T);
			var rockType = default(Type);
			var key = new CacheKey(tType, this.options);

			lock (Rock.CacheLock)
			{
				if (Rock.Cache.ContainsKey(key))
				{
					rockType = Rock.Cache[key];
				}
				else
				{
					rockType = new InMemoryMaker(tType, readOnlyHandlers, this.namespaces, this.options).Mock;

					if (!tType.ContainsRefAndOrOutParameters())
					{
						Rock.Cache.Add(key, rockType);
					}

					if(this.options.Serialization == SerializationOptions.Supported)
					{
						Rock.Binder.Assemblies.Add(rockType.Assembly);
					}
				}
			}

			return rockType;
		}

		public void Verify()
		{
			var failures = new List<string>();

			foreach (var rock in this.rocks)
			{
				failures.AddRange(rock.GetVerificationFailures());
			}

			if (failures.Count > 0)
			{
				throw new VerificationException(failures);
			}
		}
	}
}