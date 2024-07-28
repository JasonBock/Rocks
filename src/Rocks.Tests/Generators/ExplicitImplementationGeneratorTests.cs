﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ExplicitImplementationGeneratorTests
{
	[Test]
	public static async Task GenerateWithExplicitEventsAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: Rock(typeof(MockTests.IHtmlMediaElement), BuildType.Create | BuildType.Make)]

			namespace EventStuff
			{
				public class Event : EventArgs { }

				public delegate void DomEventHandler(object sender, Event ev);
			}

			namespace GlobalHandlers
			{
				public interface IGlobalEventHandlers
				{
					event global::EventStuff.DomEventHandler CanPlay;
				}
			}

			namespace Controllers
			{
				public interface IMediaController
				{
					event global::EventStuff.DomEventHandler CanPlay;
				}
			}

			namespace MockTests
			{
				public interface IHtmlMediaElement
					: global::GlobalHandlers.IGlobalEventHandlers, global::Controllers.IMediaController
				{
					void Foo();
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
				internal sealed class IHtmlMediaElementCreateExpectations
					: global::Rocks.Expectations
				{
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Action>
					{ }
					private global::Rocks.Handlers<global::MockTests.IHtmlMediaElementCreateExpectations.Handler0>? @handlers0;
					
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
						: global::MockTests.IHtmlMediaElement, global::Rocks.IRaiseEvents
					{
						public Mock(global::MockTests.IHtmlMediaElementCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0)]
						public void Foo()
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
						
						#pragma warning disable CS0067
						private global::EventStuff.DomEventHandler? IGlobalEventHandlers_CanPlay;
						event global::EventStuff.DomEventHandler? global::GlobalHandlers.IGlobalEventHandlers.CanPlay
						{
							add => this.IGlobalEventHandlers_CanPlay += value;
							remove => this.IGlobalEventHandlers_CanPlay -= value;
						}
						private global::EventStuff.DomEventHandler? IMediaController_CanPlay;
						event global::EventStuff.DomEventHandler? global::Controllers.IMediaController.CanPlay
						{
							add => this.IMediaController_CanPlay += value;
							remove => this.IMediaController_CanPlay -= value;
						}
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
						
						private global::MockTests.IHtmlMediaElementCreateExpectations Expectations { get; }
					}
					
					internal sealed class MethodExpectations
					{
						internal MethodExpectations(global::MockTests.IHtmlMediaElementCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::MockTests.IHtmlMediaElementCreateExpectations.Adornments.AdornmentsForHandler0 Foo()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::MockTests.IHtmlMediaElementCreateExpectations.Handler0();
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(handler); }
							else { this.Expectations.handlers0.Add(handler); }
							return new(handler);
						}
						
						private global::MockTests.IHtmlMediaElementCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.IHtmlMediaElementCreateExpectations.MethodExpectations Methods { get; }
					
					internal IHtmlMediaElementCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.IHtmlMediaElement Instance()
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
						public interface IAdornmentsForIHtmlMediaElement<TAdornments>
							: global::Rocks.IAdornments<TAdornments>
							where TAdornments : IAdornmentsForIHtmlMediaElement<TAdornments>
						{ }
						
						public sealed class AdornmentsForHandler0
							: global::Rocks.Adornments<AdornmentsForHandler0, global::MockTests.IHtmlMediaElementCreateExpectations.Handler0, global::System.Action>, IAdornmentsForIHtmlMediaElement<AdornmentsForHandler0>
						{
							public AdornmentsForHandler0(global::MockTests.IHtmlMediaElementCreateExpectations.Handler0 handler)
								: base(handler) { }
						}
					}
				}
				
				internal static class IHtmlMediaElementAdornmentsEventExtensions
				{
					internal static TAdornments RaiseCanPlay<TAdornments>(this TAdornments self, global::EventStuff.Event args) where TAdornments : global::MockTests.IHtmlMediaElementCreateExpectations.Adornments.IAdornmentsForIHtmlMediaElement<TAdornments> => 
						self.AddRaiseEvent(new("CanPlay", args));
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
				internal sealed class IHtmlMediaElementMakeExpectations
				{
					internal global::MockTests.IHtmlMediaElement Instance()
					{
						return new Mock();
					}
					
					private sealed class Mock
						: global::MockTests.IHtmlMediaElement
					{
						public Mock()
						{
						}
						
						public void Foo()
						{
						}
						
						#pragma warning disable CS0067
						private global::EventStuff.DomEventHandler? IGlobalEventHandlers_CanPlay;
						event global::EventStuff.DomEventHandler? global::GlobalHandlers.IGlobalEventHandlers.CanPlay
						{
							add => this.IGlobalEventHandlers_CanPlay += value;
							remove => this.IGlobalEventHandlers_CanPlay -= value;
						}
						private global::EventStuff.DomEventHandler? IMediaController_CanPlay;
						event global::EventStuff.DomEventHandler? global::Controllers.IMediaController.CanPlay
						{
							add => this.IMediaController_CanPlay += value;
							remove => this.IMediaController_CanPlay -= value;
						}
						#pragma warning restore CS0067
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
				("MockTests.IHtmlMediaElement_Rock_Create.g.cs", createGeneratedCode),
				("MockTests.IHtmlMediaElement_Rock_Make.g.cs", makeGeneratedCode)
			],
			[]);
	}

	[Test]
	public static async Task GenerateWithExplicitPropertySetterAsync()
	{
		var code =
			"""
			using Rocks;
			
			[assembly: Rock(typeof(ILeftRight), BuildType.Create | BuildType.Make)]

			namespace Values
			{
				public sealed class Information { }
			}

			public interface ILeft
			{
				Values.Information Value { get; set; }
			}
			
			public interface IRight
			{
				Values.Information Value { get; set; }
			}

			public interface ILeftRight
				: ILeft, IRight { }
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
			internal sealed class ILeftRightCreateExpectations
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Func<global::Values.Information>, global::Values.Information>
				{ }
				private global::Rocks.Handlers<global::ILeftRightCreateExpectations.Handler0>? @handlers0;
				internal sealed class Handler1
					: global::Rocks.Handler<global::System.Action<global::Values.Information>>
				{
					public global::Rocks.Argument<global::Values.Information> @value { get; set; }
				}
				private global::Rocks.Handlers<global::ILeftRightCreateExpectations.Handler1>? @handlers1;
				internal sealed class Handler2
					: global::Rocks.Handler<global::System.Func<global::Values.Information>, global::Values.Information>
				{ }
				private global::Rocks.Handlers<global::ILeftRightCreateExpectations.Handler2>? @handlers2;
				internal sealed class Handler3
					: global::Rocks.Handler<global::System.Action<global::Values.Information>>
				{
					public global::Rocks.Argument<global::Values.Information> @value { get; set; }
				}
				private global::Rocks.Handlers<global::ILeftRightCreateExpectations.Handler3>? @handlers3;
				
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
					: global::ILeftRight
				{
					public Mock(global::ILeftRightCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0, global::Rocks.PropertyAccessor.Get)]
					[global::Rocks.MemberIdentifier(1, global::Rocks.PropertyAccessor.Set)]
					global::Values.Information global::ILeft.Value
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
						set
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
					global::Values.Information global::IRight.Value
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
						set
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
					
					private global::ILeftRightCreateExpectations Expectations { get; }
				}
				internal sealed class ExplicitPropertyExpectationsForILeft
				{
					internal sealed class ExplicitPropertyGetterExpectationsForILeft
					{
						internal ExplicitPropertyGetterExpectationsForILeft(global::ILeftRightCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::ILeftRightCreateExpectations.Adornments.AdornmentsForHandler0 Value()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::ILeftRightCreateExpectations.Handler0();
							if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(handler); }
							else { this.Expectations.handlers0.Add(handler); }
							return new(handler);
						}
						private global::ILeftRightCreateExpectations Expectations { get; }
					}
					internal sealed class ExplicitPropertySetterExpectationsForILeft
					{
						internal ExplicitPropertySetterExpectationsForILeft(global::ILeftRightCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::ILeftRightCreateExpectations.Adornments.AdornmentsForHandler1 Value(global::Rocks.Argument<global::Values.Information> @value)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@value);
						
							var handler = new global::ILeftRightCreateExpectations.Handler1
							{
								value = @value,
							};
						
							if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(handler); }
							else { this.Expectations.handlers1.Add(handler); }
							return new(handler);
						}
						private global::ILeftRightCreateExpectations Expectations { get; }
					}
					
					internal ExplicitPropertyExpectationsForILeft(global::ILeftRightCreateExpectations expectations) =>
						(this.Getters, this.Setters) = (new(expectations), new(expectations));
					
					internal global::ILeftRightCreateExpectations.ExplicitPropertyExpectationsForILeft.ExplicitPropertyGetterExpectationsForILeft Getters { get; }
					internal global::ILeftRightCreateExpectations.ExplicitPropertyExpectationsForILeft.ExplicitPropertySetterExpectationsForILeft Setters { get; }
				}
				internal sealed class ExplicitPropertyExpectationsForIRight
				{
					internal sealed class ExplicitPropertyGetterExpectationsForIRight
					{
						internal ExplicitPropertyGetterExpectationsForIRight(global::ILeftRightCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::ILeftRightCreateExpectations.Adornments.AdornmentsForHandler2 Value()
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							var handler = new global::ILeftRightCreateExpectations.Handler2();
							if (this.Expectations.handlers2 is null) { this.Expectations.handlers2 = new(handler); }
							else { this.Expectations.handlers2.Add(handler); }
							return new(handler);
						}
						private global::ILeftRightCreateExpectations Expectations { get; }
					}
					internal sealed class ExplicitPropertySetterExpectationsForIRight
					{
						internal ExplicitPropertySetterExpectationsForIRight(global::ILeftRightCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::ILeftRightCreateExpectations.Adornments.AdornmentsForHandler3 Value(global::Rocks.Argument<global::Values.Information> @value)
						{
							global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
							global::System.ArgumentNullException.ThrowIfNull(@value);
						
							var handler = new global::ILeftRightCreateExpectations.Handler3
							{
								value = @value,
							};
						
							if (this.Expectations.handlers3 is null) { this.Expectations.handlers3 = new(handler); }
							else { this.Expectations.handlers3.Add(handler); }
							return new(handler);
						}
						private global::ILeftRightCreateExpectations Expectations { get; }
					}
					
					internal ExplicitPropertyExpectationsForIRight(global::ILeftRightCreateExpectations expectations) =>
						(this.Getters, this.Setters) = (new(expectations), new(expectations));
					
					internal global::ILeftRightCreateExpectations.ExplicitPropertyExpectationsForIRight.ExplicitPropertyGetterExpectationsForIRight Getters { get; }
					internal global::ILeftRightCreateExpectations.ExplicitPropertyExpectationsForIRight.ExplicitPropertySetterExpectationsForIRight Setters { get; }
				}
				
				internal global::ILeftRightCreateExpectations.ExplicitPropertyExpectationsForILeft ExplicitPropertiesForILeft { get; }
				internal global::ILeftRightCreateExpectations.ExplicitPropertyExpectationsForIRight ExplicitPropertiesForIRight { get; }
				
				internal ILeftRightCreateExpectations() =>
					(this.ExplicitPropertiesForILeft, this.ExplicitPropertiesForIRight) = (new(this), new(this));
				
				internal global::ILeftRight Instance()
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
					public interface IAdornmentsForILeftRight<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForILeftRight<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::ILeftRightCreateExpectations.Handler0, global::System.Func<global::Values.Information>, global::Values.Information>, IAdornmentsForILeftRight<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::ILeftRightCreateExpectations.Handler0 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler1
						: global::Rocks.Adornments<AdornmentsForHandler1, global::ILeftRightCreateExpectations.Handler1, global::System.Action<global::Values.Information>>, IAdornmentsForILeftRight<AdornmentsForHandler1>
					{
						public AdornmentsForHandler1(global::ILeftRightCreateExpectations.Handler1 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler2
						: global::Rocks.Adornments<AdornmentsForHandler2, global::ILeftRightCreateExpectations.Handler2, global::System.Func<global::Values.Information>, global::Values.Information>, IAdornmentsForILeftRight<AdornmentsForHandler2>
					{
						public AdornmentsForHandler2(global::ILeftRightCreateExpectations.Handler2 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler3
						: global::Rocks.Adornments<AdornmentsForHandler3, global::ILeftRightCreateExpectations.Handler3, global::System.Action<global::Values.Information>>, IAdornmentsForILeftRight<AdornmentsForHandler3>
					{
						public AdornmentsForHandler3(global::ILeftRightCreateExpectations.Handler3 handler)
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
			internal sealed class ILeftRightMakeExpectations
			{
				internal global::ILeftRight Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::ILeftRight
				{
					public Mock()
					{
					}
					
					global::Values.Information global::ILeft.Value
					{
						get => default!;
						set { }
					}
					global::Values.Information global::IRight.Value
					{
						get => default!;
						set { }
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
				("ILeftRight_Rock_Create.g.cs", createGeneratedCode),
				("ILeftRight_Rock_Make.g.cs", makeGeneratedCode)
			],
			[]);
	}

	[Test]
	public static async Task GenerateWithDifferenceInReturnTypeAsync()
	{
		var code =
			"""
			using Rocks;
			
			[assembly: Rock(typeof(IIterable<>), BuildType.Create | BuildType.Make)]

			public interface IIterator
			{
				void Iterate();
			}

			public interface IIterator<out T>
				: IIterator
			{
				new T Iterate();
			}
			
			public interface IIterable
			{
				IIterator GetIterator();
			}

			public interface IIterable<out T>
				: IIterable
			{
				new IIterator<T> GetIterator();
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
			internal sealed class IIterableCreateExpectations<T>
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Func<global::IIterator<T>>, global::IIterator<T>>
				{ }
				private global::Rocks.Handlers<global::IIterableCreateExpectations<T>.Handler0>? @handlers0;
				internal sealed class Handler1
					: global::Rocks.Handler<global::System.Func<global::IIterator>, global::IIterator>
				{ }
				private global::Rocks.Handlers<global::IIterableCreateExpectations<T>.Handler1>? @handlers1;
				
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
					: global::IIterable<T>
				{
					public Mock(global::IIterableCreateExpectations<T> @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public global::IIterator<T> GetIterator()
					{
						if (this.Expectations.handlers0 is not null)
						{
							var @handler = this.Expectations.handlers0.First;
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
							return @result!;
						}
						
						throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
					}
					
					[global::Rocks.MemberIdentifier(1)]
					global::IIterator global::IIterable.GetIterator()
					{
						if (this.Expectations.handlers1 is not null)
						{
							var @handler = this.Expectations.handlers1.First;
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
							return @result!;
						}
						
						throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(1)}");
					}
					
					private global::IIterableCreateExpectations<T> Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::IIterableCreateExpectations<T> expectations) =>
						this.Expectations = expectations;
					
					internal global::IIterableCreateExpectations<T>.Adornments.AdornmentsForHandler0 GetIterator()
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						var handler = new global::IIterableCreateExpectations<T>.Handler0();
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(handler); }
						else { this.Expectations.handlers0.Add(handler); }
						return new(handler);
					}
					
					private global::IIterableCreateExpectations<T> Expectations { get; }
				}
				internal sealed class ExplicitMethodExpectationsForIIterable
				{
					internal ExplicitMethodExpectationsForIIterable(global::IIterableCreateExpectations<T> expectations) =>
						this.Expectations = expectations;
				
					internal global::IIterableCreateExpectations<T>.Adornments.AdornmentsForHandler1 GetIterator()
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						var handler = new global::IIterableCreateExpectations<T>.Handler1();
						if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(handler); }
						else { this.Expectations.handlers1.Add(handler); }
						return new(handler);
					}
					
					private global::IIterableCreateExpectations<T> Expectations { get; }
				}
				
				internal global::IIterableCreateExpectations<T>.MethodExpectations Methods { get; }
				internal global::IIterableCreateExpectations<T>.ExplicitMethodExpectationsForIIterable ExplicitMethodsForIIterable { get; }
				
				internal IIterableCreateExpectations() =>
					(this.Methods, this.ExplicitMethodsForIIterable) = (new(this), new(this));
				
				internal global::IIterable<T> Instance()
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
					public interface IAdornmentsForIIterable<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForIIterable<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::IIterableCreateExpectations<T>.Handler0, global::System.Func<global::IIterator<T>>, global::IIterator<T>>, IAdornmentsForIIterable<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::IIterableCreateExpectations<T>.Handler0 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler1
						: global::Rocks.Adornments<AdornmentsForHandler1, global::IIterableCreateExpectations<T>.Handler1, global::System.Func<global::IIterator>, global::IIterator>, IAdornmentsForIIterable<AdornmentsForHandler1>
					{
						public AdornmentsForHandler1(global::IIterableCreateExpectations<T>.Handler1 handler)
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
			internal sealed class IIterableMakeExpectations<T>
			{
				internal global::IIterable<T> Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::IIterable<T>
				{
					public Mock()
					{
					}
					
					public global::IIterator<T> GetIterator()
					{
						return default!;
					}
					global::IIterator global::IIterable.GetIterator()
					{
						return default!;
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
				("IIterableT_Rock_Create.g.cs", createGeneratedCode),
				("IIterableT_Rock_Make.g.cs", makeGeneratedCode)
			],
			[]);
	}

	[Test]
	public static async Task GenerateWithExplicitImplementationAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Collections.Generic;
			
			[assembly: Rock(typeof(ISetupList), BuildType.Create | BuildType.Make)]

			public interface ISetup { }

			public interface ISetupList
				: IEnumerable<ISetup> { }
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
			internal sealed class ISetupListCreateExpectations
				: global::Rocks.Expectations
			{
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Func<global::System.Collections.Generic.IEnumerator<global::ISetup>>, global::System.Collections.Generic.IEnumerator<global::ISetup>>
				{ }
				private global::Rocks.Handlers<global::ISetupListCreateExpectations.Handler0>? @handlers0;
				internal sealed class Handler1
					: global::Rocks.Handler<global::System.Func<global::System.Collections.IEnumerator>, global::System.Collections.IEnumerator>
				{ }
				private global::Rocks.Handlers<global::ISetupListCreateExpectations.Handler1>? @handlers1;
				
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
					: global::ISetupList
				{
					public Mock(global::ISetupListCreateExpectations @expectations)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0)]
					public global::System.Collections.Generic.IEnumerator<global::ISetup> GetEnumerator()
					{
						if (this.Expectations.handlers0 is not null)
						{
							var @handler = this.Expectations.handlers0.First;
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
							return @result!;
						}
						
						throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(0)}");
					}
					
					[global::Rocks.MemberIdentifier(1)]
					global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
					{
						if (this.Expectations.handlers1 is not null)
						{
							var @handler = this.Expectations.handlers1.First;
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
							return @result!;
						}
						
						throw new global::Rocks.Exceptions.ExpectationException($"No handlers were found for {this.GetType().GetMemberDescription(1)}");
					}
					
					private global::ISetupListCreateExpectations Expectations { get; }
				}
				
				internal sealed class MethodExpectations
				{
					internal MethodExpectations(global::ISetupListCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::ISetupListCreateExpectations.Adornments.AdornmentsForHandler0 GetEnumerator()
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						var handler = new global::ISetupListCreateExpectations.Handler0();
						if (this.Expectations.handlers0 is null) { this.Expectations.handlers0 = new(handler); }
						else { this.Expectations.handlers0.Add(handler); }
						return new(handler);
					}
					
					private global::ISetupListCreateExpectations Expectations { get; }
				}
				internal sealed class ExplicitMethodExpectationsForIEnumerable
				{
					internal ExplicitMethodExpectationsForIEnumerable(global::ISetupListCreateExpectations expectations) =>
						this.Expectations = expectations;
				
					internal global::ISetupListCreateExpectations.Adornments.AdornmentsForHandler1 GetEnumerator()
					{
						global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
						var handler = new global::ISetupListCreateExpectations.Handler1();
						if (this.Expectations.handlers1 is null) { this.Expectations.handlers1 = new(handler); }
						else { this.Expectations.handlers1.Add(handler); }
						return new(handler);
					}
					
					private global::ISetupListCreateExpectations Expectations { get; }
				}
				
				internal global::ISetupListCreateExpectations.MethodExpectations Methods { get; }
				internal global::ISetupListCreateExpectations.ExplicitMethodExpectationsForIEnumerable ExplicitMethodsForIEnumerable { get; }
				
				internal ISetupListCreateExpectations() =>
					(this.Methods, this.ExplicitMethodsForIEnumerable) = (new(this), new(this));
				
				internal global::ISetupList Instance()
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
					public interface IAdornmentsForISetupList<TAdornments>
						: global::Rocks.IAdornments<TAdornments>
						where TAdornments : IAdornmentsForISetupList<TAdornments>
					{ }
					
					public sealed class AdornmentsForHandler0
						: global::Rocks.Adornments<AdornmentsForHandler0, global::ISetupListCreateExpectations.Handler0, global::System.Func<global::System.Collections.Generic.IEnumerator<global::ISetup>>, global::System.Collections.Generic.IEnumerator<global::ISetup>>, IAdornmentsForISetupList<AdornmentsForHandler0>
					{
						public AdornmentsForHandler0(global::ISetupListCreateExpectations.Handler0 handler)
							: base(handler) { }
					}
					public sealed class AdornmentsForHandler1
						: global::Rocks.Adornments<AdornmentsForHandler1, global::ISetupListCreateExpectations.Handler1, global::System.Func<global::System.Collections.IEnumerator>, global::System.Collections.IEnumerator>, IAdornmentsForISetupList<AdornmentsForHandler1>
					{
						public AdornmentsForHandler1(global::ISetupListCreateExpectations.Handler1 handler)
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
			internal sealed class ISetupListMakeExpectations
			{
				internal global::ISetupList Instance()
				{
					return new Mock();
				}
				
				private sealed class Mock
					: global::ISetupList
				{
					public Mock()
					{
					}
					
					public global::System.Collections.Generic.IEnumerator<global::ISetup> GetEnumerator()
					{
						return default!;
					}
					global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
					{
						return default!;
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
				("ISetupList_Rock_Create.g.cs", createGeneratedCode),
				("ISetupList_Rock_Make.g.cs", makeGeneratedCode)
			],
			[]);
	}
}