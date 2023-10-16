#pragma warning disable EF1001
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Rocks.CodeGenerationTest.Mappings
{
   internal static class EntityFrameworkMappings
	{
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
					typeof(EntityTypeAttributeConventionBase<>), new()
					{
						{ "TAttribute", "global::System.Attribute" },
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
					typeof(InternalPropertyBaseBuilder<>), new()
					{
						{ "TPropertyBase", "global::Microsoft.EntityFrameworkCore.Metadata.Internal.PropertyBase" },
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
			};
	}

	namespace EntityFramework
   {
	  public sealed class MappedLoggerCategory
			: LoggerCategory<MappedLoggerCategory>
		{ }
	}
}
#pragma warning restore EF1001