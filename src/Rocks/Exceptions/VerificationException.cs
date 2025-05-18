namespace Rocks.Exceptions;

/// <summary>
/// Thrown when expectations on a mock have not been met.
/// This is thrown in <see cref="Expectations"/>
/// when one of the <c>Verify()</c> methods is called.
/// </summary>
[Serializable]
public sealed class VerificationException
	: Exception
{
	/// <summary>
	/// Creates a new <see cref="VerificationException"/> instance
	/// with the given list of failures.
	/// </summary>
	/// <param name="failures">A list of verification errors.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="failures"/> is <c>null</c>.</exception>
	public VerificationException(IReadOnlyList<string> failures) =>
		this.Failures = failures ?? throw new ArgumentNullException(nameof(failures));

	/// <summary>
	/// Gets the exception message, which is a concatenation of the <see cref="Failures"/> values.
	/// </summary>
	public override string Message =>
		this.Failures.Count > 0 ?
			$"The following verification failure(s) occurred: {string.Join(", ", this.Failures)}" :
			$"No failures were reported.";

	/// <summary>
	/// Gets the list of verification failures.
	/// </summary>
   public IReadOnlyList<string> Failures { get; }
}