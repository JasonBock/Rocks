namespace Rocks.Runtime;

/// <summary>
/// Defines the different kinds of validation
/// that can be done.
/// </summary>
public enum ValidationState
{
	/// <summary>
	/// Specifies that no validation occurs.
	/// </summary>
	None, 
	/// <summary>
	/// Specifies that a method will be used for validation.
	/// </summary>
	Evaluation, 
	/// <summary>
	/// Specifies that a value is used for exact validation.
	/// </summary>
	Value, 
	/// <summary>
	/// Specifies that the default value of a type is used for exact validation.
	/// </summary>
	DefaultValue
}