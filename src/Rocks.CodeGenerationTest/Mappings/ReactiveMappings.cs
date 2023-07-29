using System.Reactive.Concurrency;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class ReactiveMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(ScheduledItem<>), new()
					{
						{ "TAbsolute", "global::Rocks.CodeGenerationTest.Mappings.Reactive.MappedReactiveComparable" },
					}
				},
				{
					typeof(SchedulerQueue<>), new()
					{
						{ "TAbsolute", "global::Rocks.CodeGenerationTest.Mappings.Reactive.MappedReactiveComparable" },
					}
				},
				{
					typeof(VirtualTimeScheduler<,>), new()
					{
						{ "TAbsolute", "global::Rocks.CodeGenerationTest.Mappings.Reactive.MappedReactiveComparable" },
						{ "TRelative", "object" },
					}
				},
				{
					typeof(VirtualTimeSchedulerBase<,>), new()
					{
						{ "TAbsolute", "global::Rocks.CodeGenerationTest.Mappings.Reactive.MappedReactiveComparable" },
						{ "TRelative", "object" },
					}
				},
			};
	}

	namespace Reactive
	{
		public sealed class MappedReactiveComparable
			: IComparable<MappedReactiveComparable>
		{
			public int CompareTo(MappedReactiveComparable? other) => throw new NotImplementedException();
		}
	}
}