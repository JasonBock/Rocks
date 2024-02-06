﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class DoesNotReturnGeneratorTests
{
	[Test]
	public static async Task GenerateClassCreateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Diagnostics.CodeAnalysis;

			[assembly: RockCreate<MockTests.ClassTest>]

			namespace MockTests
			{
				public class ClassTest
				{
					[DoesNotReturn]
					public virtual void VoidMethod() => throw new NotSupportedException();

					[DoesNotReturn]
					public virtual int IntMethod() => throw new NotSupportedException();
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
					internal sealed class Handler4
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					private global::Rocks.Handlers<global::MockTests.ClassTestCreateExpectations.Handler4>? @handlers4;
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
							if (this.handlers4 is not null) { failures.AddRange(this.Verify(this.handlers4, 4)); }
					
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
						
						[global::System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute]
						[global::Rocks.MemberIdentifier(3, "void VoidMethod()")]
						public override void VoidMethod()
						{
							if (this.Expectations.handlers3 is not null)
							{
								var @handler = this.Expectations.handlers3.First;
								@handler.CallCount++;
								@handler.Callback?.Invoke();
								throw new global::Rocks.Exceptions.DoesNotReturnException();
							}
							else
							{
								base.VoidMethod();
								throw new global::Rocks.Exceptions.DoesNotReturnException();
							}
						}
						
						[global::System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute]
						[global::Rocks.MemberIdentifier(4, "int IntMethod()")]
						public override int IntMethod()
						{
							if (this.Expectations.handlers4 is not null)
							{
								var @handler = this.Expectations.handlers4.First;
								@handler.CallCount++;
								_ = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								throw new global::Rocks.Exceptions.DoesNotReturnException();
							}
							else
							{
								_ = base.IntMethod();
								throw new global::Rocks.Exceptions.DoesNotReturnException();
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
						
						internal global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler3, global::System.Action> VoidMethod()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler3();
							if (this.Expectations.handlers3 is null ) { this.Expectations.handlers3 = new(handler); }
							else { this.Expectations.handlers3.Add(handler); }
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler4, global::System.Func<int>, int> IntMethod()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler4();
							if (this.Expectations.handlers4 is null ) { this.Expectations.handlers4 = new(handler); }
							else { this.Expectations.handlers4.Add(handler); }
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
	public static async Task GenerateClassMakeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Diagnostics.CodeAnalysis;

			[assembly: RockMake<MockTests.ClassTest>]

			namespace MockTests
			{
				public class ClassTest
				{
					[DoesNotReturn]
					public virtual void VoidMethod() => throw new NotSupportedException();

					[DoesNotReturn]
					public virtual int IntMethod() => throw new NotSupportedException();
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
						[global::System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute]
						public override void VoidMethod()
						{
							throw new global::Rocks.Exceptions.DoesNotReturnException();
						}
						[global::System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute]
						public override int IntMethod()
						{
							throw new global::Rocks.Exceptions.DoesNotReturnException();
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
	public static async Task GenerateInterfaceCreateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Diagnostics.CodeAnalysis;

			[assembly: RockCreate<MockTests.IInterfaceTest>]

			namespace MockTests
			{
				public interface IInterfaceTest
				{
					[DoesNotReturn]
					void VoidMethod();

					[DoesNotReturn]
					int IntMethod();
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class IInterfaceTestCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Action>
					{ }
					private global::Rocks.Handlers<global::MockTests.IInterfaceTestCreateExpectations.Handler0>? @handlers0;
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					private global::Rocks.Handlers<global::MockTests.IInterfaceTestCreateExpectations.Handler1>? @handlers1;
					
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
						: global::MockTests.IInterfaceTest
					{
						public Mock(global::MockTests.IInterfaceTestCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute]
						[global::Rocks.MemberIdentifier(0, "void VoidMethod()")]
						public void VoidMethod()
						{
							if (this.Expectations.handlers0 is not null)
							{
								var @handler = this.Expectations.handlers0.First;
								@handler.CallCount++;
								@handler.Callback?.Invoke();
								throw new global::Rocks.Exceptions.DoesNotReturnException();
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void VoidMethod()");
							}
						}
						
						[global::System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute]
						[global::Rocks.MemberIdentifier(1, "int IntMethod()")]
						public int IntMethod()
						{
							if (this.Expectations.handlers1 is not null)
							{
								var @handler = this.Expectations.handlers1.First;
								@handler.CallCount++;
								_ = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								throw new global::Rocks.Exceptions.DoesNotReturnException();
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for int IntMethod()");
						}
						
						private global::MockTests.IInterfaceTestCreateExpectations Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.IInterfaceTestCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.IInterfaceTestCreateExpectations.Handler0, global::System.Action> VoidMethod()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.IInterfaceTestCreateExpectations.Handler0();
							if (this.Expectations.handlers0 is null ) { this.Expectations.handlers0 = new(handler); }
							else { this.Expectations.handlers0.Add(handler); }
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.IInterfaceTestCreateExpectations.Handler1, global::System.Func<int>, int> IntMethod()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.IInterfaceTestCreateExpectations.Handler1();
							if (this.Expectations.handlers1 is null ) { this.Expectations.handlers1 = new(handler); }
							else { this.Expectations.handlers1.Add(handler); }
							return new(handler);
						}
						
						private global::MockTests.IInterfaceTestCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IInterfaceTestCreateExpectations.MethodExpectations Methods { get; }
					
					internal IInterfaceTestCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IInterfaceTest Instance()
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
			[(typeof(RockAttributeGenerator), "MockTests.IInterfaceTest_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateInterfaceMakeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Diagnostics.CodeAnalysis;

			[assembly: RockMake<MockTests.IInterfaceTest>]

			namespace MockTests
			{
				public interface IInterfaceTest
				{
					[DoesNotReturn]
					void VoidMethod();

					[DoesNotReturn]
					int IntMethod();
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
				internal sealed class IInterfaceTestMakeExpectations
				{
					internal global::MockTests.IInterfaceTest Instance()
					{
						return new Mock();
					}
					
					private sealed class Mock
						: global::MockTests.IInterfaceTest
					{
						public Mock()
						{
						}
						
						[global::System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute]
						public void VoidMethod()
						{
							throw new global::Rocks.Exceptions.DoesNotReturnException();
						}
						[global::System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute]
						public int IntMethod()
						{
							throw new global::Rocks.Exceptions.DoesNotReturnException();
						}
					}
				}
			}
			
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "MockTests.IInterfaceTest_Rock_Make.g.cs", generatedCode)],
			[]);
	}
}