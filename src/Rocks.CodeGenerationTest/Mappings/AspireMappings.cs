namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class AspireMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(IResourceBuilder<>), new()
					{
						{ "T", "global::Aspire.Hosting.ApplicationModel.IResource" },
					}
				},
				{
					typeof(IResourceWithParent<>), new()
					{
						{ "T", "global::Aspire.Hosting.ApplicationModel.IResource" },
					}
				},
			};
	}
}