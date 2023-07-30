using HandlebarsDotNet;
using HandlebarsDotNet.Collections;
using HandlebarsDotNet.Decorators;
using HandlebarsDotNet.Helpers;
using HandlebarsDotNet.Iterators;
using HandlebarsDotNet.PathStructure;
using HandlebarsDotNet.ValueProviders;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class HandlebarsMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(CascadeIndex<,,>), new()
					{
						{ "TKey", "object" },
						{ "TValue", "object" },
						{ "TComparer", "global::System.Collections.Generic.IEqualityComparer<object>" },
					}
				},
				{
					typeof(FixedSizeDictionary<,,>), new()
					{
						{ "TKey", "object" },
						{ "TValue", "object" },
						{ "TComparer", "global::System.Collections.Generic.IEqualityComparer<object>" },
					}
				},
				{
					typeof(IDecoratorDescriptor<>), new()
					{
						{ "TOptions", "global::Rocks.CodeGenerationTest.Mappings.HandlebarsDotNet.MappedDecoratorOptions" },
					}
				},
				{
					typeof(IDescriptor<>), new()
					{
						{ "TOptions", "global::Rocks.CodeGenerationTest.Mappings.HandlebarsDotNet.MappedOptions" },
					}
				},
				{
					typeof(IHelperDescriptor<>), new()
					{
						{ "TOptions", "global::Rocks.CodeGenerationTest.Mappings.HandlebarsDotNet.MappedHelperOptions" },
					}
				},
				{
					typeof(ListIterator<,>), new()
					{
						{ "T", "global::System.Collections.Generic.IList<object>" },
						{ "TValue", "object" },
					}
				},
				{
					typeof(ObservableIndex<,,>), new()
					{
						{ "TKey", "object" },
						{ "TValue", "object" },
						{ "TComparer", "global::System.Collections.Generic.IEqualityComparer<object>" },
					}
				},
				{
					typeof(ReadOnlyListIterator<,>), new()
					{
						{ "T", "global::System.Collections.Generic.IReadOnlyList<object>" },
						{ "TValue", "object" },
					}
				},
			};
	}

	namespace HandlebarsDotNet
	{
		public struct MappedDecoratorOptions : IDecoratorOptions
		{
			public DataValues Data => throw new NotImplementedException();

			public PathInfo Name => throw new NotImplementedException();

			public BindingContext Frame => throw new NotImplementedException();

			public IIndexed<string, IHelperDescriptor<BlockHelperOptions>> GetBlockHelpers() => throw new NotImplementedException();
			public IIndexed<string, IHelperDescriptor<HelperOptions>> GetHelpers() => throw new NotImplementedException();
		}

		public struct MappedHelperOptions : IHelperOptions
		{
			public DataValues Data => throw new NotImplementedException();

			public PathInfo Name => throw new NotImplementedException();

			public BindingContext Frame => throw new NotImplementedException();
		}

		public struct MappedOptions : IOptions
		{
			public BindingContext Frame => throw new NotImplementedException();
		}
	}
}