// TODO: This is giving a weird compilation error.
// Something to do with ID2D1PixelShader.
/*
Severity	Code	Description	Project	File	Line	Suppression State
Error	CS8785	Generator 'ID2D1ShaderGenerator' failed to generate source. It will not contribute to the output and compilation errors may occur as a result. Exception was of type 'InvalidCastException' with message 'Unable to cast object of type 'Microsoft.CodeAnalysis.CSharp.Symbols.PublicModel.TypeParameterSymbol' to type 'Microsoft.CodeAnalysis.INamedTypeSymbol'.'	Rocks.CodeGenerationTest	C:\Users\jason\source\repos\Rocks\src\Rocks.CodeGenerationTest\CSC	1	Active
*/

using ComputeSharp.D2D1;
using ComputeSharp.D2D1.Interop;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class ComputeSharpMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(D2D1TransformMapper<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ComputeSharp.MappedPixelShader" },
					}
				},
				{
					typeof(D2D1TransformMapperFactory<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ComputeSharp.MappedPixelShader" },
					}
				},
			};
	}

	namespace ComputeSharp
	{
		//[D2D1InputCount]
#pragma warning disable CMPSD2D0058 // Missing [D2DResourceTextureIndex] attribute
		public partial struct MappedPixelShader
#pragma warning restore CMPSD2D0058 // Missing [D2DResourceTextureIndex] attribute
			 : ID2D1PixelShader
		{
			public float4 Execute() => default;
		}
	}
}