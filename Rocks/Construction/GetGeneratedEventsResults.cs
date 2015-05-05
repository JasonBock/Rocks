using System.Collections.ObjectModel;

namespace Rocks.Construction
{
	internal sealed class GetGeneratedEventsResults
	{
		internal GetGeneratedEventsResults(ReadOnlyCollection<string> events, bool requiresObsoleteSuppression)
		{
			this.Events = events;
			this.RequiresObsoleteSuppression = requiresObsoleteSuppression;
		}

		internal ReadOnlyCollection<string> Events { get; }
		internal bool RequiresObsoleteSuppression { get; }
	}
}
