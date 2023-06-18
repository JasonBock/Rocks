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
			};
	}

	namespace ComputeSharp
   {
#pragma warning disable CMPSD2D0058 // Missing [D2DResourceTextureIndex] attribute
	  public partial struct MappedPixelShader
#pragma warning restore CMPSD2D0058 // Missing [D2DResourceTextureIndex] attribute
			 : ID2D1PixelShader
		{
			public float4 Execute() => default;
		}
	}
}