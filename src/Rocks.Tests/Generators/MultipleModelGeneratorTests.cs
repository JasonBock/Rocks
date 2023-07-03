using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class MultipleModelGeneratorTests
{
	[Test]
	public static async Task GenerateWithMultipleCreatesAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public interface ITarget
				{
					string Retrieve(int value);
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<ITarget>();
					}

					public static void GenerateAgain()
					{
						var rock = Rock.Create<ITarget>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable

			namespace MockTests
			{
				internal static class CreateExpectationsOfITargetExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.ITarget> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.ITarget> @self) =>
						new(@self);
					
					internal static global::MockTests.ITarget Instance(this global::Rocks.Expectations.Expectations<global::MockTests.ITarget> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockITarget(@self);
							@self.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockITarget
						: global::MockTests.ITarget
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockITarget(global::Rocks.Expectations.Expectations<global::MockTests.ITarget> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
						[global::Rocks.MemberIdentifier(0, "string Retrieve(int @value)")]
						public string Retrieve(int @value)
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (((global::Rocks.Argument<int>)@methodHandler.Expectations[0]).IsValid(@value))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											((global::System.Func<int, string>)@methodHandler.Method)(@value) :
											((global::Rocks.HandlerInformation<string>)@methodHandler).ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for string Retrieve(int @value)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for string Retrieve(int @value)");
						}
						
					}
				}
				
				internal static class MethodExpectationsOfITargetExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.ITarget, global::System.Func<int, string>, string> Retrieve(this global::Rocks.Expectations.MethodExpectations<global::MockTests.ITarget> @self, global::Rocks.Argument<int> @value)
					{
						global::System.ArgumentNullException.ThrowIfNull(@value);
						return new global::Rocks.MethodAdornments<global::MockTests.ITarget, global::System.Func<int, string>, string>(@self.Add<string>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ITarget_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithMultipleMakeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public interface ITarget
				{
					string Retrieve(int value);
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Make<ITarget>();
					}

					public static void GenerateAgain()
					{
						var rock = Rock.Make<ITarget>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class MakeExpectationsOfITargetExtensions
				{
					internal static global::MockTests.ITarget Instance(this global::Rocks.MakeGeneration<global::MockTests.ITarget> @self)
					{
						return new RockITarget();
					}
					
					private sealed class RockITarget
						: global::MockTests.ITarget
					{
						public RockITarget()
						{
						}
						
						public string Retrieve(int @value)
						{
							return default!;
						}
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "ITarget_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}