using Rocks.Expectations;

namespace Rocks;

/// <summary>
/// Creates types used for either expectation definitions
/// or simple "holder" objects.
/// </summary>
public static class Rock
{
	/// <summary>
	/// Creates an <see cref="Expectations{T}"/> for the given type.
	/// </summary>
	/// <typeparam name="T">The type to set expectations for.</typeparam>
	/// <returns>A new <see cref="Expectations{T}"/> instance.</returns>
	public static Expectations<T> Create<T>()
		where T : class => new();

	/// <summary>
	/// Creates a <see cref="MakeGeneration{T}"/> for the given type.
	/// </summary>
	/// <typeparam name="T">The type to generate a non-verifiable mock for.</typeparam>
	/// <returns>A new <see cref="MakeGeneration{T}"/> instance.</returns>
	public static MakeGeneration<T> Make<T>()
		where T : class => new();
}