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
	}
}
