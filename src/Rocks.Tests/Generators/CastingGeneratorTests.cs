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
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			using Rocks.Extensions;
			
			namespace MockTests
			{
				internal sealed class IHaveOpenGenericsOfGuidCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0<T>
						: global::Rocks.Handler<global::System.Action<T, string, global::System.Guid>>
					{
						public global::Rocks.Argument<T> @value { get; set; }
						public global::Rocks.Argument<string> @data { get; set; }
						public global::Rocks.Argument<global::System.Guid> @information { get; set; }
					}
					private global::Rocks.Handlers<global::Rocks.Handler>? @handlers0;
					internal sealed class Handler1<T>
						: global::Rocks.Handler<global::System.Func<T, string, global::System.Guid, T>, T>
					{
						public global::Rocks.Argument<T> @value { get; set; }
						public global::Rocks.Argument<string> @data { get; set; }
						public global::Rocks.Argument<global::System.Guid> @information { get; set; }
					}
					private global::Rocks.Handlers<global::Rocks.Handler>? @handlers1;
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Action<int, string>>
					{
						public global::Rocks.Argument<int> @value { get; set; }
						public global::Rocks.Argument<string> @data { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler2>? @handlers2;
					internal sealed class Handler3
						: global::Rocks.Handler<global::System.Func<int, string, int>, int>
					{
						public global::Rocks.Argument<int> @value { get; set; }
						public global::Rocks.Argument<string> @data { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler3>? @handlers3;
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							if (this.handlers0 is not null) { failures.AddRange(this.Verify(this.handlers0, 0)); }
							if (this.handlers1 is not null) { failures.AddRange(this.Verify(this.handlers1, 1)); }
							if (this.handlers2 is not null) { failures.AddRange(this.Verify(this.handlers2, 2)); }
							if (this.handlers3 is not null) { failures.AddRange(this.Verify(this.handlers3, 3)); }
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class Mock
						: global::MockTests.IHaveOpenGenerics<global::System.Guid>
					{
						public Mock(global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0)]
						public void HasGenerics<T>(T @value, string @data, global::System.Guid @information)
						{
							if (this.Expectations.handlers0 is not null)
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
									throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(0)}");
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
							}
						}
						
						[global::Rocks.MemberIdentifier(1)]
						public T HasGenericsWithReturn<T>(T @value, string @data, global::System.Guid @information)
						{
							if (this.Expectations.handlers1 is not null)
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
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(1)}");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(1)}");
						}
						
						[global::Rocks.MemberIdentifier(2)]
						public void NoGenerics(int @value, string @data)
						{
							if (this.Expectations.handlers2 is not null)
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
									throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(2)}");
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(2)}");
							}
						}
						
						[global::Rocks.MemberIdentifier(3)]
						public int NoGenericsWithReturn(int @value, string @data)
						{
							if (this.Expectations.handlers3 is not null)
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
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(3)}");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(3)}");
						}
						
						private global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Adornments.AdornmentsForHandler0<T> HasGenerics<T>(global::Rocks.Argument<T> @value, global::Rocks.Argument<string> @data, global::Rocks.Argument<global::System.Guid> @information)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@value);
							global::System.ArgumentNullException.ThrowIfNull(@data);
							global::System.ArgumentNullException.ThrowIfNull(@information);
							
							var @handler = new global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler0<T>
							{
								@value = @value,
								@data = @data,
								@information = @information,
							};
							
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						
						internal global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Adornments.AdornmentsForHandler1<T> HasGenericsWithReturn<T>(global::Rocks.Argument<T> @value, global::Rocks.Argument<string> @data, global::Rocks.Argument<global::System.Guid> @information)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@value);
							global::System.ArgumentNullException.ThrowIfNull(@data);
							global::System.ArgumentNullException.ThrowIfNull(@information);
							
							var @handler = new global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler1<T>
							{
								@value = @value,
								@data = @data,
								@information = @information,
							};
							
							if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(@handler); }
							else { this.Expectations.handlers1.Add(@handler); }
							return new(@handler);
						}
						
						internal global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Adornments.AdornmentsForHandler2 NoGenerics(global::Rocks.Argument<int> @value, global::Rocks.Argument<string> @data)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@value);
							global::System.ArgumentNullException.ThrowIfNull(@data);
							
							var @handler = new global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler2
							{
								@value = @value,
								@data = @data,
							};
							
							if (this.Expectations.handlers2 is null) { this.Expectations.handlers2 = new(@handler); }
							else { this.Expectations.handlers2.Add(@handler); }
							return new(@handler);
						}
						
						internal global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Adornments.AdornmentsForHandler3 NoGenericsWithReturn(global::Rocks.Argument<int> @value, global::Rocks.Argument<string> @data)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@value);
							global::System.ArgumentNullException.ThrowIfNull(@data);
							
							var @handler = new global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler3
							{
								@value = @value,
								@data = @data,
							};
							
							if (this.Expectations.handlers3 is null) { this.Expectations.handlers3 = new(@handler); }
							else { this.Expectations.handlers3.Add(@handler); }
							return new(@handler);
						}
						
						private global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.MethodExpectations Methods { get; }
					
					internal IHaveOpenGenericsOfGuidCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IHaveOpenGenerics<global::System.Guid> Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new Mock(this);
							this.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					internal static class Adornments
					{
						public interface IAdornmentsForIHaveOpenGenericsOfGuid<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForIHaveOpenGenericsOfGuid<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0<T>
							: global::Rocks.Adornments<AdornmentsForHandler0<T>, global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler0<T>, global::System.Action<T, string, global::System.Guid>>, IAdornmentsForIHaveOpenGenericsOfGuid<AdornmentsForHandler0<T>>
						{
							public AdornmentsForHandler0(global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler0<T> handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler1<T>
							: global::Rocks.Adornments<AdornmentsForHandler1<T>, global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler1<T>, global::System.Func<T, string, global::System.Guid, T>, T>, IAdornmentsForIHaveOpenGenericsOfGuid<AdornmentsForHandler1<T>>
						{
							public AdornmentsForHandler1(global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler1<T> handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler2
							: global::Rocks.Adornments<AdornmentsForHandler2, global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler2, global::System.Action<int, string>>, IAdornmentsForIHaveOpenGenericsOfGuid<AdornmentsForHandler2>
						{
							public AdornmentsForHandler2(global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler2 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler3
							: global::Rocks.Adornments<AdornmentsForHandler3, global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler3, global::System.Func<int, string, int>, int>, IAdornmentsForIHaveOpenGenericsOfGuid<AdornmentsForHandler3>
						{
							public AdornmentsForHandler3(global::MockTests.IHaveOpenGenericsOfGuidCreateExpectations.Handler3 handler)
								: base(handler) { }
						}
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "MockTests.IHaveOpenGenericsSystem.Guid_Rock_Create.g.cs", generatedCode)],
			[]);
	}
}