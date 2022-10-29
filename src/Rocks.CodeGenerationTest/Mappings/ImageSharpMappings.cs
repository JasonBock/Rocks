using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors;
using SixLabors.ImageSharp.Processing.Processors.Dithering;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using System.Numerics;

namespace Rocks.CodeGenerationTest.Mappings
{
	internal static class ImageSharpMappings
	{
		internal static Dictionary<Type, Dictionary<string, string>> GetMappedTypes() =>
			new()
			{
				{
					typeof(CloningImageProcessor<>), new()
					{
						{ "TPixel", "global::Rocks.CodeGenerationTest.Mappings.ImageSharp.MappedPixel" },
					}
				},
				{
					typeof(ICloningImageProcessor<>), new()
					{
						{ "TPixel", "global::Rocks.CodeGenerationTest.Mappings.ImageSharp.MappedPixel" },
					}
				},
				{
					typeof(IImageProcessor<>), new()
					{
						{ "TPixel", "global::Rocks.CodeGenerationTest.Mappings.ImageSharp.MappedPixel" },
					}
				},
				{
					typeof(Image<>), new()
					{
						{ "TPixel", "global::Rocks.CodeGenerationTest.Mappings.ImageSharp.MappedPixel" },
					}
				},
				{
					typeof(ImageFrame<>), new()
					{
						{ "TPixel", "global::Rocks.CodeGenerationTest.Mappings.ImageSharp.MappedPixel" },
					}
				},
				{
					typeof(ImageProcessor<>), new()
					{
						{ "TPixel", "global::Rocks.CodeGenerationTest.Mappings.ImageSharp.MappedPixel" },
					}
				},
				{
					typeof(IndexedImageFrame<>), new()
					{
						{ "TPixel", "global::Rocks.CodeGenerationTest.Mappings.ImageSharp.MappedPixel" },
					}
				},
				{
					typeof(IPaletteDitherImageProcessor<>), new()
					{
						{ "TPixel", "global::Rocks.CodeGenerationTest.Mappings.ImageSharp.MappedPixel" },
					}
				},
				{
					typeof(IPixel<>), new()
					{
						{ "TSelf", "global::Rocks.CodeGenerationTest.Mappings.ImageSharp.MappedPixel" },
					}
				},
				{
					typeof(IQuantizer<>), new()
					{
						{ "TPixel", "global::Rocks.CodeGenerationTest.Mappings.ImageSharp.MappedPixel" },
					}
				},
				{
					typeof(IResamplingTransformImageProcessor<>), new()
					{
						{ "TPixel", "global::Rocks.CodeGenerationTest.Mappings.ImageSharp.MappedPixel" },
					}
				},
				{
					typeof(PixelBlender<>), new()
					{
						{ "TPixel", "global::Rocks.CodeGenerationTest.Mappings.ImageSharp.MappedPixel" },
					}
				},
				{
					typeof(PixelOperations<>), new()
					{
						{ "TPixel", "global::Rocks.CodeGenerationTest.Mappings.ImageSharp.MappedPixel" },
					}
				},
			};
	}

	namespace ImageSharp
	{
		public struct MappedPixel
			: IPixel<MappedPixel>
		{
			public PixelOperations<MappedPixel> CreatePixelOperations() => throw new NotImplementedException();
			public bool Equals(MappedPixel other) => throw new NotImplementedException();
			public void FromAbgr32(Abgr32 source) => throw new NotImplementedException();
			public void FromArgb32(Argb32 source) => throw new NotImplementedException();
			public void FromBgr24(Bgr24 source) => throw new NotImplementedException();
			public void FromBgra32(Bgra32 source) => throw new NotImplementedException();
			public void FromBgra5551(Bgra5551 source) => throw new NotImplementedException();
			public void FromL16(L16 source) => throw new NotImplementedException();
			public void FromL8(L8 source) => throw new NotImplementedException();
			public void FromLa16(La16 source) => throw new NotImplementedException();
			public void FromLa32(La32 source) => throw new NotImplementedException();
			public void FromRgb24(Rgb24 source) => throw new NotImplementedException();
			public void FromRgb48(Rgb48 source) => throw new NotImplementedException();
			public void FromRgba32(Rgba32 source) => throw new NotImplementedException();
			public void FromRgba64(Rgba64 source) => throw new NotImplementedException();
			public void FromScaledVector4(Vector4 vector) => throw new NotImplementedException();
			public void FromVector4(Vector4 vector) => throw new NotImplementedException();
			public void ToRgba32(ref Rgba32 dest) => throw new NotImplementedException();
			public Vector4 ToScaledVector4() => throw new NotImplementedException();
			public Vector4 ToVector4() => throw new NotImplementedException();
		}
	}
}