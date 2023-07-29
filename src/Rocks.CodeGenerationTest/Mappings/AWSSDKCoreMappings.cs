using Amazon.Runtime;
using Amazon.Runtime.EventStreams;
using Amazon.Runtime.EventStreams.Internal;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;
using System.Security.Cryptography;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class AWSSDKCoreMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(DictionaryUnmarshaller<,,,>), new()
					{
						{ "TKey", "object" },
						{ "TValue", "object" },
						{ "TKeyUnmarshaller", "global::Rocks.CodeGenerationTest.Mappings.AWSSDKCore.MappedUnmarshaller" },
						{ "TValueUnmarshaller", "global::Rocks.CodeGenerationTest.Mappings.AWSSDKCore.MappedUnmarshaller" },
					}
				},
				{
					typeof(KeyValueUnmarshaller<,,,>), new()
					{
						{ "K", "object" },
						{ "V", "object" },
						{ "KUnmarshaller", "global::Rocks.CodeGenerationTest.Mappings.AWSSDKCore.MappedUnmarshaller" },
						{ "VUnmarshaller", "global::Rocks.CodeGenerationTest.Mappings.AWSSDKCore.MappedUnmarshaller" },
					}
				},
				{
					typeof(EventStream<,>), new()
					{
						{ "T", "global::Amazon.Runtime.EventStreams.Internal.IEventStreamEvent" },
						{ "TE", "global::Rocks.CodeGenerationTest.Mappings.AWSSDKCore.MappedEventStreamException" },
					}
				},
				{
					typeof(EventStreamEventReceivedArgs<>), new()
					{
						{ "T", "global::Amazon.Runtime.EventStreams.Internal.IEventStreamEvent" },
					}
				},
				{
					typeof(EncryptStream<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.AWSSDKCore.MappedEncryptionWrapper" },
					}
				},
				{
					typeof(EnumerableEventStream<,>), new()
					{
						{ "T", "global::Amazon.Runtime.EventStreams.Internal.IEventStreamEvent" },
						{ "TE", "global::Rocks.CodeGenerationTest.Mappings.AWSSDKCore.MappedEventStreamException" },
					}
				},
				{
					typeof(ListUnmarshaller<,>), new()
					{
						{ "I", "object" },
						{ "IUnmarshaller", "global::Rocks.CodeGenerationTest.Mappings.AWSSDKCore.MappedUnmarshaller" },
					}
				},
				{
					typeof(HashStream<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.AWSSDKCore.MappedHashingWrapper" },
					}
				},
				{
					typeof(IEventStream<,>), new()
					{
						{ "T", "global::global::Amazon.Runtime.EventStreams.Internal.IEventStreamEvent" },
						{ "TE", "global::Rocks.CodeGenerationTest.Mappings.AWSSDKCore.MappedEventStreamException" },
					}
				},
				{
					typeof(IRequestMarshaller<,>), new()
					{
						{ "R", "object" },
						{ "T", "global::Amazon.Runtime.Internal.Transform.MarshallerContext" },
					}
				},
				{
					typeof(EncryptUploadPartStream<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.AWSSDKCore.MappedEncryptionWrapper" },
					}
				},
				{
					typeof(EventStreamExceptionReceivedArgs<>), new()
					{
						{ "T", "global::Amazon.Runtime.EventStreams.Internal.EventStreamException" },
					}
				},
				{
					typeof(IEnumerableEventStream<,>), new()
					{
						{ "T", "global::global::Amazon.Runtime.EventStreams.Internal.IEventStreamEvent" },
						{ "TE", "global::Rocks.CodeGenerationTest.Mappings.AWSSDKCore.MappedEventStreamException" },
					}
				},
				{
					typeof(IExceptionHandler<>), new()
					{
						{ "T", "global::System.Exception" },
					}
				},
				{
					typeof(ExceptionHandler<>), new()
					{
						{ "T", "global::System.Exception" },
					}
				},
				{
					typeof(DecryptStream<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.AWSSDKCore.MappedDecryptionWrapper" },
					}
				},
			};
	}

	namespace AWSSDKCore
	{
		public sealed class MappedHashingWrapper
			: IHashingWrapper
		{
			public void AppendBlock(byte[] buffer) => throw new NotImplementedException();
			public void AppendBlock(byte[] buffer, int offset, int count) => throw new NotImplementedException();
			public byte[] AppendLastBlock(byte[] buffer) => throw new NotImplementedException();
			public byte[] AppendLastBlock(byte[] buffer, int offset, int count) => throw new NotImplementedException();
			public void Clear() => throw new NotImplementedException();
			public byte[] ComputeHash(byte[] buffer) => throw new NotImplementedException();
			public byte[] ComputeHash(Stream stream) => throw new NotImplementedException();
			public void Dispose() => throw new NotImplementedException();
		}

		public sealed class MappedDecryptionWrapper
			: IDecryptionWrapper
		{
			public ICryptoTransform Transformer => throw new NotImplementedException();

			public void CreateDecryptor() => throw new NotImplementedException();
			public void SetDecryptionData(byte[] key, byte[] IV) => throw new NotImplementedException();
		}

		public sealed class MappedEncryptionWrapper
			: IEncryptionWrapper
		{
			public int AppendBlock(byte[] buffer, int offset, int count, byte[] target, int targetOffset) => throw new NotImplementedException();
			public byte[] AppendLastBlock(byte[] buffer, int offset, int count) => throw new NotImplementedException();
			public void CreateEncryptor() => throw new NotImplementedException();
			public void Reset() => throw new NotImplementedException();
			public void SetEncryptionData(byte[] key, byte[] IV) => throw new NotImplementedException();
		}

		public sealed class MappedEventStreamException
			: EventStreamException
		{ }

		public sealed class MappedUnmarshaller
			: IUnmarshaller<object, XmlUnmarshallerContext>, IUnmarshaller<object, JsonUnmarshallerContext>
		{
			public object Unmarshall(XmlUnmarshallerContext input) => throw new NotImplementedException();
			public object Unmarshall(JsonUnmarshallerContext input) => throw new NotImplementedException();
		}
	}
}