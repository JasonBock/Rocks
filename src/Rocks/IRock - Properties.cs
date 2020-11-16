using System;
using System.Linq.Expressions;

namespace Rocks
{
	public partial interface IRock<T> 
		where T : class
	{
		PropertyMethodAdornments Handle(string name);
		PropertyMethodAdornments Handle(Expression<Func<object[]>> indexers);
		PropertyMethodAdornments Handle(string name, uint expectedCallCount);
		PropertyMethodAdornments Handle(Expression<Func<object[]>> indexers, uint expectedCallCount);
		MethodAdornments Handle<TPropertyValue>(string name, Func<TPropertyValue> getter);
		MethodAdornments Handle<TPropertyValue>(string name, Action<TPropertyValue> setter);
		MethodAdornments Handle<TPropertyValue>(string name, Func<TPropertyValue> getter, uint expectedCallCount);
		PropertyMethodAdornments Handle<TPropertyValue>(string name, Func<TPropertyValue> getter, Action<TPropertyValue> setter);
		MethodAdornments Handle<TPropertyValue>(string name, Action<TPropertyValue> setter, uint expectedCallCount);
		PropertyMethodAdornments Handle<TPropertyValue>(string name, Func<TPropertyValue> getter, Action<TPropertyValue> setter, uint expectedCallCount);
		MethodAdornments Handle<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Func<T1, TPropertyValue> getter);
		MethodAdornments Handle<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Action<T1, TPropertyValue> setter);
		PropertyMethodAdornments Handle<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Func<T1, TPropertyValue> getter, Action<T1, TPropertyValue> setter);
		MethodAdornments Handle<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Func<T1, TPropertyValue> getter, uint expectedCallCount);
		MethodAdornments Handle<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Action<T1, TPropertyValue> setter, uint expectedCallCount);
		PropertyMethodAdornments Handle<T1, TPropertyValue>(Expression<Func<T1>> indexer1, Func<T1, TPropertyValue> getter, Action<T1, TPropertyValue> setter, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Func<T1, T2, TPropertyValue> getter);
		MethodAdornments Handle<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Action<T1, T2, TPropertyValue> setter);
		MethodAdornments Handle<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Func<T1, T2, TPropertyValue> getter, uint expectedCallCount);
		PropertyMethodAdornments Handle<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Func<T1, T2, TPropertyValue> getter, Action<T1, T2, TPropertyValue> setter);
		MethodAdornments Handle<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Action<T1, T2, TPropertyValue> setter, uint expectedCallCount);
		PropertyMethodAdornments Handle<T1, T2, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Func<T1, T2, TPropertyValue> getter, Action<T1, T2, TPropertyValue> setter, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Func<T1, T2, T3, TPropertyValue> getter);
		MethodAdornments Handle<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Action<T1, T2, T3, TPropertyValue> setter);
		MethodAdornments Handle<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Func<T1, T2, T3, TPropertyValue> getter, uint expectedCallCount);
		PropertyMethodAdornments Handle<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Func<T1, T2, T3, TPropertyValue> getter, Action<T1, T2, T3, TPropertyValue> setter);
		MethodAdornments Handle<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Action<T1, T2, T3, TPropertyValue> setter, uint expectedCallCount);
		PropertyMethodAdornments Handle<T1, T2, T3, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Func<T1, T2, T3, TPropertyValue> getter, Action<T1, T2, T3, TPropertyValue> setter, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Func<T1, T2, T3, T4, TPropertyValue> getter);
		MethodAdornments Handle<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Action<T1, T2, T3, T4, TPropertyValue> setter);
		MethodAdornments Handle<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Func<T1, T2, T3, T4, TPropertyValue> getter, uint expectedCallCount);
		PropertyMethodAdornments Handle<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Func<T1, T2, T3, T4, TPropertyValue> getter, Action<T1, T2, T3, T4, TPropertyValue> setter);
		MethodAdornments Handle<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Action<T1, T2, T3, T4, TPropertyValue> setter, uint expectedCallCount);
		PropertyMethodAdornments Handle<T1, T2, T3, T4, TPropertyValue>(Expression<Func<T1>> indexer1, Expression<Func<T2>> indexer2, Expression<Func<T3>> indexer3, Expression<Func<T4>> indexer4, Func<T1, T2, T3, T4, TPropertyValue> getter, Action<T1, T2, T3, T4, TPropertyValue> setter, uint expectedCallCount);
	}
}