namespace Rocks.Runtime;

/// <summary>
/// A custom <see cref="Argument"/> that supports ref struct types.
/// </summary>
/// <typeparam name="T">The type of the ref struct.</typeparam>
[Serializable]
public sealed class RefStructArgument<T>
	: Argument
	where T : allows ref struct
{
	private readonly Predicate<T>? evaluation;
	private readonly ValidationState validation;

	/// <summary>
	/// Creates a new <see cref="RefStructArgument{T}"/> instance.
	/// </summary>
	public RefStructArgument() =>
		this.validation = ValidationState.None;

	/// <summary>
	/// Creates a new <see cref="RefStructArgument{T}"/> with an evaluation delegate.
	/// </summary>
	/// <param name="evaluation">A delegate to use for value evaluation.</param>
	public RefStructArgument(Predicate<T> @evaluation) =>
		(this.evaluation, this.validation) =
			(@evaluation, ValidationState.Evaluation);

	/// <summary>
	/// Validates a given <typeparamref name="T"/> value.
	/// </summary>
	/// <param name="value">The value to validate.</param>
	/// <returns><c>true</c> if <paramref name="value"/> is valid, otherwise, <c>false</c>.</returns>
	/// <exception cref="NotSupportedException">The current <see cref="ValidationState"/> value is not valid.</exception>
	public bool IsValid(T @value) =>
		this.validation switch
		{
			ValidationState.None => true,
			ValidationState.Evaluation => this.evaluation!(@value),
			_ => throw new NotSupportedException("Invalid validation state."),
		};
}