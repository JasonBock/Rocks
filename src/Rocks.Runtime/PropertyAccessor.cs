namespace Rocks.Runtime;

/// <summary>
/// Defines the different combinations of 
/// property accessors.
/// </summary>
public enum PropertyAccessor
{
	/// <summary>
	/// Used for a "get"
	/// </summary>
	Get,
	/// <summary>
	/// Used for a "set"
	/// </summary>
	Set,
	/// <summary>
	/// Used for a "get/set" 
	/// </summary>
	GetAndSet,
	/// <summary>
	/// Used for a "init" 
	/// </summary>
	Init,
	/// <summary>
	/// Used for a "get/init" 
	/// </summary>
	GetAndInit
}