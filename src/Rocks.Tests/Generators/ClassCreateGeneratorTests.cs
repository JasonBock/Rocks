﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ClassCreateGeneratorTests
{
	[Test]
	public static async Task GenerateCreateAsync()
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
			
			namespace MockTests
			{
				internal sealed class ClassTestCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<object?, bool>, bool>
					{
						public global::Rocks.Argument<object?> @obj { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.ClassTestCreateExpectations.Handler0>? @handlers0;
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					private global::Rocks.Handlers<global::MockTests.ClassTestCreateExpectations.Handler1>? @handlers1;
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<string?>, string?>
					{ }
					private global::Rocks.Handlers<global::MockTests.ClassTestCreateExpectations.Handler2>? @handlers2;
					internal sealed class Handler3
						: global::Rocks.Handler<global::System.Action>
					{ }
					private global::Rocks.Handlers<global::MockTests.ClassTestCreateExpectations.Handler3>? @handlers3;
					#pragma warning restore CS8618
					
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
						: global::MockTests.ClassTest
					{
						public Mock(global::MockTests.ClassTestCreateExpectations @expectations)
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
						
						[global::Rocks.MemberIdentifier(2, "string? ToString()")]
						public override string? ToString()
						{
							if (this.Expectations.handlers2 is not null)
							{
								var @handler = this.Expectations.handlers2.First;
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
							if (this.Expectations.handlers3 is not null)
							{
								var @handler = this.Expectations.handlers3.First;
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
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.ClassTestCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @obj)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@obj);
							
							var @handler = new global::MockTests.ClassTestCreateExpectations.Handler0
							{
								@obj = @obj,
							};
							
							if (this.Expectations.handlers0 is null ) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler1, global::System.Func<int>, int> GetHashCode()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler1();
							if (this.Expectations.handlers1 is null ) { this.Expectations.handlers1 = new(handler); }
							else { this.Expectations.handlers1.Add(handler); }
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler2, global::System.Func<string?>, string?> ToString()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler2();
							if (this.Expectations.handlers2 is null ) { this.Expectations.handlers2 = new(handler); }
							else { this.Expectations.handlers2.Add(handler); }
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler3, global::System.Action> Foo()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler3();
							if (this.Expectations.handlers3 is null ) { this.Expectations.handlers3 = new(handler); }
							else { this.Expectations.handlers3.Add(handler); }
							return new(handler);
						}
						
						private global::MockTests.ClassTestCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.ClassTestCreateExpectations.MethodExpectations Methods { get; }
					
					internal ClassTestCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.ClassTest Instance()
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
				}
			}
			
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "MockTests.ClassTest_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateMakeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockMake<MockTests.ClassTest>]

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
			
			#pragma warning disable CS8775
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class ClassTestMakeExpectations
				{
					internal global::MockTests.ClassTest Instance()
					{
						return new Mock();
					}
					
					private sealed class Mock
						: global::MockTests.ClassTest
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
						public override string? ToString()
						{
							return default!;
						}
						public override void Foo()
						{
						}
					}
				}
			}
			
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "MockTests.ClassTest_Rock_Make.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateCreateAndMakeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<MockTests.ClassTest>]
			[assembly: RockMake<MockTests.ClassTest>]

			namespace MockTests
			{
				public class ClassTest
				{
					public ClassTest() { }

					public virtual void Foo() { }
				}
			}
			""";

		var generatedCreateCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class ClassTestCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<object?, bool>, bool>
					{
						public global::Rocks.Argument<object?> @obj { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.ClassTestCreateExpectations.Handler0>? @handlers0;
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					private global::Rocks.Handlers<global::MockTests.ClassTestCreateExpectations.Handler1>? @handlers1;
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<string?>, string?>
					{ }
					private global::Rocks.Handlers<global::MockTests.ClassTestCreateExpectations.Handler2>? @handlers2;
					internal sealed class Handler3
						: global::Rocks.Handler<global::System.Action>
					{ }
					private global::Rocks.Handlers<global::MockTests.ClassTestCreateExpectations.Handler3>? @handlers3;
					#pragma warning restore CS8618
					
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
						: global::MockTests.ClassTest
					{
						public Mock(global::MockTests.ClassTestCreateExpectations @expectations)
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
						
						[global::Rocks.MemberIdentifier(2, "string? ToString()")]
						public override string? ToString()
						{
							if (this.Expectations.handlers2 is not null)
							{
								var @handler = this.Expectations.handlers2.First;
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
							if (this.Expectations.handlers3 is not null)
							{
								var @handler = this.Expectations.handlers3.First;
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
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.ClassTestCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @obj)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@obj);
							
							var @handler = new global::MockTests.ClassTestCreateExpectations.Handler0
							{
								@obj = @obj,
							};
							
							if (this.Expectations.handlers0 is null ) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler1, global::System.Func<int>, int> GetHashCode()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler1();
							if (this.Expectations.handlers1 is null ) { this.Expectations.handlers1 = new(handler); }
							else { this.Expectations.handlers1.Add(handler); }
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler2, global::System.Func<string?>, string?> ToString()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler2();
							if (this.Expectations.handlers2 is null ) { this.Expectations.handlers2 = new(handler); }
							else { this.Expectations.handlers2.Add(handler); }
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler3, global::System.Action> Foo()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler3();
							if (this.Expectations.handlers3 is null ) { this.Expectations.handlers3 = new(handler); }
							else { this.Expectations.handlers3.Add(handler); }
							return new(handler);
						}
						
						private global::MockTests.ClassTestCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.ClassTestCreateExpectations.MethodExpectations Methods { get; }
					
					internal ClassTestCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.ClassTest Instance()
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
				}
			}
			
			""";

		var generatedMakeCode =
			"""
			// <auto-generated/>
			
			#pragma warning disable CS8775
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class ClassTestMakeExpectations
				{
					internal global::MockTests.ClassTest Instance()
					{
						return new Mock();
					}
					
					private sealed class Mock
						: global::MockTests.ClassTest
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
						public override string? ToString()
						{
							return default!;
						}
						public override void Foo()
						{
						}
					}
				}
			}
			
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			new[] 
			{ 
				(typeof(RockAttributeGenerator), "MockTests.ClassTest_Rock_Create.g.cs", generatedCreateCode),
				(typeof(RockAttributeGenerator), "MockTests.ClassTest_Rock_Make.g.cs", generatedMakeCode),
			},
			[]);
	}
}