using Rocks.CodeGenerationTest.Extensions.Extensions;

namespace Rocks.CodeGenerationTest.Mappings;

/*
Mapping tips:

* If a generic parameter is constrained to System.Enum, create an enum (see GraphQL)
*/

internal static class MappedTypes
{
	internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
		new Dictionary<Type, Dictionary<string, string>>()
			.AddItems(AngleSharpMappings.GetMappedTypes())
			.AddItems(AspireMappings.GetMappedTypes())
			.AddItems(AsposeMappings.GetMappedTypes())
			.AddItems(AutoMapperMappings.GetMappedTypes())
			.AddItems(AvaloniaMappings.GetMappedTypes())
			.AddItems(AutofacMappings.GetMappedTypes())
			.AddItems(AWSSDKCoreMappings.GetMappedTypes())
			.AddItems(BenchmarkDotNetMappings.GetMappedTypes())
			.AddItems(CoravelMappings.GetMappedTypes())
			.AddItems(CslaMappings.GetMappedTypes())
			.AddItems(ElasticSearchMappings.GetMappedTypes())
			.AddItems(EntityFrameworkMappings.GetMappedTypes())
			.AddItems(FluentAssertionsMappings.GetMappedTypes())
			.AddItems(FluentValidationMappings.GetMappedTypes())
			.AddItems(GraphQLMappings.GetMappedTypes())
			.AddItems(HandlebarsMappings.GetMappedTypes())
			.AddItems(ImageSharpMappings.GetMappedTypes())
			.AddItems(LanguageExtMappings.GetMappedTypes())
			.AddItems(MassTransitMappings.GetMappedTypes())
			.AddItems(MediatRMappings.GetMappedTypes())
			.AddItems(MessagePackMappings.GetMappedTypes())
			.AddItems(NinjectMappings.GetMappedTypes())
			.AddItems(NpgsqlMappings.GetMappedTypes())
			.AddItems(OpenApiMappings.GetMappedTypes())
			.AddItems(OrleansMappings.GetMappedTypes())
			.AddItems(ProtoActorMappings.GetMappedTypes())
			.AddItems(QuartzMappings.GetMappedTypes())
			.AddItems(ReactiveMappings.GetMappedTypes())
			.AddItems(SilkMappings.GetMappedTypes())
			.AddItems(StripeMappings.GetMappedTypes())
			.AddItems(TopshelfMappings.GetMappedTypes())
			.AddItems(TwilioMappings.GetMappedTypes())
			.AddItems(VerifyMappings.GetMappedTypes());
}