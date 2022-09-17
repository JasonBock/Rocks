using Microsoft.CodeAnalysis;
using Rocks;
using System.Collections.Immutable;
using Rocks.CodeGenerationTest;
using ComputeSharp;
using ComputeSharp.D2D1;
using Csla;
using Moq;
using SixLabors.ImageSharp;
using TerraFX.Interop.DirectX;
using TerraFX.Interop;

var targetAssemblies = new Type[] 
{ 
	// Core .NET types
	//typeof(object), typeof(Dictionary<,>), typeof(ImmutableArray), typeof(HttpMessageHandler),

	// ComputeSharp
	//typeof(AutoConstructorAttribute),
	
	// ComputeSharp.D2D1
	//typeof(D2DCompileOptionsAttribute),
	
	// CSLA
	//typeof(DataPortal<>),
	
	// Moq
	//typeof(Mock<>),
	
	// ImageSharp
	//typeof(GraphicsOptions),
	
	// TerraFX.Interop.D3D12MemoryAllocator
	//typeof(D3D12MA_Allocation),
	
	// TerraFX.Interop.Windows
	//typeof(INativeGuid),
	
	// Microsoft.CodeAnalysis.CSharp
	typeof(SyntaxTree),
}.Select(_ => _.Assembly).ToHashSet();

Console.WriteLine($"Testing {nameof(RockCreateGenerator)}");
TestGenerator.Generate(new RockCreateGenerator(), targetAssemblies);
Console.WriteLine();

Console.WriteLine($"Testing {nameof(RockMakeGenerator)}");
TestGenerator.Generate(new RockMakeGenerator(), targetAssemblies);
Console.WriteLine();

Console.WriteLine("Generator testing complete");