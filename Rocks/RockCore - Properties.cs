using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static Rocks.Extensions.ExpressionOfTExtensions;
using static Rocks.Extensions.IDictionaryOfTKeyTValueExtensions;
using static Rocks.Extensions.PropertyInfoExtensions;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks
{
	internal abstract partial class RockCore<T>
		: IRock<T>
		where T : class
	{
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
	}
}
