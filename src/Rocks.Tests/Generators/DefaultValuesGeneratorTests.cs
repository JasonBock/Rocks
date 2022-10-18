using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class DefaultValuesGeneratorTests
{
	[Test]
	public static async Task CreateWithPositiveInfinityAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			public interface IUseInfinity
			{
			  void Use(double value = double.PositiveInfinity);
			}

			public static class Test
			{
				public static void Generate()
				{
					var rock = Rock.Create<IUseInfinity>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfIUseInfinityExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::IUseInfinity> Methods(this global::Rocks.Expectations.Expectations<global::IUseInfinity> @self) =>
					new(@self);
				
				internal static global::IUseInfinity Instance(this global::Rocks.Expectations.Expectations<global::IUseInfinity> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						return new RockIUseInfinity(@self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockIUseInfinity
					: global::IUseInfinity
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockIUseInfinity(global::Rocks.Expectations.Expectations<global::IUseInfinity> @expectations) =>
						this.handlers = @expectations.Handlers;
					
					[global::Rocks.MemberIdentifier(0, "void Use(double @value)")]
					public void Use(double @value = double.PositiveInfinity)
					{
						if (this.handlers.TryGetValue(0, out var @methodHandlers))
						{
							var @foundMatch = false;
							
							foreach (var @methodHandler in @methodHandlers)
							{
								if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<double>>(@methodHandler.Expectations[0]).IsValid(@value))
								{
									@foundMatch = true;
									
									if (@methodHandler.Method is not null)
									{
										global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<double>>(@methodHandler.Method)(@value);
									}
									
									@methodHandler.IncrementCallCount();
									break;
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void Use(double @value = double.PositiveInfinity)");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void Use(double @value = double.PositiveInfinity)");
						}
					}
					
				}
			}
			
			internal static class MethodExpectationsOfIUseInfinityExtensions
			{
				internal static global::Rocks.MethodAdornments<global::IUseInfinity, global::System.Action<double>> Use(this global::Rocks.Expectations.MethodExpectations<global::IUseInfinity> @self, global::Rocks.Argument<double> @value)
				{
					global::System.ArgumentNullException.ThrowIfNull(@value);
					return new global::Rocks.MethodAdornments<global::IUseInfinity, global::System.Action<double>>(@self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value.Transform(double.PositiveInfinity) }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IUseInfinity_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task MakeWithPositiveInfinityAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			public interface IUseInfinity
			{
			  void Use(double value = double.PositiveInfinity);
			}

			public static class Test
			{
				public static void Generate()
				{
					var rock = Rock.Make<IUseInfinity>();
				}
			}
			""";

		var generatedCode = "";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "IUseInfinity_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}