using AngleSharp.Dom;
using AngleSharp.Media;

namespace Rocks.CodeGenerationTest.Mappings
{
   internal static class AngleSharpMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(IResourceService<>), new()
					{
						{ "TResource", "global::AngleSharp.Media.IResourceInfo" },
					}
				},
				{
					typeof(IHtmlCollection<>), new()
					{
						{ "T", "global::AngleSharp.Dom.IElement" },
					}
				},
				{
					typeof(IElementFactory<,>), new()
					{
						{ "TDocument", "global::AngleSharp.Dom.IDocument" },
						{ "TElement", "global::AngleSharp.Dom.IElement" },
					}
				},
			};
	}
}