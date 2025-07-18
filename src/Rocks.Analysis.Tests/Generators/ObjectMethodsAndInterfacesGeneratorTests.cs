﻿using NUnit.Framework;

namespace Rocks.Analysis.Tests.Generators;

public static class ObjectMethodsAndInterfacesGeneratorTests
{
	[Test]
	public static async Task GenerateWhenObjectMethodIsHiddenAsStaticAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(MockTests.StaticToString), BuildType.Create | BuildType.Make)]

			#nullable enable

			namespace MockTests
			{
				public class StaticToString
				{
					protected static new string ToString() => "c";   
				}
			}
			""";

		var createGeneratedCode =
			""""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			using Rocks.Extensions;
			
			namespace MockTests
			{
				[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
				internal sealed class StaticToStringCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<object?, bool>, bool>
					{
						public global::Rocks.Argument<object?> @obj { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.StaticToStringCreateExpectations.Handler0>? @handlers0;
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					private global::Rocks.Handlers<global::MockTests.StaticToStringCreateExpectations.Handler1>? @handlers1;
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked && !this.WasExceptionThrown)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							if (this.handlers0 is not null) { failures.AddRange(this.Verify(this.handlers0, 0)); }
							if (this.handlers1 is not null) { failures.AddRange(this.Verify(this.handlers1, 1)); }
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class Mock
						: global::MockTests.StaticToString
					{
						public Mock(global::MockTests.StaticToStringCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0)]
						public override bool Equals(object? @obj)
						{
							if (this.Expectations.handlers0 is not null)
							{
								foreach (var @handler in this.Expectations.handlers0)
								{
									if (@handler.@obj.IsValid(@obj!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@obj!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								this.Expectations.WasExceptionThrown = true;
								throw new global::Rocks.Exceptions.ExpectationException(
									$"""
									No handlers match for {this.GetType().GetMemberDescription(0)}
										obj: {@obj.FormatValue()}
									""");
							}
							else
							{
								return base.Equals(@obj: @obj!);
							}
						}
						
						[global::Rocks.MemberIdentifier(1)]
						public override int GetHashCode()
						{
							if (this.Expectations.handlers1 is not null)
							{
								var @handler = this.Expectations.handlers1.First;
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							else
							{
								return base.GetHashCode();
							}
						}
						
						private global::MockTests.StaticToStringCreateExpectations Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.StaticToStringCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.StaticToStringCreateExpectations.Adornments.AdornmentsForHandler0 Equals(global::Rocks.Argument<object?> @obj)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@obj);
							
							var @handler = new global::MockTests.StaticToStringCreateExpectations.Handler0
							{
								@obj = @obj,
							};
							
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						
						internal new global::MockTests.StaticToStringCreateExpectations.Adornments.AdornmentsForHandler1 GetHashCode()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.StaticToStringCreateExpectations.Handler1();
							if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(handler); }
							else { this.Expectations.handlers1.Add(handler); }
							return new(handler);
						}
						
						private global::MockTests.StaticToStringCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.StaticToStringCreateExpectations.MethodExpectations Methods { get; }
					
					public StaticToStringCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.StaticToString Instance()
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
						public interface IAdornmentsForStaticToString<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForStaticToString<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.StaticToStringCreateExpectations.Handler0, global::System.Func<object?, bool>, bool>, IAdornmentsForStaticToString<AdornmentsForHandler0>
						{
							public AdornmentsForHandler0(global::MockTests.StaticToStringCreateExpectations.Handler0 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler1
							: global::Rocks.Adornments<AdornmentsForHandler1, global::MockTests.StaticToStringCreateExpectations.Handler1, global::System.Func<int>, int>, IAdornmentsForStaticToString<AdornmentsForHandler1>
						{
							public AdornmentsForHandler1(global::MockTests.StaticToStringCreateExpectations.Handler1 handler)
								: base(handler) { }
						}
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			"""";

		var makeGeneratedCode =
			"""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			namespace MockTests
			{
				[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
				internal sealed class StaticToStringMakeExpectations
				{
					internal global::MockTests.StaticToString Instance()
					{
						return new Mock();
					}
					
					private sealed class Mock
						: global::MockTests.StaticToString
					{
						public Mock()
						{
						}
						
						public override bool Equals(object? @obj)
						{
							return default!;
						}
						public override int GetHashCode()
						{
							return default!;
						}
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[
				("MockTests.StaticToString_Rock_Create.g.cs", createGeneratedCode),
				("MockTests.StaticToString_Rock_Make.g.cs", makeGeneratedCode)
			],
			[]);
	}

	[Test]
	public static async Task GenerateWithDiffByReturnTypeMatchesAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(MockTests.IMatchObject<>), BuildType.Create | BuildType.Make)]

			#nullable enable

			namespace MockTests
			{
				public interface IMatchObject<T>
				{
					string Equals(T? other);
					int ReferenceEquals(T? objA, T? objB);
					bool MemberwiseClone();
				}
			}
			""";

		var createGeneratedCode =
			""""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			using Rocks.Extensions;
			
			namespace MockTests
			{
				[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
				internal sealed class IMatchObjectCreateExpectations<T>
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<T?, string>, string>
					{
						public global::Rocks.Argument<T?> @other { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.IMatchObjectCreateExpectations<T>.Handler0>? @handlers0;
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<T?, T?, int>, int>
					{
						public global::Rocks.Argument<T?> @objA { get; set; }
						public global::Rocks.Argument<T?> @objB { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.IMatchObjectCreateExpectations<T>.Handler1>? @handlers1;
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<bool>, bool>
					{ }
					private global::Rocks.Handlers<global::MockTests.IMatchObjectCreateExpectations<T>.Handler2>? @handlers2;
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked && !this.WasExceptionThrown)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							if (this.handlers0 is not null) { failures.AddRange(this.Verify(this.handlers0, 0)); }
							if (this.handlers1 is not null) { failures.AddRange(this.Verify(this.handlers1, 1)); }
							if (this.handlers2 is not null) { failures.AddRange(this.Verify(this.handlers2, 2)); }
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class Mock
						: global::MockTests.IMatchObject<T>
					{
						public Mock(global::MockTests.IMatchObjectCreateExpectations<T> @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0)]
						public string Equals(T? @other)
						{
							if (this.Expectations.handlers0 is not null)
							{
								foreach (var @handler in this.Expectations.handlers0)
								{
									if (@handler.@other.IsValid(@other!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@other!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								this.Expectations.WasExceptionThrown = true;
								throw new global::Rocks.Exceptions.ExpectationException(
									$"""
									No handlers match for {this.GetType().GetMemberDescription(0)}
										other: {@other.FormatValue()}
									""");
							}
							
							this.Expectations.WasExceptionThrown = true;
							throw new global::Rocks.Exceptions.ExpectationException(
								$"""
								No handlers were found for {this.GetType().GetMemberDescription(0)}
									other: {@other.FormatValue()}
								""");
						}
						
						[global::Rocks.MemberIdentifier(1)]
						public int ReferenceEquals(T? @objA, T? @objB)
						{
							if (this.Expectations.handlers1 is not null)
							{
								foreach (var @handler in this.Expectations.handlers1)
								{
									if (@handler.@objA.IsValid(@objA!) &&
										@handler.@objB.IsValid(@objB!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@objA!, @objB!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								this.Expectations.WasExceptionThrown = true;
								throw new global::Rocks.Exceptions.ExpectationException(
									$"""
									No handlers match for {this.GetType().GetMemberDescription(1)}
										objA: {@objA.FormatValue()}
										objB: {@objB.FormatValue()}
									""");
							}
							
							this.Expectations.WasExceptionThrown = true;
							throw new global::Rocks.Exceptions.ExpectationException(
								$"""
								No handlers were found for {this.GetType().GetMemberDescription(1)}
									objA: {@objA.FormatValue()}
									objB: {@objB.FormatValue()}
								""");
						}
						
						[global::Rocks.MemberIdentifier(2)]
						bool global::MockTests.IMatchObject<T>.MemberwiseClone()
						{
							if (this.Expectations.handlers2 is not null)
							{
								var @handler = this.Expectations.handlers2.First;
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							
							this.Expectations.WasExceptionThrown = true;
							throw new global::Rocks.Exceptions.ExpectationException(
								$"""
								No handlers were found for {this.GetType().GetMemberDescription(2)}
								""");
						}
						
						private global::MockTests.IMatchObjectCreateExpectations<T> Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.IMatchObjectCreateExpectations<T> expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.IMatchObjectCreateExpectations<T>.Adornments.AdornmentsForHandler0 Equals(global::Rocks.Argument<T?> @other)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@other);
							
							var @handler = new global::MockTests.IMatchObjectCreateExpectations<T>.Handler0
							{
								@other = @other,
							};
							
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						
						internal global::MockTests.IMatchObjectCreateExpectations<T>.Adornments.AdornmentsForHandler1 ReferenceEquals(global::Rocks.Argument<T?> @objA, global::Rocks.Argument<T?> @objB)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@objA);
							global::System.ArgumentNullException.ThrowIfNull(@objB);
							
							var @handler = new global::MockTests.IMatchObjectCreateExpectations<T>.Handler1
							{
								@objA = @objA,
								@objB = @objB,
							};
							
							if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(@handler); }
							else { this.Expectations.handlers1.Add(@handler); }
							return new(@handler);
						}
						
						private global::MockTests.IMatchObjectCreateExpectations<T> Expectations { get; }
					}
					internal sealed class ExplicitMethodExpectationsForIMatchObjectOfT
					{
						internal ExplicitMethodExpectationsForIMatchObjectOfT(global::MockTests.IMatchObjectCreateExpectations<T> expectations) =>
							this.Expectations = expectations;
					
						internal new global::MockTests.IMatchObjectCreateExpectations<T>.Adornments.AdornmentsForHandler2 MemberwiseClone()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.IMatchObjectCreateExpectations<T>.Handler2();
							if (this.Expectations.handlers2 is null) { this.Expectations.handlers2 = new(handler); }
							else { this.Expectations.handlers2.Add(handler); }
							return new(handler);
						}
						
						private global::MockTests.IMatchObjectCreateExpectations<T> Expectations { get; }
					}
					
					internal global::MockTests.IMatchObjectCreateExpectations<T>.MethodExpectations Methods { get; }
					internal global::MockTests.IMatchObjectCreateExpectations<T>.ExplicitMethodExpectationsForIMatchObjectOfT ExplicitMethodsForIMatchObjectOfT { get; }
					
					public IMatchObjectCreateExpectations() =>
						(this.Methods, this.ExplicitMethodsForIMatchObjectOfT) = (new(this), new(this));
					
					internal global::MockTests.IMatchObject<T> Instance()
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
						public interface IAdornmentsForIMatchObject<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForIMatchObject<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.IMatchObjectCreateExpectations<T>.Handler0, global::System.Func<T?, string>, string>, IAdornmentsForIMatchObject<AdornmentsForHandler0>
						{
							public AdornmentsForHandler0(global::MockTests.IMatchObjectCreateExpectations<T>.Handler0 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler1
							: global::Rocks.Adornments<AdornmentsForHandler1, global::MockTests.IMatchObjectCreateExpectations<T>.Handler1, global::System.Func<T?, T?, int>, int>, IAdornmentsForIMatchObject<AdornmentsForHandler1>
						{
							public AdornmentsForHandler1(global::MockTests.IMatchObjectCreateExpectations<T>.Handler1 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler2
							: global::Rocks.Adornments<AdornmentsForHandler2, global::MockTests.IMatchObjectCreateExpectations<T>.Handler2, global::System.Func<bool>, bool>, IAdornmentsForIMatchObject<AdornmentsForHandler2>
						{
							public AdornmentsForHandler2(global::MockTests.IMatchObjectCreateExpectations<T>.Handler2 handler)
								: base(handler) { }
						}
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			"""";

		var makeGeneratedCode =
			"""
			// <auto-generated/>
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			namespace MockTests
			{
				[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
				internal sealed class IMatchObjectMakeExpectations<T>
				{
					internal global::MockTests.IMatchObject<T> Instance()
					{
						return new Mock();
					}
					
					private sealed class Mock
						: global::MockTests.IMatchObject<T>
					{
						public Mock()
						{
						}
						
						public string Equals(T? @other)
						{
							return default!;
						}
						public int ReferenceEquals(T? @objA, T? @objB)
						{
							return default!;
						}
						bool global::MockTests.IMatchObject<T>.MemberwiseClone()
						{
							return default!;
						}
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[
				("MockTests.IMatchObjectT_Rock_Create.g.cs", createGeneratedCode),
				("MockTests.IMatchObjectT_Rock_Make.g.cs", makeGeneratedCode)
			],
			[]);
	}
}