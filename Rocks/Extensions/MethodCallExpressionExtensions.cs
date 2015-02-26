using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class MethodCallExpressionExtensions
	{
		internal static ReadOnlyDictionary<string, ArgumentExpectation> GetArgumentExpectations(this MethodCallExpression @this)
		{
			var expectations = new Dictionary<string, ArgumentExpectation>();

			var argumentIndex = 0;
			var methodArguments = @this.Method.GetParameters();

			foreach (var argument in @this.Arguments)
			{
				var methodArgument = methodArguments[argumentIndex];

				var argumentExpectationType = typeof(ArgumentExpectation<>).MakeGenericType(
					!methodArgument.ParameterType.IsByRef ? methodArgument.ParameterType : methodArgument.ParameterType.GetElementType());
				var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

            switch (argument.NodeType)
				{
					case ExpressionType.Constant:
						var value = (argument as ConstantExpression).Value;
						expectations.Add(methodArgument.Name,
							argumentExpectationType.GetConstructor(flags, null, new[] { methodArgument.ParameterType }, null)
								.Invoke(new[] { value }) as ArgumentExpectation);
						break;
					case ExpressionType.Call:
						var argumentMethodCall = (argument as MethodCallExpression);
						var argumentMethod = argumentMethodCall.Method;
						var isMethod = typeof(Arg).GetMethod(nameof(Arg.Is));
						var isAnyMethod = typeof(Arg).GetMethod(nameof(Arg.IsAny));

						if (argumentMethod.Name == isAnyMethod.Name && argumentMethod.DeclaringType == isAnyMethod.DeclaringType)
						{
							expectations.Add(methodArgument.Name,
								argumentExpectationType.GetConstructor(flags, null, Type.EmptyTypes, null)
									.Invoke(null) as ArgumentExpectation);
						}
						else if (argumentMethod.Name == isMethod.Name && argumentMethod.DeclaringType == isMethod.DeclaringType)
						{
							var evaluation = argumentMethodCall.Arguments[0];
							var genericMethodType = typeof(Func<,>).MakeGenericType(methodArgument.ParameterType, typeof(bool));
							expectations.Add(methodArgument.Name,
								argumentExpectationType.GetConstructor(flags, null, new[] { genericMethodType }, null)
									.Invoke(new[] { (evaluation as LambdaExpression).Compile() }) as ArgumentExpectation);
						}
						else
						{
							expectations.Add(methodArgument.Name,
								argumentExpectationType.GetConstructor(flags, null, new[] { typeof(Expression) }, null)
									.Invoke(new[] { argument }) as ArgumentExpectation);
						}

						break;
					default:
						expectations.Add(methodArgument.Name,
							argumentExpectationType.GetConstructor(flags, null, new[] { typeof(Expression) }, null)
								.Invoke(new[] { argument }) as ArgumentExpectation);
						break;
				}

				argumentIndex++;
			}

			return new ReadOnlyDictionary<string, ArgumentExpectation>(expectations);
		}
	}
}
