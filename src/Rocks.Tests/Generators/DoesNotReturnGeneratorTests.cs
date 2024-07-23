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
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			using Rocks.Extensions;
			
			namespace MockTests
			{
				[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
				internal sealed class ClassTestCreateExpectations
					: global::Rocks.Expectations
				{
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
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(0)}");
							}
							else
							{
								return base.Equals(obj: @obj!);
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
						
						[global::Rocks.MemberIdentifier(2)]
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
						[global::Rocks.MemberIdentifier(3)]
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
						[global::Rocks.MemberIdentifier(4)]
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
						
						internal global::MockTests.ClassTestCreateExpectations.Adornments.AdornmentsForHandler0 Equals(global::Rocks.Argument<object?> @obj)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@obj);
							
							var @handler = new global::MockTests.ClassTestCreateExpectations.Handler0
							{
								@obj = @obj,
							};
							
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						
						internal new global::MockTests.ClassTestCreateExpectations.Adornments.AdornmentsForHandler1 GetHashCode()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler1();
							if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(handler); }
							else { this.Expectations.handlers1.Add(handler); }
							return new(handler);
						}
						
						internal new global::MockTests.ClassTestCreateExpectations.Adornments.AdornmentsForHandler2 ToString()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler2();
							if (this.Expectations.handlers2 is null) { this.Expectations.handlers2 = new(handler); }
							else { this.Expectations.handlers2.Add(handler); }
							return new(handler);
						}
						
						internal global::MockTests.ClassTestCreateExpectations.Adornments.AdornmentsForHandler3 VoidMethod()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler3();
							if (this.Expectations.handlers3 is null) { this.Expectations.handlers3 = new(handler); }
							else { this.Expectations.handlers3.Add(handler); }
							return new(handler);
						}
						
						internal global::MockTests.ClassTestCreateExpectations.Adornments.AdornmentsForHandler4 IntMethod()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.ClassTestCreateExpectations.Handler4();
							if (this.Expectations.handlers4 is null) { this.Expectations.handlers4 = new(handler); }
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
					
					internal static class Adornments
					{
						public interface IAdornmentsForClassTest<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForClassTest<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.ClassTestCreateExpectations.Handler0, global::System.Func<object?, bool>, bool>, IAdornmentsForClassTest<AdornmentsForHandler0>
						{
							public AdornmentsForHandler0(global::MockTests.ClassTestCreateExpectations.Handler0 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler1
							: global::Rocks.Adornments<AdornmentsForHandler1, global::MockTests.ClassTestCreateExpectations.Handler1, global::System.Func<int>, int>, IAdornmentsForClassTest<AdornmentsForHandler1>
						{
							public AdornmentsForHandler1(global::MockTests.ClassTestCreateExpectations.Handler1 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler2
							: global::Rocks.Adornments<AdornmentsForHandler2, global::MockTests.ClassTestCreateExpectations.Handler2, global::System.Func<string?>, string?>, IAdornmentsForClassTest<AdornmentsForHandler2>
						{
							public AdornmentsForHandler2(global::MockTests.ClassTestCreateExpectations.Handler2 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler3
							: global::Rocks.Adornments<AdornmentsForHandler3, global::MockTests.ClassTestCreateExpectations.Handler3, global::System.Action>, IAdornmentsForClassTest<AdornmentsForHandler3>
						{
							public AdornmentsForHandler3(global::MockTests.ClassTestCreateExpectations.Handler3 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler4
							: global::Rocks.Adornments<AdornmentsForHandler4, global::MockTests.ClassTestCreateExpectations.Handler4, global::System.Func<int>, int>, IAdornmentsForClassTest<AdornmentsForHandler4>
						{
							public AdornmentsForHandler4(global::MockTests.ClassTestCreateExpectations.Handler4 handler)
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
			[(typeof(RockGenerator), "MockTests.ClassTest_Rock_Create.g.cs", generatedCode)],
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
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			namespace MockTests
			{
				[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[(typeof(RockGenerator), "MockTests.ClassTest_Rock_Make.g.cs", generatedCode)],
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
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			using Rocks.Extensions;
			
			namespace MockTests
			{
				[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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
						[global::Rocks.MemberIdentifier(0)]
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
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
							}
						}
						
						[global::System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute]
						[global::Rocks.MemberIdentifier(1)]
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
							
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(1)}");
						}
						
						private global::MockTests.IInterfaceTestCreateExpectations Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.IInterfaceTestCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.IInterfaceTestCreateExpectations.Adornments.AdornmentsForHandler0 VoidMethod()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.IInterfaceTestCreateExpectations.Handler0();
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(handler); }
							else { this.Expectations.handlers0.Add(handler); }
							return new(handler);
						}
						
						internal global::MockTests.IInterfaceTestCreateExpectations.Adornments.AdornmentsForHandler1 IntMethod()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.IInterfaceTestCreateExpectations.Handler1();
							if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(handler); }
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
					
					internal static class Adornments
					{
						public interface IAdornmentsForIInterfaceTest<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForIInterfaceTest<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.IInterfaceTestCreateExpectations.Handler0, global::System.Action>, IAdornmentsForIInterfaceTest<AdornmentsForHandler0>
						{
							public AdornmentsForHandler0(global::MockTests.IInterfaceTestCreateExpectations.Handler0 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler1
							: global::Rocks.Adornments<AdornmentsForHandler1, global::MockTests.IInterfaceTestCreateExpectations.Handler1, global::System.Func<int>, int>, IAdornmentsForIInterfaceTest<AdornmentsForHandler1>
						{
							public AdornmentsForHandler1(global::MockTests.IInterfaceTestCreateExpectations.Handler1 handler)
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
			[(typeof(RockGenerator), "MockTests.IInterfaceTest_Rock_Create.g.cs", generatedCode)],
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
			
			#pragma warning disable CS8618
			#pragma warning disable CS8633
			#pragma warning disable CS8714
			#pragma warning disable CS8775
			
			#nullable enable
			
			namespace MockTests
			{
				[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[(typeof(RockGenerator), "MockTests.IInterfaceTest_Rock_Make.g.cs", generatedCode)],
			[]);
	}
}