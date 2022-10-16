#pragma warning disable CS0618

using NSubstitute.Core.Events;

namespace Rocks.CodeGenerationTest.Mappings;

internal static class NSubstituteMappings
{
	internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
		new()
		{
			{
				typeof(EventHandlerWrapper<>), new()
				{
					{ "TEventArgs", "global::System.EventArgs" },
				}
			},
		};
}

#pragma warning restore CS0618 