using Aspose.Email.Clients.Imap.Models;
using Aspose.Email.Mapi;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class AsposeMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(ImapMessageSet<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.Aspose.MappedImapMessageSet" },
					}
				},
				{
					typeof(ImapMethodParametersGeneral<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.Aspose.MappedImapMethodParametersGeneral" },
					}
				},
			};
	}

	namespace Aspose
	{
		public sealed class MappedImapMessageSet
			: ImapMessageSet<MappedImapMessageSet>
		{ }

		public sealed class MappedImapMethodParametersGeneral
			: ImapMethodParametersGeneral<MappedImapMethodParametersGeneral>
		{ }
	}
}