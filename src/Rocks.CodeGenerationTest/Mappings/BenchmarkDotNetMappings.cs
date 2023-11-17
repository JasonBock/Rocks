using BenchmarkDotNet.Characteristics;
using BenchmarkDotNet.Jobs;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class BenchmarkDotNetMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(CharacteristicObject<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.BenchmarkDotNet.MappedCharacteristicObject" },
					}
				},
				{
					typeof(JobMode<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.BenchmarkDotNet.MappedJobMode" },
					}
				},
			};
	}

	namespace BenchmarkDotNet
	{
		public sealed class MappedCharacteristicObject
			: CharacteristicObject<MappedCharacteristicObject>
		{ }

		public sealed class MappedJobMode
			: JobMode<MappedJobMode>
		{ }
	}
}