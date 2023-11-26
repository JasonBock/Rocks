using Rocks.Expectations;

namespace Rocks;

/// <summary>
/// Allow a number of mocks to be creates
/// within an <see cref="IDisposable"/> context.
/// When <see cref="Dispose"/> is called,
/// <see cref="IExpectations.Verify"/> is invoked on every created mock.
/// </summary>
public sealed class RockRepository
	: IDisposable
{
	private bool isDisposed = false;
	private readonly List<IExpectations> rocks = [];

	/// <summary>
	/// Creates a new <see cref="Expectations{T}"/> instance,
	/// and stores the mock reference for future verification.
	/// </summary>
	/// <typeparam name="T">The type to mock.</typeparam>
	/// <returns>A new <see cref="Expectations{T}"/>.</returns>
	public Expectations<T> Create<T>()
		where T : class
	{
		if(this.isDisposed)
		{
			throw new ObjectDisposedException(this.GetType().Name);
		}

		var expectations = new Expectations<T>();
		this.rocks.Add(expectations);
		return expectations;
	}

	/// <summary>
	/// Disposes the object, which will call
	/// <see cref="IExpectations.Verify"/> on every created mock.
	/// </summary>
	public void Dispose()
	{
		if (this.isDisposed)
		{
			throw new ObjectDisposedException(this.GetType().Name);
		}

		foreach (var chunk in this.rocks)
		{
			chunk.Verify();
		}

		this.isDisposed = true;
	}
}