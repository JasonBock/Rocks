using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Rocks.Sketchpad
{
	public sealed class ArgumentExpectation<T>
	{
		private bool isAny;
		private bool isExpression;
		private bool isValue;
		private Delegate expression;
		private T value = default(T);

		public ArgumentExpectation()
		{
			this.isAny = true;
		}

		public ArgumentExpectation(T value)
		{
			if(value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			this.isValue = true;
			this.value = value;
		}

		public ArgumentExpectation(Expression expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}

			this.isExpression = true;
			this.expression = Expression.Lambda(expression).Compile();
		}

		public bool IsValid(T value)
		{
			return this.isAny ? true :
				this.isValue ? this.value.Equals(value) :
				((T)this.expression.DynamicInvoke()).Equals(value);
		}
	}

	public static class ExpressionEvaluation
	{
		public static void Evaluate<T>(Expression<Action<T>> handler)
		{
			var argumentIndex = 0;
			var expressionMethod = (handler.Body as MethodCallExpression);
			var expressionMethodArguments = expressionMethod.Method.GetParameters();

         foreach (var argument in expressionMethod.Arguments)
			{
				var expressionMethodArgument = expressionMethodArguments[argumentIndex];

            switch (argument.NodeType)
				{
					case ExpressionType.Constant:
						Console.Out.WriteLine($"{nameof(ExpressionType.Constant)} for {expressionMethodArgument.Name}: {(argument as ConstantExpression).Value}");
						break;
					case ExpressionType.Call:
						var method = (argument as MethodCallExpression).Method;
						var validMethod = typeof(Arg).GetMethod(nameof(Arg.IsAny));

                  if (method.Name == validMethod.Name && method.DeclaringType == validMethod.DeclaringType)
						{
							Console.Out.WriteLine($"{nameof(ExpressionType.Call)} for {expressionMethodArgument.Name}: {method.Name}");
						}
						else
						{
							Console.Out.WriteLine($"{nameof(ExpressionType.Call)} captured for {expressionMethodArgument.Name}: {argument.ToString()}");
							var callInvoker = Expression.Lambda(argument).Compile();
							Console.Out.WriteLine($"{nameof(ExpressionType.Call)} value for {expressionMethodArgument.Name}: {callInvoker.DynamicInvoke()}");
						}
						break;
					default:
						Console.Out.WriteLine($"{argument.NodeType.ToString()} captured for {expressionMethodArgument.Name}: {argument.ToString()}");
						var defaultInvoker = Expression.Lambda(argument).Compile();
						Console.Out.WriteLine($"{argument.NodeType.ToString()} value for {expressionMethodArgument.Name}: {defaultInvoker.DynamicInvoke()}");
						break;
				}

				argumentIndex++;
         }
		}

		public static void Evaluate<T, TReturn>(Expression<Func<T, TReturn>> handler)
		{
			var m = (handler.Body as MethodCallExpression).Method;
		}
	}

	public interface IExpressions
	{
		void TestMethod<U>(int a, string b, U c);
	}

	public class Data
	{
		public int Field;
		public int Property { get; set; }

		public int Value() { return Field + Property; }
	}

	public static class Arg
	{
		public static T Is<T>(Func<T, bool> evaluation) { return default(T); }
		public static T IsAny<T>() { return default(T); }
	}

	class Program
	{
		public static int Q;

		static void Main(string[] args)
		{
			var data = new Data { Field = 22, Property = 33 };
			Program.Q = 100;
			var dataF = data.Field;

			ExpressionEvaluation.Evaluate<IExpressions>(_ => _.TestMethod(data.Value(), "", Guid.Empty));
			//Program.TestGenerics();
		}


		private static void TestGenerics()
		{
			var genericType = typeof(IGeneric<string>);

			var methodThatUsesTName = nameof(IGeneric<string>.MethodThatUsesT);
			Console.Out.WriteLine(methodThatUsesTName);
			var methodThatUsesT = genericType.GetMethod(methodThatUsesTName);
			Console.Out.WriteLine(nameof(methodThatUsesT.IsGenericMethod) + ": " + methodThatUsesT.IsGenericMethod);
			Console.Out.WriteLine(nameof(methodThatUsesT.IsGenericMethodDefinition) + ": " + methodThatUsesT.IsGenericMethodDefinition);
			Console.Out.WriteLine();

			var methodThatUsesItsOwnGenericName = nameof(IGeneric<string>.MethodThatUsesItsOwnGeneric);
			Console.Out.WriteLine(methodThatUsesItsOwnGenericName);
			var methodThatUsesItsOwnGeneric = genericType.GetMethod(methodThatUsesItsOwnGenericName);
			Console.Out.WriteLine(nameof(methodThatUsesItsOwnGeneric.IsGenericMethod) + ": " + methodThatUsesItsOwnGeneric.IsGenericMethod);
			Console.Out.WriteLine(nameof(methodThatUsesItsOwnGeneric.IsGenericMethodDefinition) + ": " + methodThatUsesItsOwnGeneric.IsGenericMethodDefinition);
			var methodThatUsesItsOwnGenericConstructed = methodThatUsesItsOwnGeneric.MakeGenericMethod(typeof(int));
			Console.Out.WriteLine(nameof(methodThatUsesItsOwnGenericConstructed.IsGenericMethod) + ": " + methodThatUsesItsOwnGenericConstructed.IsGenericMethod);
			Console.Out.WriteLine(nameof(methodThatUsesItsOwnGenericConstructed.IsGenericMethodDefinition) + ": " + methodThatUsesItsOwnGenericConstructed.IsGenericMethodDefinition);
			Program.ShowGenericAttributes(methodThatUsesItsOwnGeneric);
			Console.Out.WriteLine();

			var methodThatUsesItsOwnGenericWithNonTypeConstraintsName = nameof(IGeneric<string>.MethodThatUsesItsOwnGenericWithNonTypeConstraints);
			Console.Out.WriteLine(methodThatUsesItsOwnGenericWithNonTypeConstraintsName);
			var methodThatUsesItsOwnGenericWithNonTypeConstraints = genericType.GetMethod(methodThatUsesItsOwnGenericWithNonTypeConstraintsName);
			Console.Out.WriteLine(nameof(methodThatUsesItsOwnGenericWithNonTypeConstraints.IsGenericMethod) + ": " + methodThatUsesItsOwnGenericWithNonTypeConstraints.IsGenericMethod);
			Console.Out.WriteLine(nameof(methodThatUsesItsOwnGenericWithNonTypeConstraints.IsGenericMethodDefinition) + ": " + methodThatUsesItsOwnGenericWithNonTypeConstraints.IsGenericMethodDefinition);
			Console.Out.WriteLine(methodThatUsesItsOwnGenericWithNonTypeConstraints.GetParameters()[0].ParameterType.Name);
			Console.Out.WriteLine(methodThatUsesItsOwnGenericWithNonTypeConstraints.GetParameters()[1].ParameterType.Name);
			Program.ShowGenericAttributes(methodThatUsesItsOwnGenericWithNonTypeConstraints);
			Console.Out.WriteLine();

			var methodThatUsesItsOwnGenericWithTypeConstraintsName = nameof(IGeneric<string>.MethodThatUsesItsOwnGenericWithTypeConstraints);
			Console.Out.WriteLine(methodThatUsesItsOwnGenericWithTypeConstraintsName);
			var methodThatUsesItsOwnGenericWithTypeConstraints = genericType.GetMethod(methodThatUsesItsOwnGenericWithTypeConstraintsName);
			Console.Out.WriteLine(nameof(methodThatUsesItsOwnGenericWithTypeConstraints.IsGenericMethod) + ": " + methodThatUsesItsOwnGenericWithTypeConstraints.IsGenericMethod);
			Console.Out.WriteLine(nameof(methodThatUsesItsOwnGenericWithTypeConstraints.IsGenericMethodDefinition) + ": " + methodThatUsesItsOwnGenericWithTypeConstraints.IsGenericMethodDefinition);
			Program.ShowGenericAttributes(methodThatUsesItsOwnGenericWithTypeConstraints);
			Console.Out.WriteLine();
		}

		private static void ShowGenericAttributes(MethodInfo methodThatUsesItsOwnGeneric)
		{
			foreach (var genericArgument in methodThatUsesItsOwnGeneric.GetGenericArguments())
			{
				Console.Out.WriteLine(nameof(genericArgument.IsInterface) + ": " + genericArgument.IsInterface);
				Console.Out.WriteLine(nameof(genericArgument.IsClass) + ": " + genericArgument.IsClass);
				Console.Out.WriteLine(nameof(genericArgument.IsValueType) + ": " + genericArgument.IsValueType);
				Console.Out.WriteLine(Program.ListGenericParameterAttributes(genericArgument));
				Console.Out.WriteLine(genericArgument.Name);
				foreach (var constraint in genericArgument.GetGenericParameterConstraints())
				{
					Console.Out.WriteLine(constraint);
				}
			}
		}

		private static string ListGenericParameterAttributes(Type t)
		{
			var attributes = t.GenericParameterAttributes;
			var variance = attributes & GenericParameterAttributes.VarianceMask;
			var info = variance == GenericParameterAttributes.None ?
				"No variance flag;" :
				(variance & GenericParameterAttributes.Covariant) != 0 ?
					"Covariant;" : "Contravariant;";

			var constraints = attributes & GenericParameterAttributes.SpecialConstraintMask;

			if (constraints == GenericParameterAttributes.None)
			{
				info += " No special constraints";
			}
			else
			{
				if ((constraints & GenericParameterAttributes.ReferenceTypeConstraint) != 0)
				{
					info += " ReferenceTypeConstraint";
				}
				if ((constraints & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0)
				{
					info += " NotNullableValueTypeConstraint";
				}
				if ((constraints & GenericParameterAttributes.DefaultConstructorConstraint) != 0)
				{
					info += " DefaultConstructorConstraint";
				}
			}

			return info;
		}
	}
}
