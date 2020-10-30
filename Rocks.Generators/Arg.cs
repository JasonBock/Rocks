using System;

namespace Rocks
{
	[Serializable]
	public abstract class Arg
	{
		public static Arg<T> Any<T>() => new Arg<T>();

		public static Arg<T> Is<T>(T value) => new Arg<T>(value);

		public static Arg<T> Validate<T>(Predicate<T> evaluation) => new Arg<T>(evaluation);

		protected Arg() { }
	}

	[Serializable]
	public sealed class Arg<T>
		: Arg
	{
		private readonly Predicate<T>? evaluation;
		private readonly T? value;

		internal Arg() { }

		internal Arg(T value) => this.value = value;

		internal Arg(Predicate<T> evaluation) => this.evaluation = evaluation;

		public bool IsValid(T value) =>
			this.value is not null ? ObjectEquality.AreEqual(value, this.value) : this.evaluation!(value);
	}
}