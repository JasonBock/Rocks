using System.ComponentModel;

namespace Rocks;

internal unsafe delegate bool PointerArgumentEvaluation<T>(T* @value) where T : unmanaged;

/// <summary>
/// Defines the type for a pointer argument expectation.
/// </summary>
/// <typeparam name="T">The type of the argument.</typeparam>
public sealed unsafe class PointerArgument<T>
	: Argument
	where T : unmanaged
{
	private readonly PointerArgumentEvaluation<T>? evaluation;
	private readonly T* value;
	private readonly ValidationState validation;

	internal PointerArgument() => this.validation = ValidationState.None;

	internal PointerArgument(T* @value)
	{
		this.value = @value;
		this.validation = ValidationState.Value;
	}

	internal PointerArgument(PointerArgumentEvaluation<T> @evaluation)
	{
		this.evaluation = @evaluation;
		this.validation = ValidationState.Evaluation;
	}

	/// <summary>
	/// Converts a pointer value to a <see cref="PointerArgument{T}"/> instance.
	/// </summary>
	/// <param name="value">The pointer value.</param>
	/// <returns>A new <see cref="PointerArgument{T}"/> instance.</returns>
	public static implicit operator PointerArgument<T>(T* @value) => new(@value);

	/// <summary>
	/// Determines if the given pointer value matches the expectation.
	/// </summary>
	/// <param name="value">The pointer value to test.</param>
	/// <returns><c>true</c> if validation is successful; otherwise, <c>false</c>.</returns>
	/// <exception cref="NotSupportedException"></exception>
	/// <exception cref="InvalidEnumArgumentException"></exception>
	public bool IsValid(T* @value) =>
		this.validation switch
		{
			ValidationState.None => true,
			ValidationState.Value => @value == this.value,
			ValidationState.Evaluation => this.evaluation!(@value),
			ValidationState.DefaultValue => throw new NotSupportedException("Cannot validate an argument value in the ValidationState.DefaultValue state."),
			_ => throw new InvalidEnumArgumentException($"Invalid value for validation: {this.validation}")
		};
}
