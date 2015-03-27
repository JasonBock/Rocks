using Rocks.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

				if (property.CanWrite)
				{
					Console.Out.WriteLine(property.SetMethod.GetArgumentNameList());
				}

				if (property.GetIndexParameters().Length > 0)
				{
					Console.Out.WriteLine($"Property {property.Name} is an indexer.");

					// The value "argument" on the setter comes last in the argument list
				}

				Console.Out.WriteLine();
			}

			Properties.TestIndexers(() => new object[] { 44, Properties.GetValue(), Arg.IsAny<string>(),
				Arg.Is<Guid>(_ => true), "44", Guid.NewGuid(), Arg.Is<string>(_ => false) });
		}

		internal static string GetArgumentNameList(this MethodBase @this)
		{
			return string.Join(", ",
				(from parameter in @this.GetParameters()
				 select $"{parameter.Name}"));
		}

		private static string GetValue()
		{
			return "44";
		}

		private static void HandleCall(MethodCallExpression expression, List<Type> indexerTypes)
		{
			var argumentMethod = expression.Method;

			if (argumentMethod.DeclaringType == typeof(Arg))
			{
				indexerTypes.Add(argumentMethod.GetGenericArguments()[0]);
			}
			else
			{
				indexerTypes.Add(argumentMethod.ReturnType);
			}
		}

		private static void HandleConstant(ConstantExpression expression, List<Type> indexerTypes)
		{
			indexerTypes.Add(expression.Value.GetType());
		}

		private static void TestIndexers(Expression<Func<object[]>> indexers)
		{
			var expressions = (indexers.Body as NewArrayExpression);
			var indexerTypes = new List<Type>();

			foreach (var expression in expressions.Expressions)
			{
				switch(expression.NodeType)
				{
					case ExpressionType.Constant:
						Properties.HandleConstant(expression as ConstantExpression, indexerTypes);
						break;
					case ExpressionType.Call:
						Properties.HandleCall(expression as MethodCallExpression, indexerTypes);
						break;
					case ExpressionType.Convert:
						var operand = (expression as UnaryExpression).Operand;

						if(operand.NodeType == ExpressionType.Constant)
						{
							Properties.HandleConstant(operand as ConstantExpression, indexerTypes);
						}
						else if (operand.NodeType == ExpressionType.Call)
						{
							Properties.HandleCall(operand as MethodCallExpression, indexerTypes);
						}
						else
						{
							throw new NotSupportedException();
						}

						break;
					default:
						throw new NotSupportedException();
				}
			}
		}
	}

	public class RockProperties<T>
	{
		// Must check the property that it exists, it's virtual, and it has the required get and/or set as needed.
		// Get and/or set
		public void HandleProperty(string name) { }
		// Get and/or set
		public void HandleProperty(string name, object[] indexers) { }
		// Get and/or set
		public void HandleProperty(string name, uint expectedCallCount) { }
		// Get and/or set
		public void HandleProperty(string name, object[] indexers, uint expectedCallCount) { }
		// Get
		public void HandleProperty<TResult>(string name, Func<TResult> getter) { }
		// Get
		public void HandleProperty<TResult>(string name, object[] indexers, Func<TResult> getter) { }
		// Get
		public void HandleProperty<TResult>(string name, Func<TResult> getter, uint expectedCallCount) { }
		// Get
		public void HandleProperty<TResult>(string name, object[] indexers, Func<TResult> getter, uint expectedCallCount) { }
		// Set
		public void HandleProperty(string name, Action setter) { }
		// Set
		public void HandleProperty(string name, object[] indexers, Action setter) { }
		// Set
		public void HandleProperty(string name, Action setter, uint expectedCallCount) { }
		// Set
		public void HandleProperty(string name, object[] indexers, Action setter, uint expectedCallCount) { }
		// Get AND set
		public void HandleProperty<TResult>(string name, Func<TResult> getter, Action setter) { }
		// Get AND set
		public void HandleProperty<TResult>(string name, object[] indexers, Func<TResult> getter, Action setter) { }
		// Get AND set
		public void HandleProperty<TResult>(string name, Func<TResult> getter, Action setter, uint expectedCallCount) { }
		// Get AND set
		public void HandleProperty<TResult>(string name, object[] indexers, Func<TResult> getter, Action setter, uint expectedCallCount) { }
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
