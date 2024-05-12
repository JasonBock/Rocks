﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class InterfaceGeneratorTests
{
	[Test]
	public static async Task GenerateWithSealedMemberAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<ISealed>]

			public interface ISealed
			{
				void NonSealedMethod();
				sealed void SealedMethod() { }

				string NonSealedData { get; set; }
				sealed string SealedData { get => ""; set { } }

				event EventHandler NonSealedEvent;
				sealed event EventHandler SealedEvent { add { } remove { } }
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
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class ISealedCreateExpectations
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Action>
				{ }
				private global::Rocks.Handlers<global::ISealedCreateExpectations.Handler0>? @handlers0;
				internal sealed class Handler1
					: global::Rocks.Handler<global::System.Func<string>, string>
				{ }
				private global::Rocks.Handlers<global::ISealedCreateExpectations.Handler1>? @handlers1;
				internal sealed class Handler2
					: global::Rocks.Handler<global::System.Action<string>>
				{
					public global::Rocks.Argument<string> @value { get; set; }
				}
				private global::Rocks.Handlers<global::ISealedCreateExpectations.Handler2>? @handlers2;
				
				public override void Verify()
				{
					if (this.WasInstanceInvoked)
					{
						var failures = new global::System.Collections.Generic.List<string>();
				
						if (this.handlers0 is not null) { failures.AddRange(this.Verify(this.handlers0, 0)); }
						if (this.handlers1 is not null) { failures.AddRange(this.Verify(this.handlers1, 1)); }
						if (this.handlers2 is not null) { failures.AddRange(this.Verify(this.handlers2, 2)); }
				
						if (failures.Count > 0)
						{
							throw new global::Rocks.Exceptions.VerificationException(failures);
						}
					}
				}
				
				private sealed class Mock
					: global::ISealed, global::Rocks.IRaiseEvents
				{
					public Mock(global::ISealedCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public void NonSealedMethod()
					{
						if (this.Expectations.handlers0 is not null)
						{
							var @handler = this.Expectations.handlers0.First;
							@handler.CallCount++;
							@handler.Callback?.Invoke();
							@handler.RaiseEvents(this);
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
						}
					}
					
					[global::Rocks.MemberIdentifier(1, global::Rocks.PropertyAccessor.Get)]
					[global::Rocks.MemberIdentifier(2, global::Rocks.PropertyAccessor.Set)]
					public string NonSealedData
					{
						get
						{
							if (this.Expectations.handlers1 is not null)
							{
								var @handler = this.Expectations.handlers1.First;
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								@handler.RaiseEvents(this);
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(1)})");
						}
						set
						{
							if (this.Expectations.handlers2 is not null)
							{
								var @foundMatch = false;
								foreach (var @handler in this.Expectations.handlers2)
								{
									if (@handler.value.IsValid(value!))
									{
										@handler.CallCount++;
										@foundMatch = true;
										@handler.Callback?.Invoke(value!);
										
										if (!@foundMatch)
										{
											throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(2)}");
										}
										
										@handler.RaiseEvents(this);
										break;
									}
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(2)}");
							}
						}
					}
					
					#pragma warning disable CS0067
					public event global::System.EventHandler? NonSealedEvent;
					#pragma warning restore CS0067
					
					void global::Rocks.IRaiseEvents.Raise(string @fieldName, object @args)
					{
						var @thisType = this.GetType();
						var @eventDelegate = (global::System.MulticastDelegate)thisType.GetField(@fieldName, 
							global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.NonPublic)!.GetValue(this)!;
						
						if (@eventDelegate is not null)
						{
							foreach (var @handler in @eventDelegate.GetInvocationList())
							{
								@handler.Method.Invoke(@handler.Target, new object[]{this, @args});
							}
						}
					}
					
					private global::ISealedCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::ISealedCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::ISealedCreateExpectations.Adornments.AdornmentsForHandler0 NonSealedMethod()
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						var handler = new global::ISealedCreateExpectations.Handler0();
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(handler); }
						else { this.Expectations.handlers0.Add(handler); }
						return new(handler);
					}
					
					private global::ISealedCreateExpectations Expectations { get; }
				}
				
				internal sealed class PropertyExpectations
				{
					internal sealed class PropertyGetterExpectations
					{
						internal PropertyGetterExpectations(global::ISealedCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::ISealedCreateExpectations.Adornments.AdornmentsForHandler1 NonSealedData()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::ISealedCreateExpectations.Handler1();
							if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(handler); }
							else { this.Expectations.handlers1.Add(handler); }
							return new(handler);
						}
						private global::ISealedCreateExpectations Expectations { get; }
					}
					
					internal sealed class PropertySetterExpectations
					{
						internal PropertySetterExpectations(global::ISealedCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::ISealedCreateExpectations.Adornments.AdornmentsForHandler2 NonSealedData(global::Rocks.Argument<string> @value)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@value);
						
							var handler = new global::ISealedCreateExpectations.Handler2
							{
								value = @value,
							};
						
							if (this.Expectations.handlers2 is null) { this.Expectations.handlers2 = new(handler); }
							else { this.Expectations.handlers2.Add(handler); }
							return new(handler);
						}
						private global::ISealedCreateExpectations Expectations { get; }
					}
					
					internal PropertyExpectations(global::ISealedCreateExpectations expectations) =>
						(this.Getters, this.Setters) = (new(expectations), new(expectations));
					
					internal global::ISealedCreateExpectations.PropertyExpectations.PropertyGetterExpectations Getters { get; }
					internal global::ISealedCreateExpectations.PropertyExpectations.PropertySetterExpectations Setters { get; }
				}
				
				internal global::ISealedCreateExpectations.MethodExpectations Methods { get; }
				internal global::ISealedCreateExpectations.PropertyExpectations Properties { get; }
				
				internal ISealedCreateExpectations() =>
					(this.Methods, this.Properties) = (new(this), new(this));
				
				internal global::ISealed Instance()
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
					public interface IAdornmentsForISealed<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForISealed<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::ISealedCreateExpectations.Handler0, global::System.Action>, IAdornmentsForISealed<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::ISealedCreateExpectations.Handler0 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler1
						: global::Rocks.Adornments<AdornmentsForHandler1, global::ISealedCreateExpectations.Handler1, global::System.Func<string>, string>, IAdornmentsForISealed<AdornmentsForHandler1>
					{
						public AdornmentsForHandler1(global::ISealedCreateExpectations.Handler1 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler2
						: global::Rocks.Adornments<AdornmentsForHandler2, global::ISealedCreateExpectations.Handler2, global::System.Action<string>>, IAdornmentsForISealed<AdornmentsForHandler2>
					{
						public AdornmentsForHandler2(global::ISealedCreateExpectations.Handler2 handler)
							: base(handler) { }
					}
				}
			}
			
			internal static class ISealedAdornmentsEventExtensions
			{
				internal static TAdornments RaiseNonSealedEvent<TAdornments>(this TAdornments self, global::System.EventArgs args) where TAdornments : global::ISealedCreateExpectations.Adornments.IAdornmentsForISealed<TAdornments> => 
					self.AddRaiseEvent(new("NonSealedEvent", args));
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "ISealed_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateWithStaticMemberAsync()
	{
		var code =
			"""
			using Rocks;
			
			[assembly: RockCreate<IRequest>]

			public interface IRequest
			{
				public static string ToString(IRequest request) => "";
				public static string ToMethodCallString(IRequest request) => "";
				void AddInvokeMethodOptions(int options);
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
			
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			internal sealed class IRequestCreateExpectations
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Action<int>>
				{
					public global::Rocks.Argument<int> @options { get; set; }
				}
				private global::Rocks.Handlers<global::IRequestCreateExpectations.Handler0>? @handlers0;
				
				public override void Verify()
				{
					if (this.WasInstanceInvoked)
					{
						var failures = new global::System.Collections.Generic.List<string>();
				
						if (this.handlers0 is not null) { failures.AddRange(this.Verify(this.handlers0, 0)); }
				
						if (failures.Count > 0)
						{
							throw new global::Rocks.Exceptions.VerificationException(failures);
						}
					}
				}
				
				private sealed class Mock
					: global::IRequest
				{
					public Mock(global::IRequestCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public void AddInvokeMethodOptions(int @options)
					{
						if (this.Expectations.handlers0 is not null)
						{
							var @foundMatch = false;
							
							foreach (var @handler in this.Expectations.handlers0)
							{
								if (@handler.@options.IsValid(@options!))
								{
									@foundMatch = true;
									@handler.CallCount++;
									@handler.Callback?.Invoke(@options!);
									break;
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(0)}");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
						}
					}
					
					private global::IRequestCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::IRequestCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::IRequestCreateExpectations.Adornments.AdornmentsForHandler0 AddInvokeMethodOptions(global::Rocks.Argument<int> @options)
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						global::System.ArgumentNullException.ThrowIfNull(@options);
						
						var @handler = new global::IRequestCreateExpectations.Handler0
						{
							@options = @options,
						};
						
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
						else { this.Expectations.handlers0.Add(@handler); }
						return new(@handler);
					}
					
					private global::IRequestCreateExpectations Expectations { get; }
				}
				
				internal global::IRequestCreateExpectations.MethodExpectations Methods { get; }
				
				internal IRequestCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::IRequest Instance()
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
					public interface IAdornmentsForIRequest<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForIRequest<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::IRequestCreateExpectations.Handler0, global::System.Action<int>>, IAdornmentsForIRequest<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::IRequestCreateExpectations.Handler0 handler)
							: base(handler) { }
					}
				}
			}
			
			#pragma warning restore CS8618
			#pragma warning restore CS8633
			#pragma warning restore CS8714
			#pragma warning restore CS8775
			""";

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "IRequest_Rock_Create.g.cs", generatedCode)],
			[]);
	}

	[Test]
	public static async Task GenerateWithMethodAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<MockTests.ITarget>]

			namespace MockTests
			{
				public interface ITarget
				{
					string Retrieve(int value);
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
				internal sealed class ITargetCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<int, string>, string>
					{
						public global::Rocks.Argument<int> @value { get; set; }
					}
					private global::Rocks.Handlers<global::MockTests.ITargetCreateExpectations.Handler0>? @handlers0;
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							if (this.handlers0 is not null) { failures.AddRange(this.Verify(this.handlers0, 0)); }
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class Mock
						: global::MockTests.ITarget
					{
						public Mock(global::MockTests.ITargetCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0)]
						public string Retrieve(int @value)
						{
							if (this.Expectations.handlers0 is not null)
							{
								foreach (var @handler in this.Expectations.handlers0)
								{
									if (@handler.@value.IsValid(@value!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@value!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException($"No handlers match for {this.GetType().GetMemberDescription(0)}");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
						}
						
						private global::MockTests.ITargetCreateExpectations Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.ITargetCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.ITargetCreateExpectations.Adornments.AdornmentsForHandler0 Retrieve(global::Rocks.Argument<int> @value)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@value);
							
							var @handler = new global::MockTests.ITargetCreateExpectations.Handler0
							{
								@value = @value,
							};
							
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(@handler); }
							else { this.Expectations.handlers0.Add(@handler); }
							return new(@handler);
						}
						
						private global::MockTests.ITargetCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.ITargetCreateExpectations.MethodExpectations Methods { get; }
					
					internal ITargetCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.ITarget Instance()
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
						public interface IAdornmentsForITarget<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForITarget<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.ITargetCreateExpectations.Handler0, global::System.Func<int, string>, string>, IAdornmentsForITarget<AdornmentsForHandler0>
						{
							public AdornmentsForHandler0(global::MockTests.ITargetCreateExpectations.Handler0 handler)
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

		await TestAssistants.RunGeneratorAsync<RockAttributeGenerator>(code,
			[(typeof(RockAttributeGenerator), "MockTests.ITarget_Rock_Create.g.cs", generatedCode)],
			[]);
	}
}