using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;

namespace Rocks
{
   // TODO: Consider making this immutable - that is,
   // make AddRaiseEvent and IncrementCallCount return 
	// new instances.
   [Serializable]
	public class HandlerInformation
	{
		public const string ErrorAtLeastOnceCallCount = "The method should have been called at least once.";
		private const string ErrorExpectedCallCount = "The expected call count is incorrect. Expected: {0}, received: {1}.";

		private int callCount;

		[NonSerialized]
		private readonly List<RaiseEventInformation> raiseEvents = new List<RaiseEventInformation>();

		internal HandlerInformation()
			: this(null, ImmutableDictionary<int, Arg>.Empty)
		{ }

		internal HandlerInformation(Delegate method)
			: this(method, ImmutableDictionary<int, Arg>.Empty)
		{ }

		internal HandlerInformation(ImmutableDictionary<int, Arg> expectations)
			: this(null, expectations)
		{ }

		internal HandlerInformation(Delegate? method, ImmutableDictionary<int, Arg> expectations) =>
			(this.Method, this.Expectations) = (method, expectations);

		internal void AddRaiseEvent(RaiseEventInformation raiseEvent) => this.raiseEvents.Add(raiseEvent);

		public void IncrementCallCount() => Interlocked.Increment(ref this.callCount);

		public void RaiseEvents(IMockWithEvents target)
		{
			if(target is null) { throw new ArgumentNullException(nameof(target)); }

			foreach (var raiseEvent in this.raiseEvents)
			{
				target.Raise(raiseEvent.Name, raiseEvent.Args);
			}
		}

		internal void SetExpectedCallCount(uint expectedCallCount) => this.ExpectedCallCount = expectedCallCount;

		internal IReadOnlyList<string> Verify()
		{
			var verifications = new List<string>();

			if (this.ExpectedCallCount != this.callCount)
			{
				verifications.Add(string.Format(
					CultureInfo.CurrentCulture, HandlerInformation.ErrorExpectedCallCount, 
					this.ExpectedCallCount.ToString(CultureInfo.CurrentCulture), this.callCount.ToString(CultureInfo.CurrentCulture)));
			}

			return verifications.AsReadOnly();
		}

		internal int CallCount => this.callCount;

		public ImmutableDictionary<int, Arg> Expectations { get; }
		internal uint ExpectedCallCount { get; private set; } = 1;
		public Delegate? Method { get; }
		internal ReadOnlyCollection<RaiseEventInformation> GetRaiseEvents() => this.raiseEvents.AsReadOnly();
	}
}