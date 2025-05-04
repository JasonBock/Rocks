namespace Rocks;

/// <summary>
/// Provides a way to create expectations
/// and have <see cref="Expectations.Verify()"/> automatically called
/// when <see cref="IDisposable.Dispose()"/> is invoked.
/// </summary>
public sealed class RockContext
  : IDisposable
{
	private bool isDisposed;
	private readonly List<Expectations> createdExpectations = [];

	/// <summary>
	/// Creates an instance of the provided expectations type.
	/// </summary>
	/// <typeparam name="T">The <see cref="Expectations"/> type to create.</typeparam>
	/// <returns>
	/// A new <typeparamref name="T"/> instance that will have
	/// <see cref="Expectations.Verify()"/> called when the context is disposed.
	/// </returns>
	public T Create<T>()
		where T : Expectations, new()
	{
		ObjectDisposedException.ThrowIf(this.isDisposed, this);

		var expectations = new T();
		this.createdExpectations.Add(expectations);
		return expectations;
	}

	/// <summary>
	/// Verifies all tracked expectations.
	/// </summary>
	public void Dispose()
	{
		ObjectDisposedException.ThrowIf(this.isDisposed, this);

		foreach (var createdExpectations in this.createdExpectations)
		{
			createdExpectations.Verify();
		}

		this.isDisposed = true;
	}
}