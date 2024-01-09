namespace Rocks;

/// <summary>
/// Used to specify a type that should have a mock of type "create" built.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class RockCreateAttribute<T>()
	: Attribute
	where T : class
{ }