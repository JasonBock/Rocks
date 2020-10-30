using System;
using System.Collections.Immutable;

namespace Rocks
{
#pragma warning disable CS8653, CS8601
	[Serializable]
	public sealed class HandlerInformation<T>
		: HandlerInformation
	{
		internal HandlerInformation(ImmutableDictionary<string, Arg> expectations)
			: base(null, expectations) => this.ReturnValue = default;

		internal HandlerInformation(Delegate? method, ImmutableDictionary<string, Arg> expectations)
			: base(method, expectations) => this.ReturnValue = default;

		public T? ReturnValue { get; internal set; }
   }
#pragma warning restore CS8653, CS8061
}