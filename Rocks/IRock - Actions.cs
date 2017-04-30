using System;
using System.Linq.Expressions;

namespace Rocks
{
	public partial interface IRock<T> 
		where T : class
	{
		MethodAdornments Handle(Expression<Action<T>> expression);
		MethodAdornments Handle(Expression<Action<T>> expression, uint expectedCallCount);
		MethodAdornments Handle(Expression<Action<T>> expression, Action handler);
		MethodAdornments Handle(Expression<Action<T>> expression, Action handler, uint expectedCallCount);
		MethodAdornments Handle<T1>(Expression<Action<T>> expression, Action<T1> handler);
		MethodAdornments Handle<T1>(Expression<Action<T>> expression, Action<T1> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2>(Expression<Action<T>> expression, Action<T1, T2> handler);
		MethodAdornments Handle<T1, T2>(Expression<Action<T>> expression, Action<T1, T2> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3>(Expression<Action<T>> expression, Action<T1, T2, T3> handler);
		MethodAdornments Handle<T1, T2, T3>(Expression<Action<T>> expression, Action<T1, T2, T3> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, T4>(Expression<Action<T>> expression, Action<T1, T2, T3, T4> handler);
		MethodAdornments Handle<T1, T2, T3, T4>(Expression<Action<T>> expression, Action<T1, T2, T3, T4> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, T4, T5>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5> handler);
		MethodAdornments Handle<T1, T2, T3, T4, T5>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6> handler);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7> handler);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8> handler);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> handler);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> handler);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> handler);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> handler);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> handler);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> handler);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> handler);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> handler, uint expectedCallCount);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> handler);
		MethodAdornments Handle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Expression<Action<T>> expression, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> handler, uint expectedCallCount);
	}
}