using System;
using System.Collections.Generic;
using System.Threading;

namespace Rocks
{
	public sealed class HandlerInformation
	{
		private const string ErrorExpectedCallCount = "The expected call count is incorrect. Expected: {0}, received: {1}.";

		private int callCount;
		private uint expectedCount;

		public HandlerInformation(Delegate method)
			: this(method, 1)
		{ }

		public HandlerInformation(Delegate method, uint expectedCount)
		{
			this.Method = method;
			this.expectedCount = expectedCount;
		}

		public void IncrementCallCount()
		{
			Interlocked.Increment(ref this.callCount);
		}

		public IReadOnlyList<string> Verify()
		{
			var verifications = new List<string>();

			if(this.expectedCount != this.callCount)
			{
				verifications.Add(string.Format(HandlerInformation.ErrorExpectedCallCount,
					this.expectedCount, this.callCount));
			}

			return verifications.AsReadOnly();
		}

		public int CallCount
		{
			get { return this.callCount; }
		}

		public Delegate Method { get; private set; }
	}
}
