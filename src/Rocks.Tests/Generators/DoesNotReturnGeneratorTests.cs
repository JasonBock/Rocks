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
			
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
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
					
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<string?>, string?>
					{ }
					
					internal sealed class Handler3
						: global::Rocks.Handler<global::System.Action>
					{ }
					
					internal sealed class Handler4
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.ClassTestCreateExpectations.Handler0> @handlers0 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.ClassTestCreateExpectations.Handler1> @handlers1 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.ClassTestCreateExpectations.Handler2> @handlers2 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.ClassTestCreateExpectations.Handler3> @handlers3 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.ClassTestCreateExpectations.Handler4> @handlers4 = new();
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							failures.AddRange(this.Verify(handlers0));
							failures.AddRange(this.Verify(handlers1));
							failures.AddRange(this.Verify(handlers2));
							failures.AddRange(this.Verify(handlers3));
							failures.AddRange(this.Verify(handlers4));
					
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
						
						[global::System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute]
						[global::Rocks.MemberIdentifier(3, "void VoidMethod()")]
						public override void VoidMethod()
						{
							if (this.Expectations.handlers3.Count > 0)
							{
								var @handler = this.Expectations.handlers3[0];
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
							if (this.Expectations.handlers4.Count > 0)
							{
								var @handler = this.Expectations.handlers4[0];
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
					
					internal sealed class ClassTestMethodExpectations
					{
						internal ClassTestMethodExpectations(global::MockTests.ClassTestCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @obj)
						{
							global::System.ArgumentNullException.ThrowIfNull(@obj);
							
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler0
							{
								@obj = @obj,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler1, global::System.Func<int>, int> GetHashCode()
						{
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler1();
							this.Expectations.handlers1.Add(handler);
							return new(handler);
						}
						
						internal new global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler2, global::System.Func<string?>, string?> ToString()
						{
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler2();
							this.Expectations.handlers2.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler3, global::System.Action> VoidMethod()
						{
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler3();
							this.Expectations.handlers3.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.ClassTestCreateExpectations.Handler4, global::System.Func<int>, int> IntMethod()
						{
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler4();
							this.Expectations.handlers4.Add(handler);
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
			[]).ConfigureAwait(false);
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
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class ClassTestMakeExpectations
				{
					internal global::MockTests.ClassTest Instance()
					{
						return new RockClassTest();
					}
					
					private sealed class RockClassTest
						: global::MockTests.ClassTest
					{
						public RockClassTest()
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
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.ClassTest_Rock_Make.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
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
			
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			namespace MockTests
			{
				internal sealed class IInterfaceTestCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Action>
					{ }
					
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					
					private readonly global::System.Collections.Generic.List<global::MockTests.IInterfaceTestCreateExpectations.Handler0> @handlers0 = new();
					private readonly global::System.Collections.Generic.List<global::MockTests.IInterfaceTestCreateExpectations.Handler1> @handlers1 = new();
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							failures.AddRange(this.Verify(handlers0));
							failures.AddRange(this.Verify(handlers1));
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class RockIInterfaceTest
						: global::MockTests.IInterfaceTest
					{
						public RockIInterfaceTest(global::MockTests.IInterfaceTestCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute]
						[global::Rocks.MemberIdentifier(0, "void VoidMethod()")]
						public void VoidMethod()
						{
							if (this.Expectations.handlers0.Count > 0)
							{
								var @handler = this.Expectations.handlers0[0];
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
							if (this.Expectations.handlers1.Count > 0)
							{
								var @handler = this.Expectations.handlers1[0];
								@handler.CallCount++;
								_ = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								throw new global::Rocks.Exceptions.DoesNotReturnException();
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for int IntMethod()");
						}
						
						private global::MockTests.IInterfaceTestCreateExpectations Expectations { get; }
					}
					
					internal sealed class IInterfaceTestMethodExpectations
					{
						internal IInterfaceTestMethodExpectations(global::MockTests.IInterfaceTestCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.IInterfaceTestCreateExpectations.Handler0, global::System.Action> VoidMethod()
						{
							var handler = new global::MockTests.IInterfaceTestCreateExpectations.Handler0();
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						internal global::Rocks.Adornments<global::MockTests.IInterfaceTestCreateExpectations.Handler1, global::System.Func<int>, int> IntMethod()
						{
							var handler = new global::MockTests.IInterfaceTestCreateExpectations.Handler1();
							this.Expectations.handlers1.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.IInterfaceTestCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IInterfaceTestCreateExpectations.IInterfaceTestMethodExpectations Methods { get; }
					
					internal IInterfaceTestCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IInterfaceTest Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockIInterfaceTest(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.IInterfaceTest_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
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
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class IInterfaceTestMakeExpectations
				{
					internal global::MockTests.IInterfaceTest Instance()
					{
						return new RockIInterfaceTest();
					}
					
					private sealed class RockIInterfaceTest
						: global::MockTests.IInterfaceTest
					{
						public RockIInterfaceTest()
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
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.IInterfaceTest_Rock_Make.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}
}