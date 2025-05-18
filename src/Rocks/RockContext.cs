namespace Rocks;

/// <summary>
/// Provides a way to create expectations
/// and have <see cref="Expectations.Verify()"/> automatically called
/// when <see cref="IDisposable.Dispose()"/> is invoked.
/// </summary>
public sealed class RockContext
  : IDisposable
{
	private DisableVerification disableVerification = DisableVerification.No;
	private bool isDisposed;
	private readonly List<Expectations> createdExpectations = [];

	/// <summary>
	/// Creates a new <see cref="RockContext"/> instance.
	/// </summary>
	/// <param name="disableVerification">Allows for verification to be disabled. The default is <c>false</c>.</param>
	public RockContext(DisableVerification disableVerification = DisableVerification.No) =>
		this.disableVerification = disableVerification;

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

		if (this.disableVerification == DisableVerification.No)
		{
			foreach (var createdExpectations in this.createdExpectations)
			{
				createdExpectations.Verify();
			}
		}

		this.isDisposed = true;
	}
}