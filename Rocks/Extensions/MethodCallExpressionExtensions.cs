using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

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

				var argumentExpectationType = !methodArgument.ParameterType.IsByRef ? 
					methodArgument.ParameterType : methodArgument.ParameterType.GetElementType();
				expectations.Add(methodArgument.Name, argument.Create(argumentExpectationType));
				argumentIndex++;
			}

			return new ReadOnlyDictionary<string, ArgumentExpectation>(expectations);
		}
	}
}
