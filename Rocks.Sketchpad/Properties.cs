using System;
using System.Linq.Expressions;

namespace Rocks.Sketchpad
{
	internal static class Properties
	{
		internal static void Test()
		{
			var propertyType = typeof(IProperties);

			foreach (var property in propertyType.GetProperties())
			{
				Console.Out.WriteLine($"Property: {property.Name}, {nameof(property.CanRead)}: {property.CanRead} - {nameof(property.CanWrite)}: {property.CanWrite}");

				if ((property.CanRead && property.GetGetMethod().GetParameters().Length > 0) ||
					(property.CanWrite && property.GetSetMethod().GetParameters().Length > 1))
				{
					Console.Out.WriteLine($"Property {property.Name} is an indexer.");
				}

				Console.Out.WriteLine();
			}

			Expression<Func<IProperties, int>> p = _ => _.ReadAndWrite;

			//Properties.HandleGet(_ => _.WriteOnly);
			//Properties.HandleSet(_ => _.ReadAndWrite);
		}

		internal static void HandleGet<TResult>(Expression<Func<IProperties, TResult>> expression)
		{
		}

		internal static void HandleSet(Expression<Action<IProperties>> expression)
		{
		}
	}

	public class RockProperties<T>
	{
		public void HandleGetProperty<TResult>(Expression<Func<T, TResult>> expression) { }

		public void HandleGetProperty<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> getter) { }

		public void HandleGetProperty<TResult>(Expression<Func<T, TResult>> expression, uint expectedCallCount) { }

		public void HandleGetProperty<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> getter, uint expectedCallCount) { }

		public void HandleSetProperty(Expression<Action<T>> expression) { }

		public void HandleSetProperty(Expression<Action<T>> expression, Action setter) { }

		public void HandleSetProperty(Expression<Action<T>> expression, uint expectedCallCount) { }

		public void HandleSetProperty(Expression<Action<T>> expression, Action setter, uint expectedCallCount) { }

		public void HandleGetSetProperty<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> getter, Action setter) { }

		public void HandleGetSetProperty<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> getter, uint expectedGetterCallCount, Action setter) { }

		public void HandleGetSetProperty<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> getter, Action setter, uint expectedSetterCallCount) { }

		public void HandleGetSetProperty<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> getter, uint expectedGetterCallCount, Action setter, uint expectedSetterCallCount) { }





		public void HandleProperty<TIndex, TResult>(Expression<Action<T>> expression, Func<TIndex, TResult> getter) { }

		public void HandleProperty<TIndex, TResult>(Expression<Action<T>> expression, Func<TIndex, TResult> getter, uint expectedCallCount) { }

		public void HandleProperty<TIndex>(Expression<Action<T>> expression, Action<TIndex> setter) { }

		public void HandleProperty<TIndex>(Expression<Action<T>> expression, Action<TIndex> setter, uint expectedCallCount) { }

		public void HandleProperty<TResult>(Expression<Action<T>> expression, Func<TResult> getter, Action setter) { }

		public void HandleProperty<TResult>(Expression<Action<T>> expression, Func<TResult> getter, uint getterExpectedCallCount, Action setter, uint setterExpectedCallCount) { }

		public void HandleProperty<TResult, TIndex>(Expression<Action<T>> expression, Func<TIndex, TResult> getter, Action<TIndex> setter) { }

		public void HandleProperty<TResult, TIndex>(Expression<Action<T>> expression, Func<TIndex, TResult> getter, uint getterExpectedCallCount, Action<TIndex> setter, uint setterExpectedCallCount) { }
	}

	public interface IProperties
	{
		int ReadAndWrite { get; set; }
		int ReadOnly { get; }
		int WriteOnly { set; }
		string this[int index] { get; set; }
		string this[string key] { get; set; }
		string this[int index, string key] { get; set; }
	}
}
