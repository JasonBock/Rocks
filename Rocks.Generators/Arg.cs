using System;
using System.ComponentModel;

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
		private enum Validation
		{
			None, Evaluation, Value
		}

		private readonly Predicate<T>? evaluation;
		private readonly Validation validation;
		private readonly T? value;

		internal Arg() => this.validation = Validation.None;

		internal Arg(T value) => 
			(this.value, this.validation) = (value, Validation.Value);

		internal Arg(Predicate<T> evaluation) => 
			(this.evaluation, this.validation) = (evaluation, Validation.Evaluation);

		public bool IsValid(T value) =>
			this.validation switch
			{
				Validation.None => true,
				Validation.Value => ObjectEquality.AreEqual(value, this.value),
				Validation.Evaluation => this.evaluation!(value),
				_ => throw new InvalidEnumArgumentException($"Invalid value for validation: {this.evaluation}")
			};
	}
}