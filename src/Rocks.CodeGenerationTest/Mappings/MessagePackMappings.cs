using MessagePack.Formatters;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class MessagePackMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(CollectionFormatterBase<,>), new()
					{
						{ "TElement", "object" },
						{ "TCollection", "global::System.Collections.Generic.IEnumerable<object>" },
					}
				},
				{
					typeof(CollectionFormatterBase<,,>), new()
					{
						{ "TElement", "object" },
						{ "TIntermediate", "object" },
						{ "TCollection", "global::System.Collections.Generic.IEnumerable<object>" },
					}
				},
				{
					typeof(CollectionFormatterBase<,,,>), new()
					{
						{ "TElement", "object" },
						{ "TIntermediate", "object" },
						{ "TEnumerator", "global::System.Collections.Generic.IEnumerator<object>" },
						{ "TCollection", "global::System.Collections.Generic.IEnumerable<object>" },
					}
				},
				{
					typeof(DictionaryFormatterBase<,,>), new()
					{
						{ "TKey", "object" },
						{ "TValue", "object" },
						{ "TDictionary", "global::System.Collections.Generic.IDictionary<object, object>" },
					}
				},
				{
					typeof(DictionaryFormatterBase<,,,>), new()
					{
						{ "TKey", "object" },
						{ "TValue", "object" },
						{ "TIntermediate", "object" },
						{ "TDictionary", "global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<object, object>>" },
					}
				},
				{
					typeof(DictionaryFormatterBase<,,,,>), new()
					{
						{ "TKey", "object" },
						{ "TValue", "object" },
						{ "TIntermediate", "object" },
						{ "TEnumerator", "global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<object, object>>" },
						{ "TDictionary", "global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<object, object>>" },
					}
				},
			};
	}
}