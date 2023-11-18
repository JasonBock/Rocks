using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Util;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class QuartzMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(AndMatcher<>), new()
					{
						{ "TKey", "global::Rocks.CodeGenerationTest.Mappings.Quartz.MappedKey" },
					}
				},
				{
					typeof(EverythingMatcher<>), new()
					{
						{ "TKey", "global::Rocks.CodeGenerationTest.Mappings.Quartz.MappedKey" },
					}
				},
				{
					typeof(GroupMatcher<>), new()
					{
						{ "TKey", "global::Rocks.CodeGenerationTest.Mappings.Quartz.MappedKey" },
					}
				},
				{
					typeof(IMatcher<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.Quartz.MappedKey" },
					}
				},
				{
					typeof(KeyMatcher<>), new()
					{
						{ "TKey", "global::Rocks.CodeGenerationTest.Mappings.Quartz.MappedKey" },
					}
				},
				{
					typeof(NameMatcher<>), new()
					{
						{ "TKey", "global::Rocks.CodeGenerationTest.Mappings.Quartz.MappedKey" },
					}
				},
				{
					typeof(NotMatcher<>), new()
					{
						{ "TKey", "global::Rocks.CodeGenerationTest.Mappings.Quartz.MappedKey" },
					}
				},
				{
					typeof(OrMatcher<>), new()
					{
						{ "TKey", "global::Rocks.CodeGenerationTest.Mappings.Quartz.MappedKey" },
					}
				},
				{
					typeof(ScheduleBuilder<>), new()
					{
						{ "T", "global::Quartz.ITrigger" },
					}
				},
				{
					typeof(StringMatcher<>), new()
					{
						{ "TKey", "global::Rocks.CodeGenerationTest.Mappings.Quartz.MappedKey" },
					}
				},
			};
	}

	namespace Quartz
	{
		public sealed class MappedKey
			: Key<MappedKey>
		{ }
	}
}