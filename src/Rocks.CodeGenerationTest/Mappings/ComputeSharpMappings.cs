using ComputeSharp.D2D1;
using ComputeSharp.D2D1.__Internals;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class ComputeSharpMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(ID2D1TransformMapper<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ComputeSharp.MappedPixelShader" },
					}
				},
				{
					typeof(ID2D1TransformMapperFactory<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ComputeSharp.MappedPixelShader" },
					}
				},
			};
	}

	namespace ComputeSharp
	{
		public struct MappedPixelShader
			: ID2D1PixelShader
		{
			public void BuildHlslSource(out string hlslSource) => throw new NotImplementedException();
			public float4 Execute() => throw new NotImplementedException();
			public uint GetInputCount() => throw new NotImplementedException();
			public uint GetInputType(uint index) => throw new NotImplementedException();
			public void GetOutputBuffer(out uint precision, out uint depth) => throw new NotImplementedException();
			public uint GetPixelOptions() => throw new NotImplementedException();
			public void InitializeFromDispatchData(ReadOnlySpan<byte> data) => throw new NotImplementedException();
#pragma warning disable CS0618 // Type or member is obsolete
			public void LoadBytecode<TLoader>(ref TLoader loader, D2D1ShaderProfile? shaderProfile, D2D1CompileOptions? options) where TLoader : struct, ID2D1BytecodeLoader => throw new NotImplementedException();
			public void LoadDispatchData<TLoader>(ref TLoader loader) where TLoader : struct, ID2D1DispatchDataLoader => throw new NotImplementedException();
			public void LoadInputDescriptions<TLoader>(ref TLoader loader) where TLoader : struct, ID2D1InputDescriptionsLoader => throw new NotImplementedException();
			public void LoadResourceTextureDescriptions<TLoader>(ref TLoader loader) where TLoader : struct, ID2D1ResourceTextureDescriptionsLoader => throw new NotImplementedException();
#pragma warning restore CS0618 // Type or member is obsolete
		}
	}
}