using Rocks.Options;
using System;

namespace Rocks
{
	internal sealed class CacheKey
		: IEquatable<CacheKey?>
	{
		private readonly Type type;
		private readonly RockOptions options;

		internal CacheKey(Type type, RockOptions options) =>
			(this.type, this.options) = (type, options);

		/// <summary>
		/// Determines whether two specified <see cref="CacheKey" /> objects have the same value. 
		/// </summary>
		/// <param name="a">A <see cref="CacheKey" /> or a null reference.</param>
		/// <param name="b">A <see cref="CacheKey" /> or a null reference.</param>
		/// <returns><b>true</b> if the value of <paramref name="a"/> is the same as the value of <paramref name="b"/>; otherwise, <b>false</b>. </returns>
		public static bool operator ==(CacheKey a, CacheKey b)
		{
			var areEqual = false;

			if (object.ReferenceEquals(a, b))
			{
				areEqual = true;
			}

			if (!(a is null) && !(b is null))
			{
				areEqual = a.Equals(b);
			}

			return areEqual;
		}

		/// <summary>
		/// Determines whether two specified <see cref="CacheKey" /> objects have different value. 
		/// </summary>
		/// <param name="a">A <see cref="CacheKey" /> or a null reference.</param>
		/// <param name="b">A <see cref="CacheKey" /> or a null reference.</param>
		/// <returns><b>true</b> if the value of <paramref name="a"/> is different from the value of <paramref name="b"/>; otherwise, <b>false</b>. </returns>
		public static bool operator !=(CacheKey a, CacheKey b) => !(a == b);

		/// <summary>
		/// Determines whether this instance of <see cref="CacheKey" /> and a 
		/// specified <see cref="CacheKey" /> object have the same value. 
		/// </summary>
		/// <param name="other">A <see cref="CacheKey" />.</param>
		/// <returns><b>true</b> if <paramref name="other"/> is a <see cref="CacheKey" /> and its value 
		/// is the same as this instance; otherwise, <b>false</b>.</returns>
		public bool Equals(CacheKey? other)
		{
			var areEqual = false;

			if (!(other is null))
			{
				areEqual = this.GetHashCode() == other.GetHashCode();
			}

			return areEqual;
		}

		/// <summary>
		/// Determines whether this instance of <see cref="CacheKey" /> and a specified object, 
		/// which must also be a <see cref="CacheKey" /> object, have the same value. 
		/// </summary>
		/// <param name="obj">An <see cref="Object" />.</param>
		/// <returns><b>true</b> if <paramref name="obj"/> is a <see cref="CacheKey" /> and its value 
		/// is the same as this instance; otherwise, <b>false</b>.</returns>
		public override bool Equals(object obj) => this.Equals(obj as CacheKey);

		public override int GetHashCode() => HashCode.Combine(this.type.GetHashCode(), this.options.GetHashCode());
	}
}
