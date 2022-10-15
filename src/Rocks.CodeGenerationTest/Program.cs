using Rocks;
using Rocks.CodeGenerationTest;
using Rocks.CodeGenerationTest.Mappings;

//TestWithCode();
//TestWithType();
TestWithTypes();

#pragma warning disable CS8321 // Local function is declared but never used
static void TestWithCode()
{
	TestGenerator.Generate(new RockCreateGenerator(),
		"""
		using Microsoft.EntityFrameworkCore.Metadata;
		using Rocks;
		using System;

		public static class Test
		{
			public static void Go()
			{
				var expectations = Rock.Create<INavigation>();
			}
		}
		""",
		new[] { typeof(Microsoft.EntityFrameworkCore.Metadata.INavigation) });
}


static void TestWithType() =>
	 TestGenerator.Generate(new RockCreateGenerator(), 
	 new[] 
	 { 
		 typeof(IGrouping<string, Serilog.Parsing.PropertyToken>)
	 }, Array.Empty<Type>(), null);

static void TestWithTypes()
{
	var targetAssemblies = new Type[]
	{
		// Core .NET types
		//typeof(object), typeof(Dictionary<,>),
		//typeof(System.Collections.Immutable.ImmutableArray), typeof(HttpMessageHandler),

		// CSLA
		//typeof(Csla.DataPortal<>),

		// ComputeSharp
		//typeof(ComputeSharp.AutoConstructorAttribute),

		// ComputeSharp.D2D1
		// ID2D1TransformMapperFactory will fail because it needs a struct 
		// that can be unmanaged and implement ID2D1PixelShader. If that's
		// done, then it works just fine.
		//typeof(ComputeSharp.D2D1.D2DCompileOptionsAttribute),

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
		typeof(AutoMapper.AutoMapAttribute),

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

		// TerraFX.Interop.DirectX.D3D12MA_Allocation
		//typeof(TerraFX.Interop.DirectX.D3D12MA_Allocation),

		// TerraFX.Interop.Windows
		//typeof(TerraFX.Interop.INativeGuid),

		// SharpZipLib
		//typeof(ICSharpCode.SharpZipLib.SharpZipBaseException),

		// MediatR
		//typeof(MediatR.ISender),

		// System.Reactive
		//typeof(System.Reactive.ExperimentalAttribute),

		// NSubstitute
		//typeof(NSubstitute.Arg),

		// TODO: Azure.Identity, NSubstitute, AWSSDK.Core, AngleSharp, MassTransit, Bogus, SkiaSharp,
		// ClangSharp, LLVMSharp, Silk.NET, System.Reflection.Metadata
	}.Select(_ => _.Assembly).ToHashSet();

	var typesToLoadAssembliesFrom = new Type[]
	{
		typeof(System.Linq.Expressions.LambdaExpression),
		typeof(System.Net.Mail.MailMessage),
	};

	var genericTypeMappings = MappedTypes.GetMappedTypes();

	Console.WriteLine($"Testing {nameof(RockCreateGenerator)}");
	TestGenerator.Generate(new RockCreateGenerator(), targetAssemblies, typesToLoadAssembliesFrom, genericTypeMappings);
	Console.WriteLine();

	Console.WriteLine($"Testing {nameof(RockMakeGenerator)}");
	TestGenerator.Generate(new RockMakeGenerator(), targetAssemblies, typesToLoadAssembliesFrom, genericTypeMappings);
	Console.WriteLine();

	Console.WriteLine("Generator testing complete");
}
#pragma warning restore CS8321 // Local function is declared but never used