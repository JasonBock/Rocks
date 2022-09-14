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
				internal static class CreateExpectationsOfIHaveOpenGenericsOfGuidExtensions
				{
					internal static MethodExpectations<IHaveOpenGenerics<Guid>> Methods(this Expectations<IHaveOpenGenerics<Guid>> self) =>
						new(self);
					
					internal static IHaveOpenGenerics<Guid> Instance(this Expectations<IHaveOpenGenerics<Guid>> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockIHaveOpenGenericsOfGuid(self);
						}
						else
						{
							throw new NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIHaveOpenGenericsOfGuid
						: IHaveOpenGenerics<Guid>
					{
						private readonly Dictionary<int, List<HandlerInformation>> handlers;
						
						public RockIHaveOpenGenericsOfGuid(Expectations<IHaveOpenGenerics<Guid>> expectations) =>
							this.handlers = expectations.Handlers;
						
						[MemberIdentifier(0, "void HasGenerics<T>(T value, string data, Guid information)")]
						public void HasGenerics<T>(T value, string data, Guid information)
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								var foundMatch = false;
								
								foreach (var methodHandler in methodHandlers)
								{
									if (((methodHandler.Expectations[0] as Argument<T>)?.IsValid(value) ?? false) &&
										((Argument<string>)methodHandler.Expectations[1]).IsValid(data) &&
										((Argument<Guid>)methodHandler.Expectations[2]).IsValid(information))
									{
										foundMatch = true;
										
										if (methodHandler.Method is not null && methodHandler.Method is Action<T, string, Guid> method)
										{
											method(value, data, information);
										}
										
										methodHandler.IncrementCallCount();
										break;
									}
								}
								
								if (!foundMatch)
								{
									throw new ExpectationException("No handlers match for void HasGenerics<T>(T value, string data, Guid information)");
								}
							}
							else
							{
								throw new ExpectationException("No handlers were found for void HasGenerics<T>(T value, string data, Guid information)");
							}
						}
						
						[MemberIdentifier(1, "T HasGenericsWithReturn<T>(T value, string data, Guid information)")]
						public T HasGenericsWithReturn<T>(T value, string data, Guid information)
						{
							if (this.handlers.TryGetValue(1, out var methodHandlers))
							{
								foreach (var methodHandler in methodHandlers)
								{
									if (((methodHandler.Expectations[0] as Argument<T>)?.IsValid(value) ?? false) &&
										((Argument<string>)methodHandler.Expectations[1]).IsValid(data) &&
										((Argument<Guid>)methodHandler.Expectations[2]).IsValid(information))
									{
										var result = methodHandler.Method is not null && methodHandler.Method is Func<T, string, Guid, T> methodReturn ?
											methodReturn(value, data, information) :
											methodHandler is HandlerInformation<T> returnValue ?
												returnValue.ReturnValue :
												throw new MockException($"No return value could be obtained for T of type {typeof(T).FullName}.");
										methodHandler.IncrementCallCount();
										return result!;
									}
								}
								
								throw new ExpectationException("No handlers match for T HasGenericsWithReturn<T>(T value, string data, Guid information)");
							}
							
							throw new ExpectationException("No handlers were found for T HasGenericsWithReturn<T>(T value, string data, Guid information)");
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
									throw new ExpectationException("No handlers match for void NoGenerics(int value, string data)");
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
				
				internal static class MethodExpectationsOfIHaveOpenGenericsOfGuidExtensions
				{
					internal static MethodAdornments<IHaveOpenGenerics<Guid>, Action<T, string, Guid>> HasGenerics<T>(this MethodExpectations<IHaveOpenGenerics<Guid>> self, Argument<T> value, Argument<string> data, Argument<Guid> information)
					{
						ArgumentNullException.ThrowIfNull(value);
						ArgumentNullException.ThrowIfNull(data);
						ArgumentNullException.ThrowIfNull(information);
						return new MethodAdornments<IHaveOpenGenerics<Guid>, Action<T, string, Guid>>(self.Add(0, new List<Argument>(3) { value, data, information }));
					}
					internal static MethodAdornments<IHaveOpenGenerics<Guid>, Func<T, string, Guid, T>, T> HasGenericsWithReturn<T>(this MethodExpectations<IHaveOpenGenerics<Guid>> self, Argument<T> value, Argument<string> data, Argument<Guid> information)
					{
						ArgumentNullException.ThrowIfNull(value);
						ArgumentNullException.ThrowIfNull(data);
						ArgumentNullException.ThrowIfNull(information);
						return new MethodAdornments<IHaveOpenGenerics<Guid>, Func<T, string, Guid, T>, T>(self.Add<T>(1, new List<Argument>(3) { value, data, information }));
					}
					internal static MethodAdornments<IHaveOpenGenerics<Guid>, Action<int, string>> NoGenerics(this MethodExpectations<IHaveOpenGenerics<Guid>> self, Argument<int> value, Argument<string> data)
					{
						ArgumentNullException.ThrowIfNull(value);
						ArgumentNullException.ThrowIfNull(data);
						return new MethodAdornments<IHaveOpenGenerics<Guid>, Action<int, string>>(self.Add(2, new List<Argument>(2) { value, data }));
					}
					internal static MethodAdornments<IHaveOpenGenerics<Guid>, Func<int, string, int>, int> NoGenericsWithReturn(this MethodExpectations<IHaveOpenGenerics<Guid>> self, Argument<int> value, Argument<string> data)
					{
						ArgumentNullException.ThrowIfNull(value);
						ArgumentNullException.ThrowIfNull(data);
						return new MethodAdornments<IHaveOpenGenerics<Guid>, Func<int, string, int>, int>(self.Add<int>(3, new List<Argument>(2) { value, data }));
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IHaveOpenGenericsOfGuid_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}