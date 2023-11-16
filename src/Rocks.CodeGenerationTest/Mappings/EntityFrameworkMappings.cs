#pragma warning disable EF1001
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Reflection;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class EntityFrameworkMappings
	{
		//private static readonly JsonNullableStructCollectionReaderWriter<IEnumerable<int?>, object, int> x;

		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(AnnotatableBuilder<,>), new()
					{
						{ "TMetadata", "global::Microsoft.EntityFrameworkCore.Infrastructure.ConventionAnnotatable" },
						{ "TModelBuilder", "global::Microsoft.EntityFrameworkCore.Metadata.Builders.IConventionModelBuilder" },
					}
				},
				{
					typeof(ClrICollectionAccessor<,,>), new()
					{
						{ "TEntity", "object" },
						{ "TCollection", "global::System.Collections.Generic.List<object>" },
						{ "TElement", "object" },
					}
				},
				{
					typeof(CompiledAsyncEnumerableQuery<,>), new()
					{
						{ "TContext", "global::Microsoft.EntityFrameworkCore.DbContext" },
						{ "TResult", "object" },
					}
				},
				{
					typeof(CompiledAsyncTaskQuery<,>), new()
					{
						{ "TContext", "global::Microsoft.EntityFrameworkCore.DbContext" },
						{ "TResult", "object" },
					}
				},
				{
					typeof(CompiledQuery<,>), new()
					{
						{ "TContext", "global::Microsoft.EntityFrameworkCore.DbContext" },
						{ "TResult", "object" },
					}
				},
				{
					typeof(CompiledQueryBase<,>), new()
					{
						{ "TContext", "global::Microsoft.EntityFrameworkCore.DbContext" },
						{ "TResult", "object" },
					}
				},
				{
					typeof(DatabaseProvider<>), new()
					{
						{ "TOptionsExtension", "global::Microsoft.EntityFrameworkCore.Infrastructure.IDbContextOptionsExtension" },
					}
				},
				{
					typeof(DbContextFactory<>), new()
					{
						{ "TContext", "global::Microsoft.EntityFrameworkCore.DbContext" },
					}
				},
				{
					typeof(DbContextFactorySource<>), new()
					{
						{ "TContext", "global::Microsoft.EntityFrameworkCore.DbContext" },
					}
				},
				{
					typeof(DbContextOptions<>), new()
					{
						{ "TContext", "global::Microsoft.EntityFrameworkCore.DbContext" },
					}
				},
				{
					typeof(DbContextOptionsBuilder<>), new()
					{
						{ "TContext", "global::Microsoft.EntityFrameworkCore.DbContext" },
					}
				},
				{
					typeof(DbContextPool<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
					}
				},
				{
					typeof(DiagnosticsLogger<>), new()
					{
						{ "TLoggerCategory", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedLoggerCategory" },
					}
				},
				{
					typeof(EnumToNumberConverter<,>), new()
					{
						{ "TEnum", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedEnum" },
						{ "TNumber", "int" },
					}
				},
				{
					typeof(EnumToStringConverter<>), new()
					{
						{ "TEnum", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedEnum" },
					}
				},
				{
					typeof(IConventionPropertyBaseBuilder<>), new()
					{
						{ "TBuilder", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedConventionPropertyBaseBuilder" },
					}
				},
				{
					typeof(IDbContextFactory<>), new()
					{
						{ "TContext", "global::Microsoft.EntityFrameworkCore.DbContext" },
					}
				},
				{
					typeof(IDbContextFactorySource<>), new()
					{
						{ "TContext", "global::Microsoft.EntityFrameworkCore.DbContext" },
					}
				},
				{
					typeof(IDbContextPool<>), new()
					{
						{ "TContext", "global::Microsoft.EntityFrameworkCore.DbContext" },
					}
				},
				{
					typeof(IDesignTimeDbContextFactory<>), new()
					{
						{ "TContext", "global::Microsoft.EntityFrameworkCore.DbContext" },
					}
				},
				{
					typeof(IDiagnosticsLogger<>), new()
					{
						{ "TLoggerCategory", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedLoggerCategory" },
					}
				},
				{
					typeof(IScopedDbContextLease<>), new()
					{
						{ "TContext", "global::Microsoft.EntityFrameworkCore.DbContext" },
					}
				},
				{
					typeof(InterceptorAggregator<>), new()
					{
						{ "TInterceptor", "global::Microsoft.EntityFrameworkCore.Diagnostics.IInterceptor" },
					}
				},
				{
					typeof(InternalPropertyBaseBuilder<,>), new()
					{
						{ "TBuilder", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedConventionPropertyBaseBuilder" },
						{ "TPropertyBase", "global::Microsoft.EntityFrameworkCore.Metadata.Internal.PropertyBase" },
					}
				},
				{
					typeof(JsonCollectionReaderWriter<,,>), new()
					{
						{ "TCollection", "global::System.Collections.Generic.IEnumerable<object>" },
						{ "TConcreteCollection", "object" },
						{ "TElement", "object" },
					}
				},
				{
					typeof(JsonNullableStructCollectionReaderWriter<,,>), new()
					{
						{ "TCollection", "global::System.Collections.Generic.IEnumerable<int?>" },
						{ "TConcreteCollection", "object" },
						{ "TElement", "int" },
					}
				},
				{
					typeof(NavigationAttributeConventionBase<>), new()
					{
						{ "TAttribute", "global::System.Attribute" },
					}
				},
				{
					typeof(PooledDbContextFactory<>), new()
					{
						{ "TContext", "global::Microsoft.EntityFrameworkCore.DbContext" },
					}
				},
				{
					typeof(PropertyAttributeConventionBase<>), new()
					{
						{ "TAttribute", "global::System.Attribute" },
					}
				},
				{
					typeof(StringToEnumConverter<>), new()
					{
						{ "TEnum", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedEnum" },
					}
				},
				{
					typeof(StringEnumConverter<,,>), new()
					{
						{ "TModel", "object" },
						{ "TProvider", "object" },
						{ "TEnum", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedEnum" },
					}
				},
				{
					typeof(TypeAttributeConventionBase<>), new()
					{
						{ "TAttribute", "global::System.Attribute" },
					}
				},
			};
	}

	namespace EntityFramework
	{
		public enum MappedEnum { }

		public sealed class MappedConventionPropertyBaseBuilder
			: IConventionPropertyBaseBuilder<MappedConventionPropertyBaseBuilder>
		{
			public IConventionPropertyBase Metadata => throw new NotImplementedException();

			public IConventionModelBuilder ModelBuilder => throw new NotImplementedException();

			IConventionAnnotatable IConventionAnnotatableBuilder.Metadata => throw new NotImplementedException();

			public bool CanRemoveAnnotation(string name, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public bool CanSetAnnotation(string name, object? value, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public bool CanSetField(string? fieldName, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public bool CanSetField(FieldInfo? fieldInfo, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public bool CanSetPropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public MappedConventionPropertyBaseBuilder? HasAnnotation(string name, object? value, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public MappedConventionPropertyBaseBuilder? HasField(string? fieldName, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public MappedConventionPropertyBaseBuilder? HasField(FieldInfo? fieldInfo, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public MappedConventionPropertyBaseBuilder? HasNoAnnotation(string name, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public MappedConventionPropertyBaseBuilder? HasNonNullAnnotation(string name, object? value, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public MappedConventionPropertyBaseBuilder? UsePropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation = false) => throw new NotImplementedException();
			IConventionAnnotatableBuilder? IConventionAnnotatableBuilder.HasAnnotation(string name, object? value, bool fromDataAnnotation) => throw new NotImplementedException();
			IConventionAnnotatableBuilder? IConventionAnnotatableBuilder.HasNoAnnotation(string name, bool fromDataAnnotation) => throw new NotImplementedException();
			IConventionAnnotatableBuilder? IConventionAnnotatableBuilder.HasNonNullAnnotation(string name, object? value, bool fromDataAnnotation) => throw new NotImplementedException();
		}

		public sealed class MappedLoggerCategory
			: LoggerCategory<MappedLoggerCategory>
		{ }
	}
}
#pragma warning restore EF1001