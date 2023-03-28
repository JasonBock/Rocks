namespace Rocks.Exceptions;

[Serializable]
public sealed class VerificationException
	: Exception
{
	public VerificationException(IReadOnlyList<string> failures) =>
		this.Failures = failures ?? throw new ArgumentNullException(nameof(failures));
	public override string Message =>
		this.Failures.Count > 0 ?
			$"The following verification failure(s) occured: {string.Join(", ", this.Failures)}" :
			$"No failures were reported.";

   public IReadOnlyList<string> Failures { get; }
}