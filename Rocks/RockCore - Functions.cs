using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		private MethodAdornments HandleLambda<TResult>(LambdaExpression expression,
			Func<ReadOnlyDictionary<string, ArgumentExpectation>, HandlerInformation<TResult>> infoGenerator)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = infoGenerator(methodCall.GetArgumentExpectations());

			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments<TResult>(info);
		}

		private MethodAdornments<TResult> HandleLambdaWithResult<TResult>(LambdaExpression expression, 
			Func<ReadOnlyDictionary<string, ArgumentExpectation>, HandlerInformation<TResult>> infoGenerator)
		{
			var methodCall = ((MethodCallExpression)expression.Body);
			var method = methodCall.Method;
			method.AddNamespaces(this.Namespaces);

			var info = infoGenerator(methodCall.GetArgumentExpectations());

			this.Handlers.AddOrUpdate(method.MetadataToken,
				() => new List<HandlerInformation> { info }, _ => _.Add(info));
			return new MethodAdornments<TResult>(info);
		}

		public MethodAdornments<TResult> Handle<TResult>(Expression<Func<T, TResult>> expression) => 
			this.HandleLambdaWithResult(expression, expectations => new HandlerInformation<TResult>(expectations));

		public MethodAdornments<TResult> Handle<TResult>(Expression<Func<T, TResult>> expression, uint expectedCallCount) => 
			this.HandleLambdaWithResult(expression, expectations => new HandlerInformation<TResult>(expectedCallCount, expectations));

		public MethodAdornments Handle<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> handler) => 
			this.HandleLambda(expression, expectations => new HandlerInformation<TResult>(handler, expectations));

		public MethodAdornments Handle<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> handler, uint expectedCallCount) => 
			this.HandleLambda(expression, expectations => new HandlerInformation<TResult>(handler, expectedCallCount, expectations));

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
	}
}
