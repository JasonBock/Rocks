using FluentValidation.Validators;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class FluentValidationMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(AbstractComparisonValidator<,>), new()
					{
						{ "T", "object" },
						{ "TProperty", "global::Rocks.CodeGenerationTest.Mappings.FluentValidation.MappedProperty" },
					}
				},
				{
					typeof(GreaterThanOrEqualValidator<,>), new()
					{
						{ "T", "object" },
						{ "TProperty", "global::Rocks.CodeGenerationTest.Mappings.FluentValidation.MappedProperty" },
					}
				},
				{
					typeof(GreaterThanValidator<,>), new()
					{
						{ "T", "object" },
						{ "TProperty", "global::Rocks.CodeGenerationTest.Mappings.FluentValidation.MappedProperty" },
					}
				},
				{
					typeof(LessThanOrEqualValidator<,>), new()
					{
						{ "T", "object" },
						{ "TProperty", "global::Rocks.CodeGenerationTest.Mappings.FluentValidation.MappedProperty" },
					}
				},
				{
					typeof(LessThanValidator<,>), new()
					{
						{ "T", "object" },
						{ "TProperty", "global::Rocks.CodeGenerationTest.Mappings.FluentValidation.MappedProperty" },
					}
				},
			};
	}

	namespace FluentValidation
	{
		public sealed class MappedProperty 
			: IComparable<MappedProperty>, IComparable
		{
			public int CompareTo(MappedProperty? other) => throw new NotImplementedException();
			public int CompareTo(object? obj) => throw new NotImplementedException();
		}
	}
}