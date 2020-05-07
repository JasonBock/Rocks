using System;
using System.Diagnostics.CodeAnalysis;

namespace Rocks
{
	[Serializable]
   public sealed class ArgumentIsEvaluationExpectation<T>
	   : ArgumentExpectation<T>
   {
		internal ArgumentIsEvaluationExpectation(Func<T, bool> evaluation) =>
			this.Evaluation = evaluation ?? throw new ArgumentNullException(nameof(evaluation));

#pragma warning disable CS8604 // Possible null reference argument.
		public override bool IsValid([AllowNull] T value) => this.Evaluation(value);
#pragma warning restore CS8604 // Possible null reference argument.

		internal Func<T, bool> Evaluation { get; }
   }
}