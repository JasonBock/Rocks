namespace Rocks.Expectations;

/// <summary>
/// Defines expectations for the getters on explicit indexers.
/// </summary>
/// <typeparam name="T">The mock type.</typeparam>
/// <typeparam name="TContainingType">The type that defines the indexers.</typeparam>
public sealed class ExplicitIndexerGetterExpectations<T, TContainingType>
	: ExplicitIndexerExpectations<T, TContainingType>
	where T : class, TContainingType
{
	/// <summary>
	/// Creates a new <see cref="ExplicitIndexerGetterExpectations{T, TContainingType}"/> instance.
	/// </summary>
	/// <param name="expectations">The expectations.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="expectations"/> is <c>null</c>.</exception>
	public ExplicitIndexerGetterExpectations(ExplicitIndexerExpectations<T, TContainingType> expectations)
		: base(expectations ?? throw new ArgumentNullException(nameof(expectations))) { }
}