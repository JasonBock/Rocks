﻿using Rocks.CodeGenerationTest.Extensions.Extensions;

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
			.AddItems(CslaMappings.GetMappedTypes())
			.AddItems(ElasticSearchMappings.GetMappedTypes())
			.AddItems(EntityFrameworkMappings.GetMappedTypes())
			.AddItems(FluentAssertionsMappings.GetMappedTypes())
			.AddItems(FluentValidationMappings.GetMappedTypes())
			.AddItems(GoogleProtobufMappings.GetMappedTypes())
			.AddItems(HandlebarsMappings.GetMappedTypes())
			.AddItems(ILGPUMappings.GetMappedTypes())
			.AddItems(ImageSharpMappings.GetMappedTypes())
			.AddItems(MassTransitMappings.GetMappedTypes())
			.AddItems(MediatRMappings.GetMappedTypes())
			.AddItems(MessagePackMappings.GetMappedTypes())
			.AddItems(NinjectMappings.GetMappedTypes())
			.AddItems(NServiceBusMappings.GetMappedTypes())
			.AddItems(NSubstituteMappings.GetMappedTypes())
			.AddItems(ReactiveMappings.GetMappedTypes())
			.AddItems(SilkMappings.GetMappedTypes())
			.AddItems(ShouldlyMappings.GetMappedTypes())
			.AddItems(StripeMappings.GetMappedTypes())
			.AddItems(TopshelfMappings.GetMappedTypes())
			.AddItems(TwilioMappings.GetMappedTypes());
}