// Lifted from:
// https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/CallerArgumentExpressionAttribute.cs,7b959540ed2c1ea9
// Polyfill is needed because this attribute was added for collection expressions,
// and is not available in NS 2.0

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
internal sealed class CallerArgumentExpressionAttribute : Attribute
{
   public CallerArgumentExpressionAttribute(string parameterName) => 
		this.ParameterName = parameterName;

   public string ParameterName { get; }
}