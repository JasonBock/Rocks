using Rocks.CodeGenerationTest.Extensions.Extensions;

namespace Rocks.CodeGenerationTest.Mappings;

internal static class MappedTypes
{
	internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
		new Dictionary<Type, Dictionary<string, string>>()
			.AddItems(AngleSharpMappings.GetMappedTypes())
			.AddItems(AutoMapperMappings.GetMappedTypes())
			.AddItems(AvaloniaMappings.GetMappedTypes())
			.AddItems(AutofacMappings.GetMappedTypes())
			.AddItems(AWSSDKCoreMappings.GetMappedTypes())
			.AddItems(ComputeSharpMappings.GetMappedTypes())
			.AddItems(CslaMappings.GetMappedTypes())
			.AddItems(EntityFrameworkMappings.GetMappedTypes())
			.AddItems(FluentAssertionsMappings.GetMappedTypes())
			.AddItems(FluentValidationMappings.GetMappedTypes())
			.AddItems(GoogleProtobufMappings.GetMappedTypes())
			.AddItems(ILGPUMappings.GetMappedTypes())
			.AddItems(ImageSharpMappings.GetMappedTypes())
			.AddItems(MassTransitMappings.GetMappedTypes())
			.AddItems(MediatRMappings.GetMappedTypes())
			.AddItems(MessagePackMappings.GetMappedTypes())
			.AddItems(NSubstituteMappings.GetMappedTypes())
			.AddItems(ReactiveMappings.GetMappedTypes())
			.AddItems(SilkMappings.GetMappedTypes());
}