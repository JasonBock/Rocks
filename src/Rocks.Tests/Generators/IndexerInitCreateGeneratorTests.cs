using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class IndexerInitCreateGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public abstract class Target
				{
					public abstract int this[int a] { get; init; }
				}
	
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<Target>();
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
				internal static class CreateExpectationsOfTargetExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.Target> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.Target> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.IndexerExpectations<global::MockTests.Target> Indexers(this global::Rocks.Expectations.Expectations<global::MockTests.Target> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.IndexerGetterExpectations<global::MockTests.Target> Getters(this global::Rocks.Expectations.IndexerExpectations<global::MockTests.Target> @self) =>
						new(@self);
					
					internal static global::MockTests.Target Instance(this global::Rocks.Expectations.Expectations<global::MockTests.Target> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							return new RockTarget(@self);
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockTarget
						: global::MockTests.Target
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockTarget(global::Rocks.Expectations.Expectations<global::MockTests.Target> @expectations) =>
							this.handlers = @expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "bool Equals(object? @obj)")]
						public override bool Equals(object? @obj)
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(@methodHandler.Expectations[0]).IsValid(@obj))
									{
										var @result = @methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, bool>>(@methodHandler.Method)(@obj) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(@methodHandler).ReturnValue;
										@methodHandler.IncrementCallCount();
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool Equals(object? @obj)");
							}
							else
							{
								return base.Equals(@obj);
							}
						}
						
						[global::Rocks.MemberIdentifier(1, "int GetHashCode()")]
						public override int GetHashCode()
						{
							if (this.handlers.TryGetValue(1, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
								@methodHandler.IncrementCallCount();
								return @result!;
							}
							else
							{
								return base.GetHashCode();
							}
						}
						
						[global::Rocks.MemberIdentifier(2, "string? ToString()")]
						public override string? ToString()
						{
							if (this.handlers.TryGetValue(2, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string?>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string?>>(@methodHandler).ReturnValue;
								@methodHandler.IncrementCallCount();
								return @result!;
							}
							else
							{
								return base.ToString();
							}
						}
						
						[global::Rocks.MemberIdentifier(3, "this[int @a]")]
						[global::Rocks.MemberIdentifier(4, "this[int @a]")]
						public override int this[int @a]
						{
							get
							{
								if (this.handlers.TryGetValue(3, out var @methodHandlers))
								{
									foreach (var @methodHandler in @methodHandlers)
									{
										if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<int>>(@methodHandler.Expectations[0]).IsValid(@a))
										{
											var @result = @methodHandler.Method is not null ?
												global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int, int>>(@methodHandler.Method)(@a) :
												global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
											@methodHandler.IncrementCallCount();
											return @result!;
										}
									}
									
									throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[int @a]");
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for this[int @a])");
							}
							init { }
						}
					}
				}
				
				internal static class MethodExpectationsOfTargetExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.Target, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Target> @self, global::Rocks.Argument<object?> @obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(@obj);
						return new global::Rocks.MethodAdornments<global::MockTests.Target, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.Target, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Target> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Target, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.Target, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Target> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Target, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				
				internal static class IndexerGetterExpectationsOfTargetExtensions
				{
					internal static global::Rocks.IndexerAdornments<global::MockTests.Target, global::System.Func<int, int>, int> This(this global::Rocks.Expectations.IndexerGetterExpectations<global::MockTests.Target> @self, global::Rocks.Argument<int> @a)
					{
						global::System.ArgumentNullException.ThrowIfNull(@a);
						return new global::Rocks.IndexerAdornments<global::MockTests.Target, global::System.Func<int, int>, int>(@self.Add<int>(3, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @a }));
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "Target_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}