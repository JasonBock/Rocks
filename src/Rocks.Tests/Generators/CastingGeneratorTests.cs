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
				public interface IHaveOpenGenerics
				{
					void HasGenerics<T>(T value, string data);
					T HasGenericsWithReturn<T>(T value, string data);
					void NoGenerics(int value, string data);
					int NoGenericsWithReturn(int value, string data);
				}
	
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IHaveOpenGenerics>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			using MockTests;
			using Rocks;
			using Rocks.Exceptions;
			using Rocks.Expectations;
			using System;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			#nullable enable
			namespace MockTests
			{
				internal static class CreateExpectationsOfIHaveOpenGenericsExtensions
				{
					internal static MethodExpectations<IHaveOpenGenerics> Methods(this Expectations<IHaveOpenGenerics> self) =>
						new(self);
					
					internal static IHaveOpenGenerics Instance(this Expectations<IHaveOpenGenerics> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockIHaveOpenGenerics(self);
						}
						else
						{
							throw new NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIHaveOpenGenerics
						: IHaveOpenGenerics
					{
						private readonly Dictionary<int, List<HandlerInformation>> handlers;
						
						public RockIHaveOpenGenerics(Expectations<IHaveOpenGenerics> expectations) =>
							this.handlers = expectations.Handlers;
						
						[MemberIdentifier(0, "void HasGenerics<T>(T value, string data)")]
						public void HasGenerics<T>(T value, string data)
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								var foundMatch = false;
								
								foreach (var methodHandler in methodHandlers)
								{
									if (((methodHandler.Expectations[0] as Argument<T>)?.IsValid(value) ?? false) &&
										((Argument<string>)methodHandler.Expectations[1]).IsValid(data))
									{
										foundMatch = true;
										
										if (methodHandler.Method is not null && methodHandler.Method is Action<T, string> method)
										{
											method(value, data);
										}
										
										methodHandler.IncrementCallCount();
										break;
									}
								}
								
								if (!foundMatch)
								{
									throw new ExpectationException("No handlers match for void HasGenerics<T>(T value, string data))");
								}
							}
							else
							{
								throw new ExpectationException("No handlers were found for void HasGenerics<T>(T value, string data)");
							}
						}
						
						[MemberIdentifier(1, "T HasGenericsWithReturn<T>(T value, string data)")]
						public T HasGenericsWithReturn<T>(T value, string data)
						{
							if (this.handlers.TryGetValue(1, out var methodHandlers))
							{
								foreach (var methodHandler in methodHandlers)
								{
									if (((methodHandler.Expectations[0] as Argument<T>)?.IsValid(value) ?? false) &&
										((Argument<string>)methodHandler.Expectations[1]).IsValid(data))
									{
										var result = methodHandler.Method is not null && methodHandler.Method is Func<T, string, T> methodReturn ?
											methodReturn(value, data) :
											methodHandler is HandlerInformation<T> returnValue ?
												returnValue.ReturnValue :
												throw new MockException($"No return value could be obtained for T of type {typeof(T).FullName}.");
										methodHandler.IncrementCallCount();
										return result!;
									}
								}
								
								throw new ExpectationException("No handlers match for T HasGenericsWithReturn<T>(T value, string data)");
							}
							
							throw new ExpectationException("No handlers were found for T HasGenericsWithReturn<T>(T value, string data)");
						}
						
						[MemberIdentifier(2, "void NoGenerics(int value, string data)")]
						public void NoGenerics(int value, string data)
						{
							if (this.handlers.TryGetValue(2, out var methodHandlers))
							{
								var foundMatch = false;
								
								foreach (var methodHandler in methodHandlers)
								{
									if (((Argument<int>)methodHandler.Expectations[0]).IsValid(value) &&
										((Argument<string>)methodHandler.Expectations[1]).IsValid(data))
									{
										foundMatch = true;
										
										if (methodHandler.Method is not null)
										{
											((Action<int, string>)methodHandler.Method)(value, data);
										}
										
										methodHandler.IncrementCallCount();
										break;
									}
								}
								
								if (!foundMatch)
								{
									throw new ExpectationException("No handlers match for void NoGenerics(int value, string data))");
								}
							}
							else
							{
								throw new ExpectationException("No handlers were found for void NoGenerics(int value, string data)");
							}
						}
						
						[MemberIdentifier(3, "int NoGenericsWithReturn(int value, string data)")]
						public int NoGenericsWithReturn(int value, string data)
						{
							if (this.handlers.TryGetValue(3, out var methodHandlers))
							{
								foreach (var methodHandler in methodHandlers)
								{
									if (((Argument<int>)methodHandler.Expectations[0]).IsValid(value) &&
										((Argument<string>)methodHandler.Expectations[1]).IsValid(data))
									{
										var result = methodHandler.Method is not null ?
											((Func<int, string, int>)methodHandler.Method)(value, data) :
											((HandlerInformation<int>)methodHandler).ReturnValue;
										methodHandler.IncrementCallCount();
										return result!;
									}
								}
								
								throw new ExpectationException("No handlers match for int NoGenericsWithReturn(int value, string data)");
							}
							
							throw new ExpectationException("No handlers were found for int NoGenericsWithReturn(int value, string data)");
						}
						
					}
				}
				
				internal static class MethodExpectationsOfIHaveOpenGenericsExtensions
				{
					internal static MethodAdornments<IHaveOpenGenerics, Action<T, string>> HasGenerics<T>(this MethodExpectations<IHaveOpenGenerics> self, Argument<T> value, Argument<string> data)
					{
						ArgumentNullException.ThrowIfNull(value);
						ArgumentNullException.ThrowIfNull(data);
						return new MethodAdornments<IHaveOpenGenerics, Action<T, string>>(self.Add(0, new List<Argument>(2) { value, data }));
					}
					internal static MethodAdornments<IHaveOpenGenerics, Func<T, string, T>, T> HasGenericsWithReturn<T>(this MethodExpectations<IHaveOpenGenerics> self, Argument<T> value, Argument<string> data)
					{
						ArgumentNullException.ThrowIfNull(value);
						ArgumentNullException.ThrowIfNull(data);
						return new MethodAdornments<IHaveOpenGenerics, Func<T, string, T>, T>(self.Add<T>(1, new List<Argument>(2) { value, data }));
					}
					internal static MethodAdornments<IHaveOpenGenerics, Action<int, string>> NoGenerics(this MethodExpectations<IHaveOpenGenerics> self, Argument<int> value, Argument<string> data)
					{
						ArgumentNullException.ThrowIfNull(value);
						ArgumentNullException.ThrowIfNull(data);
						return new MethodAdornments<IHaveOpenGenerics, Action<int, string>>(self.Add(2, new List<Argument>(2) { value, data }));
					}
					internal static MethodAdornments<IHaveOpenGenerics, Func<int, string, int>, int> NoGenericsWithReturn(this MethodExpectations<IHaveOpenGenerics> self, Argument<int> value, Argument<string> data)
					{
						ArgumentNullException.ThrowIfNull(value);
						ArgumentNullException.ThrowIfNull(data);
						return new MethodAdornments<IHaveOpenGenerics, Func<int, string, int>, int>(self.Add<int>(3, new List<Argument>(2) { value, data }));
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IHaveOpenGenerics_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}