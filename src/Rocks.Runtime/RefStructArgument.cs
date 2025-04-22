namespace Rocks.Runtime;

[Serializable]
public sealed class RefStructArgument<T>
	: Argument
	where T : allows ref struct
{
	private readonly Predicate<T>? evaluation;
	private readonly ValidationState validation;

	public RefStructArgument() =>
		this.validation = ValidationState.None;

	public RefStructArgument(Predicate<T> @evaluation) =>
		(this.evaluation, this.validation) =
			(@evaluation, ValidationState.Evaluation);

	public bool IsValid(T @value) =>
		this.validation switch
		{
			ValidationState.None => true,
			ValidationState.Evaluation => this.evaluation!(@value),
			_ => throw new NotSupportedException("Invalid validation state."),
		};
}