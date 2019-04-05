using System;

namespace Rocks
{
	[Serializable]
   public sealed class ArgumentIsEvaluationExpectation<T>
		: ArgumentExpectation
   {
		internal ArgumentIsEvaluationExpectation(Func<T, bool> evaluation) =>
			this.Evaluation = evaluation ?? throw new ArgumentNullException(nameof(evaluation));

		public bool IsValid(T value) => this.Evaluation(value);

		internal Func<T, bool> Evaluation { get; }
   }
}
