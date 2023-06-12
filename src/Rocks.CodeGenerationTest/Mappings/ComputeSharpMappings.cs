// TODO: This is giving a weird compilation error.
// Something to do with ID2D1PixelShader.
/*
Severity	Code	Description	Project	File	Line	Suppression State
Error	CS8785	Generator 'ID2D1ShaderGenerator' failed to generate source. It will not contribute to the output and compilation errors may occur as a result. Exception was of type 'InvalidCastException' with message 'Unable to cast object of type 'Microsoft.CodeAnalysis.CSharp.Symbols.PublicModel.TypeParameterSymbol' to type 'Microsoft.CodeAnalysis.INamedTypeSymbol'.'	Rocks.CodeGenerationTest	C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\CSC	1	Active
*/

using ComputeSharp.D2D1;
using ComputeSharp.D2D1.__Internals;
using System.Diagnostics.CodeAnalysis;

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
//		public struct MappedPixelShader
//			: ID2D1PixelShader
//		{
//			public void BuildHlslSource(out string hlslSource) => throw new NotImplementedException();
//			public float4 Execute() => throw new NotImplementedException();
//			public uint GetInputCount() => throw new NotImplementedException();
//			public uint GetInputType(uint index) => throw new NotImplementedException();
//			public void GetOutputBuffer(out uint precision, out uint depth) => throw new NotImplementedException();
//			public uint GetPixelOptions() => throw new NotImplementedException();
//			public void InitializeFromDispatchData(ReadOnlySpan<byte> data) => throw new NotImplementedException();
//#pragma warning disable CS0618 // Type or member is obsolete
//			public void LoadBytecode<TLoader>(ref TLoader loader, D2D1ShaderProfile? shaderProfile, D2D1CompileOptions? options) where TLoader : struct, ID2D1BytecodeLoader => throw new NotImplementedException();
//			public void LoadBytecode<TLoader>(ref TLoader loader, [NotNull] ref D2D1ShaderProfile? shaderProfile, [NotNull] ref D2D1CompileOptions? compileOptions) where TLoader : struct, ID2D1BytecodeLoader => throw new NotImplementedException();
//			public void LoadDispatchData<TLoader>(ref TLoader loader) where TLoader : struct, ID2D1DispatchDataLoader => throw new NotImplementedException();
//			public void LoadInputDescriptions<TLoader>(ref TLoader loader) where TLoader : struct, ID2D1InputDescriptionsLoader => throw new NotImplementedException();
//			public void LoadResourceTextureDescriptions<TLoader>(ref TLoader loader) where TLoader : struct, ID2D1ResourceTextureDescriptionsLoader => throw new NotImplementedException();
//#pragma warning restore CS0618 // Type or member is obsolete
//		}
	}
}