using Rocks.Exceptions;
using System;
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
						var method = @this.GetType().GetMemberDescription(pair.Key);

						failures.Add($"Type: {@this.GetType().FullName}, method: {method}, message: {failure}");
					}
				}
			}

			return failures.AsReadOnly();
		}

		// TODO: Why is this on "object"? Seems wrong. The call to Raise()
		// won't occur unless the mock object implements IMockWithEvents
		public static void Raise(this object @this, string eventName, EventArgs args)
		{
			if (@this is IMockWithEvents mock)
			{
				mock.Raise(eventName, args);
			}
		}

		public static void Verify(this IMock @this)
		{
			if(@this is null) { throw new ArgumentNullException(nameof(@this)); }

			var failures = @this.GetVerificationFailures();

			if (failures.Count > 0)
			{
				throw new VerificationException(failures);
			}
		}
	}
}