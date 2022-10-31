using Rocks.CodeGenerationTest.Extensions.Extensions;

namespace Rocks.CodeGenerationTest.Mappings;

internal static class MappedTypes
{
	internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
		new Dictionary<Type, Dictionary<string, string>>()
			.AddItems(CslaMappings.GetMappedTypes())
			.AddItems(ComputeSharpMappings.GetMappedTypes())
			.AddItems(ImageSharpMappings.GetMappedTypes())
			.AddItems(AutoMapperMappings.GetMappedTypes())
			.AddItems(EntityFrameworkMappings.GetMappedTypes())
			.AddItems(FluentAssertionsMappings.GetMappedTypes())
			.AddItems(GoogleProtobufMappings.GetMappedTypes())
			.AddItems(MediatRMappings.GetMappedTypes())
			.AddItems(ReactiveMappings.GetMappedTypes())
			.AddItems(NSubstituteMappings.GetMappedTypes())
			.AddItems(AWSSDKCoreMappings.GetMappedTypes())
			.AddItems(AngleSharpMappings.GetMappedTypes())
			.AddItems(MassTransitMappings.GetMappedTypes())
			.AddItems(SilkMappings.GetMappedTypes());
}