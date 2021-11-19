using System.Collections.Immutable;

namespace Rocks;

[Serializable]
public sealed class HandlerInformation<T>
	 : HandlerInformation
{
	internal HandlerInformation(ImmutableArray<Argument> expectations)
		: base(null, expectations) => this.ReturnValue = default;

	internal HandlerInformation(Delegate? method, ImmutableArray<Argument> expectations)
		: base(method, expectations) => this.ReturnValue = default;

	public T? ReturnValue { get; internal set; }
}