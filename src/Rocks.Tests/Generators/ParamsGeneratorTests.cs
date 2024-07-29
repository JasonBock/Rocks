﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ParamsGeneratorTests
{
	[Test]
	public static async Task GenerateWhenParamsIsRefStructAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: Rock(typeof(ParamMethods), BuildType.Create | BuildType.Make)]

			public class ParamMethods
			{
			  public virtual void Do(params string[] args) { }
			  public virtual void DoSpan(params ReadOnlySpan<string> args) { }
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
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class ParamMethodsCreateExpectations
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Func<object?, bool>, bool>
				{
					public global::Rocks.Argument<object?> @obj { get; set; }
				}
				private global::Rocks.Handlers<global::ParamMethodsCreateExpectations.Handler0>? @handlers0;
				internal sealed class Handler1
					: global::Rocks.Handler<global::System.Func<int>, int>
				{ }
				private global::Rocks.Handlers<global::ParamMethodsCreateExpectations.Handler1>? @handlers1;
				internal sealed class Handler2
					: global::Rocks.Handler<global::System.Func<string?>, string?>
				{ }
				private global::Rocks.Handlers<global::ParamMethodsCreateExpectations.Handler2>? @handlers2;
				internal sealed class Handler3
					: global::Rocks.Handler<global::System.Action<string[]>>
				{
					public global::Rocks.Argument<string[]> @args { get; set; }
				}
				private global::Rocks.Handlers<global::ParamMethodsCreateExpectations.Handler3>? @handlers3;
				internal sealed class Handler4
					: global::Rocks.Handler<global::System.Action<global::System.ReadOnlySpan<string>>>
				{
					public global::Rocks.RefStructArgument<global::System.ReadOnlySpan<string>> @args { get; set; }
				}
				private global::Rocks.Handlers<global::ParamMethodsCreateExpectations.Handler4>? @handlers4;
				
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
					: global::ParamMethods
				{
					public Mock(global::ParamMethodsCreateExpectations @expectations)
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
					
					[global::Rocks.MemberIdentifier(3)]
					public override void Do(params string[] @args)
					{
						if (this.Expectations.handlers3 is not null)
						{
							var @foundMatch = false;
							
							foreach (var @handler in this.Expectations.handlers3)
							{
								if (@handler.@args.IsValid(@args!))
								{
									@foundMatch = true;
									@handler.CallCount++;
									@handler.Callback?.Invoke(@args!);
									break;
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(3)}");
							}
						}
						else
						{
							base.Do(args: @args!);
						}
					}
					
					[global::Rocks.MemberIdentifier(4)]
					public override void DoSpan(params global::System.ReadOnlySpan<string> @args)
					{
						if (this.Expectations.handlers4 is not null)
						{
							var @foundMatch = false;
							
							foreach (var @handler in this.Expectations.handlers4)
							{
								if (@handler.@args.IsValid(@args!))
								{
									@foundMatch = true;
									@handler.CallCount++;
									@handler.Callback?.Invoke(@args!);
									break;
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(4)}");
							}
						}
						else
						{
							base.DoSpan(args: @args!);
						}
					}
					
					private global::ParamMethodsCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::ParamMethodsCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::ParamMethodsCreateExpectations.Adornments.AdornmentsForHandler0 Equals(global::Rocks.Argument<object?> @obj)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@obj);
						
						var @handler = new global::ParamMethodsCreateExpectations.Handler0
						{
							@obj = @obj,
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					
					internal new global::ParamMethodsCreateExpectations.Adornments.AdornmentsForHandler1 GetHashCode()
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						var handler = new global::ParamMethodsCreateExpectations.Handler1();
						if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(handler); }
						else { this.Expectations.handlers1.Add(handler); }
						return new(handler);
					}
					
					internal new global::ParamMethodsCreateExpectations.Adornments.AdornmentsForHandler2 ToString()
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						var handler = new global::ParamMethodsCreateExpectations.Handler2();
						if (this.Expectations.handlers2 is null) { this.Expectations.handlers2 = new(handler); }
						else { this.Expectations.handlers2.Add(handler); }
						return new(handler);
					}
					
					internal global::ParamMethodsCreateExpectations.Adornments.AdornmentsForHandler3 Do(global::Rocks.Argument<string[]> @args)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@args);
						
						var @handler = new global::ParamMethodsCreateExpectations.Handler3
						{
							@args = @args,
						};
						
						if (this.Expectations.handlers3 is null) { this.Expectations.handlers3 = new(@handler); }
						else { this.Expectations.handlers3.Add(@handler); }
						return new(@handler);
					}
					
					internal global::ParamMethodsCreateExpectations.Adornments.AdornmentsForHandler4 DoSpan(global::Rocks.RefStructArgument<global::System.ReadOnlySpan<string>> @args)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@args);
						
						var @handler = new global::ParamMethodsCreateExpectations.Handler4
						{
							@args = @args,
						};
						
						if (this.Expectations.handlers4 is null) { this.Expectations.handlers4 = new(@handler); }
						else { this.Expectations.handlers4.Add(@handler); }
						return new(@handler);
					}
					
					private global::ParamMethodsCreateExpectations Expectations { get; }
				}
				
				internal global::ParamMethodsCreateExpectations.MethodExpectations Methods { get; }
				
				internal ParamMethodsCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::ParamMethods Instance()
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
					public interface IAdornmentsForParamMethods<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForParamMethods<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::ParamMethodsCreateExpectations.Handler0, global::System.Func<object?, bool>, bool>, IAdornmentsForParamMethods<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::ParamMethodsCreateExpectations.Handler0 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler1
						: global::Rocks.Adornments<AdornmentsForHandler1, global::ParamMethodsCreateExpectations.Handler1, global::System.Func<int>, int>, IAdornmentsForParamMethods<AdornmentsForHandler1>
					{
						public AdornmentsForHandler1(global::ParamMethodsCreateExpectations.Handler1 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler2
						: global::Rocks.Adornments<AdornmentsForHandler2, global::ParamMethodsCreateExpectations.Handler2, global::System.Func<string?>, string?>, IAdornmentsForParamMethods<AdornmentsForHandler2>
					{
						public AdornmentsForHandler2(global::ParamMethodsCreateExpectations.Handler2 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler3
						: global::Rocks.Adornments<AdornmentsForHandler3, global::ParamMethodsCreateExpectations.Handler3, global::System.Action<string[]>>, IAdornmentsForParamMethods<AdornmentsForHandler3>
					{
						public AdornmentsForHandler3(global::ParamMethodsCreateExpectations.Handler3 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler4
						: global::Rocks.Adornments<AdornmentsForHandler4, global::ParamMethodsCreateExpectations.Handler4, global::System.Action<global::System.ReadOnlySpan<string>>>, IAdornmentsForParamMethods<AdornmentsForHandler4>
					{
						public AdornmentsForHandler4(global::ParamMethodsCreateExpectations.Handler4 handler)
							: base(handler) { }
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
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class ParamMethodsMakeExpectations
			{
				internal global::ParamMethods Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::ParamMethods
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
					public override void Do(params string[] @args)
					{
					}
					public override void DoSpan(params global::System.ReadOnlySpan<string> @args)
					{
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
				("ParamMethods_Rock_Create.g.cs", createGeneratedCode),
				("ParamMethods_Rock_Make.g.cs", makeGeneratedCode),
			],
			[]);
	}
}
