using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Rocks.Extensions.IDictionaryOfTKeyTValueExtensions;
using static Rocks.Extensions.MethodCallExpressionExtensions;
using static Rocks.Extensions.MethodInfoExtensions;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks
{
	internal abstract partial class RockCore<T>
		: IRock<T>
		where T : class
	{
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
	}
}
