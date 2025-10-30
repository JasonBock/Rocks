#define INCLUDE_PASSING
//#define INCLUDE_FAILING

using DotNet.Testcontainers.Containers;
using Microsoft.CodeAnalysis;
using R3;
using Rocks;
using Rocks.Analysis;
using Rocks.CodeGenerationTest;
using Rocks.CodeGenerationTest.Extensions;
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
			.IsValidTarget([]));

static void TestWithCode()
{
	var typesToLoadAssembliesFrom = new Type[]
	{
		typeof(System.Linq.Expressions.LambdaExpression),
	};

	TestGenerator.Generate(new RockGenerator(),
		"""
		#nullable enable

		using Rocks;
		using System;
				
		[assembly: Rock(typeof(MonadIO<>), BuildType.Create)]

		public interface K<in F, A> { }

		public class IO { }

		public interface MonadIO<M> where M : MonadIO<M>
		{
			static K<M, A> LiftIO<A>(K<IO, A> ma) => null!;
		}
		""",
		typesToLoadAssembliesFrom);
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
		typeof(Rocks.Arg),
	}.ToImmutableArray();

#pragma warning disable EF1001 // Internal EF Core API usage.
#pragma warning disable EF9100 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
	(var issues, var times) = TestGenerator.Generate(new RockGenerator(),
		[typeof(Autofac.Core.IActivatedEventArgs<>)],
		typesToLoadAssembliesFrom,
		[], Rocks.Analysis.BuildType.Create);
#pragma warning restore EF9100 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning restore EF1001 // Internal EF Core API usage.

	Console.WriteLine($"Generation Time: {times.GeneratorTime}, Emit Time: {times.EmitTime}");
	PrintIssues(issues);
}

static void TestWithTypeNoEmit()
{
	for (var i = 0; i < 50; i++)
	{
		(var issues, var times) = TestGenerator.GenerateNoEmit(new RockGenerator(),
			[typeof(AngleSharp.Svg.Dom.ISvgSvgElement)],
			[], [], Rocks.Analysis.BuildType.Create);

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
		new (typeof(Alba.AlbaHost), []),
		new (typeof(AngleSharp.BrowsingContext), []),
		new (typeof(Arch.Buffer.CommandBuffer), []),
		new (typeof(Ardalis.GuardClauses.Guard), []),
		new (typeof(AsmResolver.ByteArrayEqualityComparer), []),
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
		new (typeof(CoreWCF.ActionNotSupportedException), []),
		new (typeof(Csla.DataPortal<>), []),
		new (typeof(CsvHelper.ArrayHelper), []),
		new (typeof(Cursively.CsvAsyncInput), []),
		new (typeof(Dapper.DbString), []),
		new (typeof(DiffEngine.BuildServerDetector), []),
		new (typeof(DnsClient.DnsDatagramReader), ["DnsClientAlias"]),
		new (typeof(DotNext.BasicExtensions), []),
		new (typeof(DotNext.Threading.AsyncAutoResetEvent), []),
		new (typeof(DotNext.IO.DataTransferObject), []),
		new (typeof(DotNext.Metaprogramming.AsyncLambdaFlags), []),
		new (typeof(DSharpPlus.AnsiColor), []),
		new (typeof(DynamicData.ChangeReason), []),
		new (typeof(EfficientDynamoDb.Batch), []),
		new (typeof(Elastic.Clients.Elasticsearch.BlockingSubscribeExtensions), []),
		new (typeof(EntityFramework.Exceptions.Common.CannotInsertNullException), []),
		new (typeof(FluentValidation.ApplyConditionTo), []),
		new (typeof(Flurl.GeneratedExtensions), []),
		new (typeof(Ganss.Xss.HtmlSanitizer), []),
		new (typeof(Garnet.CommandLineBooleanOption), []),
		new (typeof(Google.Apis.ETagAction), []),
		new (typeof(GraphQL.AllowAnonymousAttribute), []),
		new (typeof(Grpc.Core.AuthContext), []),
		new (typeof(HandlebarsDotNet.Arguments), []),
		new (typeof(HarmonyLib.AccessTools), []),
		new (typeof(Humanizer.ByteSizeExtensions), []),
		new (typeof(ICSharpCode.SharpZipLib.SharpZipBaseException), []),
		new (typeof(IdentityModel.Base64Url), []),
		new (typeof(LanguageExt.FuncExtensions), []),
		new (typeof(Lifti.ChildNodeMap), []),
		new (typeof(LLVMSharp.AddrSpaceCastInst), []),
		new (typeof(MassTransit.AbstractUriException), []),
		new (typeof(MathNet.Numerics.AppSwitches), []),
		new (typeof(MediatR.ISender), []),
		new (typeof(MessagePack.FormatterNotRegisteredException), []),
		new (typeof(Microsoft.CodeAnalysis.SyntaxTree), []),
		new (typeof(Microsoft.EntityFrameworkCore.Infrastructure.AccessorExtensions), []),
		new (typeof(Microsoft.Extensions.DependencyInjection.AsyncServiceScope), []),
		new (typeof(Microsoft.Extensions.Logging.LogDefineOptions), []),
		new (typeof(Microsoft.Extensions.ServiceDiscovery.ConfigurationServiceEndpointProviderOptions), []),
		new (typeof(Microsoft.FluentUI.AspNetCore.Components.AccordionChangeEventArgs), []),
		new (typeof(Microsoft.Identity.Client.AadAuthorityAudience), []),
		new (typeof(Microsoft.Kiota.Abstractions.ApiClientBuilder), []),
		new (typeof(Microsoft.OpenApi.OpenApiWriterAnyExtensions), []),
		new (typeof(Microsoft.Quantum.AmplitudeAmplification.AmpAmpByOracle), []),
		new (typeof(Mono.Cecil.FixedSysStringMarshalInfo), []),
		new (typeof(Mscc.GenerativeAI.BlockedError), []),
		new (typeof(NetFabric.EnumerableExtensions), []),
		new (typeof(nietras.SeparatedValues.SepReaderExtensions), []),
		new (typeof(Ninject.ActivationException), []),
		new (typeof(NodaTime.AmbiguousTimeException), []),
		new (typeof(NuGet.Common.ActivityCorrelationId), []),
		new (typeof(Octokit.AbuseException), []),
		new (typeof(OpenTelemetry.BaseProvider), []),
		new (typeof(Oqtane.Documentation.PrivateApi), []),
		new (typeof(Orleans.Grain), []),
		new (typeof(Paramore.Brighter.Channel), []),
		new (typeof(Prometheus.ChildBase), []),
		new (typeof(Proto.ActorContextDecorator), []),
		new (typeof(Pulumi.Alias), []),
		new (typeof(Quartz.AdoProviderExtensions), []),
		new (typeof(R3.CancellationDisposable), []),
		new (typeof(RabbitMQ.Client.AmqpTcpEndpoint), []),
		new (typeof(RecordParser.Builders.Reader.FixedLengthReaderBuilder<>), []),
		new (typeof(Refit.AliasAsAttribute), []),
		new (typeof(Renci.SshNet.AuthenticationMethod), []),
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
		new (typeof(TestcontainersHealthStatus), []),
		new (typeof(Topshelf.Credentials), []),
		new (typeof(Twilio.Base.Page<>), []),
		new (typeof(VerifyTests.AsStringResult), []),
		new (typeof(Wasmtime.ActionResult), []),
		new (typeof(WireMock.IMapping), []),
		new (typeof(Wisp.CosArray), []),
		new (typeof(ZiggyCreatures.Caching.Fusion.FusionCache), []),
		new (typeof(ZLogger.AsyncStreamLineMessageWriter), []),
#endif
#if INCLUDE_FAILING
#endif
   };

	var typesToLoadAssembliesFrom = new Type[]
	{
		typeof(AnyOfTypes.AnyOf<,>),
		typeof(Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler),
		typeof(System.Security.Claims.ClaimsPrincipal),
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
	}.ToImmutableArray();

	Console.WriteLine($"Getting mapped types...");
	var totalDiscoveredTypeCount = 0;
	var issues = new List<Issue>();
	var visitedAssemblies = new HashSet<Assembly>();

	var generatorTasks = new List<Task<GeneratorResults>>();
	// 4 seems to be a reasonable number. Going higher
	// doesn't seem to help. I may have some contention in the underlying code
	// that I don't realize. Also, if I go to 20 on my 20-core machine,
	// I get an error that doesn't happen when the count is lower.
	var maximumGeneratorTasksCount = 4;

	Console.WriteLine($"Generator task count: {maximumGeneratorTasksCount}");

	static GeneratorResults Generate(
		TypeAliasesMapping typeAliasesMapping, ImmutableArray<Type> typesToLoadAssembliesFrom)
	{
		var targetAssemblySet = new HashSet<Assembly> { typeAliasesMapping.type.Assembly };

		var discoveredTypes = TestGenerator.GetTargets(
			targetAssemblySet, [], typesToLoadAssembliesFrom, typeAliasesMapping.aliases);

		(var createIssues, _) = TestGenerator.Generate(
			new RockGenerator(), discoveredTypes, typesToLoadAssembliesFrom, typeAliasesMapping.aliases, Rocks.Analysis.BuildType.Create);
		(var makeIssues, _) = TestGenerator.Generate(
			new RockGenerator(), discoveredTypes, typesToLoadAssembliesFrom, typeAliasesMapping.aliases, Rocks.Analysis.BuildType.Make);

		return new(typeAliasesMapping.type.Assembly.GetName().Name, discoveredTypes.Length, createIssues, makeIssues);
	}

	foreach (var targetMapping in targetMappings)
	{
		if (visitedAssemblies.Add(targetMapping.type.Assembly) &&
			generatorTasks.Count < maximumGeneratorTasksCount)
		{
			Console.WriteLine($"Started {targetMapping.type.Assembly.GetName().Name}");
			generatorTasks.Add(Task.Run(() => Generate(targetMapping, typesToLoadAssembliesFrom)));
		}

		if (generatorTasks.Count >= maximumGeneratorTasksCount)
		{
			var finishedTaskIndex = Task.WaitAny([.. generatorTasks]);
			var finishedTask = generatorTasks[finishedTaskIndex].Result;

			Console.WriteLine($"Finished {finishedTask.AssemblyName}");
			totalDiscoveredTypeCount += finishedTask.DiscoveredTypesCount;
			issues.AddRange(finishedTask.CreateIssues);
			issues.AddRange(finishedTask.MakeIssues);

			generatorTasks.RemoveAt(finishedTaskIndex);
		}
	}

	Console.WriteLine();
	Console.WriteLine("Waiting for remaining tasks to finish...");
	Console.WriteLine();

	while (generatorTasks.Count > 0)
	{
		var finishedTaskIndex = Task.WaitAny([.. generatorTasks]);
		var finishedTask = generatorTasks[finishedTaskIndex].Result;

		Console.WriteLine($"Finished {finishedTask.AssemblyName}");
		totalDiscoveredTypeCount += finishedTask.DiscoveredTypesCount;
		issues.AddRange(finishedTask.CreateIssues);
		issues.AddRange(finishedTask.MakeIssues);

		generatorTasks.RemoveAt(finishedTaskIndex);
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
		new (typeof(Microsoft.EntityFrameworkCore.Infrastructure.AccessorExtensions), []),
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
	}.ToImmutableArray();

	var issues = new List<Issue>();

	var visitedAssemblies = new HashSet<Assembly>();

	var typeGenerationTimes = new List<TypeGenerationTime>();

	foreach (var targetMapping in targetMappings)
	{
		if (visitedAssemblies.Add(targetMapping.type.Assembly))
		{
			Console.WriteLine($"Getting target types for {targetMapping.type.Assembly.GetName().Name}");
			var targetAssemblySet = new HashSet<Assembly> { targetMapping.type.Assembly };

			var discoveredTypes = TestGenerator.GetTargets(
				targetAssemblySet, [], typesToLoadAssembliesFrom, targetMapping.aliases);

			foreach (var discoveredType in discoveredTypes)
			{
				Console.WriteLine($"Generating for type {discoveredType.FullName}...");
				(_, var generatorElapsedTime) = TestGenerator.Generate(
					new RockGenerator(), [discoveredType], typesToLoadAssembliesFrom, targetMapping.aliases, Rocks.Analysis.BuildType.Create);
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