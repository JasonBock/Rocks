using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using static Rocks.Extensions.ExpressionOfTExtensions;
using static Rocks.Extensions.IDictionaryOfTKeyTValueExtensions;
using static Rocks.Extensions.IMockExtensions;
using static Rocks.Extensions.MethodCallExpressionExtensions;
using static Rocks.Extensions.MethodInfoExtensions;
using static Rocks.Extensions.PropertyInfoExtensions;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks
{
	internal abstract class RockCore<T> 
		: IRock<T>
		where T : class
	{
		protected RockCore() { }

		protected ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> CreateReadOnlyHandlerDictionary()
		{
			return new ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>(
				this.Handlers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.AsReadOnly()));
		}

		public MethodAdornments Handle(Expression<Action<T>> expression, Delegate handler)
		{
			this.Namespaces.Add(handler.GetType().Namespace);
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle(Expression<Action<T>> expression, Delegate handler, uint expectedCallCount)
		{
			this.Namespaces.Add(handler.GetType().Namespace);
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle(Expression<Action<T>> expression)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle(Expression<Action<T>> expression, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle(Expression<Action<T>> expression, Action handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle(Expression<Action<T>> expression, Action handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1>(Expression<Action<T>> expression, Action<T1> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1>(Expression<Action<T>> expression, Action<T1> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2>(Expression<Action<T>> expression, Action<T1, T2> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2>(Expression<Action<T>> expression, Action<T1, T2> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3>(Expression<Action<T>> expression, Action<T1, T2, T3> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3>(Expression<Action<T>> expression, Action<T1, T2, T3> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4>(Expression<Action<T>> expression, Action<T1, T2, T3, T4> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4>(Expression<Action<T>> expression, Action<T1, T2, T3, T4> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments<TResult> Handle<TResult>(Expression<Func<T, TResult>> expression)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments<TResult>(info);
		}

		public MethodAdornments<TResult> Handle<TResult>(Expression<Func<T, TResult>> expression, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments<TResult>(info);
		}

		public MethodAdornments Handle<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, TResult>(Expression<Func<T, TResult>> expression, Func<T1, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, TResult>(Expression<Func<T, TResult>> expression, Func<T1, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> handler)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> handler, uint expectedCallCount)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = new HandlerInformation<TResult>(handler, expectedCallCount, methodCall.GetArgumentExpectations());
			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public PropertyMethodAdornments Handle(string name)
		{
			var property = typeof(T).FindProperty(name);
			var getterInfo = default(HandlerInformation);
			var setterInfo = default(HandlerInformation);

			if (property.CanRead)
			{
				getterInfo = property.GetGetterHandler();
				this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
					() => new List<HandlerInformation> { getterInfo }, _ => _.Add(getterInfo));
			}

			if (property.CanWrite)
			{
				setterInfo = property.GetSetterHandler();
				this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
					() => new List<HandlerInformation> { setterInfo }, _ => _.Add(setterInfo));
			}

			return new PropertyMethodAdornments(
				getterInfo != null ? new MethodAdornments(getterInfo) : null, 
				setterInfo != null ? new MethodAdornments(setterInfo) : null);
		}

		// TODO, probably need to rename method so I don't get ambiguous calls :(
		//public MethodAdornments HandleProperty<TPropertyValue>(string name, Expression<Func<TPropertyValue>> setterExpectation)
		//{
		//	var property = typeof(T).FindProperty(name);

		//	if (property.CanRead)
		//	{
		//		this.Handlers[property.GetMethod.MetadataToken] = property.GetGetterHandler();
		//	}

		//	if (property.CanWrite)
		//	{
		//		this.Handlers[property.SetMethod.MetadataToken] = new HandlerInformation(
		//			property.CreateDefaultSetterExpectation());
		//	}
		//}

		public PropertyMethodAdornments Handle(string name, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(name);
			var getterInfo = default(HandlerInformation);
			var setterInfo = default(HandlerInformation);

			if (property.CanRead)
			{
				getterInfo = property.GetGetterHandler(expectedCallCount);
				this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
					() => new List<HandlerInformation> { getterInfo }, _ => _.Add(getterInfo));
			}

			if (property.CanWrite)
			{
				setterInfo = property.GetSetterHandler(expectedCallCount);
				this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
					() => new List<HandlerInformation> { setterInfo }, _ => _.Add(setterInfo));
			}

			return new PropertyMethodAdornments(
				getterInfo != null ? new MethodAdornments(getterInfo) : null,
				setterInfo != null ? new MethodAdornments(setterInfo) : null);
		}

		public MethodAdornments Handle<TPropertyValue>(string name, Func<TPropertyValue> getter)
		{
			var property = typeof(T).FindProperty(name, PropertyAccessors.Get);
			var info = property.GetGetterHandler(getter);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<TPropertyValue>(string name, Func<TPropertyValue> getter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(name, PropertyAccessors.Get);
			var info = property.GetGetterHandler(getter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<TPropertyValue>(string name, Action<TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(name, PropertyAccessors.Set);
			var info = property.GetSetterHandler(setter);
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<TPropertyValue>(string name, Action<TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(name, PropertyAccessors.Set);
			var info = property.GetSetterHandler(setter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public PropertyMethodAdornments Handle<TPropertyValue>(string name, Func<TPropertyValue> getter, Action<TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(name, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(getter);
			var setInfo = property.GetSetterHandler(setter);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
			return new PropertyMethodAdornments(new MethodAdornments(getInfo), new MethodAdornments(setInfo));
		}

		public PropertyMethodAdornments Handle<TPropertyValue>(string name, Func<TPropertyValue> getter, Action<TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(name, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(getter, expectedCallCount);
			var setInfo = property.GetSetterHandler(setter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
			return new PropertyMethodAdornments(new MethodAdornments(getInfo), new MethodAdornments(setInfo));
		}

		public PropertyMethodAdornments Handle(Expression<Func<object[]>> indexers)
		{
			var indexerExpressions = indexers.ParseForPropertyIndexers();
			var property = typeof(T).FindProperty(indexerExpressions.Select(_ => _.Type).ToArray());
			var getterInfo = default(HandlerInformation);
			var setterInfo = default(HandlerInformation);

			if (property.CanRead)
			{
				getterInfo = property.GetGetterHandler(indexerExpressions);
				this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
					() => new List<HandlerInformation> { getterInfo }, _ => _.Add(getterInfo));
			}

			if (property.CanWrite)
			{
				setterInfo = property.GetSetterHandler(indexerExpressions);
				this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
					() => new List<HandlerInformation> { setterInfo }, _ => _.Add(setterInfo));
			}

			return new PropertyMethodAdornments(
				getterInfo != null ? new MethodAdornments(getterInfo) : null,
				setterInfo != null ? new MethodAdornments(setterInfo) : null);
		}

		public PropertyMethodAdornments Handle(Expression<Func<object[]>> indexers, uint expectedCallCount)
		{
			var indexerExpressions = indexers.ParseForPropertyIndexers();
			var property = typeof(T).FindProperty(indexerExpressions.Select(_ => _.Type).ToArray());
			var getterInfo = default(HandlerInformation);
			var setterInfo = default(HandlerInformation);

			if (property.CanRead)
			{
				getterInfo = property.GetGetterHandler(indexerExpressions, expectedCallCount);
				this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
					() => new List<HandlerInformation> { getterInfo }, _ => _.Add(getterInfo));
			}

			if (property.CanWrite)
			{
				setterInfo = property.GetSetterHandler(indexerExpressions, expectedCallCount);
				this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
					() => new List<HandlerInformation> { setterInfo }, _ => _.Add(setterInfo));
			}

			return new PropertyMethodAdornments(
				getterInfo != null ? new MethodAdornments(getterInfo) : null,
				setterInfo != null ? new MethodAdornments(setterInfo) : null);
		}

		public MethodAdornments Handle<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Func<T1, TPropertyValue> getter)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1) }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), getter);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Func<T1, TPropertyValue> getter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1) }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), getter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Action<T1, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1) }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), setter);
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Action<T1, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1) }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), setter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public PropertyMethodAdornments Handle<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Func<T1, TPropertyValue> getter, Action<T1, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1) }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), getter);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), setter);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
			return new PropertyMethodAdornments(new MethodAdornments(getInfo), new MethodAdornments(setInfo));
		}

		public PropertyMethodAdornments Handle<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Func<T1, TPropertyValue> getter, Action<T1, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1) }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), getter, expectedCallCount);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body }.AsReadOnly(), setter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
			return new PropertyMethodAdornments(new MethodAdornments(getInfo), new MethodAdornments(setInfo));
		}

		public MethodAdornments Handle<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Func<T1, T2, TPropertyValue> getter)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2) }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), getter);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Func<T1, T2, TPropertyValue> getter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2) }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), getter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Action<T1, T2, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2) }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), setter);
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Action<T1, T2, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2) }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), setter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public PropertyMethodAdornments Handle<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Func<T1, T2, TPropertyValue> getter, Action<T1, T2, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2) }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), getter);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), setter);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
			return new PropertyMethodAdornments(new MethodAdornments(getInfo), new MethodAdornments(setInfo));
		}

		public PropertyMethodAdornments Handle<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Func<T1, T2, TPropertyValue> getter, Action<T1, T2, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2) }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), getter, expectedCallCount);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body }.AsReadOnly(), setter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
			return new PropertyMethodAdornments(new MethodAdornments(getInfo), new MethodAdornments(setInfo));
		}

		public MethodAdornments Handle<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Func<T1, T2, T3, TPropertyValue> getter)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2), typeof(T3) }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), getter);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Func<T1, T2, T3, TPropertyValue> getter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2), typeof(T3) }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), getter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Action<T1, T2, T3, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2), typeof(T3) }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), setter);
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Action<T1, T2, T3, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2), typeof(T3) }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), setter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public PropertyMethodAdornments Handle<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Func<T1, T2, T3, TPropertyValue> getter, Action<T1, T2, T3, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2), typeof(T3) }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), getter);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), setter);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
			return new PropertyMethodAdornments(new MethodAdornments(getInfo), new MethodAdornments(setInfo));
		}

		public PropertyMethodAdornments Handle<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Func<T1, T2, T3, TPropertyValue> getter, Action<T1, T2, T3, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2), typeof(T3) }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), getter, expectedCallCount);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body }.AsReadOnly(), setter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
			return new PropertyMethodAdornments(new MethodAdornments(getInfo), new MethodAdornments(setInfo));
		}

		public MethodAdornments Handle<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Func<T1, T2, T3, T4, TPropertyValue> getter)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), getter);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Func<T1, T2, T3, T4, TPropertyValue> getter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, PropertyAccessors.Get);
			var info = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), getter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Action<T1, T2, T3, T4, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), setter);
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public MethodAdornments Handle<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Action<T1, T2, T3, T4, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, PropertyAccessors.Set);
			var info = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), setter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments(info);
		}

		public PropertyMethodAdornments Handle<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Func<T1, T2, T3, T4, TPropertyValue> getter, Action<T1, T2, T3, T4, TPropertyValue> setter)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), getter);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), setter);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
			return new PropertyMethodAdornments(new MethodAdornments(getInfo), new MethodAdornments(setInfo));
		}

		public PropertyMethodAdornments Handle<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Func<T1, T2, T3, T4, TPropertyValue> getter, Action<T1, T2, T3, T4, TPropertyValue> setter, uint expectedCallCount)
		{
			var property = typeof(T).FindProperty(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, PropertyAccessors.GetAndSet);
			var getInfo = property.GetGetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), getter, expectedCallCount);
			var setInfo = property.GetSetterHandler(new List<Expression> { indexer1.Body, indexer2.Body, indexer3.Body, indexer4.Body }.AsReadOnly(), setter, expectedCallCount);
			this.Handlers.AddOrUpdate(property.GetMethod.MetadataToken,
				() => new List<HandlerInformation> { getInfo }, _ => _.Add(getInfo));
			this.Handlers.AddOrUpdate(property.SetMethod.MetadataToken,
				() => new List<HandlerInformation> { setInfo }, _ => _.Add(setInfo));
			return new PropertyMethodAdornments(new MethodAdornments(getInfo), new MethodAdornments(setInfo));
		}

		public abstract T Make();

		public abstract T Make(object[] constructorArguments);

		public void Verify()
		{
			var failures = new List<string>();

			foreach (var rock in this.Rocks)
			{
				failures.AddRange(rock.GetVerificationFailures());
			}

			if (failures.Count > 0)
			{
				throw new VerificationException(failures);
			}
		}

		protected Dictionary<int, List<HandlerInformation>> Handlers { get; } = new Dictionary<int, List<HandlerInformation>>();
		protected SortedSet<string> Namespaces { get; } = new SortedSet<string>();
		protected List<IMock> Rocks { get; } = new List<IMock>();
	}
}
