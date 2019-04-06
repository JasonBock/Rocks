using Rocks.Exceptions;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Rocks.Extensions
{
	internal static class ExpressionExtensions
	{
		internal static ArgumentExpectation Create(this Expression @this) =>
			@this.CreateConstantExpectation(@this.Type);

		internal static ArgumentExpectation Create(this Expression @this, Type expectationType, ParameterInfo parameter)
		{
			switch (@this.NodeType)
			{
				case ExpressionType.Constant:
					return @this.CreateConstantExpectation(expectationType);
				case ExpressionType.Call:
					var argumentMethodCall = (MethodCallExpression)@this;
					var argumentMethod = argumentMethodCall.Method;
					var isMethod = typeof(Arg).GetMethod(nameof(Arg.Is));
					var isAnyMethod = typeof(Arg).GetMethod(nameof(Arg.IsAny));
					var isDefaultMethod = typeof(Arg).GetMethod(nameof(Arg.IsDefault));

					if (argumentMethod.Name == isAnyMethod.Name && argumentMethod.DeclaringType == isAnyMethod.DeclaringType)
					{
						return (ArgumentExpectation)typeof(ArgumentIsAnyExpectation<>).MakeGenericType(expectationType)
							.GetConstructor(ReflectionValues.PublicNonPublicInstance,
								null, Type.EmptyTypes, null).Invoke(null);
					}
					else if (argumentMethod.Name == isMethod.Name && argumentMethod.DeclaringType == isMethod.DeclaringType)
					{
						var evaluation = argumentMethodCall.Arguments[0];
						var genericMethodType = typeof(Func<,>).MakeGenericType(@this.Type, typeof(bool));
						return (ArgumentExpectation)typeof(ArgumentIsEvaluationExpectation<>).MakeGenericType(expectationType)
							.GetConstructor(ReflectionValues.PublicNonPublicInstance,
								null, new[] { genericMethodType }, null).Invoke(new[] { ((LambdaExpression)evaluation).Compile() });
					}
					else if (argumentMethod.Name == isDefaultMethod.Name)
					{
						if (!parameter.IsOptional)
						{
							throw new ExpectationException(ErrorMessages.GetCannotUseIsDefaultWithNonOptionalArguments
								(expectationType.Name, argumentMethod.Name, parameter.Name));
						}
						else
						{
							return (ArgumentExpectation)typeof(ArgumentIsValueExpectation<>).MakeGenericType(expectationType)
								.GetConstructor(ReflectionValues.PublicNonPublicInstance,
									null, new[] { @this.Type }, null).Invoke(new[] { parameter.DefaultValue });
						}
					}
					else
					{
						return (ArgumentExpectation)typeof(ArgumentIsExpressionExpectation<>).MakeGenericType(expectationType)
							.GetConstructor(ReflectionValues.PublicNonPublicInstance,
								null, new[] { typeof(Expression) }, null).Invoke(new[] { @this });
					}
				default:
					return (ArgumentExpectation)typeof(ArgumentIsExpressionExpectation<>).MakeGenericType(expectationType)
						.GetConstructor(ReflectionValues.PublicNonPublicInstance,
							null, new[] { typeof(Expression) }, null).Invoke(new[] { @this });
			}
		}

		private static ArgumentExpectation CreateConstantExpectation(this Expression @this, Type expectationType)
		{
			var value = ((ConstantExpression)@this).Value;
			return (ArgumentExpectation)typeof(ArgumentIsValueExpectation<>).MakeGenericType(expectationType)
				.GetConstructor(ReflectionValues.PublicNonPublicInstance,
					null, new[] { @this.Type }, null).Invoke(new[] { value });
		}
   }
}
