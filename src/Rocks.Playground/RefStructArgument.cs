namespace Rocks;

[global::System.Serializable]
public sealed class RefStructArgument<T>
	 : global::Rocks.Argument
	 where T : allows ref struct
{
	private readonly global::System.Predicate<T>? evaluation;
	private readonly global::Rocks.ValidationState validation;

	internal RefStructArgument() => 
		this.validation = global::Rocks.ValidationState.None;

   internal RefStructArgument(global::System.Predicate<T> @evaluation) => 
		(this.evaluation, this.validation) =
			(@evaluation, global::Rocks.ValidationState.Evaluation);

   public bool IsValid(T @value) =>
		 this.validation switch
		 {
			 global::Rocks.ValidationState.None => true,
			 global::Rocks.ValidationState.Evaluation => this.evaluation!(@value),
			 _ => throw new global::System.NotSupportedException("Invalid validation state."),
		 };
}