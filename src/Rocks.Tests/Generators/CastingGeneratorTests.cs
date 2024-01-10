﻿using NUnit.Framework;

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

			[assembly: RockCreate<MockTests.IHaveOpenGenerics<Guid>>]

			namespace MockTests
			{
				public interface IHaveOpenGenerics<Q>
				{
					void HasGenerics<T>(T value, string data, Q information);
					T HasGenericsWithReturn<T>(T value, string data, Q information);
					void NoGenerics(int value, string data);
					int NoGenericsWithReturn(int value, string data);
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			namespace MockTests
			{
				internal sealed class IHaveOpenGenericsOfGuidCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler0<T>
						: global::Rocks.Handler<global::System.Action<T, string, global::System.Guid>>
					{
						public global::Rocks.Argument<T> @value { get; set; }
						public global::Rocks.Argument<string> @data { get; set; }
						public global::Rocks.Argument<global::System.Guid> @information { get; set; }
					}
					
					internal sealed class Handler1<T>
						: global::Rocks.Handler<global::System.Func<T, string, global::System.Guid, T>, T>
					{
						public global::Rocks.Argument<T> @value { get; set; }
						public global::Rocks.Argument<string> @data { get; set; }
						public global::Rocks.Argument<global::System.Guid> @information { get; set; }
					}
					
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Action<int, string>>
					{
						public global::Rocks.Argument<int> @value { get; set; }
						public global::Rocks.Argument<string> @data { get; set; }
					}
					
					internal sealed class Handler3
						: global::Rocks.Handler<global::System.Func<int, string, int>, int>
					{
						public global::Rocks.Argument<int> @value { get; set; }
						public global::Rocks.Argument<string> @data { get; set; }
					}
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::Rocks.Handler> @handlers0 = new();
					private readonly global::System.Collections.Generic.List<global::Rocks.Handler> @handlers1 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler2> @handlers2 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler3> @handlers3 = new();
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							failures.AddRange(this.Verify(handlers0));
							failures.AddRange(this.Verify(handlers1));
							failures.AddRange(this.Verify(handlers2));
							failures.AddRange(this.Verify(handlers3));
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class RockIHaveOpenGenericsOfGuid
						: global::MockTests.IHaveOpenGenerics<global::System.Guid>
					{
						public RockIHaveOpenGenericsOfGuid(global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0, "void HasGenerics<T>(T @value, string @data, global::System.Guid @information)")]
						public void HasGenerics<T>(T @value, string @data, global::System.Guid @information)
						{
							if (this.Expectations.handlers0.Count > 0)
							{
								var @foundMatch = false;
								
								foreach (var @genericHandler in this.Expectations.handlers0)
								{
									if (@genericHandler is global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler0<T> @handler)
									{
										if (@handler.@value.IsValid(@value!) &&
											@handler.@data.IsValid(@data!) &&
											@handler.@information.IsValid(@information!))
										{
											@foundMatch = true;
											@handler.CallCount++;
											@handler.Callback?.Invoke(@value!, @data!, @information!);
											break;
										}
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
							if (this.Expectations.handlers1.Count > 0)
							{
								foreach (var @genericHandler in this.Expectations.handlers1)
								{
									if (@genericHandler is global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler1<T> @handler)
									{
										if (@handler.@value.IsValid(@value!) &&
											@handler.@data.IsValid(@data!) &&
											@handler.@information.IsValid(@information!))
										{
											@handler.CallCount++;
											var @result = @handler.Callback is not null ?
												@handler.Callback(@value!, @data!, @information!) : @handler.ReturnValue;
											return @result!;
										}
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for T HasGenericsWithReturn<T>(T @value, string @data, global::System.Guid @information)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for T HasGenericsWithReturn<T>(T @value, string @data, global::System.Guid @information)");
						}
						
						[global::Rocks.MemberIdentifier(2, "void NoGenerics(int @value, string @data)")]
						public void NoGenerics(int @value, string @data)
						{
							if (this.Expectations.handlers2.Count > 0)
							{
								var @foundMatch = false;
								
								foreach (var @handler in this.Expectations.handlers2)
								{
									if (@handler.@value.IsValid(@value!) &&
										@handler.@data.IsValid(@data!))
									{
										@foundMatch = true;
										@handler.CallCount++;
										@handler.Callback?.Invoke(@value!, @data!);
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
							if (this.Expectations.handlers3.Count > 0)
							{
								foreach (var @handler in this.Expectations.handlers3)
								{
									if (@handler.@value.IsValid(@value!) &&
										@handler.@data.IsValid(@data!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@value!, @data!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for int NoGenericsWithReturn(int @value, string @data)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for int NoGenericsWithReturn(int @value, string @data)");
						}
						
						private global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations Expectations { get; }
					}
					
					internal sealed class IHaveOpenGenericsOfGuidMethodExpectations
					{
						internal IHaveOpenGenericsOfGuidMethodExpectations(global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler0<T>, global::System.Action<T, string, global::System.Guid>> HasGenerics<T>(global::Rocks.Argument<T> @value, global::Rocks.Argument<string> @data, global::Rocks.Argument<global::System.Guid> @information)
						{
							global::System.ArgumentNullException.ThrowIfNull(@value);
							global::System.ArgumentNullException.ThrowIfNull(@data);
							global::System.ArgumentNullException.ThrowIfNull(@information);
							
							var handler = new global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler0<T>
							{
								@value = @value,
								@data = @data,
								@information = @information,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler1<T>, global::System.Func<T, string, global::System.Guid, T>, T> HasGenericsWithReturn<T>(global::Rocks.Argument<T> @value, global::Rocks.Argument<string> @data, global::Rocks.Argument<global::System.Guid> @information)
						{
							global::System.ArgumentNullException.ThrowIfNull(@value);
							global::System.ArgumentNullException.ThrowIfNull(@data);
							global::System.ArgumentNullException.ThrowIfNull(@information);
							
							var handler = new global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler1<T>
							{
								@value = @value,
								@data = @data,
								@information = @information,
							};
							
							this.Expectations.handlers1.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler2, global::System.Action<int, string>> NoGenerics(global::Rocks.Argument<int> @value, global::Rocks.Argument<string> @data)
						{
							global::System.ArgumentNullException.ThrowIfNull(@value);
							global::System.ArgumentNullException.ThrowIfNull(@data);
							
							var handler = new global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler2
							{
								@value = @value,
								@data = @data,
							};
							
							this.Expectations.handlers2.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler3, global::System.Func<int, string, int>, int> NoGenericsWithReturn(global::Rocks.Argument<int> @value, global::Rocks.Argument<string> @data)
						{
							global::System.ArgumentNullException.ThrowIfNull(@value);
							global::System.ArgumentNullException.ThrowIfNull(@data);
							
							var handler = new global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler3
							{
								@value = @value,
								@data = @data,
							};
							
							this.Expectations.handlers3.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.IHaveOpenGenericsOfGuidMethodExpectations Methods { get; }
					
					internal IHaveOpenGenericsOfGuidCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IHaveOpenGenerics<global::System.Guid> Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockIHaveOpenGenericsOfGuid(this);
							this.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.IHaveOpenGenericsSystem.Guid_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}
}