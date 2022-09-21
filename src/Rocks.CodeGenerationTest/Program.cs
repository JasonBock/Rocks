﻿using Moq;
using Rocks;
using Rocks.CodeGenerationTest;
using System.Linq.Expressions;

var x = typeof(Mock);
//TestWithCode();
TestWithType();
//TestWithTypes();

#pragma warning disable CS8321 // Local function is declared but never used
static void TestWithCode()
{
	TestGenerator.Generate(new RockCreateGenerator(),
		"""
		using Csla;
		using Rocks;
		using System;

		[Serializable]
		public class Customer
			: BusinessBase<Customer> { }

		public static class Test
		{
			public static void Go()
			{
				var expectations = Rock.Create<BusinessBase<Customer>>();
			}
		}
		""",
		typeof(Csla.BusinessBase<>),
		typeof(System.ComponentModel.DataAnnotations.RequiredAttribute),
		typeof(System.ComponentModel.PropertyChangingEventArgs));
}

static void TestWithType() =>
	 TestGenerator.Generate(new RockCreateGenerator(), new[] { typeof(Moq.Mock<>) }, typeof(Expression));

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
		typeof(Moq.Mock<>),

		// ImageSharp
		//typeof(SixLabors.ImageSharp.GraphicsOptions),

		// TerraFX.Interop.D3D12MemoryAllocator
		//typeof(TerraFX.Interop.DirectX.D3D12MA_Allocation),

		// TerraFX.Interop.Windows
		//typeof(TerraFX.Interop.INativeGuid),

		// Microsoft.CodeAnalysis.CSharp
		//typeof(Microsoft.CodeAnalysis.SyntaxTree),

		// System.Text.Json
		//typeof(System.Text.Json.JsonDocument),

		// Microsoft.Extensions.DependencyInjection
		//typeof(Microsoft.Extensions.DependencyInjection.AsyncServiceScope),

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

		// IdentityModel
		//typeof(IdentityModel.Base64Url),

		// Google.Protobuf
		//typeof(Google.Protobuf.ByteString),

		// CsvHelper
		//typeof(CsvHelper.ArrayHelper),

		// TODO: Azure.Identity, Antlr, SharpZipLib, MediatR, System.Reactive, 
		// NSubstitute, AWSSDK.Core, AngleSharp, MassTransit, Bogus, SkiaSharp,
		// ClangSharp, LLVMSharp, Silk.NET, System.Reflection.Metadata
	}.Select(_ => _.Assembly).ToHashSet();

	Console.WriteLine($"Testing {nameof(RockCreateGenerator)}");
	TestGenerator.Generate(new RockCreateGenerator(), targetAssemblies);
	Console.WriteLine();

	Console.WriteLine($"Testing {nameof(RockMakeGenerator)}");
	TestGenerator.Generate(new RockMakeGenerator(), targetAssemblies);
	Console.WriteLine();

	Console.WriteLine("Generator testing complete");
}
#pragma warning restore CS8321 // Local function is declared but never used