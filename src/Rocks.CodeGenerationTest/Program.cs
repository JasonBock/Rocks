#define INCLUDE_PASSING
//#define INCLUDE_FAILING

using Microsoft.CodeAnalysis;
using Rocks;
using Rocks.CodeGenerationTest;
using Rocks.CodeGenerationTest.Extensions;
using Rocks.CodeGenerationTest.Mappings;
using System.Collections.Immutable;
using System.Reflection;

//TestTypeValidity();
//TestWithCode();
//TestWithType();
TestWithTypes();

#pragma warning disable CS8321 // Local function is declared but never used
static void TestTypeValidity() =>
	Console.WriteLine(
		typeof(SixLabors.ImageSharp.ImageFormatException)
			.IsValidTarget(null));

static void TestWithCode()
{
	TestGenerator.Generate(new RockCreateGenerator(),
		"""
		using Rocks;
		using System;
		
		#nullable enable

		public abstract class GeneratorBase
		{
			protected abstract string GetBuildArtifactsDirectoryPath(string assemblyLocation, string programName);
		}

		public class Generator
			: GeneratorBase
		{
			protected override string GetBuildArtifactsDirectoryPath(string buildPartition, string programName) => "";
		}

		public static class Test
		{
		    public static void Go()
		    {
		        var expectations = Rock.Create<Generator>();
		    }
		}
		""",
		[]);
}

static void TestWithType() =>
	PrintIssues(TestGenerator.Generate(new RockCreateGenerator(),
		[typeof(BenchmarkDotNet.Toolchains.Roslyn.Generator)],
		[],
		[]));

static void TestWithTypes()
{
	var targetAssemblies = new Type[]
	{
#if INCLUDE_PASSING
		//// .NET types
		//typeof(Dictionary<,>),
		//typeof(HttpMessageHandler),
		//typeof(object),
		//typeof(System.Collections.Immutable.ImmutableArray),
		//typeof(System.Text.Json.JsonDocument),
		//typeof(System.Threading.Channels.BoundedChannelFullMode),

		//// NuGet references
		//typeof(AngleSharp.BrowsingContext),
		//typeof(Ardalis.GuardClauses.Guard),
		//typeof(Aspire.Hosting.ContainerResourceBuilderExtensions),
		typeof(Aspose.Email.AlternateView),
		//typeof(Autofac.ContainerBuilder),
		//typeof(AutoFixture.AutoPropertiesTarget),
		//typeof(AutoMapper.AutoMapAttribute),
		//typeof(Avalonia.AppBuilder),
		//typeof(AWSSDK.Runtime.Internal.Util.ChecksumCRTWrapper),
		//typeof(Azure.Core.AccessToken),
		//typeof(Azure.Messaging.ServiceBus.CreateMessageBatchOptions),
		//typeof(Azure.Storage.Blobs.BlobClient),
		//typeof(Azure.Storage.Queues.QueueClient),
		//typeof(BenchmarkDotNet.Analysers.AnalyserBase),
		//typeof(Blazored.LocalStorage.ChangedEventArgs),
		//typeof(Blazored.Video.BlazoredVideo),
		//typeof(Bogus.Binder),
		//typeof(Castle.DynamicProxy.ProxyGenerationOptions),
		//typeof(ClangSharp.AbstractConditionalOperator),
		//typeof(Confluent.Kafka.Acks),
		//typeof(Csla.DataPortal<>),
		//typeof(CsvHelper.ArrayHelper),
		//typeof(Dapper.DbString),
		//typeof(DiffEngine.BuildServerDetector),
		// Commenting this out until this is addressed:
		// https://github.com/JasonBock/Rocks/issues/281
		typeof(DnsClient.DnsDatagramReader),
		//typeof(Elasticsearch.Net.ApiCallDetails),
		//typeof(EntityFramework.Exceptions.Common.CannotInsertNullException),
		//typeof(FluentAssertions.AggregateExceptionExtractor),
		//typeof(FluentValidation.ApplyConditionTo),
		//typeof(Flurl.GeneratedExtensions),
		//typeof(Google.Apis.ETagAction),
		//typeof(GraphQL.AllowAnonymousAttribute),
		//typeof(Grpc.Core.AuthContext),
		//typeof(HandlebarsDotNet.Arguments),
		//typeof(Humanizer.ByteSizeExtensions),
		//typeof(ICSharpCode.SharpZipLib.SharpZipBaseException),
		//typeof(IdentityModel.Base64Url),
		//typeof(LanguageExt.FuncExtensions),
		//typeof(LLVMSharp.AddrSpaceCastInst),
		//typeof(MassTransit.AbstractUriException),
		//typeof(MathNet.Numerics.AppSwitches),
		//typeof(MediatR.ISender),
		//typeof(MessagePack.FormatterNotRegisteredException),
		//typeof(Microsoft.CodeAnalysis.SyntaxTree),
		//typeof(Microsoft.EntityFrameworkCore.Infrastructure.EntityFrameworkEventSource),
		//typeof(Microsoft.Extensions.DependencyInjection.AsyncServiceScope),
		//typeof(Microsoft.Extensions.Logging.LogDefineOptions),
		//typeof(Microsoft.Extensions.ServiceDiscovery.ServiceEndPointResolver),
		//typeof(Microsoft.OpenApi.Any.AnyType),
		//typeof(Microsoft.Quantum.AmplitudeAmplification.AmpAmpByOracle),
		//typeof(Mono.Cecil.FixedSysStringMarshalInfo),
		//typeof(Ninject.ActivationException),
		//typeof(NodaTime.AmbiguousTimeException),
		//typeof(NuGet.Common.ActivityCorrelationId),
		//typeof(Orleans.Grain),
		//typeof(Proto.ActorContextDecorator),
		//typeof(Pulumi.Alias),
		//typeof(Quartz.AdoProviderExtensions),
		//typeof(RabbitMQ.Client.AmqpTcpEndpoint),
		//typeof(Refit.AliasAsAttribute),
		//typeof(RestSharp.BodyParameter),
		//typeof(Serilog.Core.IDestructuringPolicy),
		//typeof(ServiceStack.ActionExecExtensions),
		//typeof(Sigil.CatchBlock),
		//typeof(Silk.NET.Core.Attributes.CountAttribute),
		//typeof(SimpleInjector.ActivationException),
		//typeof(SixLabors.ImageSharp.GraphicsOptions),
		//typeof(SkiaSharp.GRBackend),
		//typeof(StackExchange.Redis.Aggregate),
		//typeof(Stripe.Account),
		//typeof(System.IO.Abstractions.DirectoryAclExtensions),
		//typeof(System.Reactive.ExperimentalAttribute),
		//typeof(System.Reflection.Metadata.ArrayShape),
		//typeof(System.Text.Json.JsonCommentHandling),
		//typeof(TerraFX.Interop.DirectX.D3D12MA_Allocation),
		//typeof(TerraFX.Interop.INativeGuid),
		//typeof(Topshelf.Credentials),
		//typeof(Twilio.Base.Page<>),
		//typeof(Wasmtime.ActionResult),
#endif
#if INCLUDE_FAILING
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
		typeof(System.Reflection.ConstructorInfo),
		typeof(System.Xml.Linq.SaveOptions),
		typeof(Azure.Core.Amqp.AmqpAnnotatedMessage),
	};

	var genericTypeMappings = MappedTypes.GetMappedTypes();
	var totalDiscoveredTypeCount = 0;
	var issues = new List<Issue>();

	foreach (var targetAssembly in targetAssemblies)
	{
		Console.WriteLine($"Getting target types for {targetAssembly.GetName().Name}");
		var targetAssemblySet = new HashSet<Assembly> { targetAssembly };
		var typesToIgnore = Array.Empty<Type>();
		var discoveredTypes = TestGenerator.GetDiscoveredTypes(targetAssemblySet, genericTypeMappings, typesToIgnore);
		totalDiscoveredTypeCount += discoveredTypes.Length;
		Console.WriteLine($"Type count found for {targetAssembly.GetName().Name} - {discoveredTypes.Length}");

		Console.WriteLine($"Testing {targetAssembly.GetName().Name} - {nameof(RockCreateGenerator)}");
		issues.AddRange(TestGenerator.Generate(new RockCreateGenerator(), discoveredTypes, typesToLoadAssembliesFrom, genericTypeMappings));

		Console.WriteLine($"Testing {targetAssembly.GetName().Name} - {nameof(RockMakeGenerator)}");
		issues.AddRange(TestGenerator.Generate(new RockCreateGenerator(), discoveredTypes, typesToLoadAssembliesFrom, genericTypeMappings));

		Console.WriteLine();
	}

	Console.WriteLine(
		$$"""
		Generator testing complete.
			Total assembly count is {{targetAssemblies.Count}}
			Total discovered type count is {{totalDiscoveredTypeCount}}
		""");

	PrintIssues(issues.ToImmutableArray());
}

static void PrintIssues(ImmutableArray<Issue> issues)
{
	if (issues.Length > 0)
	{
		var currentColor = Console.ForegroundColor;

		var errors = issues
			.Where(_ => _.Severity == DiagnosticSeverity.Error)
			.GroupBy(_ => _.Id)
			.OrderBy(_ => _.Key).ToArray();

		if (errors.Length > 0)
		{
			Console.ForegroundColor = ConsoleColor.Red;

			Console.WriteLine("Error Counts");

			var errorCount = 0;

			foreach (var errorGroup in errors)
			{
				var errorGroupCount = errorGroup.Count();
				errorCount += errorGroupCount;
				Console.WriteLine($"\tCode: {errorGroup.Key}, Count: {errorGroupCount}");
			}

			Console.WriteLine($"Total Error Count: {errorCount}");
			Console.WriteLine();

			foreach (var errorGroup in errors)
			{
				foreach (var error in errorGroup)
				{
					var errorCode = error.Location.SourceTree?.GetText().GetSubText(error.Location.SourceSpan) ?? null;
					Console.WriteLine(
						$$"""
						Error:

						ID: {{error.Id}}
						Description: {{error.Description}}
						Code:
						{{errorCode}}

						""");
				}
			}
		}

		var warnings = issues
			.Where(_ => _.Severity == DiagnosticSeverity.Warning)
			.GroupBy(_ => _.Id)
			.OrderBy(_ => _.Key).ToArray();

		if (warnings.Length > 0)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;

			Console.WriteLine("Warning Counts");

			var warningCount = 0;

			foreach (var warningGroup in warnings)
			{
				var warningGroupCount = warningGroup.Count();
				warningCount += warningGroupCount;
				Console.WriteLine($"\tCode: {warningGroup.Key}, Count: {warningGroupCount}");
			}

			Console.WriteLine($"Total Warning Count: {warningCount}");
			Console.WriteLine();

			foreach (var warningGrouip in warnings)
			{
				foreach (var warning in warningGrouip)
				{
					var warningCode = warning.Location.SourceTree?.GetText().GetSubText(warning.Location.SourceSpan) ?? null;
					Console.WriteLine(
						$$"""
						Warning:

						ID: {{warning.Id}}
						Description: {{warning.Description}}
						Code:
						{{warningCode}}

						""");
				}
			}
		}

		Console.ForegroundColor = currentColor;
	}
}
#pragma warning restore CS8321 // Local function is declared but never used