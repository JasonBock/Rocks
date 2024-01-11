﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class VirtualsWithImplementationsGeneratorTests
{
	[Test]
	public static async Task GenerateForMethodWithParamsReturnsVoidAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<MockTests.VoidMethodWithParams>]

			namespace MockTests
			{
				public class VoidMethodWithParams
				{
					public virtual void CallMe(params string[] values) { }
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class VoidMethodWithParamsCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<object?, bool>, bool>
					{
						public global::Rocks.Argument<object?> @obj { get; set; }
					}
					
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<string?>, string?>
					{ }
					
					internal sealed class Handler3
						: global::Rocks.Handler<global::System.Action<string[]>>
					{
						public global::Rocks.Argument<string[]> @values { get; set; }
					}
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.VoidMethodWithParamsCreateExpectations.Handler0> @handlers0 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.VoidMethodWithParamsCreateExpectations.Handler1> @handlers1 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.VoidMethodWithParamsCreateExpectations.Handler2> @handlers2 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.VoidMethodWithParamsCreateExpectations.Handler3> @handlers3 = new();
					
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
					
					private sealed class RockVoidMethodWithParams
						: global::MockTests.VoidMethodWithParams
					{
						public RockVoidMethodWithParams(global::MockTests.VoidMethodWithParamsCreateExpectations @expectations)
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
						
						[global::Rocks.MemberIdentifier(3, "void CallMe(params string[] @values)")]
						public override void CallMe(params string[] @values)
						{
							if (this.Expectations.handlers3.Count > 0)
							{
								var @foundMatch = false;
								
								foreach (var @handler in this.Expectations.handlers3)
								{
									if (@handler.@values.IsValid(@values!))
									{
										@foundMatch = true;
										@handler.CallCount++;
										@handler.Callback?.Invoke(@values!);
										break;
									}
								}
								
								if (!@foundMatch)
								{
									throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void CallMe(params string[] @values)");
								}
							}
							else
							{
								base.CallMe(values: @values!);
							}
						}
						
						private global::MockTests.VoidMethodWithParamsCreateExpectations Expectations { get; }
					}
					
					internal sealed class VoidMethodWithParamsMethodExpectations
					{
						internal VoidMethodWithParamsMethodExpectations(global::MockTests.VoidMethodWithParamsCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.VoidMethodWithParamsCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @obj)
						{
							global::System.ArgumentNullException.ThrowIfNull(@obj);
							
							var handler = new global::MockTests.VoidMethodWithParamsCreateExpectations.Handler0
							{
								@obj = @obj,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.VoidMethodWithParamsCreateExpectations.Handler1, global::System.Func<int>, int> GetHashCode()
						{
							var handler = new global::MockTests.VoidMethodWithParamsCreateExpectations.Handler1();
							this.Expectations.handlers1.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.VoidMethodWithParamsCreateExpectations.Handler2, global::System.Func<string?>, string?> ToString()
						{
							var handler = new global::MockTests.VoidMethodWithParamsCreateExpectations.Handler2();
							this.Expectations.handlers2.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.VoidMethodWithParamsCreateExpectations.Handler3, global::System.Action<string[]>> CallMe(global::Rocks.Argument<string[]> @values)
						{
							global::System.ArgumentNullException.ThrowIfNull(@values);
							
							var handler = new global::MockTests.VoidMethodWithParamsCreateExpectations.Handler3
							{
								@values = @values,
							};
							
							this.Expectations.handlers3.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.VoidMethodWithParamsCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.VoidMethodWithParamsCreateExpectations.VoidMethodWithParamsMethodExpectations Methods { get; }
					
					internal VoidMethodWithParamsCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.VoidMethodWithParams Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockVoidMethodWithParams(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.VoidMethodWithParams_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateForMethodWithParamsReturnsValueAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<MockTests.ValueMethodWithParams>]

			namespace MockTests
			{
				public class ValueMethodWithParams
				{
					public virtual int CallMe(params string[] values) => default;
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class ValueMethodWithParamsCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<object?, bool>, bool>
					{
						public global::Rocks.Argument<object?> @obj { get; set; }
					}
					
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<string?>, string?>
					{ }
					
					internal sealed class Handler3
						: global::Rocks.Handler<global::System.Func<string[], int>, int>
					{
						public global::Rocks.Argument<string[]> @values { get; set; }
					}
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.ValueMethodWithParamsCreateExpectations.Handler0> @handlers0 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.ValueMethodWithParamsCreateExpectations.Handler1> @handlers1 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.ValueMethodWithParamsCreateExpectations.Handler2> @handlers2 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.ValueMethodWithParamsCreateExpectations.Handler3> @handlers3 = new();
					
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
					
					private sealed class RockValueMethodWithParams
						: global::MockTests.ValueMethodWithParams
					{
						public RockValueMethodWithParams(global::MockTests.ValueMethodWithParamsCreateExpectations @expectations)
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
						
						[global::Rocks.MemberIdentifier(3, "int CallMe(params string[] @values)")]
						public override int CallMe(params string[] @values)
						{
							if (this.Expectations.handlers3.Count > 0)
							{
								foreach (var @handler in this.Expectations.handlers3)
								{
									if (@handler.@values.IsValid(@values!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@values!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for int CallMe(params string[] @values)");
							}
							else
							{
								return base.CallMe(values: @values!);
							}
						}
						
						private global::MockTests.ValueMethodWithParamsCreateExpectations Expectations { get; }
					}
					
					internal sealed class ValueMethodWithParamsMethodExpectations
					{
						internal ValueMethodWithParamsMethodExpectations(global::MockTests.ValueMethodWithParamsCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.ValueMethodWithParamsCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @obj)
						{
							global::System.ArgumentNullException.ThrowIfNull(@obj);
							
							var handler = new global::MockTests.ValueMethodWithParamsCreateExpectations.Handler0
							{
								@obj = @obj,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.ValueMethodWithParamsCreateExpectations.Handler1, global::System.Func<int>, int> GetHashCode()
						{
							var handler = new global::MockTests.ValueMethodWithParamsCreateExpectations.Handler1();
							this.Expectations.handlers1.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.ValueMethodWithParamsCreateExpectations.Handler2, global::System.Func<string?>, string?> ToString()
						{
							var handler = new global::MockTests.ValueMethodWithParamsCreateExpectations.Handler2();
							this.Expectations.handlers2.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.ValueMethodWithParamsCreateExpectations.Handler3, global::System.Func<string[], int>, int> CallMe(global::Rocks.Argument<string[]> @values)
						{
							global::System.ArgumentNullException.ThrowIfNull(@values);
							
							var handler = new global::MockTests.ValueMethodWithParamsCreateExpectations.Handler3
							{
								@values = @values,
							};
							
							this.Expectations.handlers3.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.ValueMethodWithParamsCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.ValueMethodWithParamsCreateExpectations.ValueMethodWithParamsMethodExpectations Methods { get; }
					
					internal ValueMethodWithParamsCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.ValueMethodWithParams Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockValueMethodWithParams(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.ValueMethodWithParams_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateForInterfaceReturnsVoidAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<MockTests.IHaveImplementation>]

			namespace MockTests
			{
				public interface IHaveImplementation
				{
					void Foo() { }
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class IHaveImplementationCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Action>
					{ }
					
					private readonly global::System.Collections.Generic.List<global::MockTests.IHaveImplementationCreateExpectations.Handler0> @handlers0 = new();
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							failures.AddRange(this.Verify(handlers0));
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class RockIHaveImplementation
						: global::MockTests.IHaveImplementation
					{
						private readonly global::MockTests.IHaveImplementation shimForIHaveImplementation;
						public RockIHaveImplementation(global::MockTests.IHaveImplementationCreateExpectations @expectations)
						{
							(this.Expectations, this.shimForIHaveImplementation) = (@expectations, new ShimIHaveImplementation43912089203065484038465384033944109657192660075(this));
						}
						
						[global::Rocks.MemberIdentifier(0, "void Foo()")]
						public void Foo()
						{
							if (this.Expectations.handlers0.Count > 0)
							{
								var @handler = this.Expectations.handlers0[0];
								@handler.CallCount++;
								@handler.Callback?.Invoke();
							}
							else
							{
								this.shimForIHaveImplementation.Foo();
							}
						}
						
						
						private sealed class ShimIHaveImplementation43912089203065484038465384033944109657192660075
							: global::MockTests.IHaveImplementation
						{
							private readonly RockIHaveImplementation mock;
							
							public ShimIHaveImplementation43912089203065484038465384033944109657192660075(RockIHaveImplementation @mock) =>
								this.mock = @mock;
						}
						private global::MockTests.IHaveImplementationCreateExpectations Expectations { get; }
					}
					
					internal sealed class IHaveImplementationMethodExpectations
					{
						internal IHaveImplementationMethodExpectations(global::MockTests.IHaveImplementationCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.IHaveImplementationCreateExpectations.Handler0, global::System.Action> Foo()
						{
							var handler = new global::MockTests.IHaveImplementationCreateExpectations.Handler0();
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.IHaveImplementationCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IHaveImplementationCreateExpectations.IHaveImplementationMethodExpectations Methods { get; }
					
					internal IHaveImplementationCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IHaveImplementation Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockIHaveImplementation(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.IHaveImplementation_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateForInterfaceReturnsValueAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<MockTests.IHaveImplementation>]

			namespace MockTests
			{
				public interface IHaveImplementation
				{
					int Foo() => 3;
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class IHaveImplementationCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					
					private readonly global::System.Collections.Generic.List<global::MockTests.IHaveImplementationCreateExpectations.Handler0> @handlers0 = new();
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							failures.AddRange(this.Verify(handlers0));
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class RockIHaveImplementation
						: global::MockTests.IHaveImplementation
					{
						private readonly global::MockTests.IHaveImplementation shimForIHaveImplementation;
						public RockIHaveImplementation(global::MockTests.IHaveImplementationCreateExpectations @expectations)
						{
							(this.Expectations, this.shimForIHaveImplementation) = (@expectations, new ShimIHaveImplementation43912089203065484038465384033944109657192660075(this));
						}
						
						[global::Rocks.MemberIdentifier(0, "int Foo()")]
						public int Foo()
						{
							if (this.Expectations.handlers0.Count > 0)
							{
								var @handler = this.Expectations.handlers0[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								return @result!;
							}
							else
							{
								return this.shimForIHaveImplementation.Foo();
							}
						}
						
						
						private sealed class ShimIHaveImplementation43912089203065484038465384033944109657192660075
							: global::MockTests.IHaveImplementation
						{
							private readonly RockIHaveImplementation mock;
							
							public ShimIHaveImplementation43912089203065484038465384033944109657192660075(RockIHaveImplementation @mock) =>
								this.mock = @mock;
						}
						private global::MockTests.IHaveImplementationCreateExpectations Expectations { get; }
					}
					
					internal sealed class IHaveImplementationMethodExpectations
					{
						internal IHaveImplementationMethodExpectations(global::MockTests.IHaveImplementationCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.IHaveImplementationCreateExpectations.Handler0, global::System.Func<int>, int> Foo()
						{
							var handler = new global::MockTests.IHaveImplementationCreateExpectations.Handler0();
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.IHaveImplementationCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IHaveImplementationCreateExpectations.IHaveImplementationMethodExpectations Methods { get; }
					
					internal IHaveImplementationCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IHaveImplementation Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockIHaveImplementation(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.IHaveImplementation_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateForClassReturnsVoidAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<MockTests.HaveImplementation>]

			namespace MockTests
			{
				public class HaveImplementation
				{
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
				internal sealed class HaveImplementationCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<object?, bool>, bool>
					{
						public global::Rocks.Argument<object?> @obj { get; set; }
					}
					
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<string?>, string?>
					{ }
					
					internal sealed class Handler3
						: global::Rocks.Handler<global::System.Action>
					{ }
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.HaveImplementationCreateExpectations.Handler0> @handlers0 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.HaveImplementationCreateExpectations.Handler1> @handlers1 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.HaveImplementationCreateExpectations.Handler2> @handlers2 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.HaveImplementationCreateExpectations.Handler3> @handlers3 = new();
					
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
					
					private sealed class RockHaveImplementation
						: global::MockTests.HaveImplementation
					{
						public RockHaveImplementation(global::MockTests.HaveImplementationCreateExpectations @expectations)
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
						
						private global::MockTests.HaveImplementationCreateExpectations Expectations { get; }
					}
					
					internal sealed class HaveImplementationMethodExpectations
					{
						internal HaveImplementationMethodExpectations(global::MockTests.HaveImplementationCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.HaveImplementationCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @obj)
						{
							global::System.ArgumentNullException.ThrowIfNull(@obj);
							
							var handler = new global::MockTests.HaveImplementationCreateExpectations.Handler0
							{
								@obj = @obj,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.HaveImplementationCreateExpectations.Handler1, global::System.Func<int>, int> GetHashCode()
						{
							var handler = new global::MockTests.HaveImplementationCreateExpectations.Handler1();
							this.Expectations.handlers1.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.HaveImplementationCreateExpectations.Handler2, global::System.Func<string?>, string?> ToString()
						{
							var handler = new global::MockTests.HaveImplementationCreateExpectations.Handler2();
							this.Expectations.handlers2.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.HaveImplementationCreateExpectations.Handler3, global::System.Action> Foo()
						{
							var handler = new global::MockTests.HaveImplementationCreateExpectations.Handler3();
							this.Expectations.handlers3.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.HaveImplementationCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.HaveImplementationCreateExpectations.HaveImplementationMethodExpectations Methods { get; }
					
					internal HaveImplementationCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.HaveImplementation Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockHaveImplementation(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.HaveImplementation_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateForClassReturnsValueAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<MockTests.HaveImplementation>]

			namespace MockTests
			{
				public class HaveImplementation
				{
					public virtual int Foo() => 3;
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class HaveImplementationCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<object?, bool>, bool>
					{
						public global::Rocks.Argument<object?> @obj { get; set; }
					}
					
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<string?>, string?>
					{ }
					
					internal sealed class Handler3
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.HaveImplementationCreateExpectations.Handler0> @handlers0 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.HaveImplementationCreateExpectations.Handler1> @handlers1 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.HaveImplementationCreateExpectations.Handler2> @handlers2 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.HaveImplementationCreateExpectations.Handler3> @handlers3 = new();
					
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
					
					private sealed class RockHaveImplementation
						: global::MockTests.HaveImplementation
					{
						public RockHaveImplementation(global::MockTests.HaveImplementationCreateExpectations @expectations)
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
						
						[global::Rocks.MemberIdentifier(3, "int Foo()")]
						public override int Foo()
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
								return base.Foo();
							}
						}
						
						private global::MockTests.HaveImplementationCreateExpectations Expectations { get; }
					}
					
					internal sealed class HaveImplementationMethodExpectations
					{
						internal HaveImplementationMethodExpectations(global::MockTests.HaveImplementationCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.HaveImplementationCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @obj)
						{
							global::System.ArgumentNullException.ThrowIfNull(@obj);
							
							var handler = new global::MockTests.HaveImplementationCreateExpectations.Handler0
							{
								@obj = @obj,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.HaveImplementationCreateExpectations.Handler1, global::System.Func<int>, int> GetHashCode()
						{
							var handler = new global::MockTests.HaveImplementationCreateExpectations.Handler1();
							this.Expectations.handlers1.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.HaveImplementationCreateExpectations.Handler2, global::System.Func<string?>, string?> ToString()
						{
							var handler = new global::MockTests.HaveImplementationCreateExpectations.Handler2();
							this.Expectations.handlers2.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.HaveImplementationCreateExpectations.Handler3, global::System.Func<int>, int> Foo()
						{
							var handler = new global::MockTests.HaveImplementationCreateExpectations.Handler3();
							this.Expectations.handlers3.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.HaveImplementationCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.HaveImplementationCreateExpectations.HaveImplementationMethodExpectations Methods { get; }
					
					internal HaveImplementationCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.HaveImplementation Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockHaveImplementation(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.HaveImplementation_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}
}