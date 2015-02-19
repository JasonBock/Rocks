using System;
using System.Collections.Generic;
using System.Threading;

namespace Rocks
{
	public sealed class HandlerInformation
	{
		private const string ErrorExpectedCallCount = "The expected call count is incorrect. Expected: {0}, received: {1}.";

		private int callCount;

		public HandlerInformation()
			: this(null, 1)
		{ }

		public HandlerInformation(Delegate method)
			: this(method, 1)
		{ }

		public HandlerInformation(uint ExpectedCallCount)
			: this(null, ExpectedCallCount)
		{ }

		public HandlerInformation(Delegate method, uint expectedCallCount)
		{
			this.Method = method;
			this.ExpectedCallCount = expectedCallCount;
		}

		public void IncrementCallCount()
		{
			Interlocked.Increment(ref this.callCount);
		}

		public IReadOnlyList<string> Verify()
		{
			var verifications = new List<string>();

			if(this.ExpectedCallCount != this.callCount)
			{
				verifications.Add(string.Format(HandlerInformation.ErrorExpectedCallCount,
					this.ExpectedCallCount, this.callCount));
			}

			return verifications.AsReadOnly();
		}

		public int CallCount
		{
			get { return this.callCount; }
		}

		public uint ExpectedCallCount { get; private set; }
		public Delegate Method { get; private set; }
	}
}
