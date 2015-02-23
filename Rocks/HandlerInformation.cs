using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace Rocks
{
	public class HandlerInformation
	{
		private const string ErrorExpectedCallCount = "The expected call count is incorrect. Expected: {0}, received: {1}.";

		private ReadOnlyDictionary<string, ArgumentExpectation> expectations;
		private int callCount;

		internal HandlerInformation(ReadOnlyDictionary<string, ArgumentExpectation> expectations)
			: this(null, 1, expectations)
		{ }

		internal HandlerInformation(Delegate method, ReadOnlyDictionary<string, ArgumentExpectation> expectations)
			: this(method, 1, expectations)
		{ }

		internal HandlerInformation(uint expectedCallCount, ReadOnlyDictionary<string, ArgumentExpectation> expectations)
			: this(null, expectedCallCount, expectations)
		{ }

		internal HandlerInformation(Delegate method, uint expectedCallCount, ReadOnlyDictionary<string, ArgumentExpectation> expectations)
		{
			this.Method = method;
			this.ExpectedCallCount = expectedCallCount;
			this.expectations = expectations;
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

			// TODO: Add argument verifications.

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
