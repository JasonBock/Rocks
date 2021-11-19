using Rocks.Exceptions;
using System.Collections.ObjectModel;

namespace Rocks.Extensions;

public static class IMockExtensions
{
	internal static ReadOnlyCollection<string> GetVerificationFailures(this IMock self)
	{
		var failures = new List<string>();

		foreach (var pair in self.Handlers)
		{
			foreach (var handler in pair.Value)
			{
				foreach (var failure in handler.Verify())
				{
					var method = self.GetType().GetMemberDescription(pair.Key);

					failures.Add($"Type: {self.GetType().FullName}, method: {method}, message: {failure}");
				}
			}
		}

		return failures.AsReadOnly();
	}

	public static void Verify(this IMock self)
	{
		if (self is null) { throw new ArgumentNullException(nameof(self)); }

		var failures = self.GetVerificationFailures();

		if (failures.Count > 0)
		{
			throw new VerificationException(failures);
		}
	}
}