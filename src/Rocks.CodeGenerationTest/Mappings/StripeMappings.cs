using Stripe;
using Stripe.Infrastructure;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class StripeMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(ExpandableField<>), new()
					{
						{ "T", "global::Stripe.IHasId" },
					}
				},
				{
					typeof(ExpandableFieldConverter<>), new()
					{
						{ "T", "global::Stripe.IHasId" },
					}
				},
				{
					typeof(ICreatable<,>), new()
					{
						{ "TEntity", "global::Stripe.IStripeEntity" },
						{ "TOptions", "global::Stripe.BaseOptions" },
					}
				},
				{
					typeof(IDeletable<,>), new()
					{
						{ "TEntity", "global::Rocks.CodeGenerationTest.Mappings.Stripe.MappedStripedEntityHasId" },
						{ "TOptions", "global::Stripe.BaseOptions" },
					}
				},
				{
					typeof(IExpandableField<>), new()
					{
						{ "T", "global::Stripe.IHasId" },
					}
				},
				{
					typeof(IListable<,>), new()
					{
						{ "TEntity", "global::Rocks.CodeGenerationTest.Mappings.Stripe.MappedStripedEntityHasId" },
						{ "TOptions", "global::Stripe.ListOptions" },
					}
				},
				{
					typeof(INestedCreatable<,>), new()
					{
						{ "TEntity", "global::Stripe.IStripeEntity" },
						{ "TOptions", "global::Stripe.BaseOptions" },
					}
				},
				{
					typeof(INestedDeletable<,>), new()
					{
						{ "TEntity", "global::Rocks.CodeGenerationTest.Mappings.Stripe.MappedStripedEntityHasId" },
						{ "TOptions", "global::Stripe.BaseOptions" },
					}
				},
				{
					typeof(INestedListable<,>), new()
					{
						{ "TEntity", "global::Rocks.CodeGenerationTest.Mappings.Stripe.MappedStripedEntityHasId" },
						{ "TOptions", "global::Stripe.ListOptions" },
					}
				},
				{
					typeof(INestedRetrievable<,>), new()
					{
						{ "TEntity", "global::Rocks.CodeGenerationTest.Mappings.Stripe.MappedStripedEntityHasId" },
						{ "TOptions", "global::Stripe.BaseOptions" },
					}
				},
				{
					typeof(INestedUpdatable<,>), new()
					{
						{ "TEntity", "global::Rocks.CodeGenerationTest.Mappings.Stripe.MappedStripedEntityHasId" },
						{ "TOptions", "global::Stripe.BaseOptions" },
					}
				},
				{
					typeof(IRetrievable<,>), new()
					{
						{ "TEntity", "global::Rocks.CodeGenerationTest.Mappings.Stripe.MappedStripedEntityHasId" },
						{ "TOptions", "global::Stripe.BaseOptions" },
					}
				},
				{
					typeof(ISearchable<,>), new()
					{
						{ "TEntity", "global::Rocks.CodeGenerationTest.Mappings.Stripe.MappedStripedEntityHasId" },
						{ "TOptions", "global::Stripe.SearchOptions" },
					}
				},
				{
					typeof(ISingletonRetrievable<>), new()
					{
						{ "TEntity", "global::Stripe.IStripeEntity" },
					}
				},
				{
					typeof(IUpdatable<,>), new()
					{
						{ "TEntity", "global::Rocks.CodeGenerationTest.Mappings.Stripe.MappedStripedEntityHasId" },
						{ "TOptions", "global::Stripe.ListOptions" },
					}
				},
				{
					typeof(Service<>), new()
					{
						{ "TEntityReturned", "global::Stripe.IStripeEntity" },
					}
				},
				{
					typeof(ServiceNested<>), new()
					{
						{ "TEntityReturned", "global::Stripe.IStripeEntity" },
					}
				},
				{
					typeof(StripeEntity<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.Stripe.MappedStripedEntity" },
					}
				},
			};
	}

	namespace Stripe
	{
		public sealed class MappedStripedEntity
			: StripeEntity<MappedStripedEntity>
		{ }

		public sealed class MappedStripedEntityHasId : IStripeEntity, IHasId
		{
			public string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
			public StripeResponse StripeResponse { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		}
	}
}