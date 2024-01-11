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
			
			#nullable enable
			
			internal sealed class ISealedCreateExpectations
				: global::Rocks.Expectations
			{
				#pragma warning disable CS8618
				
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Action>
				{ }
				
				internal sealed class Handler1
					: global::Rocks.Handler<global::System.Func<string>, string>
				{ }
				
				internal sealed class Handler2
					: global::Rocks.Handler<global::System.Action<string>>
				{
					public global::Rocks.Argument<string> @value { get; set; }
				}
				
				#pragma warning restore CS8618
				
				private readonly global::System.Collections.Generic.List<global::ISealedCreateExpectations.Handler0> @handlers0 = new();
				private readonly global::System.Collections.Generic.List<global::ISealedCreateExpectations.Handler1> @handlers1 = new();
				private readonly global::System.Collections.Generic.List<global::ISealedCreateExpectations.Handler2> @handlers2 = new();
				
				public override void Verify()
				{
					if (this.WasInstanceInvoked)
					{
						var failures = new global::System.Collections.Generic.List<string>();
				
						failures.AddRange(this.Verify(handlers0));
						failures.AddRange(this.Verify(handlers1));
						failures.AddRange(this.Verify(handlers2));
				
						if (failures.Count > 0)
						{
							throw new global::Rocks.Exceptions.VerificationException(failures);
						}
					}
				}
				
				private sealed class RockISealed
					: global::ISealed, global::Rocks.IRaiseEvents
				{
					public RockISealed(global::ISealedCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0, "void NonSealedMethod()")]
					public void NonSealedMethod()
					{
						if (this.Expectations.handlers0.Count > 0)
						{
							var @handler = this.Expectations.handlers0[0];
							@handler.CallCount++;
							@handler.Callback?.Invoke();
							@handler.RaiseEvents(this);
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void NonSealedMethod()");
						}
					}
					
					[global::Rocks.MemberIdentifier(1, "get_NonSealedData()")]
					[global::Rocks.MemberIdentifier(2, "set_NonSealedData(value)")]
					public string NonSealedData
					{
						get
						{
							if (this.Expectations.handlers1.Count > 0)
							{
								var @handler = this.Expectations.handlers1[0];
								@handler.CallCount++;
								var @result = @handler.Callback is not null ?
									@handler.Callback() : @handler.ReturnValue;
								@handler.RaiseEvents(this);
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_NonSealedData())");
						}
						set
						{
							if (this.Expectations.handlers2.Count > 0)
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
											throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_NonSealedData(value)");
										}
										
										@handler.RaiseEvents(this);
										break;
									}
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for set_NonSealedData(value)");
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
				
				internal sealed class ISealedMethodExpectations
				{
					internal ISealedMethodExpectations(global::ISealedCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::Rocks.Adornments<global::ISealedCreateExpectations.Handler0, global::System.Action> NonSealedMethod()
					{
						var handler = new global::ISealedCreateExpectations.Handler0();
						this.Expectations.handlers0.Add(handler);
						return new(handler);
					}
					
					private global::ISealedCreateExpectations Expectations { get; }
				}
				
				internal sealed class ISealedPropertyExpectations
				{
					internal sealed class ISealedPropertyGetterExpectations
					{
						internal ISealedPropertyGetterExpectations(global::ISealedCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::ISealedCreateExpectations.Handler1, global::System.Func<string>, string> NonSealedData()
						{
							var handler = new global::ISealedCreateExpectations.Handler1();
							this.Expectations.handlers1.Add(handler);
							return new(handler);
						}
						private global::ISealedCreateExpectations Expectations { get; }
					}
					
					internal sealed class ISealedPropertySetterExpectations
					{
						internal ISealedPropertySetterExpectations(global::ISealedCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::ISealedCreateExpectations.Handler2, global::System.Action<string>> NonSealedData(global::Rocks.Argument<string> @value)
						{
							var handler = new global::ISealedCreateExpectations.Handler2
							{
								value = @value,
							};
						
							this.Expectations.handlers2.Add(handler);
							return new(handler);
						}
						private global::ISealedCreateExpectations Expectations { get; }
					}
					
					internal ISealedPropertyExpectations(global::ISealedCreateExpectations expectations) =>
						(this.Getters, this.Setters) = (new(expectations), new(expectations));
					
					internal global::ISealedCreateExpectations.ISealedPropertyExpectations.ISealedPropertyGetterExpectations Getters { get; }
					internal global::ISealedCreateExpectations.ISealedPropertyExpectations.ISealedPropertySetterExpectations Setters { get; }
				}
				
				internal global::ISealedCreateExpectations.ISealedMethodExpectations Methods { get; }
				internal global::ISealedCreateExpectations.ISealedPropertyExpectations Properties { get; }
				
				internal ISealedCreateExpectations() =>
					(this.Methods, this.Properties) = (new(this), new(this));
				
				internal global::ISealed Instance()
				{
					if (!this.WasInstanceInvoked)
					{
						this.WasInstanceInvoked = true;
						var @mock = new RockISealed(this);
						this.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "ISealed_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
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
			
			#nullable enable
			
			internal sealed class IRequestCreateExpectations
				: global::Rocks.Expectations
			{
				#pragma warning disable CS8618
				
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Action<int>>
				{
					public global::Rocks.Argument<int> @options { get; set; }
				}
				
				#pragma warning restore CS8618
				
				private readonly global::System.Collections.Generic.List<global::IRequestCreateExpectations.Handler0> @handlers0 = new();
				
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
				
				private sealed class RockIRequest
					: global::IRequest
				{
					public RockIRequest(global::IRequestCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0, "void AddInvokeMethodOptions(int @options)")]
					public void AddInvokeMethodOptions(int @options)
					{
						if (this.Expectations.handlers0.Count > 0)
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
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void AddInvokeMethodOptions(int @options)");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void AddInvokeMethodOptions(int @options)");
						}
					}
					
					private global::IRequestCreateExpectations Expectations { get; }
				}
				
				internal sealed class IRequestMethodExpectations
				{
					internal IRequestMethodExpectations(global::IRequestCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::Rocks.Adornments<global::IRequestCreateExpectations.Handler0, global::System.Action<int>> AddInvokeMethodOptions(global::Rocks.Argument<int> @options)
					{
						global::System.ArgumentNullException.ThrowIfNull(@options);
						
						var handler = new global::IRequestCreateExpectations.Handler0
						{
							@options = @options,
						};
						
						this.Expectations.handlers0.Add(handler);
						return new(handler);
					}
					
					private global::IRequestCreateExpectations Expectations { get; }
				}
				
				internal global::IRequestCreateExpectations.IRequestMethodExpectations Methods { get; }
				
				internal IRequestCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::IRequest Instance()
				{
					if (!this.WasInstanceInvoked)
					{
						this.WasInstanceInvoked = true;
						var @mock = new RockIRequest(this);
						this.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "IRequest_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
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
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class ITargetCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<int, string>, string>
					{
						public global::Rocks.Argument<int> @value { get; set; }
					}
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.ITargetCreateExpectations.Handler0> @handlers0 = new();
					
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
					
					private sealed class RockITarget
						: global::MockTests.ITarget
					{
						public RockITarget(global::MockTests.ITargetCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0, "string Retrieve(int @value)")]
						public string Retrieve(int @value)
						{
							if (this.Expectations.handlers0.Count > 0)
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
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for string Retrieve(int @value)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for string Retrieve(int @value)");
						}
						
						private global::MockTests.ITargetCreateExpectations Expectations { get; }
					}
					
					internal sealed class ITargetMethodExpectations
					{
						internal ITargetMethodExpectations(global::MockTests.ITargetCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.ITargetCreateExpectations.Handler0, global::System.Func<int, string>, string> Retrieve(global::Rocks.Argument<int> @value)
						{
							global::System.ArgumentNullException.ThrowIfNull(@value);
							
							var handler = new global::MockTests.ITargetCreateExpectations.Handler0
							{
								@value = @value,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.ITargetCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.ITargetCreateExpectations.ITargetMethodExpectations Methods { get; }
					
					internal ITargetCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.ITarget Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockITarget(this);
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
			new[] { (typeof(RockAttributeGenerator), "MockTests.ITarget_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}
}