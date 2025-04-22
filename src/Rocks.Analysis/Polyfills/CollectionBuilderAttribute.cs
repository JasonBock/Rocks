// Lifted from:
// https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/CollectionBuilderAttribute.cs,cb7de8edc6e88f9b
// Polyfill is needed because this attribute was introduced in .NET 8
// and is not available in NS 2.0

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false)]
internal sealed class CollectionBuilderAttribute 
	: Attribute
{
   public CollectionBuilderAttribute(Type builderType, string methodName) => 
		(this.BuilderType, this.MethodName) = (builderType, methodName);

   public Type BuilderType { get; }
	public string MethodName { get; }
}