using System.Collections;

namespace Rocks;

/// <summary>
/// Defines a type to collect all the handlers for a member.
/// </summary>
/// <remarks>
/// This type is designed to be used by Rocks exclusively and is not intended
/// to be used directly.
/// </remarks>
public sealed class Handlers<THandler>
	: IEnumerable<THandler>
	where THandler : Handler
{
	internal sealed class HandlerNode
	{
		public HandlerNode(THandler handler) => this.Value = handler;
		public THandler Value { get; }
		public HandlerNode? Next { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public struct HandlerEnumerator
		: IEnumerator<THandler>
	{
		private HandlerNode? value;
		private bool first = true;

		internal HandlerEnumerator(HandlerNode value) =>
			 this.value = value;

		/// <summary>
		/// 
		/// </summary>
		public readonly THandler Current => this.value!.Value;

		readonly object IEnumerator.Current => this.value!;

		/// <summary>
		/// 
		/// </summary>
		public void Dispose() => this.value = default;

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// 
		/// </summary>
		public void Reset() => this.value = null;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="handler"></param>
	public Handlers(THandler handler) =>
		this.FirstNode = this.LastNode = new(handler);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="handler"></param>
	public void Add(THandler handler)
	{
		var node = new HandlerNode(handler);
		this.LastNode.Next = node;
		this.LastNode = node;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public HandlerEnumerator GetEnumerator() => new(this.FirstNode);

	IEnumerator<THandler> IEnumerable<THandler>.GetEnumerator() => this.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<THandler>)this).GetEnumerator();

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public THandler First => this.FirstNode.Value;
	private HandlerNode FirstNode { get; }
	private HandlerNode LastNode { get; set; }
}