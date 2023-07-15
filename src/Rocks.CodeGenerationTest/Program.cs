//#define INCLUDE_PASSING
#define INCLUDE_FAILING

using Microsoft.CodeAnalysis;
using Rocks;
using Rocks.CodeGenerationTest;
using Rocks.CodeGenerationTest.Extensions;
using Rocks.CodeGenerationTest.Mappings;

//TestTypeValidity();
//TestWithCode();
//TestWithType();
TestWithTypes();

#pragma warning disable CS8321 // Local function is declared but never used
static void TestTypeValidity() =>
	Console.WriteLine(
		typeof(ComputeSharp.D2D1.Interop.D2D1TransformMapper<>)
			.IsValidTarget(new Dictionary<Type, Dictionary<string, string>>
			{
				{
					typeof(ComputeSharp.D2D1.Interop.D2D1TransformMapper<>), new()
					{
						{ "T", "global::Rocks.CodeGenerationTest.Mappings.ComputeSharp.MappedPixelShader" }
					} 
				}
			}));

static void TestWithCode()
{
	TestGenerator.Generate(new RockCreateGenerator(),
		"""
		using Rocks;
		using System;
		using SkiaSharp;

		public static class Test
		{
		    public static void Go()
		    {
		        var expectations = Rock.Create<SKRegion>();
		    }
		}
		""",
		new[]
		{
			typeof(SkiaSharp.SKRegion),
			typeof(Uri),
		});
}

static void TestWithType() =>
	TestGenerator.Generate(new RockCreateGenerator(),
		new[]
		{
			typeof(System.Linq.Expressions.ExpressionVisitor)
		}, 
		new []
		{
			typeof(Func<,>),
			typeof(System.Reflection.PropertyInfo)
		}, null);

static void TestWithTypes()
{
	var targetAssemblies = new Type[]
	{
#if INCLUDE_PASSING
		// PASSED
		// Number of types found: 3852
		typeof(object), typeof(Dictionary<,>),
		typeof(System.Collections.Immutable.ImmutableArray), typeof(HttpMessageHandler),
		typeof(ComputeSharp.AutoConstructorAttribute),
		typeof(ComputeSharp.D2D1.D2DCompileOptionsAttribute),
		typeof(SkiaSharp.GRBackend),
		typeof(ClangSharp.AbstractConditionalOperator),
		typeof(Bogus.Binder),
		typeof(AngleSharp.BrowsingContext),
		typeof(AWSSDK.Runtime.Internal.Util.ChecksumCRTWrapper),
		typeof(Csla.DataPortal<>),
		typeof(System.Text.Json.JsonDocument),
		typeof(Microsoft.CodeAnalysis.SyntaxTree),
		typeof(Microsoft.Extensions.Logging.LogDefineOptions),
		typeof(Mono.Cecil.FixedSysStringMarshalInfo),
		typeof(System.Threading.Channels.BoundedChannelFullMode),
		typeof(Serilog.Core.IDestructuringPolicy),
		typeof(IdentityModel.Base64Url),
		typeof(Google.Protobuf.ByteString),
		typeof(TerraFX.Interop.DirectX.D3D12MA_Allocation),
		typeof(TerraFX.Interop.INativeGuid),
		typeof(ICSharpCode.SharpZipLib.SharpZipBaseException),
		typeof(MediatR.ISender),
		typeof(NSubstitute.Arg),
		typeof(StackExchange.Redis.Aggregate),
		typeof(FluentAssertions.AggregateExceptionExtractor),
		typeof(Microsoft.Extensions.DependencyInjection.AsyncServiceScope),
		typeof(Castle.DynamicProxy.ProxyGenerationOptions),
		typeof(Polly.AdvancedCircuitBreakerSyntax),
		typeof(Moq.Mock<>),
		typeof(System.Reactive.ExperimentalAttribute),
		typeof(RestSharp.BodyParameter),
		typeof(CsvHelper.ArrayHelper),
		typeof(SixLabors.ImageSharp.GraphicsOptions),
		typeof(AutoMapper.AutoMapAttribute),
		typeof(Silk.NET.Core.Attributes.CountAttribute),
		typeof(MassTransit.AbstractUriException),
		typeof(System.Reflection.Metadata.ArrayShape),
		typeof(ILGPU.ArrayMode),
		typeof(AutoMapper.AutoMapAttribute),
#endif
#if INCLUDE_FAILING
		// Number of types found: 747
		// Create: 319 errors, 0 warnings
		// Make: 38 errors, 0 warnings
		// EntityFramework
		typeof(Microsoft.EntityFrameworkCore.Infrastructure.EntityFrameworkEventSource),
#endif
	}.Select(_ => _.Assembly).ToHashSet();

	var typesToLoadAssembliesFrom = new Type[]
	{
		typeof(System.Linq.Expressions.LambdaExpression),
		typeof(System.Net.Mail.MailMessage),
		typeof(Microsoft.Extensions.Caching.Memory.IMemoryCache),
		typeof(System.Diagnostics.DiagnosticSource),
		typeof(System.Transactions.Transaction),
		typeof(System.Text.Json.JsonSerializerOptions),
		typeof(Uri),
		typeof(Func<>),
		typeof(System.Reflection.ConstructorInfo)
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