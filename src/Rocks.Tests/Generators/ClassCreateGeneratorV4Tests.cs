﻿using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ClassCreateGeneratorV4Tests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<MockTests.ClassTest>]

			namespace MockTests
			{
				public class ClassTest
				{
					public ClassTest() { }

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
				internal sealed class ClassTestCreateExpectations
					: global::Rocks.Expectations.ExpectationsV4
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler0
						: global::Rocks.HandlerV4<global::System.Func<object?, bool>, bool>
					{
						public global::Rocks.Argument<object?> obj { get; set; }
					}
					
					internal sealed class Handler1
						: global::Rocks.HandlerV4<global::System.Func<int>, int>
					{ }
					
					internal sealed class Handler2
						: global::Rocks.HandlerV4<global::System.Func<string?>, string?>
					{ }
					
					internal sealed class Handler3
						: global::Rocks.HandlerV4<global::System.Action>
					{ }
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.ClassTestCreateExpectations.Handler0> @handlers0 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.ClassTestCreateExpectations.Handler1> @handlers1 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.ClassTestCreateExpectations.Handler2> @handlers2 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.ClassTestCreateExpectations.Handler3> @handlers3 = new();
					
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
					
					private sealed class RockClassTest
						: global::MockTests.ClassTest
					{
						public RockClassTest(global::MockTests.ClassTestCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0, "bool Equals(object? @obj)")]
						public override bool Equals(object? @obj)
						{
							if (this.Expectations.handlers0.Count > 0)
							{
								foreach (var @handler in this.Expectations.handlers0)
								{
									if (@handler.obj.IsValid(@obj!))
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
							if (this.Expectations.handlers1.Count > 0)
							{
								var @handler = this.Expectations.handlers1[0];
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
						
						[global::Rocks.MemberIdentifier(2, "string? ToString()")]
						public override string? ToString()
						{
							if (this.Expectations.handlers2.Count > 0)
							{
								var @handler = this.Expectations.handlers2[0];
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
						
						[global::Rocks.MemberIdentifier(3, "void Foo()")]
						public override void Foo()
						{
							if (this.Expectations.handlers3.Count > 0)
							{
								var @handler = this.Expectations.handlers3[0];
								@handler.CallCount++;
								@handler.Callback?.Invoke();
							}
							else
							{
								base.Foo();
							}
						}
						
						private global::MockTests.ClassTestCreateExpectations Expectations { get; }
					}
					
					internal sealed class ClassTestMethodExpectations
					{
						internal ClassTestMethodExpectations(global::MockTests.ClassTestCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.AdornmentsV4<global::MockTests.ClassTestCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @obj)
						{
							global::System.ArgumentNullException.ThrowIfNull(@obj);
							
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler0
							{
								obj = @obj,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.AdornmentsV4<global::MockTests.ClassTestCreateExpectations.Handler1, global::System.Func<int>, int> GetHashCode()
						{
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler1();
							this.Expectations.handlers1.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.AdornmentsV4<global::MockTests.ClassTestCreateExpectations.Handler2, global::System.Func<string?>, string?> ToString()
						{
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler2();
							this.Expectations.handlers2.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.AdornmentsV4<global::MockTests.ClassTestCreateExpectations.Handler3, global::System.Action> Foo()
						{
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler3();
							this.Expectations.handlers3.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.ClassTestCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.ClassTestCreateExpectations.ClassTestMethodExpectations Methods { get; }
					
					internal ClassTestCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.ClassTest Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockClassTest(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.ClassTest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}