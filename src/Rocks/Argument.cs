using System.ComponentModel;

namespace Rocks;

[Serializable]
public abstract class Argument
{
	protected Argument() { }
}

[Serializable]
public sealed class Argument<T>
	: Argument
{
	private readonly Predicate<T>? evaluation;
	private readonly T? value;
	private readonly ValidationState validation;

	internal Argument() => this.validation = ValidationState.None;

	internal Argument(ValidationState state) => this.validation = state;

	internal Argument(T value) =>
		(this.value, this.validation) = (value, ValidationState.Value);

	internal Argument(Predicate<T> evaluation) =>
		(this.evaluation, this.validation) = (evaluation, ValidationState.Evaluation);

	public static implicit operator Argument<T>(T value) => new(value);

	public bool IsValid(T value) =>
		this.validation switch
		{
			ValidationState.None => true,
			ValidationState.Value => ObjectEquality.AreEqual(value, this.value),
			ValidationState.Evaluation => this.evaluation!(value),
			ValidationState.DefaultValue => throw new NotSupportedException("Cannot validate an argument value in the DefaultValue state."),
			_ => throw new InvalidEnumArgumentException($"Invalid value for validation: {this.validation}")
		};

	public Argument<T> Transform(T value) =>
		this.validation == ValidationState.DefaultValue ? new Argument<T>(value) : this;
}