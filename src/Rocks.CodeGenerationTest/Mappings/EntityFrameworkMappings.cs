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
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

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
						{ "TMetadata", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedConventionAnnotatable" },
						{ "TModelBuilder", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedConventionModelBuilder" },
					}
				},
				{
					typeof(ClrICollectionAccessor<,,>), new()
					{
						{ "TEntity", "global::System.Object" },
						{ "TCollection", "global::System.Collections.Generic.List<global::System.Object>" },
						{ "TElement", "global::System.Object" },
					}
				},
				{
					typeof(CompiledAsyncEnumerableQuery<,>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
						{ "TResult", "global::System.Object" },
					}
				},
				{
					typeof(CompiledAsyncTaskQuery<,>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
						{ "TResult", "global::System.Object" },
					}
				},
				{
					typeof(CompiledQuery<,>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
						{ "TResult", "global::System.Object" },
					}
				},
				{
					typeof(CompiledQueryBase<,>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
						{ "TResult", "global::System.Object" },
					}
				},
				{
					typeof(DatabaseProvider<>), new()
					{
						{ "TOptionsExtension", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContextOptionsExtension" },
					}
				},
				{
					typeof(DbContextFactory<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
					}
				},
				{
					typeof(DbContextFactorySource<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
					}
				},
				{
					typeof(DbContextOptions<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
					}
				},
				{
					typeof(DbContextOptionsBuilder<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
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
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
					}
				},
				{
					typeof(IDbContextFactorySource<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
					}
				},
				{
					typeof(IDbContextPool<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
					}
				},
				{
					typeof(IDesignTimeDbContextFactory<>), new()
					{
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
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
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
					}
				},
				{
					typeof(InterceptorAggregator<>), new()
					{
						{ "TInterceptor", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedInterceptor" },
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
						{ "TContext", "global::Rocks.CodeGenerationTest.Mappings.EntityFramework.MappedDbContext" },
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
		public sealed class MappedDbContextOptionsExtension
			: IDbContextOptionsExtension
		{
			public DbContextOptionsExtensionInfo Info => throw new NotImplementedException();

			public void ApplyServices(IServiceCollection services) => throw new NotImplementedException();
			public void Validate(IDbContextOptions options) => throw new NotImplementedException();
		}

		public sealed class MappedInterceptor
			: IInterceptor
		{ }

		public sealed class MappedConventionAnnotatable
			: ConventionAnnotatable
		{ }

		public sealed class MappedLoggerCategory
			: LoggerCategory<MappedLoggerCategory>
		{ }

		public sealed class MappedConventionModelBuilder
			: IConventionModelBuilder
		{
			public IConventionModel Metadata => throw new NotImplementedException();

			public IConventionModelBuilder ModelBuilder => throw new NotImplementedException();

			IConventionAnnotatable IConventionAnnotatableBuilder.Metadata => throw new NotImplementedException();

			public bool CanIgnore([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type type, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public bool CanIgnore(string typeName, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public bool CanRemoveAnnotation(string name, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public bool CanSetAnnotation(string name, object? value, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public bool CanSetChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public bool CanSetPropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public IConventionEntityTypeBuilder? Entity(string name, bool? shouldBeOwned = false, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public IConventionEntityTypeBuilder? Entity([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type type, bool? shouldBeOwned = false, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public IConventionEntityTypeBuilder? Entity(string name, string definingNavigationName, IConventionEntityType definingEntityType, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public IConventionEntityTypeBuilder? Entity([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type type, string definingNavigationName, IConventionEntityType definingEntityType, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public IConventionAnnotatableBuilder? HasAnnotation(string name, object? value, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public IConventionModelBuilder? HasChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public IConventionAnnotatableBuilder? HasNoAnnotation(string name, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public IConventionModelBuilder? HasNoEntityType(IConventionEntityType entityType, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public IConventionAnnotatableBuilder? HasNonNullAnnotation(string name, object? value, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public IConventionModelBuilder? Ignore([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type type, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public IConventionModelBuilder? Ignore(string typeName, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public bool IsIgnored([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type type, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public bool IsIgnored(string typeName, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public IConventionOwnedEntityTypeBuilder? Owned([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type type, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public IConventionEntityTypeBuilder? SharedTypeEntity(string name, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type type, bool? shouldBeOwned = false, bool fromDataAnnotation = false) => throw new NotImplementedException();
			public IConventionModelBuilder? UsePropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation = false) => throw new NotImplementedException();
		}

		public sealed class MappedDbContext
			: DbContext
		{ }
	}
}
#pragma warning restore EF1001