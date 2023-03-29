using System.Collections.Immutable;

namespace Rocks;

/// <summary>
/// Specifies expectations on a member
/// that returns a value.
/// </summary>
/// <typeparam name="T">The type of the return value.</typeparam>
[Serializable]
public sealed class HandlerInformation<T>
	 : HandlerInformation
{
	/// <summary>
	/// Creates a new <see cref="HandlerInformation{T}"/> instance
	/// with a set of argument expectations.
	/// </summary>
	/// <param name="expectations">A set of argument expectations.</param>
	internal HandlerInformation(ImmutableArray<Argument> expectations)
		: base(null, expectations) => this.ReturnValue = default;

	/// <summary>
	/// Creates a new <see cref="HandlerInformation{T}"/> instance
	/// with a set of argument expectations
	/// and a validation delegate.
	/// </summary>
	/// <param name="expectations">A set of argument expectations.</param>
	/// <param name="method">The validation delegate.</param>
	internal HandlerInformation(Delegate? method, ImmutableArray<Argument> expectations)
		: base(method, expectations) => this.ReturnValue = default;

	/// <summary>
	/// Gets or sets the return value.
	/// </summary>
	public T? ReturnValue { get; internal set; }
}