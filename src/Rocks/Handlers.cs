using System.Collections;

namespace Rocks;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public sealed class Handlers<TCallback, TReturnValue>
	: IEnumerable<Handler<TCallback, TReturnValue>>
	where TCallback : Delegate
{
	public struct HandlerEnumerator
		: IEnumerator<Handler<TCallback, TReturnValue>>
	{
		private Handler<TCallback, TReturnValue>? value;
		private bool first = true;

		internal HandlerEnumerator(Handler<TCallback, TReturnValue> value) =>
			 this.value = value;

		public readonly Handler<TCallback, TReturnValue> Current => this.value!;

		readonly object IEnumerator.Current => this.value!;

		public void Dispose() => this.value = null;

		public bool MoveNext()
		{
			if (this.first)
			{
				this.first = false;
				return true;
			}

			this.value = this.value!.Next;
			return this.value is not null;
		}

		public void Reset() => this.value = null;
	}

	public Handlers(Handler<TCallback, TReturnValue> handler)
	{
		this.First = handler;
		this.Last = handler;
	}

	public void Add(Handler<TCallback, TReturnValue> handler)
	{
		this.Last.Next = handler;
		this.Last = handler;
	}

	public HandlerEnumerator GetEnumerator() =>
		 new HandlerEnumerator(this.First);

	IEnumerator<Handler<TCallback, TReturnValue>> IEnumerable<Handler<TCallback, TReturnValue>>.GetEnumerator() =>
		 this.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Handler<TCallback, TReturnValue>>)this).GetEnumerator();

	private Handler<TCallback, TReturnValue> First { get; }
	private Handler<TCallback, TReturnValue> Last { get; set; }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
