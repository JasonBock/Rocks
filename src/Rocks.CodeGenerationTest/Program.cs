#define INCLUDE_PASSED

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
		using Rocks;
		using System;

		public static class Test
		{
			public static void Go()
			{
				var expectations = Rock.Create<SixLabors.ImageSharp.Processing.Processors.Dithering.IDither>();
			}
		}
		""",
		new[]
		{
			typeof(SixLabors.ImageSharp.Processing.Processors.Dithering.IDither),
		});
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
		// TODO: AWSSDK.Core, AngleSharp, MassTransit, Bogus, SkiaSharp,
		// ClangSharp, LLVMSharp, Silk.NET, System.Reflection.Metadata

		// PASSED
#if INCLUDE_PASSED
		// Number of types found: 373
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// Core .NET types
		typeof(object), typeof(Dictionary<,>),
		typeof(System.Collections.Immutable.ImmutableArray), typeof(HttpMessageHandler),

		// Number of types found: 373
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// CSLA
		typeof(Csla.DataPortal<>),

		// Number of types found: 0
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// ComputeSharp
		typeof(ComputeSharp.AutoConstructorAttribute),

		// Number of types found: 3
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// ComputeSharp.D2D1
		typeof(ComputeSharp.D2D1.D2DCompileOptionsAttribute),

		// Number of types found: 15
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// System.Text.Json
		typeof(System.Text.Json.JsonDocument),

		// Number of types found: 204
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// Microsoft.CodeAnalysis.CSharp
		typeof(Microsoft.CodeAnalysis.SyntaxTree),

		// Number of types found: 11
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// Microsoft.Extensions.Logging
		typeof(Microsoft.Extensions.Logging.LogDefineOptions),

		// Number of types found: 38
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// Mono.Cecil
		typeof(Mono.Cecil.FixedSysStringMarshalInfo),

		// Number of types found: 303
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// NUnit
		typeof(NUnit.Framework.TestCaseAttribute),

		// Number of types found: 6
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// System.Threading.Channels
		typeof(System.Threading.Channels.BoundedChannelFullMode),

		// Number of types found: 28
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// Serilog
		typeof(Serilog.Core.IDestructuringPolicy),

		// Number of types found: 52
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// IdentityModel
		typeof(IdentityModel.Base64Url),

		// Number of types found: 9
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// Google.Protobuf
		typeof(Google.Protobuf.ByteString),

		// Number of types found: 0
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// TerraFX.Interop.DirectX.D3D12MA_Allocation
		typeof(TerraFX.Interop.DirectX.D3D12MA_Allocation),

		// Number of types found: 0
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// TerraFX.Interop.Windows
		typeof(TerraFX.Interop.INativeGuid),

		// Number of types found: 75
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// SharpZipLib
		typeof(ICSharpCode.SharpZipLib.SharpZipBaseException),

		// Number of types found: 37
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// MediatR
		typeof(MediatR.ISender),

		// Number of types found: 174
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// NSubstitute
		typeof(NSubstitute.Arg),

		// Number of types found: 23
		// Create: 0 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// StackExchange.Redis
		typeof(StackExchange.Redis.Aggregate),

		// Number of types found: 213
		// Create: 0 errors, 0 warnings
		// Make: 3 errors, 0 warnings
		// FluentAssertions
		typeof(FluentAssertions.AggregateExceptionExtractor),

		// Number of types found: 9
		// Create: 0 errors, 0 warnings
		// Make: 3 errors, 0 warnings
		// Microsoft.Extensions.DependencyInjection
		typeof(Microsoft.Extensions.DependencyInjection.AsyncServiceScope),

		// Number of types found: 212
		// Create: 0 errors, 0 warnings
		// Make: 3 errors, 0 warnings
		// Castle.Core
		typeof(Castle.DynamicProxy.ProxyGenerationOptions),

		// Number of types found: 63
		// Create: 0 errors, 0 warnings
		// Make: 9 errors, 0 warnings
		// Polly
		typeof(Polly.AdvancedCircuitBreakerSyntax),

		// Number of types found: 43
		// Create: 2 errors, 0 warnings
		// Make: 1 errors, 0 warnings
		// Moq
		typeof(Moq.Mock<>),

		// Number of types found: 47
		// Create: 1 errors, 0 warnings
		// Make: 1 errors, 0 warnings
		// System.Reactive
		typeof(System.Reactive.ExperimentalAttribute),
#endif

		// FAILED

		// Number of types found: 43
		// Create: 6 errors, 0 warnings
		// Make: 0 errors, 0 warnings
		// ImageSharp
		//typeof(SixLabors.ImageSharp.GraphicsOptions),

		// Number of types found: 118
		// Create: 2 errors, 3 warnings
		// Make: 2 errors, 3 warnings
		// AutoMapper
		//typeof(AutoMapper.AutoMapAttribute),

		// Number of types found: 748
		// Create: 364 errors, 20 warnings
		// Make: 51 errors, 16 warnings
		// EntityFramework
		//typeof(Microsoft.EntityFrameworkCore.Infrastructure.EntityFrameworkEventSource),

		// Number of types found: 41
		// Create: 2 errors, 0 warnings
		// Make: 2 errors, 0 warnings
		// RestSharp
		//typeof(RestSharp.BodyParameter),

		// Number of types found: 144
		// Create: 7 errors, 0 warnings
		// Make: 7 errors, 0 warnings
		// CsvHelper
		//typeof(CsvHelper.ArrayHelper),
	}.Select(_ => _.Assembly).ToHashSet();

	var typesToLoadAssembliesFrom = new Type[]
	{
typeof(System.Linq.Expressions.LambdaExpression),
typeof(System.Net.Mail.MailMessage),
typeof(Microsoft.Extensions.Caching.Memory.IMemoryCache),
typeof(System.Diagnostics.DiagnosticSource),
typeof(System.Transactions.Transaction),
typeof(System.Text.Json.JsonSerializerOptions),
typeof(System.Uri),
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