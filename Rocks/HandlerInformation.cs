using System;
using System.Collections.Generic;
using System.Threading;

namespace Rocks
{
	public sealed class HandlerInformation
	{
		private const string ErrorExpectedCallCount = "The expected call count is incorrect. Expected: {0}, received: {1}.";

		private int callCount;

		internal HandlerInformation()
			: this(null, 1)
		{ }

		internal HandlerInformation(Delegate method)
			: this(method, 1)
		{ }

		internal HandlerInformation(uint ExpectedCallCount)
			: this(null, ExpectedCallCount)
		{ }

		internal HandlerInformation(Delegate method, uint expectedCallCount)
		{
			this.Method = method;
			this.ExpectedCallCount = expectedCallCount;
		}

		internal void IncrementCallCount()
		{
			Interlocked.Increment(ref this.callCount);
		}

		internal IReadOnlyList<string> Verify()
		{
			var verifications = new List<string>();

			if(this.ExpectedCallCount != this.callCount)
			{
				verifications.Add(string.Format(HandlerInformation.ErrorExpectedCallCount,
					this.ExpectedCallCount, this.callCount));
			}

			return verifications.AsReadOnly();
		}

		internal int CallCount
		{
			get { return this.callCount; }
		}

		internal uint ExpectedCallCount { get; private set; }
		internal Delegate Method { get; private set; }
	}
}
