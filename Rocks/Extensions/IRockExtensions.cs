using Rocks.Exceptions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rocks.Extensions
{
	public static class IRockExtensions
	{
		internal static ReadOnlyCollection<string> GetVerificationFailures(this IRock @this)
		{
			var failures = new List<string>();

			foreach (var pair in @this.Handlers)
			{
				foreach (var handler in pair.Value)
				{
					foreach (var failure in handler.Verify())
					{
						failures.Add(string.Format(Constants.ErrorMessages.VerificationFailed,
							@this.GetType().FullName, pair.Key, failure));
					}
				}
			}

			return failures.AsReadOnly();
		}

		public static void Verify(this IRock @this)
		{
			var failures = @this.GetVerificationFailures();

			if (failures.Count > 0)
			{
				throw new VerificationException(failures);
			}
		}
	}
}
