using System;
using System.ComponentModel;

namespace Rocks
{
	internal enum ValidationState
	{
		None, Evaluation, Value, DefaultValue
	}

	[Serializable]
	public abstract class Arg
	{
		public static Arg<T> Any<T>() => new();

		public static Arg<T> Is<T>(T value) => new(value);

		public static Arg<T> IsDefault<T>() => new(ValidationState.DefaultValue);

		public static Arg<T> Validate<T>(Predicate<T> evaluation) => new(evaluation);

		protected Arg() { }
	}

	[Serializable]
	public sealed class Arg<T>
		: Arg
	{
		private readonly Predicate<T>? evaluation;
		private readonly T? value;
		private readonly ValidationState validation;

		internal Arg() => this.validation = ValidationState.None;

		internal Arg(ValidationState state) => this.validation = state;

		internal Arg(T value) =>
			(this.value, this.validation) = (value, ValidationState.Value);

		internal Arg(Predicate<T> evaluation) =>
			(this.evaluation, this.validation) = (evaluation, ValidationState.Evaluation);

#pragma warning disable CA2225 // Operator overloads have named alternates
		public static implicit operator Arg<T>(T value) => new(value);
#pragma warning restore CA2225 // Operator overloads have named alternates

		public bool IsValid(T value) =>
			this.validation switch
			{
				ValidationState.None => true,
				ValidationState.Value => ObjectEquality.AreEqual(value, this.value),
				ValidationState.Evaluation => this.evaluation!(value),
				ValidationState.DefaultValue => throw new NotSupportedException("Cannot validate an argument value in the DefaultValue state."),
				_ => throw new InvalidEnumArgumentException($"Invalid value for validation: {this.validation}")
			};

		public Arg<T> Transform(T value) =>
			this.validation == ValidationState.DefaultValue ? new Arg<T>(value) : this;
	}
}