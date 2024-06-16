#define INCLUDE_PASSING
//#define INCLUDE_FAILING

using Microsoft.CodeAnalysis;
using Rocks;
using Rocks.CodeGenerationTest;
using Rocks.CodeGenerationTest.Extensions;
using Rocks.CodeGenerationTest.Mappings;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;

var stopwatch = Stopwatch.StartNew();

//TestTypeValidity();
//TestWithCode();
//TestWithType();
//TestWithTypeNoEmit();
TestWithTypes();
//TestTypesIndividually();

stopwatch.Stop();

Console.WriteLine($"Total time: {stopwatch.Elapsed}");

#pragma warning disable CS8321 // Local function is declared but never used
static void TestTypeValidity() =>
	Console.WriteLine(
		typeof(SixLabors.ImageSharp.ImageFormatException)
			.IsValidTarget([], null));

static void TestWithCode()
{
	TestGenerator.Generate(new RockAttributeGenerator(),
		"""
		using Rocks;
		using System;
		
		[assembly: RockCreate<Generator>]

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
		""",
		[]);
}

static void TestWithType()
{
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

#pragma warning disable EF1001 // Internal EF Core API usage.
	(var issues, var times) = TestGenerator.Generate(new RockAttributeGenerator(),
		[typeof(Proto.Deduplication.DeduplicationContext<>)],
		typesToLoadAssembliesFrom,
		MappedTypes.GetMappedTypes(),
		[], BuildType.Make);
#pragma warning restore EF1001 // Internal EF Core API usage.

	Console.WriteLine($"Generation Time: {times.GeneratorTime}, Emit Time: {times.EmitTime}");
	PrintIssues(issues);
}

static void TestWithTypeNoEmit()
{
	for (var i = 0; i < 50; i++)
	{
		(var issues, var times) = TestGenerator.GenerateNoEmit(new RockAttributeGenerator(),
			[typeof(AngleSharp.Svg.Dom.ISvgSvgElement)],
			[],
			[],
			[], BuildType.Create);

		Console.WriteLine($"Generation Time: {times.GeneratorTime}, Emit Time: {times.EmitTime}");
		PrintIssues(issues);
	}
}

static void TestWithTypes()
{
	Console.WriteLine($"Creating type alias mappings...");

	var targetMappings = new TypeAliasesMapping[]
	{
#if INCLUDE_PASSING
		// .NET types
		new (typeof(object), []),
		new (typeof(System.Collections.Generic.IList<>), []),
		new (typeof(System.Collections.Generic.Dictionary<,>), []),
		new (typeof(System.Net.Http.HttpMessageHandler), []),
		new (typeof(System.Collections.Immutable.ImmutableArray), []),
		new (typeof(System.Text.Json.JsonDocument), []),
		new (typeof(System.Threading.Channels.BoundedChannelFullMode), []),

		// NuGet references
		new (typeof(AngleSharp.BrowsingContext), []),
		new (typeof(Ardalis.GuardClauses.Guard), []),
		new (typeof(Aspire.Hosting.ContainerResourceBuilderExtensions), []),
		new (typeof(Aspose.Email.AlternateView), ["AsposeEmailAlias"]),
		new (typeof(Autofac.ContainerBuilder), []),
		new (typeof(AutoFixture.AutoPropertiesTarget), []),
		new (typeof(AutoMapper.AutoMapAttribute), []),
		new (typeof(Avalonia.AppBuilder), []),
		new (typeof(AWSSDK.Runtime.Internal.Util.ChecksumCRTWrapper), []),
		new (typeof(Azure.Core.AccessToken), []),
		new (typeof(Azure.Messaging.ServiceBus.CreateMessageBatchOptions), []),
		new (typeof(Azure.Storage.Blobs.BlobClient), []),
		new (typeof(Azure.Storage.Queues.QueueClient), []),
		new (typeof(BenchmarkDotNet.Analysers.AnalyserBase), []),
		new (typeof(Blazored.LocalStorage.ChangedEventArgs), []),
		new (typeof(Blazored.Video.BlazoredVideo), []),
		new (typeof(Bogus.Binder), []),
		new (typeof(Castle.DynamicProxy.ProxyGenerationOptions), []),
		new (typeof(ClangSharp.AbstractConditionalOperator), []),
		new (typeof(Confluent.Kafka.Acks), []),
		new (typeof(Coravel.CacheServiceRegistration), []),
		new (typeof(Csla.DataPortal<>), []),
		new (typeof(CsvHelper.ArrayHelper), []),
		new (typeof(Cursively.CsvAsyncInput), []),
		new (typeof(Dapper.DbString), []),
		new (typeof(DiffEngine.BuildServerDetector), []),
		new (typeof(DnsClient.DnsDatagramReader), ["DnsClientAlias"]),
		new (typeof(DSharpPlus.AnsiColor), []),
		new (typeof(Elastic.Clients.Elasticsearch.BlockingSubscribeExtensions), []),
		new (typeof(EntityFramework.Exceptions.Common.CannotInsertNullException), []),
		new (typeof(FluentAssertions.AggregateExceptionExtractor),  []),
		new (typeof(FluentValidation.ApplyConditionTo), []),
		new (typeof(Flurl.GeneratedExtensions), []),
		new (typeof(Google.Apis.ETagAction), []),
		new (typeof(GraphQL.AllowAnonymousAttribute), []),
		new (typeof(Grpc.Core.AuthContext), []),
		new (typeof(HandlebarsDotNet.Arguments), []),
		new (typeof(HarmonyLib.AccessTools), []),
		new (typeof(Humanizer.ByteSizeExtensions), []),
		new (typeof(ICSharpCode.SharpZipLib.SharpZipBaseException), []),
		new (typeof(IdentityModel.Base64Url), []),
		new (typeof(LanguageExt.FuncExtensions), []),
		new (typeof(LLVMSharp.AddrSpaceCastInst), []),
		new (typeof(MassTransit.AbstractUriException), []),
		new (typeof(MathNet.Numerics.AppSwitches), []),
		new (typeof(MediatR.ISender), []),
		new (typeof(MessagePack.FormatterNotRegisteredException), []),
		new (typeof(Microsoft.CodeAnalysis.SyntaxTree), []),
		new (typeof(Microsoft.EntityFrameworkCore.Infrastructure.EntityFrameworkEventSource), []),
		new (typeof(Microsoft.Extensions.DependencyInjection.AsyncServiceScope), []),
		new (typeof(Microsoft.Extensions.Logging.LogDefineOptions), []),
		new (typeof(Microsoft.Extensions.ServiceDiscovery.ConfigurationServiceEndpointProviderOptions), []),
		new (typeof(Microsoft.FluentUI.AspNetCore.Components.AccordionChangeEventArgs), []),
		new (typeof(Microsoft.Kiota.Abstractions.ApiClientBuilder), []),
		new (typeof(Microsoft.OpenApi.Any.AnyType), []),
		new (typeof(Microsoft.Quantum.AmplitudeAmplification.AmpAmpByOracle), []),
		new (typeof(Mono.Cecil.FixedSysStringMarshalInfo), []),
		new (typeof(Mscc.GenerativeAI.BlockedError), []),
		new (typeof(NetFabric.EnumerableExtensions), []),
		new (typeof(nietras.SeparatedValues.SepReaderExtensions), []),
		new (typeof(Ninject.ActivationException), []),
		new (typeof(NodaTime.AmbiguousTimeException), []),
		new (typeof(NuGet.Common.ActivityCorrelationId), []),
		new (typeof(Orleans.Grain), []),
		new (typeof(Paramore.Brighter.Channel), []),
		new (typeof(Proto.ActorContextDecorator), []),
		new (typeof(Pulumi.Alias), []),
		new (typeof(Quartz.AdoProviderExtensions), []),
		new (typeof(R3.CancellationDisposable), []),
		new (typeof(RabbitMQ.Client.AmqpTcpEndpoint), []),
		new (typeof(RecordParser.Builders.Reader.FixedLengthReaderBuilder<>), []),
		new (typeof(Refit.AliasAsAttribute), []),
		new (typeof(RestSharp.BodyParameter), []),
		new (typeof(Serilog.Core.IDestructuringPolicy), []),
		new (typeof(ServiceStack.ActionExecExtensions), []),
		new (typeof(Sigil.CatchBlock), []),
		new (typeof(Silk.NET.Core.Attributes.CountAttribute), []),
		new (typeof(SimpleInjector.ActivationException), []),
		new (typeof(SixLabors.ImageSharp.GraphicsOptions), []),
		new (typeof(SkiaSharp.GRBackend), []),
		new (typeof(StackExchange.Redis.Aggregate), []),
		new (typeof(Stateless.FiringMode), []),
		new (typeof(Stripe.Account), []),
		new (typeof(Sylvan.Data.Csv.AmbiguousColumnException), []),
		new (typeof(System.IO.Abstractions.DirectoryAclExtensions), []),
		new (typeof(System.Numerics.Tensors.TensorPrimitives), []),
		new (typeof(System.Reactive.ExperimentalAttribute), []),
		new (typeof(System.Reflection.Metadata.ArrayShape), []),
		new (typeof(System.Text.Json.JsonCommentHandling), []),
		new (typeof(TerraFX.Interop.DirectX.D3D12MA_Allocation), []),
		new (typeof(TerraFX.Interop.INativeGuid), []),
		new (typeof(Topshelf.Credentials), []),
		new (typeof(Twilio.Base.Page<>), []),
		new (typeof(VerifyTests.AsStringResult), []),
		new (typeof(Wasmtime.ActionResult), []),
		new (typeof(Wisp.IByteReader), []),
#endif
#if INCLUDE_FAILING
#endif
   };

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

	Console.WriteLine($"Getting mapped types...");
	var genericTypeMappings = MappedTypes.GetMappedTypes();
	var totalDiscoveredTypeCount = 0;
	var issues = new List<Issue>();

	var visitedAssemblies = new HashSet<Assembly>();

	foreach (var targetMapping in targetMappings)
	{
		if (visitedAssemblies.Add(targetMapping.type.Assembly))
		{
			Console.WriteLine($"Getting target types for {targetMapping.type.Assembly.GetName().Name}");
			var targetAssemblySet = new HashSet<Assembly> { targetMapping.type.Assembly };

			var discoveredTypes = TestGenerator.GetTargets(targetAssemblySet, [], typesToLoadAssembliesFrom,
				genericTypeMappings, targetMapping.aliases);

			Console.WriteLine($"Testing {targetMapping.type.Assembly.GetName().Name} - {BuildType.Create}");
			(var createIssues, _) = TestGenerator.Generate(
				new RockAttributeGenerator(), discoveredTypes, typesToLoadAssembliesFrom, genericTypeMappings, targetMapping.aliases, BuildType.Create);
			issues.AddRange(createIssues);

			Console.WriteLine($"Testing {targetMapping.type.Assembly.GetName().Name} - {BuildType.Make}");
			(var makeIssues, _) = TestGenerator.Generate(
				new RockAttributeGenerator(), discoveredTypes, typesToLoadAssembliesFrom, genericTypeMappings, targetMapping.aliases, BuildType.Make);
			issues.AddRange(makeIssues);

			totalDiscoveredTypeCount += discoveredTypes.Length;
			Console.WriteLine($"Type count found for {targetMapping.type.Assembly.GetName().Name} - {discoveredTypes.Length}");
			Console.WriteLine();
		}
	}

	Console.WriteLine(
		$$"""
		Generator testing complete.
			Total assembly count is {{visitedAssemblies.Count}}
			Total discovered type count is {{totalDiscoveredTypeCount}}
		""");

	PrintIssues([.. issues]);
}

static void TestTypesIndividually()
{
	var targetMappings = new TypeAliasesMapping[]
	{
		new (typeof(AngleSharp.BrowsingContext), []),
		new (typeof(Microsoft.EntityFrameworkCore.Infrastructure.EntityFrameworkEventSource), []),
	};

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
	var issues = new List<Issue>();

	var visitedAssemblies = new HashSet<Assembly>();

	var typeGenerationTimes = new List<TypeGenerationTime>();

	foreach (var targetMapping in targetMappings)
	{
		if (visitedAssemblies.Add(targetMapping.type.Assembly))
		{
			Console.WriteLine($"Getting target types for {targetMapping.type.Assembly.GetName().Name}");
			var targetAssemblySet = new HashSet<Assembly> { targetMapping.type.Assembly };

			var discoveredTypes = TestGenerator.GetTargets(targetAssemblySet, [], typesToLoadAssembliesFrom,
				genericTypeMappings, targetMapping.aliases);

			foreach (var discoveredType in discoveredTypes)
			{
				Console.WriteLine($"Generating for type {discoveredType.FullName}...");
				(_, var generatorElapsedTime) = TestGenerator.Generate(
					new RockAttributeGenerator(), [discoveredType], typesToLoadAssembliesFrom, genericTypeMappings, targetMapping.aliases, BuildType.Create);
				typeGenerationTimes.Add(new(discoveredType, generatorElapsedTime));
			}
		}
	}

	Console.WriteLine();
	Console.WriteLine("Slowest Generation Times");
	foreach (var typeGenerationTime in typeGenerationTimes.OrderByDescending(_ => _.Times.GeneratorTime).Take(10))
	{
		Console.WriteLine($"Generation Time: {typeGenerationTime.Times.GeneratorTime}, Emit Time: {typeGenerationTime.Times.EmitTime}, Type: {typeGenerationTime.Type.FullName}");
	}

	Console.WriteLine();
	Console.WriteLine("Slowest Emit Times");
	foreach (var typeGenerationTime in typeGenerationTimes.OrderByDescending(_ => _.Times.EmitTime).Take(10))
	{
		Console.WriteLine($"Generation Time: {typeGenerationTime.Times.GeneratorTime}, Emit Time: {typeGenerationTime.Times.EmitTime}, Type: {typeGenerationTime.Type.FullName}");
	}

	Console.WriteLine();
	Console.WriteLine("Fasted Generation Times");
	foreach (var typeGenerationTime in typeGenerationTimes.OrderBy(_ => _.Times.GeneratorTime).Take(10))
	{
		Console.WriteLine($"Generation Time: {typeGenerationTime.Times.GeneratorTime}, Emit Time: {typeGenerationTime.Times.EmitTime}, Type: {typeGenerationTime.Type.FullName}");
	}

	Console.WriteLine();
	Console.WriteLine("Fasted Emit Times");
	foreach (var typeGenerationTime in typeGenerationTimes.OrderBy(_ => _.Times.EmitTime).Take(10))
	{
		Console.WriteLine($"Generation Time: {typeGenerationTime.Times.GeneratorTime}, Emit Time: {typeGenerationTime.Times.EmitTime}, Type: {typeGenerationTime.Type.FullName}");
	}
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

			foreach (var errorGroup in errors.OrderBy(_ => _.Count()))
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

			var errorCount = 0;

			foreach (var errorGroup in errors.OrderBy(_ => _.Count()))
			{
				var errorGroupCount = errorGroup.Count();
				errorCount += errorGroupCount;
				Console.WriteLine($"\tCode: {errorGroup.Key}, Count: {errorGroupCount}");
			}

			Console.WriteLine($"Total Error Count: {errorCount}");
			Console.WriteLine();
		}

		var warnings = issues
			.Where(_ => _.Severity == DiagnosticSeverity.Warning)
			.GroupBy(_ => _.Id)
			.OrderBy(_ => _.Key).ToArray();

		if (warnings.Length > 0)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;

			Console.WriteLine("Warning Counts");

			foreach (var warningGroup in warnings.OrderBy(_ => _.Count()))
			{
				foreach (var warning in warningGroup)
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

			var warningCount = 0;

			foreach (var warningGroup in warnings.OrderBy(_ => _.Count()))
			{
				var warningGroupCount = warningGroup.Count();
				warningCount += warningGroupCount;
				Console.WriteLine($"\tCode: {warningGroup.Key}, Count: {warningGroupCount}");
			}

			Console.WriteLine($"Total Warning Count: {warningCount}");
			Console.WriteLine();
		}

		Console.ForegroundColor = currentColor;
	}
}