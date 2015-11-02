using NUnit.Framework;
using Rocks.Construction;
using System.Collections.ObjectModel;

namespace Rocks.Tests.Construction
{
	[TestFixture]
	public sealed class GetGeneratedEventsResultsTests
	{
		[Test]
		public void Create()
		{
			var events = new ReadOnlyCollection<string>(new string[0]);
			var requires = true;

			var results = new GetGeneratedEventsResults(events, requires);

			Assert.AreSame(events, results.Events, nameof(results.Events));
			Assert.AreEqual(requires, results.RequiresObsoleteSuppression,
				nameof(results.RequiresObsoleteSuppression));
		}
	}
}
