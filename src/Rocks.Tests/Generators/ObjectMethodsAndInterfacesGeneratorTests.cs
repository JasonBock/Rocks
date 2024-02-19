﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ObjectMethodsAndInterfacesGeneratorTests
{
	[Test]
	public static async Task CreateWhenObjectMethodIsHiddenAsStaticAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<MockTests.StaticToString>]

			#nullable enable

			namespace MockTests
			{
				public class StaticToString
				{
					protected static new string ToString() => "c";   
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class StaticToStringCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
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
					#pragma warning restore CS8618
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
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
						
						[global::Rocks.MemberIdentifier(0, "bool Equals(object? @obj)")]
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
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool Equals(object? @obj)");
							}
							else
							{
								return base.Equals(obj: @obj!);
							}
						}
						
						[global::Rocks.MemberIdentifier(1, "int GetHashCode()")]
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
							
							if (this.Expectations.handlers0 is null ) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						
						internal new global::MockTests.StaticToStringCreateExpectations.Adornments.AdornmentsForHandler1 GetHashCode()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.StaticToStringCreateExpectations.Handler1();
							if (this.Expectations.handlers1 is null ) { this.Expectations.handlers1 = new(handler); }
							else { this.Expectations.handlers1.Add(handler); }
							return new(handler);
						}
						
						private global::MockTests.StaticToStringCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.StaticToStringCreateExpectations.MethodExpectations Methods { get; }
					
					internal StaticToStringCreateExpectations() =>
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
			
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "MockTests.StaticToString_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task MakeWhenObjectMethodIsHiddenAsStaticAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockMake<MockTests.StaticToString>]

			#nullable enable

			namespace MockTests
			{
				public class StaticToString
				{
					protected static new string ToString() => "c";   
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#pragma warning disable CS8775
			#nullable enable
			
			namespace MockTests
			{
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
			
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "MockTests.StaticToString_Rock_Make.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateWithExactMatchesCreateAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<MockTests.IMatchObject<object>>]

			#nullable enable

			namespace MockTests
			{
				public interface IMatchObject<T>
				{
					bool Equals(T? other);
					bool ReferenceEquals(T? objA, T? objB);
					T MemberwiseClone();
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class IMatchObjectOfobjectCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<object?, bool>, bool>
					{
						public global::Rocks.Argument<object?> @other { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler0>? @handlers0;
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<object?, object?, bool>, bool>
					{
						public global::Rocks.Argument<object?> @objA { get; set; }
						public global::Rocks.Argument<object?> @objB { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler1>? @handlers1;
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<object>, object>
					{ }
					private global::Rocks.Handlers<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler2>? @handlers2;
					#pragma warning restore CS8618
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
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
						: global::MockTests.IMatchObject<object>
					{
						public Mock(global::MockTests.IMatchObjectOfobjectCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0, "bool global::MockTests.IMatchObject<object>.Equals(object? @other)")]
						bool global::MockTests.IMatchObject<object>.Equals(object? @other)
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
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool global::MockTests.IMatchObject<object>.Equals(object? @other)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for bool global::MockTests.IMatchObject<object>.Equals(object? @other)");
						}
						
						[global::Rocks.MemberIdentifier(1, "bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)")]
						bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)
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
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)");
						}
						
						[global::Rocks.MemberIdentifier(2, "object global::MockTests.IMatchObject<object>.MemberwiseClone()")]
						object global::MockTests.IMatchObject<object>.MemberwiseClone()
						{
							if (this.Expectations.handlers2 is not null)
							{
								var @handler = this.Expectations.handlers2.First;
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for object global::MockTests.IMatchObject<object>.MemberwiseClone()");
						}
						
						private global::MockTests.IMatchObjectOfobjectCreateExpectations Expectations { get; }
					}
					
					internal sealed class ExplicitMethodExpectationsForIMatchObjectOfobject
					{
						internal ExplicitMethodExpectationsForIMatchObjectOfobject(global::MockTests.IMatchObjectOfobjectCreateExpectations expectations) =>
							this.Expectations = expectations;
					
						internal global::MockTests.IMatchObjectOfobjectCreateExpectations.Adornments.AdornmentsForHandler0 Equals(global::Rocks.Argument<object?> @other)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@other);
							
							var @handler = new global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler0
							{
								@other = @other,
							};
							
							if (this.Expectations.handlers0 is null ) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						
						internal global::MockTests.IMatchObjectOfobjectCreateExpectations.Adornments.AdornmentsForHandler1 ReferenceEquals(global::Rocks.Argument<object?> @objA, global::Rocks.Argument<object?> @objB)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@objA);
							global::System.ArgumentNullException.ThrowIfNull(@objB);
							
							var @handler = new global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler1
							{
								@objA = @objA,
								@objB = @objB,
							};
							
							if (this.Expectations.handlers1 is null ) { this.Expectations.handlers1 = new(@handler); }
							else { this.Expectations.handlers1.Add(@handler); }
							return new(@handler);
						}
						
						internal new global::MockTests.IMatchObjectOfobjectCreateExpectations.Adornments.AdornmentsForHandler2 MemberwiseClone()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler2();
							if (this.Expectations.handlers2 is null ) { this.Expectations.handlers2 = new(handler); }
							else { this.Expectations.handlers2.Add(handler); }
							return new(handler);
						}
						
						private global::MockTests.IMatchObjectOfobjectCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IMatchObjectOfobjectCreateExpectations.ExplicitMethodExpectationsForIMatchObjectOfobject ExplicitMethodsForIMatchObjectOfobject { get; }
					
					internal IMatchObjectOfobjectCreateExpectations() =>
						(this.ExplicitMethodsForIMatchObjectOfobject) = (new(this));
					
					internal global::MockTests.IMatchObject<object> Instance()
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
						public interface IAdornmentsForIMatchObjectOfobject<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForIMatchObjectOfobject<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler0, global::System.Func<object?, bool>, bool>, IAdornmentsForIMatchObjectOfobject<AdornmentsForHandler0>
						{ 
							public AdornmentsForHandler0(global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler0 handler)
								: base(handler) { }				
						}
						public sealed class AdornmentsForHandler1
							: global::Rocks.Adornments<AdornmentsForHandler1, global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler1, global::System.Func<object?, object?, bool>, bool>, IAdornmentsForIMatchObjectOfobject<AdornmentsForHandler1>
						{ 
							public AdornmentsForHandler1(global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler1 handler)
								: base(handler) { }				
						}
						public sealed class AdornmentsForHandler2
							: global::Rocks.Adornments<AdornmentsForHandler2, global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler2, global::System.Func<object>, object>, IAdornmentsForIMatchObjectOfobject<AdornmentsForHandler2>
						{ 
							public AdornmentsForHandler2(global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler2 handler)
								: base(handler) { }				
						}
					}
				}
			}
			
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "MockTests.IMatchObjectobject_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateWithExactMatchesMakeAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockMake<MockTests.IMatchObject<object>>]

			#nullable enable

			namespace MockTests
			{
				public interface IMatchObject<T>
				{
					bool Equals(T? other);
					bool ReferenceEquals(T? objA, T? objB);
					T MemberwiseClone();
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#pragma warning disable CS8775
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class IMatchObjectOfobjectMakeExpectations
				{
					internal global::MockTests.IMatchObject<object> Instance()
					{
						return new Mock();
					}
					
					private sealed class Mock
						: global::MockTests.IMatchObject<object>
					{
						public Mock()
						{
						}
						
						bool global::MockTests.IMatchObject<object>.Equals(object? @other)
						{
							return default!;
						}
						bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)
						{
							return default!;
						}
						object global::MockTests.IMatchObject<object>.MemberwiseClone()
						{
							return default!;
						}
					}
				}
			}
			
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "MockTests.IMatchObjectobject_Rock_Make.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateWithDiffByReturnTypeMatchesCreateAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<MockTests.IMatchObject<object>>]

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

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class IMatchObjectOfobjectCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<object?, string>, string>
					{
						public global::Rocks.Argument<object?> @other { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler0>? @handlers0;
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<object?, object?, int>, int>
					{
						public global::Rocks.Argument<object?> @objA { get; set; }
						public global::Rocks.Argument<object?> @objB { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler1>? @handlers1;
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<bool>, bool>
					{ }
					private global::Rocks.Handlers<global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler2>? @handlers2;
					#pragma warning restore CS8618
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
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
						: global::MockTests.IMatchObject<object>
					{
						public Mock(global::MockTests.IMatchObjectOfobjectCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0, "string global::MockTests.IMatchObject<object>.Equals(object? @other)")]
						string global::MockTests.IMatchObject<object>.Equals(object? @other)
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
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for string global::MockTests.IMatchObject<object>.Equals(object? @other)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for string global::MockTests.IMatchObject<object>.Equals(object? @other)");
						}
						
						[global::Rocks.MemberIdentifier(1, "int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)")]
						int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)
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
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)");
						}
						
						[global::Rocks.MemberIdentifier(2, "bool global::MockTests.IMatchObject<object>.MemberwiseClone()")]
						bool global::MockTests.IMatchObject<object>.MemberwiseClone()
						{
							if (this.Expectations.handlers2 is not null)
							{
								var @handler = this.Expectations.handlers2.First;
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for bool global::MockTests.IMatchObject<object>.MemberwiseClone()");
						}
						
						private global::MockTests.IMatchObjectOfobjectCreateExpectations Expectations { get; }
					}
					
					internal sealed class ExplicitMethodExpectationsForIMatchObjectOfobject
					{
						internal ExplicitMethodExpectationsForIMatchObjectOfobject(global::MockTests.IMatchObjectOfobjectCreateExpectations expectations) =>
							this.Expectations = expectations;
					
						internal global::MockTests.IMatchObjectOfobjectCreateExpectations.Adornments.AdornmentsForHandler0 Equals(global::Rocks.Argument<object?> @other)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@other);
							
							var @handler = new global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler0
							{
								@other = @other,
							};
							
							if (this.Expectations.handlers0 is null ) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						
						internal global::MockTests.IMatchObjectOfobjectCreateExpectations.Adornments.AdornmentsForHandler1 ReferenceEquals(global::Rocks.Argument<object?> @objA, global::Rocks.Argument<object?> @objB)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@objA);
							global::System.ArgumentNullException.ThrowIfNull(@objB);
							
							var @handler = new global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler1
							{
								@objA = @objA,
								@objB = @objB,
							};
							
							if (this.Expectations.handlers1 is null ) { this.Expectations.handlers1 = new(@handler); }
							else { this.Expectations.handlers1.Add(@handler); }
							return new(@handler);
						}
						
						internal new global::MockTests.IMatchObjectOfobjectCreateExpectations.Adornments.AdornmentsForHandler2 MemberwiseClone()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler2();
							if (this.Expectations.handlers2 is null ) { this.Expectations.handlers2 = new(handler); }
							else { this.Expectations.handlers2.Add(handler); }
							return new(handler);
						}
						
						private global::MockTests.IMatchObjectOfobjectCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IMatchObjectOfobjectCreateExpectations.ExplicitMethodExpectationsForIMatchObjectOfobject ExplicitMethodsForIMatchObjectOfobject { get; }
					
					internal IMatchObjectOfobjectCreateExpectations() =>
						(this.ExplicitMethodsForIMatchObjectOfobject) = (new(this));
					
					internal global::MockTests.IMatchObject<object> Instance()
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
						public interface IAdornmentsForIMatchObjectOfobject<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForIMatchObjectOfobject<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler0, global::System.Func<object?, string>, string>, IAdornmentsForIMatchObjectOfobject<AdornmentsForHandler0>
						{ 
							public AdornmentsForHandler0(global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler0 handler)
								: base(handler) { }				
						}
						public sealed class AdornmentsForHandler1
							: global::Rocks.Adornments<AdornmentsForHandler1, global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler1, global::System.Func<object?, object?, int>, int>, IAdornmentsForIMatchObjectOfobject<AdornmentsForHandler1>
						{ 
							public AdornmentsForHandler1(global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler1 handler)
								: base(handler) { }				
						}
						public sealed class AdornmentsForHandler2
							: global::Rocks.Adornments<AdornmentsForHandler2, global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler2, global::System.Func<bool>, bool>, IAdornmentsForIMatchObjectOfobject<AdornmentsForHandler2>
						{ 
							public AdornmentsForHandler2(global::MockTests.IMatchObjectOfobjectCreateExpectations.Handler2 handler)
								: base(handler) { }				
						}
					}
				}
			}
			
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "MockTests.IMatchObjectobject_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateWithDiffByReturnTypeMatchesMakeAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockMake<MockTests.IMatchObject<object>>]

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

		var generatedCode =
			"""
			// <auto-generated/>
			
			#pragma warning disable CS8775
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class IMatchObjectOfobjectMakeExpectations
				{
					internal global::MockTests.IMatchObject<object> Instance()
					{
						return new Mock();
					}
					
					private sealed class Mock
						: global::MockTests.IMatchObject<object>
					{
						public Mock()
						{
						}
						
						string global::MockTests.IMatchObject<object>.Equals(object? @other)
						{
							return default!;
						}
						int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)
						{
							return default!;
						}
						bool global::MockTests.IMatchObject<object>.MemberwiseClone()
						{
							return default!;
						}
					}
				}
			}
			
			#pragma warning restore CS8775

			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "MockTests.IMatchObjectobject_Rock_Make.g.cs", generatedCode)],
			[]);
	}
}