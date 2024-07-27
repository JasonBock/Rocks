﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class PropertyInitMakeGeneratorTests
{
	[Test]
	public static async Task GenerateWithInitAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: Rock(typeof(MockTests.ITest), BuildType.Create | BuildType.Make)]

			#nullable enable

			namespace MockTests
			{
				public interface ITest
				{
					int NonNullableValueType { get; init; }
					int? NullableValueType { get; init; }
					string NonNullableReferenceType { get; init; }
					string? NullableReferenceType { get; init; }
				}
			}
			""";

		var createGeneratedCode =
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
				internal sealed class ITestCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					private global::Rocks.Handlers<global::MockTests.ITestCreateExpectations.Handler0>? @handlers0;
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Action<int>>
					{
						public global::Rocks.Argument<int> @value { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.ITestCreateExpectations.Handler1>? @handlers1;
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<int?>, int?>
					{ }
					private global::Rocks.Handlers<global::MockTests.ITestCreateExpectations.Handler2>? @handlers2;
					internal sealed class Handler3
						: global::Rocks.Handler<global::System.Action<int?>>
					{
						public global::Rocks.Argument<int?> @value { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.ITestCreateExpectations.Handler3>? @handlers3;
					internal sealed class Handler4
						: global::Rocks.Handler<global::System.Func<string>, string>
					{ }
					private global::Rocks.Handlers<global::MockTests.ITestCreateExpectations.Handler4>? @handlers4;
					internal sealed class Handler5
						: global::Rocks.Handler<global::System.Action<string>>
					{
						public global::Rocks.Argument<string> @value { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.ITestCreateExpectations.Handler5>? @handlers5;
					internal sealed class Handler6
						: global::Rocks.Handler<global::System.Func<string?>, string?>
					{ }
					private global::Rocks.Handlers<global::MockTests.ITestCreateExpectations.Handler6>? @handlers6;
					internal sealed class Handler7
						: global::Rocks.Handler<global::System.Action<string?>>
					{
						public global::Rocks.Argument<string?> @value { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.ITestCreateExpectations.Handler7>? @handlers7;
					
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
							if (this.handlers5 is not null) { failures.AddRange(this.Verify(this.handlers5, 5)); }
							if (this.handlers6 is not null) { failures.AddRange(this.Verify(this.handlers6, 6)); }
							if (this.handlers7 is not null) { failures.AddRange(this.Verify(this.handlers7, 7)); }
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class Mock
						: global::MockTests.ITest
					{
						public Mock(global::MockTests.ITestCreateExpectations @expectations, ConstructorProperties? @constructorProperties)
						{
							this.Expectations = @expectations;
							if (@constructorProperties is not null)
							{
								this.NonNullableValueType = @constructorProperties.NonNullableValueType;
								this.NullableValueType = @constructorProperties.NullableValueType;
								this.NonNullableReferenceType = @constructorProperties.NonNullableReferenceType!;
								this.NullableReferenceType = @constructorProperties.NullableReferenceType;
							}
						}
						
						[global::Rocks.MemberIdentifier(0, global::Rocks.PropertyAccessor.Get)]
						[global::Rocks.MemberIdentifier(1, global::Rocks.PropertyAccessor.Set)]
						public int NonNullableValueType
						{
							get
							{
								if (this.Expectations.handlers0 is not null)
								{
									var @handler = this.Expectations.handlers0.First;
									@handler.CallCount++;
									var @result = @handler.Callback is not null ?
										@handler.Callback() : @handler.ReturnValue;
									return @result!;
								}
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)})");
							}
							init
							{
								if (this.Expectations.handlers1 is not null)
								{
									var @foundMatch = false;
									foreach (var @handler in this.Expectations.handlers1)
									{
										if (@handler.value.IsValid(value!))
										{
											@handler.CallCount++;
											@foundMatch = true;
											@handler.Callback?.Invoke(value!);
											
											if (!@foundMatch)
											{
												throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(1)}");
											}
											
											break;
										}
									}
								}
								else
								{
									throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(1)}");
								}
							}
						}
						[global::Rocks.MemberIdentifier(2, global::Rocks.PropertyAccessor.Get)]
						[global::Rocks.MemberIdentifier(3, global::Rocks.PropertyAccessor.Set)]
						public int? NullableValueType
						{
							get
							{
								if (this.Expectations.handlers2 is not null)
								{
									var @handler = this.Expectations.handlers2.First;
									@handler.CallCount++;
									var @result = @handler.Callback is not null ?
										@handler.Callback() : @handler.ReturnValue;
									return @result!;
								}
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(2)})");
							}
							init
							{
								if (this.Expectations.handlers3 is not null)
								{
									var @foundMatch = false;
									foreach (var @handler in this.Expectations.handlers3)
									{
										if (@handler.value.IsValid(value!))
										{
											@handler.CallCount++;
											@foundMatch = true;
											@handler.Callback?.Invoke(value!);
											
											if (!@foundMatch)
											{
												throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(3)}");
											}
											
											break;
										}
									}
								}
								else
								{
									throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(3)}");
								}
							}
						}
						[global::Rocks.MemberIdentifier(4, global::Rocks.PropertyAccessor.Get)]
						[global::Rocks.MemberIdentifier(5, global::Rocks.PropertyAccessor.Set)]
						public string NonNullableReferenceType
						{
							get
							{
								if (this.Expectations.handlers4 is not null)
								{
									var @handler = this.Expectations.handlers4.First;
									@handler.CallCount++;
									var @result = @handler.Callback is not null ?
										@handler.Callback() : @handler.ReturnValue;
									return @result!;
								}
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(4)})");
							}
							init
							{
								if (this.Expectations.handlers5 is not null)
								{
									var @foundMatch = false;
									foreach (var @handler in this.Expectations.handlers5)
									{
										if (@handler.value.IsValid(value!))
										{
											@handler.CallCount++;
											@foundMatch = true;
											@handler.Callback?.Invoke(value!);
											
											if (!@foundMatch)
											{
												throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(5)}");
											}
											
											break;
										}
									}
								}
								else
								{
									throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(5)}");
								}
							}
						}
						[global::Rocks.MemberIdentifier(6, global::Rocks.PropertyAccessor.Get)]
						[global::Rocks.MemberIdentifier(7, global::Rocks.PropertyAccessor.Set)]
						public string? NullableReferenceType
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
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(6)})");
							}
							init
							{
								if (this.Expectations.handlers7 is not null)
								{
									var @foundMatch = false;
									foreach (var @handler in this.Expectations.handlers7)
									{
										if (@handler.value.IsValid(value!))
										{
											@handler.CallCount++;
											@foundMatch = true;
											@handler.Callback?.Invoke(value!);
											
											if (!@foundMatch)
											{
												throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(7)}");
											}
											
											break;
										}
									}
								}
								else
								{
									throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(7)}");
								}
							}
						}
						
						private global::MockTests.ITestCreateExpectations Expectations { get; }
					}
					internal sealed class PropertyExpectations
					{
						internal sealed class PropertyGetterExpectations
						{
							internal PropertyGetterExpectations(global::MockTests.ITestCreateExpectations expectations) =>
								this.Expectations = expectations;
							
							internal global::MockTests.ITestCreateExpectations.Adornments.AdornmentsForHandler0 NonNullableValueType()
							{
								global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
								var handler = new global::MockTests.ITestCreateExpectations.Handler0();
								if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(handler); }
								else { this.Expectations.handlers0.Add(handler); }
								return new(handler);
							}
							internal global::MockTests.ITestCreateExpectations.Adornments.AdornmentsForHandler2 NullableValueType()
							{
								global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
								var handler = new global::MockTests.ITestCreateExpectations.Handler2();
								if (this.Expectations.handlers2 is null) { this.Expectations.handlers2 = new(handler); }
								else { this.Expectations.handlers2.Add(handler); }
								return new(handler);
							}
							internal global::MockTests.ITestCreateExpectations.Adornments.AdornmentsForHandler4 NonNullableReferenceType()
							{
								global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
								var handler = new global::MockTests.ITestCreateExpectations.Handler4();
								if (this.Expectations.handlers4 is null) { this.Expectations.handlers4 = new(handler); }
								else { this.Expectations.handlers4.Add(handler); }
								return new(handler);
							}
							internal global::MockTests.ITestCreateExpectations.Adornments.AdornmentsForHandler6 NullableReferenceType()
							{
								global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
								var handler = new global::MockTests.ITestCreateExpectations.Handler6();
								if (this.Expectations.handlers6 is null) { this.Expectations.handlers6 = new(handler); }
								else { this.Expectations.handlers6.Add(handler); }
								return new(handler);
							}
							private global::MockTests.ITestCreateExpectations Expectations { get; }
						}
						
						internal sealed class PropertyInitializerExpectations
						{
							internal PropertyInitializerExpectations(global::MockTests.ITestCreateExpectations expectations) =>
								this.Expectations = expectations;
							
							internal global::MockTests.ITestCreateExpectations.Adornments.AdornmentsForHandler1 NonNullableValueType(global::Rocks.Argument<int> @value)
							{
								global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
								global::System.ArgumentNullException.ThrowIfNull(@value);
							
								var handler = new global::MockTests.ITestCreateExpectations.Handler1
								{
									value = @value,
								};
							
								if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(handler); }
								else { this.Expectations.handlers1.Add(handler); }
								return new(handler);
							}
							internal global::MockTests.ITestCreateExpectations.Adornments.AdornmentsForHandler3 NullableValueType(global::Rocks.Argument<int?> @value)
							{
								global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
								global::System.ArgumentNullException.ThrowIfNull(@value);
							
								var handler = new global::MockTests.ITestCreateExpectations.Handler3
								{
									value = @value,
								};
							
								if (this.Expectations.handlers3 is null) { this.Expectations.handlers3 = new(handler); }
								else { this.Expectations.handlers3.Add(handler); }
								return new(handler);
							}
							internal global::MockTests.ITestCreateExpectations.Adornments.AdornmentsForHandler5 NonNullableReferenceType(global::Rocks.Argument<string> @value)
							{
								global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
								global::System.ArgumentNullException.ThrowIfNull(@value);
							
								var handler = new global::MockTests.ITestCreateExpectations.Handler5
								{
									value = @value,
								};
							
								if (this.Expectations.handlers5 is null) { this.Expectations.handlers5 = new(handler); }
								else { this.Expectations.handlers5.Add(handler); }
								return new(handler);
							}
							internal global::MockTests.ITestCreateExpectations.Adornments.AdornmentsForHandler7 NullableReferenceType(global::Rocks.Argument<string?> @value)
							{
								global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
								global::System.ArgumentNullException.ThrowIfNull(@value);
							
								var handler = new global::MockTests.ITestCreateExpectations.Handler7
								{
									value = @value,
								};
							
								if (this.Expectations.handlers7 is null) { this.Expectations.handlers7 = new(handler); }
								else { this.Expectations.handlers7.Add(handler); }
								return new(handler);
							}
							private global::MockTests.ITestCreateExpectations Expectations { get; }
						}
						
						internal PropertyExpectations(global::MockTests.ITestCreateExpectations expectations) =>
							(this.Getters, this.Initializers) = (new(expectations), new(expectations));
						
						internal global::MockTests.ITestCreateExpectations.PropertyExpectations.PropertyGetterExpectations Getters { get; }
						internal global::MockTests.ITestCreateExpectations.PropertyExpectations.PropertyInitializerExpectations Initializers { get; }
					}
					
					internal global::MockTests.ITestCreateExpectations.PropertyExpectations Properties { get; }
					
					internal ITestCreateExpectations() =>
						(this.Properties) = (new(this));
					
					internal sealed class ConstructorProperties
					{
						internal int NonNullableValueType { get; init; }
						internal int? NullableValueType { get; init; }
						internal string? NonNullableReferenceType { get; init; }
						internal string? NullableReferenceType { get; init; }
					}
					
					internal global::MockTests.ITest Instance(global::MockTests.ITestCreateExpectations.ConstructorProperties? @constructorProperties)
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new Mock(this, @constructorProperties);
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
						public interface IAdornmentsForITest<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForITest<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.ITestCreateExpectations.Handler0, global::System.Func<int>, int>, IAdornmentsForITest<AdornmentsForHandler0>
						{
							public AdornmentsForHandler0(global::MockTests.ITestCreateExpectations.Handler0 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler2
							: global::Rocks.Adornments<AdornmentsForHandler2, global::MockTests.ITestCreateExpectations.Handler2, global::System.Func<int?>, int?>, IAdornmentsForITest<AdornmentsForHandler2>
						{
							public AdornmentsForHandler2(global::MockTests.ITestCreateExpectations.Handler2 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler4
							: global::Rocks.Adornments<AdornmentsForHandler4, global::MockTests.ITestCreateExpectations.Handler4, global::System.Func<string>, string>, IAdornmentsForITest<AdornmentsForHandler4>
						{
							public AdornmentsForHandler4(global::MockTests.ITestCreateExpectations.Handler4 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler6
							: global::Rocks.Adornments<AdornmentsForHandler6, global::MockTests.ITestCreateExpectations.Handler6, global::System.Func<string?>, string?>, IAdornmentsForITest<AdornmentsForHandler6>
						{
							public AdornmentsForHandler6(global::MockTests.ITestCreateExpectations.Handler6 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler1
							: global::Rocks.Adornments<AdornmentsForHandler1, global::MockTests.ITestCreateExpectations.Handler1, global::System.Action<int>>, IAdornmentsForITest<AdornmentsForHandler1>
						{
							public AdornmentsForHandler1(global::MockTests.ITestCreateExpectations.Handler1 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler3
							: global::Rocks.Adornments<AdornmentsForHandler3, global::MockTests.ITestCreateExpectations.Handler3, global::System.Action<int?>>, IAdornmentsForITest<AdornmentsForHandler3>
						{
							public AdornmentsForHandler3(global::MockTests.ITestCreateExpectations.Handler3 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler5
							: global::Rocks.Adornments<AdornmentsForHandler5, global::MockTests.ITestCreateExpectations.Handler5, global::System.Action<string>>, IAdornmentsForITest<AdornmentsForHandler5>
						{
							public AdornmentsForHandler5(global::MockTests.ITestCreateExpectations.Handler5 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler7
							: global::Rocks.Adornments<AdornmentsForHandler7, global::MockTests.ITestCreateExpectations.Handler7, global::System.Action<string?>>, IAdornmentsForITest<AdornmentsForHandler7>
						{
							public AdornmentsForHandler7(global::MockTests.ITestCreateExpectations.Handler7 handler)
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
				internal sealed class ITestMakeExpectations
				{
					internal sealed class ConstructorProperties
					{
						internal int NonNullableValueType { get; init; }
						internal int? NullableValueType { get; init; }
						internal string? NonNullableReferenceType { get; init; }
						internal string? NullableReferenceType { get; init; }
					}
					
					internal global::MockTests.ITest Instance(global::MockTests.ITestMakeExpectations.ConstructorProperties? @constructorProperties)
					{
						return new Mock(@constructorProperties);
					}
					
					private sealed class Mock
						: global::MockTests.ITest
					{
						public Mock(ConstructorProperties? @constructorProperties)
						{
							if (@constructorProperties is not null)
							{
								this.NonNullableValueType = @constructorProperties.NonNullableValueType;
								this.NullableValueType = @constructorProperties.NullableValueType;
								this.NonNullableReferenceType = @constructorProperties.NonNullableReferenceType!;
								this.NullableReferenceType = @constructorProperties.NullableReferenceType;
							}
						}
						
						public int NonNullableValueType
						{
							get => default!;
							init { }
						}
						public int? NullableValueType
						{
							get => default!;
							init { }
						}
						public string NonNullableReferenceType
						{
							get => default!;
							init { }
						}
						public string? NullableReferenceType
						{
							get => default!;
							init { }
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
				(typeof(RockGenerator), "MockTests.ITest_Rock_Create.g.cs", createGeneratedCode),
				(typeof(RockGenerator), "MockTests.ITest_Rock_Make.g.cs", makeGeneratedCode)
			],
			[]);
	}

	[Test]
	public static async Task GenerateWithRequiredAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: Rock(typeof(MockTests.Test), BuildType.Create | BuildType.Make)]

			#nullable enable
			
			namespace MockTests
			{
				public class Test
				{
					public virtual void Foo() { }
					public required int NonNullableValueType { get; init; }
					public required int? NullableValueType { get; init; }
					public required string NonNullableReferenceType { get; init; }
					public required string? NullableReferenceType { get; init; }
				}
			}
			""";

		var createGeneratedCode =
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
				internal sealed class TestCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<object?, bool>, bool>
					{
						public global::Rocks.Argument<object?> @obj { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.TestCreateExpectations.Handler0>? @handlers0;
					internal sealed class Handler1
						: global::Rocks.Handler<global::System.Func<int>, int>
					{ }
					private global::Rocks.Handlers<global::MockTests.TestCreateExpectations.Handler1>? @handlers1;
					internal sealed class Handler2
						: global::Rocks.Handler<global::System.Func<string?>, string?>
					{ }
					private global::Rocks.Handlers<global::MockTests.TestCreateExpectations.Handler2>? @handlers2;
					internal sealed class Handler3
						: global::Rocks.Handler<global::System.Action>
					{ }
					private global::Rocks.Handlers<global::MockTests.TestCreateExpectations.Handler3>? @handlers3;
					
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
						: global::MockTests.Test
					{
						[global::System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
						public Mock(global::MockTests.TestCreateExpectations @expectations, ConstructorProperties @constructorProperties)
						{
							this.Expectations = @expectations;
							this.NonNullableValueType = @constructorProperties.NonNullableValueType;
							this.NullableValueType = @constructorProperties.NullableValueType;
							this.NonNullableReferenceType = @constructorProperties.NonNullableReferenceType!;
							this.NullableReferenceType = @constructorProperties.NullableReferenceType;
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
						
						[global::Rocks.MemberIdentifier(3)]
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
						
						private global::MockTests.TestCreateExpectations Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.TestCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.TestCreateExpectations.Adornments.AdornmentsForHandler0 Equals(global::Rocks.Argument<object?> @obj)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@obj);
							
							var @handler = new global::MockTests.TestCreateExpectations.Handler0
							{
								@obj = @obj,
							};
							
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						
						internal new global::MockTests.TestCreateExpectations.Adornments.AdornmentsForHandler1 GetHashCode()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.TestCreateExpectations.Handler1();
							if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(handler); }
							else { this.Expectations.handlers1.Add(handler); }
							return new(handler);
						}
						
						internal new global::MockTests.TestCreateExpectations.Adornments.AdornmentsForHandler2 ToString()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.TestCreateExpectations.Handler2();
							if (this.Expectations.handlers2 is null) { this.Expectations.handlers2 = new(handler); }
							else { this.Expectations.handlers2.Add(handler); }
							return new(handler);
						}
						
						internal global::MockTests.TestCreateExpectations.Adornments.AdornmentsForHandler3 Foo()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.TestCreateExpectations.Handler3();
							if (this.Expectations.handlers3 is null) { this.Expectations.handlers3 = new(handler); }
							else { this.Expectations.handlers3.Add(handler); }
							return new(handler);
						}
						
						private global::MockTests.TestCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.TestCreateExpectations.MethodExpectations Methods { get; }
					
					internal TestCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal sealed class ConstructorProperties
					{
						internal required int NonNullableValueType { get; init; }
						internal required int? NullableValueType { get; init; }
						internal required string? NonNullableReferenceType { get; init; }
						internal required string? NullableReferenceType { get; init; }
					}
					
					internal global::MockTests.Test Instance(global::MockTests.TestCreateExpectations.ConstructorProperties @constructorProperties)
					{
						if (@constructorProperties is null)
						{
							throw new global::System.ArgumentNullException(nameof(@constructorProperties));
						}
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new Mock(this, @constructorProperties);
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
						public interface IAdornmentsForTest<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForTest<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.TestCreateExpectations.Handler0, global::System.Func<object?, bool>, bool>, IAdornmentsForTest<AdornmentsForHandler0>
						{
							public AdornmentsForHandler0(global::MockTests.TestCreateExpectations.Handler0 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler1
							: global::Rocks.Adornments<AdornmentsForHandler1, global::MockTests.TestCreateExpectations.Handler1, global::System.Func<int>, int>, IAdornmentsForTest<AdornmentsForHandler1>
						{
							public AdornmentsForHandler1(global::MockTests.TestCreateExpectations.Handler1 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler2
							: global::Rocks.Adornments<AdornmentsForHandler2, global::MockTests.TestCreateExpectations.Handler2, global::System.Func<string?>, string?>, IAdornmentsForTest<AdornmentsForHandler2>
						{
							public AdornmentsForHandler2(global::MockTests.TestCreateExpectations.Handler2 handler)
								: base(handler) { }
						}
						public sealed class AdornmentsForHandler3
							: global::Rocks.Adornments<AdornmentsForHandler3, global::MockTests.TestCreateExpectations.Handler3, global::System.Action>, IAdornmentsForTest<AdornmentsForHandler3>
						{
							public AdornmentsForHandler3(global::MockTests.TestCreateExpectations.Handler3 handler)
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
				internal sealed class TestMakeExpectations
				{
					internal sealed class ConstructorProperties
					{
						internal required int NonNullableValueType { get; init; }
						internal required int? NullableValueType { get; init; }
						internal required string? NonNullableReferenceType { get; init; }
						internal required string? NullableReferenceType { get; init; }
					}
					
					internal global::MockTests.Test Instance(global::MockTests.TestMakeExpectations.ConstructorProperties @constructorProperties)
					{
						global::System.ArgumentNullException.ThrowIfNull(constructorProperties);
						return new Mock(@constructorProperties);
					}
					
					private sealed class Mock
						: global::MockTests.Test
					{
						[global::System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
						public Mock(ConstructorProperties @constructorProperties)
						{
							this.NonNullableValueType = @constructorProperties.NonNullableValueType;
							this.NullableValueType = @constructorProperties.NullableValueType;
							this.NonNullableReferenceType = @constructorProperties.NonNullableReferenceType!;
							this.NullableReferenceType = @constructorProperties.NullableReferenceType;
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
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			
			""";

		await TestAssistants.RunGeneratorAsync<RockGenerator>(code,
			[
				(typeof(RockGenerator), "MockTests.Test_Rock_Create.g.cs", createGeneratedCode),
				(typeof(RockGenerator), "MockTests.Test_Rock_Make.g.cs", makeGeneratedCode)
			],
			[]);
	}
}