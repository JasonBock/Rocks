using System;
using System.Collections.Generic;
using System.Threading;

namespace Rocks
{
	public sealed class HandlerInformation
	{
		private const string ErrorExpectedCallCount = "The expected call count is incorrect. Expected: {0}, received: {1}.";

		private int callCount;
		private uint expectedCallCount;

		public HandlerInformation()
			: this(null, 1)
		{ }

		public HandlerInformation(Delegate method)
			: this(method, 1)
		{ }

		public HandlerInformation(uint expectedCallCount)
			: this(null, expectedCallCount)
		{ }

		public HandlerInformation(Delegate method, uint expectedCallCount)
		{
			this.Method = method;
			this.expectedCallCount = expectedCallCount;
		}

		public void IncrementCallCount()
		{
			Interlocked.Increment(ref this.callCount);
		}

		public IReadOnlyList<string> Verify()
		{
			var verifications = new List<string>();

			if(this.expectedCallCount != this.callCount)
			{
				verifications.Add(string.Format(HandlerInformation.ErrorExpectedCallCount,
					this.expectedCallCount, this.callCount));
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
