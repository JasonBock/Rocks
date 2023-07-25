using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Rocks.CodeGenerationTest.Mappings
{
   internal static class AvaloniaMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(DefinitionList<>), new()
					{
						{ "T", "global::Avalonia.Controls.DefinitionBase" },
					}
				},
				{
					typeof(FuncControlTemplate<>), new()
					{
						{ "T", "global::Avalonia.Controls.Primitives.TemplatedControl" },
					}
				},
				{
					typeof(FuncTemplate<>), new()
					{
						{ "TControl", "global::Avalonia.Controls.Control" },
					}
				},
				{
					typeof(FuncTemplate<,>), new()
					{
						{ "TParam", "object" },
						{ "TControl", "global::Avalonia.Controls.Control" },
					}
				},
				{
					typeof(ITemplate<>), new()
					{
						{ "TControl", "global::Avalonia.Controls.Control" },
					}
				},
			};
	}
}