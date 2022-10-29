using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class GoogleProtobufMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(IExtendableMessage<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.GoogleProtobuf.MappedExtendableMessage" },
					}
				},
				{
					typeof(IMessage<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.GoogleProtobuf.MappedMessage" },
					}
				},
			};
	}

	namespace GoogleProtobuf
	{
		public sealed class MappedExtendableMessage
			: IExtendableMessage<MappedExtendableMessage>
		{
			public MessageDescriptor Descriptor => throw new NotImplementedException();

			public int CalculateSize() => throw new NotImplementedException();
			public void ClearExtension<TValue>(Extension<MappedExtendableMessage, TValue> extension) => throw new NotImplementedException();
			public void ClearExtension<TValue>(RepeatedExtension<MappedExtendableMessage, TValue> extension) => throw new NotImplementedException();
			public MappedExtendableMessage Clone() => throw new NotImplementedException();
			public bool Equals(MappedExtendableMessage? other) => throw new NotImplementedException();
			public TValue GetExtension<TValue>(Extension<MappedExtendableMessage, TValue> extension) => throw new NotImplementedException();
			public Google.Protobuf.Collections.RepeatedField<TValue> GetExtension<TValue>(RepeatedExtension<MappedExtendableMessage, TValue> extension) => throw new NotImplementedException();
			public Google.Protobuf.Collections.RepeatedField<TValue> GetOrInitializeExtension<TValue>(RepeatedExtension<MappedExtendableMessage, TValue> extension) => throw new NotImplementedException();
			public bool HasExtension<TValue>(Extension<MappedExtendableMessage, TValue> extension) => throw new NotImplementedException();
			public void MergeFrom(MappedExtendableMessage message) => throw new NotImplementedException();
			public void MergeFrom(CodedInputStream input) => throw new NotImplementedException();
			public void SetExtension<TValue>(Extension<MappedExtendableMessage, TValue> extension, TValue value) => throw new NotImplementedException();
			public void WriteTo(CodedOutputStream output) => throw new NotImplementedException();
		}

		public sealed class MappedMessage
			: IMessage<MappedMessage>
		{
			public MessageDescriptor Descriptor => throw new NotImplementedException();

			public int CalculateSize() => throw new NotImplementedException();
			public MappedMessage Clone() => throw new NotImplementedException();
			public bool Equals(MappedMessage? other) => throw new NotImplementedException();
			public void MergeFrom(MappedMessage message) => throw new NotImplementedException();
			public void MergeFrom(CodedInputStream input) => throw new NotImplementedException();
			public void WriteTo(CodedOutputStream output) => throw new NotImplementedException();
		}
	}
}