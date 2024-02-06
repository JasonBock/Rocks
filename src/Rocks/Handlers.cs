﻿using System.Collections;

namespace Rocks;

/// <summary>
/// Defines a type to collect all the handlers for a member.
/// </summary>
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
	/// A custom enumerator for <see cref="Handlers{THandler}"/>
	/// </summary>
	public struct HandlerEnumerator
		: IEnumerator<THandler>
	{
		private HandlerNode? value;
		private bool first = true;

		internal HandlerEnumerator(HandlerNode value) =>
			 this.value = value;

		/// <summary>
		/// Gets the current <typeparamref name="THandler"/>.
		/// </summary>
		public readonly THandler Current => this.value!.Value;

		readonly object IEnumerator.Current => this.value!;

		/// <summary>
		/// Disposes the enumerator.
		/// </summary>
		public void Dispose() => this.value = default;

		/// <summary>
		/// Moves to the next <typeparamref name="THandler"/> value
		/// in the enumeration.
		/// </summary>
		/// <returns>
		/// Returns <see langword="true"/> if another handler exists in the current enumeration,
		/// otherwise, <see langword="false"/>.
		/// </returns>
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
		/// Resets the enumeration.
		/// </summary>
		public void Reset() => this.value = null;
	}

	/// <summary>
	/// Creates a new <see cref="Handlers{THandler}"/> instance.
	/// </summary>
	/// <param name="handler">The first handler to reference.</param>
	public Handlers(THandler handler) =>
		this.FirstNode = this.LastNode = new(handler);

	/// <summary>
	/// Adds a new <typeparamref name="THandler"/> instance.
	/// </summary>
	/// <param name="handler">A handler instance.</param>
	public void Add(THandler handler)
	{
		var node = new HandlerNode(handler);
		this.LastNode.Next = node;
		this.LastNode = node;
	}

	/// <summary>
	/// Gets an enumerator.
	/// </summary>
	/// <returns>A new <see cref="HandlerEnumerator"/> instance.</returns>
	public HandlerEnumerator GetEnumerator() => new(this.FirstNode);

	IEnumerator<THandler> IEnumerable<THandler>.GetEnumerator() => this.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<THandler>)this).GetEnumerator();

	/// <summary>
	/// Gets the first <typeparamref name="THandler"/> instance.
	/// </summary>
	/// <returns>The first <typeparamref name="THandler"/> instance.</returns>
	public THandler First => this.FirstNode.Value;
	private HandlerNode FirstNode { get; }
	private HandlerNode LastNode { get; set; }
}