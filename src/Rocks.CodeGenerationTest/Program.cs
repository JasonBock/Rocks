using Rocks;
using Rocks.CodeGenerationTest;

//TestWithCode();
//TestWithType();
TestWithTypes();

#pragma warning disable CS8321 // Local function is declared but never used
static void TestWithCode()
{
	TestGenerator.Generate(new RockCreateGenerator(),
		"""
		using Rocks;
		using System;
		using Google.Protobuf;

		public interface IExtendable : IExtendableMessage<IExtendable> { }
		
		public static class Test
		{
			public static void Go()
			{
				var expectations = Rock.Create<IExtendable>();
			}
		}
		""", typeof(Google.Protobuf.IExtendableMessage<>));
}

static void TestWithType() =>
	 TestGenerator.Generate(new RockCreateGenerator(), 
	 new[] 
	 { 
		 typeof(RestSharp.RestRequest)
	 });

static void TestWithTypes()
{
	var targetAssemblies = new Type[]
	{
		// Core .NET types
		//typeof(object), typeof(Dictionary<,>),
		//typeof(System.Collections.Immutable.ImmutableArray), typeof(HttpMessageHandler),

		// ComputeSharp
		//typeof(ComputeSharp.AutoConstructorAttribute),

		// ComputeSharp.D2D1
		// ID2D1TransformMapperFactory will fail because it needs a struct 
		// that can be unmanaged and implement ID2D1PixelShader. If that's
		// done, then it works just fine.
		//typeof(ComputeSharp.D2D1.D2DCompileOptionsAttribute),

		// CSLA
		//typeof(Csla.DataPortal<>),

		// Moq
		//typeof(Moq.Mock<>),

		// ImageSharp
		//typeof(SixLabors.ImageSharp.GraphicsOptions),

		// System.Text.Json
		//typeof(System.Text.Json.JsonDocument),

		// Microsoft.Extensions.DependencyInjection
		//typeof(Microsoft.Extensions.DependencyInjection.AsyncServiceScope),

		// Microsoft.CodeAnalysis.CSharp
		//typeof(Microsoft.CodeAnalysis.SyntaxTree),

		// Microsoft.Extensions.Logging
		//typeof(Microsoft.Extensions.Logging.LogDefineOptions),

		// Castle.Core
		//typeof(Castle.DynamicProxy.ProxyGenerationOptions),

		// Mono.Cecil
		//typeof(Mono.Cecil.FixedSysStringMarshalInfo),

		// AutoMapper
		//typeof(AutoMapper.AutoMapAttribute),

		// NUnit
		//typeof(NUnit.Framework.TestCaseAttribute),

		// System.Threading.Channels
		//typeof(System.Threading.Channels.BoundedChannelFullMode),

		// Serilog
		//typeof(Serilog.Core.IDestructuringPolicy),

		// Polly
		//typeof(Polly.AdvancedCircuitBreakerSyntax),

		// EntityFramework
		//typeof(Microsoft.EntityFrameworkCore.Infrastructure.EntityFrameworkEventSource),

		// FluentAssertions
		//typeof(FluentAssertions.AggregateExceptionExtractor),

		// StackExchange.Redis
		//typeof(StackExchange.Redis.Aggregate),

		// RestSharp
		//typeof(RestSharp.BodyParameter),

		// CsvHelper
		//typeof(CsvHelper.ArrayHelper),

		// IdentityModel
		//typeof(IdentityModel.Base64Url),

		// Google.Protobuf
		//typeof(Google.Protobuf.ByteString),

		// TODO: Just referencing this makes a bug.
		// TerraFX.Interop.D3D12MemoryAllocator
		typeof(TerraFX.Interop.DirectX.D3D12MA_Allocation),

		// TerraFX.Interop.Windows
		typeof(TerraFX.Interop.INativeGuid),

		// Antlr
		//typeof(ICSharpCode.SharpZipLib.SharpZipBaseException),

		// TODO: Azure.Identity, SharpZipLib, MediatR, System.Reactive, 
		// NSubstitute, AWSSDK.Core, AngleSharp, MassTransit, Bogus, SkiaSharp,
		// ClangSharp, LLVMSharp, Silk.NET, System.Reflection.Metadata
	}.Select(_ => _.Assembly).ToHashSet();

	var typesToLoadAssembliesFrom = new Type[]
	{
	};

	Console.WriteLine($"Testing {nameof(RockCreateGenerator)}");
	TestGenerator.Generate(new RockCreateGenerator(), targetAssemblies, typesToLoadAssembliesFrom);
	Console.WriteLine();

	Console.WriteLine($"Testing {nameof(RockMakeGenerator)}");
	TestGenerator.Generate(new RockMakeGenerator(), targetAssemblies, typesToLoadAssembliesFrom);
	Console.WriteLine();

	Console.WriteLine("Generator testing complete");
}
#pragma warning restore CS8321 // Local function is declared but never used