// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Rocks.Models;

/// <summary>
/// Extensions for <see cref="EquatableArray{T}"/>.
/// </summary>
internal static class EquatableArray
{
	/// <summary>
	/// Creates an <see cref="EquatableArray{T}"/> instance from a given <see cref="ImmutableArray{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of items in the input array.</typeparam>
	/// <param name="array">The input <see cref="ImmutableArray{T}"/> instance.</param>
	/// <returns>An <see cref="EquatableArray{T}"/> instance from a given <see cref="ImmutableArray{T}"/>.</returns>
	public static EquatableArray<T> AsEquatableArray<T>(this ImmutableArray<T> array)
		where T : IEquatable<T> => new(array);

	public static EquatableArray<T> Create<T>(ReadOnlySpan<T> values)
		where T : IEquatable<T> =>
		new EquatableArray<T>(values.ToImmutableArray());
}

/// <summary>
/// An imutable, equatable array. This is equivalent to <see cref="ImmutableArray{T}"/> but with value equality support.
/// </summary>
/// <typeparam name="T">The type of values in the array.</typeparam>
[CollectionBuilder(typeof(EquatableArray), nameof(EquatableArray.Create))]
internal readonly struct EquatableArray<T> : IEquatable<EquatableArray<T>>, IEnumerable<T>
	 where T : IEquatable<T>
{
	/// <summary>
	/// The underlying <typeparamref name="T"/> array.
	/// </summary>
	private readonly T[]? array;

	/// <summary>
	/// Creates a new <see cref="EquatableArray{T}"/> instance.
	/// </summary>
	/// <param name="array">The input <see cref="ImmutableArray{T}"/> to wrap.</param>
	public EquatableArray(ImmutableArray<T> array) =>
		this.array = Unsafe.As<ImmutableArray<T>, T[]?>(ref array);

	/// <summary>
	/// Gets a reference to an item at a specified position within the array.
	/// </summary>
	/// <param name="index">The index of the item to retrieve a reference to.</param>
	/// <returns>A reference to an item at a specified position within the array.</returns>
	public T this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => this.AsImmutableArray()[index];
	}

	/// <summary>
	/// Gets a value indicating whether the current array is empty.
	/// </summary>
	public bool IsEmpty
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => this.AsImmutableArray().IsEmpty;
	}

	/// <summary>
	/// Gets a value indicating whether the current array is default or empty.
	/// </summary>
	public bool IsDefaultOrEmpty
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => this.AsImmutableArray().IsDefaultOrEmpty;
	}

	/// <summary>
	/// Gets the length of the current array.
	/// </summary>
	public int Length
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => this.AsImmutableArray().Length;
	}

	/// <sinheritdoc/>
	public bool Equals(EquatableArray<T> array) => this.AsSpan().SequenceEqual(array.AsSpan());

	/// <sinheritdoc/>
	public override bool Equals(object? obj) => obj is EquatableArray<T> array && Equals(this, array);

	/// <sinheritdoc/>
	public override unsafe int GetHashCode()
	{
		if (this.array is not T[] array)
		{
			return 0;
		}

		var hashCode = 0;

		foreach (var value in array)
		{
			hashCode = unchecked(hashCode * (int)0xA5555529 + value.GetHashCode());
		}

		return hashCode;
	}

	/// <summary>
	/// Gets an <see cref="ImmutableArray{T}"/> instance from the current <see cref="EquatableArray{T}"/>.
	/// </summary>
	/// <returns>The <see cref="ImmutableArray{T}"/> from the current <see cref="EquatableArray{T}"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ImmutableArray<T> AsImmutableArray() => Unsafe.As<T[]?, ImmutableArray<T>>(ref Unsafe.AsRef(in this.array));

	/// <summary>
	/// Creates an <see cref="EquatableArray{T}"/> instance from a given <see cref="ImmutableArray{T}"/>.
	/// </summary>
	/// <param name="array">The input <see cref="ImmutableArray{T}"/> instance.</param>
	/// <returns>An <see cref="EquatableArray{T}"/> instance from a given <see cref="ImmutableArray{T}"/>.</returns>
	public static EquatableArray<T> FromImmutableArray(ImmutableArray<T> array) => new(array);

	/// <summary>
	/// Returns a <see cref="ReadOnlySpan{T}"/> wrapping the current items.
	/// </summary>
	/// <returns>A <see cref="ReadOnlySpan{T}"/> wrapping the current items.</returns>
	public ReadOnlySpan<T> AsSpan() => this.AsImmutableArray().AsSpan();

	/// <summary>
	/// Copies the contents of this <see cref="EquatableArray{T}"/> instance. to a mutable array.
	/// </summary>
	/// <returns>The newly instantiated array.</returns>
	public T[] ToArray() => [.. this.AsImmutableArray()];

	/// <summary>
	/// Gets an <see cref="ImmutableArray{T}.Enumerator"/> value to traverse items in the current array.
	/// </summary>
	/// <returns>An <see cref="ImmutableArray{T}.Enumerator"/> value to traverse items in the current array.</returns>
	public ImmutableArray<T>.Enumerator GetEnumerator() => this.AsImmutableArray().GetEnumerator();

	/// <sinheritdoc/>
	IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)this.AsImmutableArray()).GetEnumerator();

	/// <sinheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.AsImmutableArray()).GetEnumerator();

	/// <summary>
	/// Implicitly converts an <see cref="ImmutableArray{T}"/> to <see cref="EquatableArray{T}"/>.
	/// </summary>
	/// <returns>An <see cref="EquatableArray{T}"/> instance from a given <see cref="ImmutableArray{T}"/>.</returns>
	public static implicit operator EquatableArray<T>(ImmutableArray<T> array) =>
		EquatableArray<T>.FromImmutableArray(array);

	/// <summary>
	/// Implicitly converts an <see cref="EquatableArray{T}"/> to <see cref="ImmutableArray{T}"/>.
	/// </summary>
	/// <returns>An <see cref="ImmutableArray{T}"/> instance from a given <see cref="EquatableArray{T}"/>.</returns>
	public static implicit operator ImmutableArray<T>(EquatableArray<T> array) => array.AsImmutableArray();

	/// <summary>
	/// Checks whether two <see cref="EquatableArray{T}"/> values are the same.
	/// </summary>
	/// <param name="left">The first <see cref="EquatableArray{T}"/> value.</param>
	/// <param name="right">The second <see cref="EquatableArray{T}"/> value.</param>
	/// <returns>Whether <paramref name="left"/> and <paramref name="right"/> are equal.</returns>
	public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right) => left.Equals(right);

	/// <summary>
	/// Checks whether two <see cref="EquatableArray{T}"/> values are not the same.
	/// </summary>
	/// <param name="left">The first <see cref="EquatableArray{T}"/> value.</param>
	/// <param name="right">The second <see cref="EquatableArray{T}"/> value.</param>
	/// <returns>Whether <paramref name="left"/> and <paramref name="right"/> are not equal.</returns>
	public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right) => !left.Equals(right);
}