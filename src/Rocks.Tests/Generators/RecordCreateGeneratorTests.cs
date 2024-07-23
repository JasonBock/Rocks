﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class RecordCreateGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<MockTests.RecordTest>]

			namespace MockTests
			{
				public record RecordTest
				{
					public RecordTest() { }

					public virtual void Foo() { }
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
				[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
				internal sealed class RecordTestCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Action>
					{ }
					private global::Rocks.Handlers<global::MockTests.RecordTestCreateExpectations.Handler2>? @handlers2;
					internal sealed class Handler3
						: global::Rocks.Handler<global::System.Func<string>, string>
					{ }
					private global::Rocks.Handlers<global::MockTests.RecordTestCreateExpectations.Handler3>? @handlers3;
					internal sealed class Handler4
						: global::Rocks.Handler<global::System.Func<global::System.Text.StringBuilder, bool>, bool>
					{
						public global::Rocks.Argument<global::System.Text.StringBuilder> @builder { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.RecordTestCreateExpectations.Handler4>? @handlers4;
					internal sealed class Handler5
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					private global::Rocks.Handlers<global::MockTests.RecordTestCreateExpectations.Handler5>? @handlers5;
					internal sealed class Handler6
						: global::Rocks.Handler<global::System.Func<global::System.Type>, global::System.Type>
					{ }
					private global::Rocks.Handlers<global::MockTests.RecordTestCreateExpectations.Handler6>? @handlers6;
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							if (this.handlers2 is not null) { failures.AddRange(this.Verify(this.handlers2, 2)); }
							if (this.handlers3 is not null) { failures.AddRange(this.Verify(this.handlers3, 3)); }
							if (this.handlers4 is not null) { failures.AddRange(this.Verify(this.handlers4, 4)); }
							if (this.handlers5 is not null) { failures.AddRange(this.Verify(this.handlers5, 5)); }
							if (this.handlers6 is not null) { failures.AddRange(this.Verify(this.handlers6, 6)); }
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed record Mock
						: global::MockTests.RecordTest
					{
						public Mock(global::MockTests.RecordTestCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						public Mock(global::MockTests.RecordTestCreateExpectations @expectations, global::MockTests.RecordTest @original)
							: base(@original)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(2)]
						public override void Foo()
						{
							if (this.Expectations.handlers2 is not null)
							{
								var @handler = this.Expectations.handlers2.First;
								@handler.CallCount++;
								@handler.Callback?.Invoke();
							}
							else
							{
								base.Foo();
							}
						}
						
						[global::Rocks.MemberIdentifier(3)]
						public override string ToString()
						{
							if (this.Expectations.handlers3 is not null)
							{
								var @handler = this.Expectations.handlers3.First;
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							else
							{
								return base.ToString();
							}
						}
						
						[global::Rocks.MemberIdentifier(4)]
						protected override bool PrintMembers(global::System.Text.StringBuilder @builder)
						{
							if (this.Expectations.handlers4 is not null)
							{
								foreach (var @handler in this.Expectations.handlers4)
								{
									if (@handler.@builder.IsValid(@builder!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@builder!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(4)}");
							}
							else
							{
								return base.PrintMembers(builder: @builder!);
							}
						}
						
						[global::Rocks.MemberIdentifier(5)]
						public override int GetHashCode()
						{
							if (this.Expectations.handlers5 is not null)
							{
								var @handler = this.Expectations.handlers5.First;
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
						
						[global::Rocks.MemberIdentifier(6, global::Rocks.PropertyAccessor.Get)]
						protected override global::System.Type EqualityContract
						{
							get
							{
								if (this.Expectations.handlers6 is not null)
								{
									var @handler = this.Expectations.handlers6.First;
									@handler.CallCount++;
									var @result = @handler.Callback is not null ?
										@handler.Callback() : @handler.ReturnValue;
									return @result!;
								}
								else
								{
									return base.EqualityContract;
								}
							}
						}
						
						private global::MockTests.RecordTestCreateExpectations Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.RecordTestCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.RecordTestCreateExpectations.Adornments.AdornmentsForHandler2 Foo()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.RecordTestCreateExpectations.Handler2();
							if (this.Expectations.handlers2 is null) { this.Expectations.handlers2 = new(handler); }
							else { this.Expectations.handlers2.Add(handler); }
							return new(handler);
						}
						
						internal new global::MockTests.RecordTestCreateExpectations.Adornments.AdornmentsForHandler3 ToString()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.RecordTestCreateExpectations.Handler3();
							if (this.Expectations.handlers3 is null) { this.Expectations.handlers3 = new(handler); }
							else { this.Expectations.handlers3.Add(handler); }
							return new(handler);
						}
						
						internal global::MockTests.RecordTestCreateExpectations.Adornments.AdornmentsForHandler4 PrintMembers(global::Rocks.Argument<global::System.Text.StringBuilder> @builder)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@builder);
							
							var @handler = new global::MockTests.RecordTestCreateExpectations.Handler4
							{
								@builder = @builder,
							};
							
							if (this.Expectations.handlers4 is null) { this.Expectations.handlers4 = new(@handler); }
							else { this.Expectations.handlers4.Add(@handler); }
							return new(@handler);
						}
						
						internal new global::MockTests.RecordTestCreateExpectations.Adornments.AdornmentsForHandler5 GetHashCode()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.RecordTestCreateExpectations.Handler5();
							if (this.Expectations.handlers5 is null) { this.Expectations.handlers5 = new(handler); }
							else { this.Expectations.handlers5.Add(handler); }
							return new(handler);
						}
						
						private global::MockTests.RecordTestCreateExpectations Expectations { get; }
					}
					
					internal sealed class PropertyExpectations
					{
						internal sealed class PropertyGetterExpectations
						{
							internal PropertyGetterExpectations(global::MockTests.RecordTestCreateExpectations expectations) =>
								this.Expectations = expectations;
							
							internal global::MockTests.RecordTestCreateExpectations.Adornments.AdornmentsForHandler6 EqualityContract()
							{
								global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
								var handler = new global::MockTests.RecordTestCreateExpectations.Handler6();
								if (this.Expectations.handlers6 is null) { this.Expectations.handlers6 = new(handler); }
								else { this.Expectations.handlers6.Add(handler); }
								return new(handler);
							}
							private global::MockTests.RecordTestCreateExpectations Expectations { get; }
						}
						
						
						internal PropertyExpectations(global::MockTests.RecordTestCreateExpectations expectations) =>
							(this.Getters) = (new(expectations));
						
						internal global::MockTests.RecordTestCreateExpectations.PropertyExpectations.PropertyGetterExpectations Getters { get; }
					}
					
					internal global::MockTests.RecordTestCreateExpectations.MethodExpectations Methods { get; }
					internal global::MockTests.RecordTestCreateExpectations.PropertyExpectations Properties { get; }
					
					internal RecordTestCreateExpectations() =>
						(this.Methods, this.Properties) = (new(this), new(this));
					
					internal global::MockTests.RecordTest Instance()
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
					internal global::MockTests.RecordTest Instance(global::MockTests.RecordTest @original)
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new Mock(this, @original);
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
						public interface IAdornmentsForRecordTest<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForRecordTest<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler2
							: global::Rocks.Adornments<AdornmentsForHandler2, global::MockTests.RecordTestCreateExpectations.Handler2, global::System.Action>, IAdornmentsForRecordTest<AdornmentsForHandler2>
						{
							public AdornmentsForHandler2(global::MockTests.RecordTestCreateExpectations.Handler2 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler3
							: global::Rocks.Adornments<AdornmentsForHandler3, global::MockTests.RecordTestCreateExpectations.Handler3, global::System.Func<string>, string>, IAdornmentsForRecordTest<AdornmentsForHandler3>
						{
							public AdornmentsForHandler3(global::MockTests.RecordTestCreateExpectations.Handler3 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler4
							: global::Rocks.Adornments<AdornmentsForHandler4, global::MockTests.RecordTestCreateExpectations.Handler4, global::System.Func<global::System.Text.StringBuilder, bool>, bool>, IAdornmentsForRecordTest<AdornmentsForHandler4>
						{
							public AdornmentsForHandler4(global::MockTests.RecordTestCreateExpectations.Handler4 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler5
							: global::Rocks.Adornments<AdornmentsForHandler5, global::MockTests.RecordTestCreateExpectations.Handler5, global::System.Func<int>, int>, IAdornmentsForRecordTest<AdornmentsForHandler5>
						{
							public AdornmentsForHandler5(global::MockTests.RecordTestCreateExpectations.Handler5 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler6
							: global::Rocks.Adornments<AdornmentsForHandler6, global::MockTests.RecordTestCreateExpectations.Handler6, global::System.Func<global::System.Type>, global::System.Type>, IAdornmentsForRecordTest<AdornmentsForHandler6>
						{
							public AdornmentsForHandler6(global::MockTests.RecordTestCreateExpectations.Handler6 handler)
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

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[(typeof(RockGenerator), "MockTests.RecordTest_Rock_Create.g.cs", generatedCode)],
			[]);
	}
}