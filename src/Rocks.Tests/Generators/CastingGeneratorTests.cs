using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class CastingGeneratorTests
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
				public interface IHaveOpenGenerics<Q>
				{
					void HasGenerics<T>(T value, string data, Q information);
					T HasGenericsWithReturn<T>(T value, string data, Q information);
					void NoGenerics(int value, string data);
					int NoGenericsWithReturn(int value, string data);
				}
	
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IHaveOpenGenerics<Guid>>();
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
				internal static class CreateExpectationsOfIHaveOpenGenericsOfGuidExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveOpenGenerics<global::System.Guid>> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveOpenGenerics<global::System.Guid>> @self) =>
						new(@self);
					
					internal static global::MockTests.IHaveOpenGenerics<global::System.Guid> Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveOpenGenerics<global::System.Guid>> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							return new RockIHaveOpenGenericsOfGuid(@self);
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIHaveOpenGenericsOfGuid
						: global::MockTests.IHaveOpenGenerics<global::System.Guid>
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockIHaveOpenGenericsOfGuid(global::Rocks.Expectations.Expectations<global::MockTests.IHaveOpenGenerics<global::System.Guid>> @expectations) =>
							this.handlers = @expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "void HasGenerics<T>(T @value, string @data, global::System.Guid @information)")]
						public void HasGenerics<T>(T @value, string @data, global::System.Guid @information)
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								var @foundMatch = false;
								
								foreach (var @methodHandler in @methodHandlers)
								{
									if (((@methodHandler.Expectations[0] as global::Rocks.Argument<T>)?.IsValid(@value) ?? false) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(@methodHandler.Expectations[1]).IsValid(@data) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<global::System.Guid>>(@methodHandler.Expectations[2]).IsValid(@information))
									{
										@foundMatch = true;
										
										if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action<T, string, global::System.Guid> @method)
										{
											@method(@value, @data, @information);
										}
										
										@methodHandler.IncrementCallCount();
										break;
									}
								}
								
								if (!@foundMatch)
								{
									throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void HasGenerics<T>(T @value, string @data, global::System.Guid @information)");
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void HasGenerics<T>(T @value, string @data, global::System.Guid @information)");
							}
						}
						
						[global::Rocks.MemberIdentifier(1, "T HasGenericsWithReturn<T>(T @value, string @data, global::System.Guid @information)")]
						public T HasGenericsWithReturn<T>(T @value, string @data, global::System.Guid @information)
						{
							if (this.handlers.TryGetValue(1, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (((@methodHandler.Expectations[0] as global::Rocks.Argument<T>)?.IsValid(@value) ?? false) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(@methodHandler.Expectations[1]).IsValid(@data) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<global::System.Guid>>(@methodHandler.Expectations[2]).IsValid(@information))
									{
										var @result = @methodHandler.Method is not null && @methodHandler.Method is global::System.Func<T, string, global::System.Guid, T> @methodReturn ?
											@methodReturn(@value, @data, @information) :
											@methodHandler is global::Rocks.HandlerInformation<T> @returnValue ?
												@returnValue.ReturnValue :
												throw new global::Rocks.Exceptions.MockException($"No return value could be obtained for T of type {typeof(T).FullName}.");
										@methodHandler.IncrementCallCount();
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for T HasGenericsWithReturn<T>(T @value, string @data, global::System.Guid @information)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for T HasGenericsWithReturn<T>(T @value, string @data, global::System.Guid @information)");
						}
						
						[global::Rocks.MemberIdentifier(2, "void NoGenerics(int @value, string @data)")]
						public void NoGenerics(int @value, string @data)
						{
							if (this.handlers.TryGetValue(2, out var @methodHandlers))
							{
								var @foundMatch = false;
								
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<int>>(@methodHandler.Expectations[0]).IsValid(@value) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(@methodHandler.Expectations[1]).IsValid(@data))
									{
										@foundMatch = true;
										
										if (@methodHandler.Method is not null)
										{
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<int, string>>(@methodHandler.Method)(@value, @data);
										}
										
										@methodHandler.IncrementCallCount();
										break;
									}
								}
								
								if (!@foundMatch)
								{
									throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void NoGenerics(int @value, string @data)");
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void NoGenerics(int @value, string @data)");
							}
						}
						
						[global::Rocks.MemberIdentifier(3, "int NoGenericsWithReturn(int @value, string @data)")]
						public int NoGenericsWithReturn(int @value, string @data)
						{
							if (this.handlers.TryGetValue(3, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<int>>(@methodHandler.Expectations[0]).IsValid(@value) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(@methodHandler.Expectations[1]).IsValid(@data))
									{
										var @result = @methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int, string, int>>(@methodHandler.Method)(@value, @data) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
										@methodHandler.IncrementCallCount();
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for int NoGenericsWithReturn(int @value, string @data)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for int NoGenericsWithReturn(int @value, string @data)");
						}
						
					}
				}
				
				internal static class MethodExpectationsOfIHaveOpenGenericsOfGuidExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IHaveOpenGenerics<global::System.Guid>, global::System.Action<T, string, global::System.Guid>> HasGenerics<T>(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveOpenGenerics<global::System.Guid>> @self, global::Rocks.Argument<T> @value, global::Rocks.Argument<string> @data, global::Rocks.Argument<global::System.Guid> @information)
					{
						global::System.ArgumentNullException.ThrowIfNull(@value);
						global::System.ArgumentNullException.ThrowIfNull(@data);
						global::System.ArgumentNullException.ThrowIfNull(@information);
						return new global::Rocks.MethodAdornments<global::MockTests.IHaveOpenGenerics<global::System.Guid>, global::System.Action<T, string, global::System.Guid>>(@self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(3) { @value, @data, @information }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.IHaveOpenGenerics<global::System.Guid>, global::System.Func<T, string, global::System.Guid, T>, T> HasGenericsWithReturn<T>(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveOpenGenerics<global::System.Guid>> @self, global::Rocks.Argument<T> @value, global::Rocks.Argument<string> @data, global::Rocks.Argument<global::System.Guid> @information)
					{
						global::System.ArgumentNullException.ThrowIfNull(@value);
						global::System.ArgumentNullException.ThrowIfNull(@data);
						global::System.ArgumentNullException.ThrowIfNull(@information);
						return new global::Rocks.MethodAdornments<global::MockTests.IHaveOpenGenerics<global::System.Guid>, global::System.Func<T, string, global::System.Guid, T>, T>(@self.Add<T>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>(3) { @value, @data, @information }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.IHaveOpenGenerics<global::System.Guid>, global::System.Action<int, string>> NoGenerics(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveOpenGenerics<global::System.Guid>> @self, global::Rocks.Argument<int> @value, global::Rocks.Argument<string> @data)
					{
						global::System.ArgumentNullException.ThrowIfNull(@value);
						global::System.ArgumentNullException.ThrowIfNull(@data);
						return new global::Rocks.MethodAdornments<global::MockTests.IHaveOpenGenerics<global::System.Guid>, global::System.Action<int, string>>(@self.Add(2, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { @value, @data }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.IHaveOpenGenerics<global::System.Guid>, global::System.Func<int, string, int>, int> NoGenericsWithReturn(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveOpenGenerics<global::System.Guid>> @self, global::Rocks.Argument<int> @value, global::Rocks.Argument<string> @data)
					{
						global::System.ArgumentNullException.ThrowIfNull(@value);
						global::System.ArgumentNullException.ThrowIfNull(@data);
						return new global::Rocks.MethodAdornments<global::MockTests.IHaveOpenGenerics<global::System.Guid>, global::System.Func<int, string, int>, int>(@self.Add<int>(3, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { @value, @data }));
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IHaveOpenGenericsOfGuid_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}