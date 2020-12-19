using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;

namespace Rocks
{
   [Serializable]
	public class HandlerInformation
	{
		public const string ErrorAtLeastOnceCallCount = "The method should have been called at least once.";
		private const string ErrorExpectedCallCount = "The expected call count is incorrect. Expected: {0}, received: {1}.";

		private int callCount;

		[NonSerialized]
		private readonly List<RaiseEventInformation> raiseEvents = new();

		public HandlerInformation()
			: this(null, ImmutableArray<Arg>.Empty)
		{ }

		public HandlerInformation(Delegate method)
			: this(method, ImmutableArray<Arg>.Empty)
		{ }

		public HandlerInformation(ImmutableArray<Arg> expectations)
			: this(null, expectations)
		{ }

		public HandlerInformation(Delegate? method, ImmutableArray<Arg> expectations) =>
			(this.Method, this.Expectations) = (method, expectations);

		public void AddRaiseEvent(RaiseEventInformation raiseEvent) => this.raiseEvents.Add(raiseEvent);

		public void IncrementCallCount() => Interlocked.Increment(ref this.callCount);

		public void RaiseEvents(IMockWithEvents target)
		{
			if(target is null) { throw new ArgumentNullException(nameof(target)); }

			foreach (var raiseEvent in this.raiseEvents)
			{
				target.Raise(raiseEvent.FieldName, raiseEvent.Args);
			}
		}

		public void SetCallback<TCallback>(TCallback callback)
			where TCallback : Delegate => 
				this.Method = callback;

		public void SetExpectedCallCount(uint expectedCallCount) => this.ExpectedCallCount = expectedCallCount;

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

		public ImmutableArray<Arg> Expectations { get; }
		internal uint ExpectedCallCount { get; private set; } = 1;
		public Delegate? Method { get; private set; }
		internal ReadOnlyCollection<RaiseEventInformation> GetRaiseEvents() => this.raiseEvents.AsReadOnly();
	}
}