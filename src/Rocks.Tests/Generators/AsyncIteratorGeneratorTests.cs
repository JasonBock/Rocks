using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class AsyncIteratorGeneratorTests
{
	[Test]
	public static async Task GenerateCreateAsync()
	{
		var code =
			"""
			using Rocks;
			using System.Collections.Generic;
			using System.Runtime.CompilerServices;
			using System.Threading;
			using System.Threading.Tasks;
			
			public class AsyncEnumeration
			{
				public virtual async IAsyncEnumerable<string> GetRecordsAsync(
			        [EnumeratorCancellation] CancellationToken cancellationToken = default)
				{
					await Task.CompletedTask;
					yield return "y";
				}
			}
			
			public static class Test
			{
				public static void Generate()
				{
					var rock = Rock.Create<AsyncEnumeration>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfAsyncEnumerationExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::AsyncEnumeration> Methods(this global::Rocks.Expectations.Expectations<global::AsyncEnumeration> @self) =>
					new(@self);
				
				internal static global::AsyncEnumeration Instance(this global::Rocks.Expectations.Expectations<global::AsyncEnumeration> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						return new RockAsyncEnumeration(@self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockAsyncEnumeration
					: global::AsyncEnumeration
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockAsyncEnumeration(global::Rocks.Expectations.Expectations<global::AsyncEnumeration> @expectations) =>
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
					
					[global::Rocks.MemberIdentifier(3, "global::System.Collections.Generic.IAsyncEnumerable<string> GetRecordsAsync(global::System.Threading.CancellationToken @cancellationToken)")]
					public override global::System.Collections.Generic.IAsyncEnumerable<string> GetRecordsAsync([global::System.Runtime.CompilerServices.EnumeratorCancellationAttribute] global::System.Threading.CancellationToken @cancellationToken = default)
					{
						if (this.handlers.TryGetValue(3, out var @methodHandlers))
						{
							foreach (var @methodHandler in @methodHandlers)
							{
								if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<global::System.Threading.CancellationToken>>(@methodHandler.Expectations[0]).IsValid(@cancellationToken))
								{
									var @result = @methodHandler.Method is not null ?
										global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<global::System.Threading.CancellationToken, global::System.Collections.Generic.IAsyncEnumerable<string>>>(@methodHandler.Method)(@cancellationToken) :
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<global::System.Collections.Generic.IAsyncEnumerable<string>>>(@methodHandler).ReturnValue;
									@methodHandler.IncrementCallCount();
									return @result!;
								}
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers match for global::System.Collections.Generic.IAsyncEnumerable<string> GetRecordsAsync([global::System.Runtime.CompilerServices.EnumeratorCancellationAttribute] global::System.Threading.CancellationToken @cancellationToken = default)");
						}
						else
						{
							return base.GetRecordsAsync(@cancellationToken);
						}
					}
					
				}
			}
			
			internal static class MethodExpectationsOfAsyncEnumerationExtensions
			{
				internal static global::Rocks.MethodAdornments<global::AsyncEnumeration, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::AsyncEnumeration> @self, global::Rocks.Argument<object?> @obj)
				{
					global::System.ArgumentNullException.ThrowIfNull(@obj);
					return new global::Rocks.MethodAdornments<global::AsyncEnumeration, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
				}
				internal static global::Rocks.MethodAdornments<global::AsyncEnumeration, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::AsyncEnumeration> @self) =>
					new global::Rocks.MethodAdornments<global::AsyncEnumeration, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::AsyncEnumeration, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::AsyncEnumeration> @self) =>
					new global::Rocks.MethodAdornments<global::AsyncEnumeration, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::AsyncEnumeration, global::System.Func<global::System.Threading.CancellationToken, global::System.Collections.Generic.IAsyncEnumerable<string>>, global::System.Collections.Generic.IAsyncEnumerable<string>> GetRecordsAsync(this global::Rocks.Expectations.MethodExpectations<global::AsyncEnumeration> @self, global::Rocks.Argument<global::System.Threading.CancellationToken> @cancellationToken)
				{
					global::System.ArgumentNullException.ThrowIfNull(@cancellationToken);
					return new global::Rocks.MethodAdornments<global::AsyncEnumeration, global::System.Func<global::System.Threading.CancellationToken, global::System.Collections.Generic.IAsyncEnumerable<string>>, global::System.Collections.Generic.IAsyncEnumerable<string>>(@self.Add<global::System.Collections.Generic.IAsyncEnumerable<string>>(3, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @cancellationToken.Transform(default) }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "AsyncEnumeration_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateMakeAsync()
	{
		var code =
			"""
			using Rocks;
			using System.Collections.Generic;
			using System.Runtime.CompilerServices;
			using System.Threading;
			using System.Threading.Tasks;
			
			public class AsyncEnumeration
			{
				public virtual async IAsyncEnumerable<string> GetRecordsAsync(
			        [EnumeratorCancellation] CancellationToken cancellationToken = default)
				{
					await Task.CompletedTask;
					yield return "y";
				}
			}
			
			public static class Test
			{
				public static void Generate()
				{
					var rock = Rock.Make<AsyncEnumeration>();
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			internal static class MakeExpectationsOfAsyncEnumerationExtensions
			{
				internal static global::AsyncEnumeration Instance(this global::Rocks.MakeGeneration<global::AsyncEnumeration> @self) =>
					new RockAsyncEnumeration();
				
				private sealed class RockAsyncEnumeration
					: global::AsyncEnumeration
				{
					public RockAsyncEnumeration() { }
					
					public override bool Equals(object? @obj)
					{
						return default!;
					}
					public override int GetHashCode()
					{
						return default!;
					}
					public override string? ToString()
					{
						return default!;
					}
					public override global::System.Collections.Generic.IAsyncEnumerable<string> GetRecordsAsync([global::System.Runtime.CompilerServices.EnumeratorCancellationAttribute] global::System.Threading.CancellationToken @cancellationToken = default)
					{
						return default!;
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "AsyncEnumeration_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}