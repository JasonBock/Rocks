using Rocks.Exceptions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rocks.Extensions
{
	public static class IMockExtensions
	{
		internal static ReadOnlyCollection<string> GetVerificationFailures(this IMock @this)
		{
			var failures = new List<string>();

			foreach (var pair in @this.Handlers)
			{
				foreach (var handler in pair.Value)
				{
					foreach (var failure in handler.Verify())
					{
						failures.Add(ErrorMessages.GetVerificationFailed(@this.GetType().FullName, pair.Key, failure));
					}
				}
			}

			return failures.AsReadOnly();
		}

		public static void Verify(this IMock @this)
		{
			var failures = @this.GetVerificationFailures();

			if (failures.Count > 0)
			{
				throw new VerificationException(failures);
			}
		}
	}
}
