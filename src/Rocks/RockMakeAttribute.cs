namespace Rocks;

/// <summary>
/// Used to specify a type that should have a mock of type "make" built.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true)]
public sealed class RockMakeAttribute<T>()
	: Attribute
	where T : class
{ }