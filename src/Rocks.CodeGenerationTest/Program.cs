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
		typeof(ILGPU.IR.Types.TypeConverter<>)
			.IsValidTarget(new Dictionary<Type, Dictionary<string, string>>
			{
				{
					typeof(ILGPU.IR.Types.TypeConverter<>), new()
					{
						{ "TType", "global::ILGPU.IR.Types.TypeNode" }              
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
			typeof(AutoFixture.Kernel.Criterion<>)
		}, 
		Array.Empty<Type>(), null);

static void TestWithTypes()
{
	var targetAssemblies = new Type[]
	{
#if INCLUDE_PASSING
		// .NET types
		typeof(Dictionary<,>),
		typeof(HttpMessageHandler),
		typeof(object),
		typeof(System.Collections.Immutable.ImmutableArray),
		typeof(System.Text.Json.JsonDocument),
		typeof(System.Threading.Channels.BoundedChannelFullMode),
		// NuGet references
		typeof(AngleSharp.BrowsingContext),
		typeof(Autofac.ContainerBuilder),
		typeof(AutoMapper.AutoMapAttribute),
		typeof(Avalonia.AppBuilder),
		typeof(AWSSDK.Runtime.Internal.Util.ChecksumCRTWrapper),
		typeof(Bogus.Binder),
		typeof(Castle.DynamicProxy.ProxyGenerationOptions),
		typeof(ClangSharp.AbstractConditionalOperator),
		typeof(ComputeSharp.AutoConstructorAttribute),
		typeof(ComputeSharp.D2D1.D2DCompileOptionsAttribute),
		typeof(Csla.DataPortal<>),
		typeof(CsvHelper.ArrayHelper),
		typeof(Dapper.DbString),
		typeof(FluentAssertions.AggregateExceptionExtractor),
		typeof(FluentValidation.ApplyConditionTo),
		typeof(Google.Apis.ETagAction),
		typeof(Google.Protobuf.ByteString),
		typeof(Grpc.Core.AuthContext),
		typeof(ICSharpCode.SharpZipLib.SharpZipBaseException),
		typeof(IdentityModel.Base64Url),
		typeof(ILGPU.ArrayMode),
		typeof(MassTransit.AbstractUriException),
		typeof(MathNet.Numerics.AppSwitches),
		typeof(MediatR.ISender),
		typeof(MessagePack.FormatterNotRegisteredException),
		typeof(Microsoft.CodeAnalysis.SyntaxTree),
		typeof(Microsoft.EntityFrameworkCore.Infrastructure.EntityFrameworkEventSource),
		typeof(Microsoft.Extensions.DependencyInjection.AsyncServiceScope),
		typeof(Microsoft.Extensions.Logging.LogDefineOptions),
		typeof(Mono.Cecil.FixedSysStringMarshalInfo),
		typeof(Moq.Mock<>),
		typeof(NSubstitute.Arg),
		typeof(NuGet.Common.ActivityCorrelationId),
		typeof(Polly.AdvancedCircuitBreakerSyntax),
		typeof(RestSharp.BodyParameter),
		typeof(Serilog.Core.IDestructuringPolicy),
		typeof(Sigil.CatchBlock),
		typeof(Silk.NET.Core.Attributes.CountAttribute),
		typeof(SixLabors.ImageSharp.GraphicsOptions),
		typeof(SkiaSharp.GRBackend),
		typeof(StackExchange.Redis.Aggregate),
		typeof(System.Reactive.ExperimentalAttribute),
		typeof(System.Reflection.Metadata.ArrayShape),
		typeof(TerraFX.Interop.DirectX.D3D12MA_Allocation),
		typeof(TerraFX.Interop.INativeGuid),
#endif
#if INCLUDE_FAILING
		typeof(Hangfire.AttemptsExceededAction),
		//typeof(AutoFixture.AutoPropertiesTarget),
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