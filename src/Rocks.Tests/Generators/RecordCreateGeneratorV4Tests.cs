﻿using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class RecordCreateGeneratorV4Tests
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
			
			#nullable enable
			
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			namespace MockTests
			{
				internal sealed class RecordTestCreateExpectations
					: global::Rocks.Expectations.ExpectationsV4
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler2
						: global::Rocks.HandlerV4<global::System.Action>
					{ }
					
					internal sealed class Handler3
						: global::Rocks.HandlerV4<global::System.Func<string>, string>
					{ }
					
					internal sealed class Handler4
						: global::Rocks.HandlerV4<global::System.Func<global::System.Text.StringBuilder, bool>, bool>
					{
						public global::Rocks.Argument<global::System.Text.StringBuilder> @builder { get; set; }
					}
					
					internal sealed class Handler5
						: global::Rocks.HandlerV4<global::System.Func<int>, int>
					{ }
					
					internal sealed class Handler6
						: global::Rocks.HandlerV4<global::System.Func<global::System.Type>, global::System.Type>
					{ }
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.RecordTestCreateExpectations.Handler2> @handlers2 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.RecordTestCreateExpectations.Handler3> @handlers3 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.RecordTestCreateExpectations.Handler4> @handlers4 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.RecordTestCreateExpectations.Handler5> @handlers5 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.RecordTestCreateExpectations.Handler6> @handlers6 = new();
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							failures.AddRange(this.Verify(handlers2));
							failures.AddRange(this.Verify(handlers3));
							failures.AddRange(this.Verify(handlers4));
							failures.AddRange(this.Verify(handlers5));
							failures.AddRange(this.Verify(handlers6));
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed record RockRecordTest
						: global::MockTests.RecordTest
					{
						public RockRecordTest(global::MockTests.RecordTestCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						public RockRecordTest(global::MockTests.RecordTestCreateExpectations @expectations, global::MockTests.RecordTest @original)
							: base(@original)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(2, "void Foo()")]
						public override void Foo()
						{
							if (this.Expectations.handlers2.Count > 0)
							{
								var @handler = this.Expectations.handlers2[0];
								@handler.CallCount++;
								@handler.Callback?.Invoke();
							}
							else
							{
								base.Foo();
							}
						}
						
						[global::Rocks.MemberIdentifier(3, "string ToString()")]
						public override string ToString()
						{
							if (this.Expectations.handlers3.Count > 0)
							{
								var @handler = this.Expectations.handlers3[0];
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
						
						[global::Rocks.MemberIdentifier(4, "bool PrintMembers(global::System.Text.StringBuilder @builder)")]
						protected override bool PrintMembers(global::System.Text.StringBuilder @builder)
						{
							if (this.Expectations.handlers4.Count > 0)
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
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool PrintMembers(global::System.Text.StringBuilder @builder)");
							}
							else
							{
								return base.PrintMembers(builder: @builder!);
							}
						}
						
						[global::Rocks.MemberIdentifier(5, "int GetHashCode()")]
						public override int GetHashCode()
						{
							if (this.Expectations.handlers5.Count > 0)
							{
								var @handler = this.Expectations.handlers5[0];
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
						
						[global::Rocks.MemberIdentifier(6, "get_EqualityContract()")]
						protected override global::System.Type EqualityContract
						{
							get
							{
								if (this.Expectations.handlers6.Count > 0)
								{
									var @handler = this.Expectations.handlers6[0];
									@handler.CallCount++;
									var @result = @handler.Callback?.Invoke() ?? @handler.ReturnValue;
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
					
					internal sealed class RecordTestMethodExpectations
					{
						internal RecordTestMethodExpectations(global::MockTests.RecordTestCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.AdornmentsV4<global::MockTests.RecordTestCreateExpectations.Handler2, global::System.Action> Foo()
						{
							var handler = new global::MockTests.RecordTestCreateExpectations.Handler2();
							this.Expectations.handlers2.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.AdornmentsV4<global::MockTests.RecordTestCreateExpectations.Handler3, global::System.Func<string>, string> ToString()
						{
							var handler = new global::MockTests.RecordTestCreateExpectations.Handler3();
							this.Expectations.handlers3.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.AdornmentsV4<global::MockTests.RecordTestCreateExpectations.Handler4, global::System.Func<global::System.Text.StringBuilder, bool>, bool> PrintMembers(global::Rocks.Argument<global::System.Text.StringBuilder> @builder)
						{
							global::System.ArgumentNullException.ThrowIfNull(@builder);
							
							var handler = new global::MockTests.RecordTestCreateExpectations.Handler4
							{
								@builder = @builder,
							};
							
							this.Expectations.handlers4.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.AdornmentsV4<global::MockTests.RecordTestCreateExpectations.Handler5, global::System.Func<int>, int> GetHashCode()
						{
							var handler = new global::MockTests.RecordTestCreateExpectations.Handler5();
							this.Expectations.handlers5.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.RecordTestCreateExpectations Expectations { get; }
					}
					
					internal sealed class RecordTestPropertyExpectations
					{
						internal sealed class RecordTestPropertyGetterExpectations
						{
							internal RecordTestPropertyGetterExpectations(global::MockTests.RecordTestCreateExpectations expectations) =>
								this.Expectations = expectations;
							
							internal global::Rocks.AdornmentsV4<global::MockTests.RecordTestCreateExpectations.Handler6, global::System.Func<global::System.Type>, global::System.Type> EqualityContract()
							{
								var handler = new global::MockTests.RecordTestCreateExpectations.Handler6();
								this.Expectations.handlers6.Add(handler);
								return new(handler);
							}
							private global::MockTests.RecordTestCreateExpectations Expectations { get; }
						}
						
						
						internal RecordTestPropertyExpectations(global::MockTests.RecordTestCreateExpectations expectations) =>
							(this.Getters) = (new(expectations));
						
						internal global::MockTests.RecordTestCreateExpectations.RecordTestPropertyExpectations.RecordTestPropertyGetterExpectations Getters { get; }
					}
					
					internal global::MockTests.RecordTestCreateExpectations.RecordTestMethodExpectations Methods { get; }
					internal global::MockTests.RecordTestCreateExpectations.RecordTestPropertyExpectations Properties { get; }
					
					internal RecordTestCreateExpectations() =>
						(this.Methods, this.Properties) = (new(this), new(this));
					
					internal global::MockTests.RecordTest Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockRecordTest(this);
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
							var @mock = new RockRecordTest(this, @original);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.RecordTest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}