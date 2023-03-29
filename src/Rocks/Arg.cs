namespace Rocks;

/// <summary>
/// Defines different states for an argument value
/// in an expectation that will be verified
/// in a subsequent mock object.
/// </summary>
public static class Arg
{
	/// <summary>
	/// Specifies that an argument can be any value.
	/// </summary>
	/// <typeparam name="T">The type of the argument.</typeparam>
	/// <returns>A new <see cref="Argument{T}"/> that allows any value.</returns>
	public static Argument<T> Any<T>() => new();

	/// <summary>
	/// Specifies that an argument must be a specific value.
	/// </summary>
	/// <typeparam name="T">The type of the argument.</typeparam>
	/// <param name="value">The expected argument value.</param>
	/// <returns>A new <see cref="Argument{T}"/> that ensures the given value in a mock is equal to <paramref name="value"/>.</returns>
	public static Argument<T> Is<T>(T value) => new(value);

	/// <summary>
	/// Specifies that an argument is the default value.
	/// </summary>
	/// <typeparam name="T">The type of the argument.</typeparam>
	/// <returns>A new <see cref="Argument{T}"/> that ensures the given value in a mock is the default value of <typeparamref name="T"/>.</returns>
	public static Argument<T> IsDefault<T>() => new(ValidationState.DefaultValue);

	/// <summary>
	/// Specifies that an argument must pass the given validation delegate.
	/// </summary>
	/// <typeparam name="T">The type of the argument.</typeparam>
	/// <param name="evaluation">The <see cref="Predicate{T}"/> delegate that will validate the given value in the mock.</param>
	/// <returns>A new <see cref="Argument{T}"/> that validates the given value in a mock.</returns>
	public static Argument<T> Validate<T>(Predicate<T> evaluation) => new(evaluation);
}