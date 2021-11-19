namespace Rocks;

public static class Arg
{
	public static Argument<T> Any<T>() => new();

	public static Argument<T> Is<T>(T value) => new(value);

	public static Argument<T> IsDefault<T>() => new(ValidationState.DefaultValue);

	public static Argument<T> Validate<T>(Predicate<T> evaluation) => new(evaluation);
}