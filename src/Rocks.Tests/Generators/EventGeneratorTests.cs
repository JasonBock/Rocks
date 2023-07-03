using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class EventGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public interface IHaveEvents
				{
					void A();
					event EventHandler C;
				}
			
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IHaveEvents>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfIHaveEventsExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveEvents> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveEvents> @self) =>
						new(@self);
					
					internal static global::MockTests.IHaveEvents Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveEvents> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockIHaveEvents(@self);
							@self.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIHaveEvents
						: global::MockTests.IHaveEvents, global::Rocks.IRaiseEvents
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockIHaveEvents(global::Rocks.Expectations.Expectations<global::MockTests.IHaveEvents> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
						[global::Rocks.MemberIdentifier(0, "void A()")]
						public void A()
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								if (@methodHandler.Method is not null)
								{
									((global::System.Action)@methodHandler.Method)();
								}
								
								@methodHandler.RaiseEvents(this);
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void A()");
							}
						}
						
						
						#pragma warning disable CS0067
						public event global::System.EventHandler? C;
						#pragma warning restore CS0067
						
						void global::Rocks.IRaiseEvents.Raise(string @fieldName, global::System.EventArgs @args)
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
					}
				}
				
				internal static class MethodExpectationsOfIHaveEventsExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IHaveEvents, global::System.Action> A(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveEvents> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.IHaveEvents, global::System.Action>(@self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				
				internal static class MethodAdornmentsOfIHaveEventsExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IHaveEvents, TCallback> RaisesC<TCallback>(this global::Rocks.MethodAdornments<global::MockTests.IHaveEvents, TCallback> @self, global::System.EventArgs @args)
						where TCallback : global::System.Delegate
					{
						@self.Handler.AddRaiseEvent(new("C", @args));
						return @self;
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IHaveEvents_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithExplicitMembersAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public interface IExplicitInterfaceImplementationOne
				{
					void A();
					event EventHandler C;
				}

				public interface IExplicitInterfaceImplementationTwo
				{
					void A();
					event EventHandler C;
				}

				public interface IExplicitInterfaceImplementation
					: IExplicitInterfaceImplementationOne, IExplicitInterfaceImplementationTwo
				{ }

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IExplicitInterfaceImplementation>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfIExplicitInterfaceImplementationExtensions
				{
					internal static global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IExplicitInterfaceImplementation, global::MockTests.IExplicitInterfaceImplementationOne> ExplicitMethodsForIExplicitInterfaceImplementationOne(this global::Rocks.Expectations.Expectations<global::MockTests.IExplicitInterfaceImplementation> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IExplicitInterfaceImplementation, global::MockTests.IExplicitInterfaceImplementationTwo> ExplicitMethodsForIExplicitInterfaceImplementationTwo(this global::Rocks.Expectations.Expectations<global::MockTests.IExplicitInterfaceImplementation> @self) =>
						new(@self);
					
					internal static global::MockTests.IExplicitInterfaceImplementation Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IExplicitInterfaceImplementation> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockIExplicitInterfaceImplementation(@self);
							@self.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIExplicitInterfaceImplementation
						: global::MockTests.IExplicitInterfaceImplementation, global::Rocks.IRaiseEvents
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockIExplicitInterfaceImplementation(global::Rocks.Expectations.Expectations<global::MockTests.IExplicitInterfaceImplementation> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
						[global::Rocks.MemberIdentifier(0, "void global::MockTests.IExplicitInterfaceImplementationOne.A()")]
						void global::MockTests.IExplicitInterfaceImplementationOne.A()
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								if (@methodHandler.Method is not null)
								{
									((global::System.Action)@methodHandler.Method)();
								}
								
								@methodHandler.RaiseEvents(this);
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void global::MockTests.IExplicitInterfaceImplementationOne.A()");
							}
						}
						
						[global::Rocks.MemberIdentifier(1, "void global::MockTests.IExplicitInterfaceImplementationTwo.A()")]
						void global::MockTests.IExplicitInterfaceImplementationTwo.A()
						{
							if (this.handlers.TryGetValue(1, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								if (@methodHandler.Method is not null)
								{
									((global::System.Action)@methodHandler.Method)();
								}
								
								@methodHandler.RaiseEvents(this);
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void global::MockTests.IExplicitInterfaceImplementationTwo.A()");
							}
						}
						
						
						#pragma warning disable CS0067
						private global::System.EventHandler? IExplicitInterfaceImplementationOne_C;
						event global::System.EventHandler? IExplicitInterfaceImplementationOne.C
						{
							add => this.IExplicitInterfaceImplementationOne_C += value;
							remove => this.IExplicitInterfaceImplementationOne_C -= value;
						}
						private global::System.EventHandler? IExplicitInterfaceImplementationTwo_C;
						event global::System.EventHandler? IExplicitInterfaceImplementationTwo.C
						{
							add => this.IExplicitInterfaceImplementationTwo_C += value;
							remove => this.IExplicitInterfaceImplementationTwo_C -= value;
						}
						#pragma warning restore CS0067
						
						void global::Rocks.IRaiseEvents.Raise(string @fieldName, global::System.EventArgs @args)
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
					}
				}
				
				internal static class ExplicitMethodExpectationsOfIExplicitInterfaceImplementationForIExplicitInterfaceImplementationOneExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IExplicitInterfaceImplementation, global::System.Action> A(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IExplicitInterfaceImplementation, global::MockTests.IExplicitInterfaceImplementationOne> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.IExplicitInterfaceImplementation, global::System.Action>(@self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				internal static class ExplicitMethodExpectationsOfIExplicitInterfaceImplementationForIExplicitInterfaceImplementationTwoExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IExplicitInterfaceImplementation, global::System.Action> A(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IExplicitInterfaceImplementation, global::MockTests.IExplicitInterfaceImplementationTwo> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.IExplicitInterfaceImplementation, global::System.Action>(@self.Add(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				
				internal static class MethodAdornmentsOfIExplicitInterfaceImplementationExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IExplicitInterfaceImplementation, TCallback> RaisesCOnIExplicitInterfaceImplementationOne<TCallback>(this global::Rocks.MethodAdornments<global::MockTests.IExplicitInterfaceImplementation, TCallback> @self, global::System.EventArgs @args)
						where TCallback : global::System.Delegate
					{
						@self.Handler.AddRaiseEvent(new("IExplicitInterfaceImplementationOne_C", @args));
						return @self;
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.IExplicitInterfaceImplementation, TCallback> RaisesCOnIExplicitInterfaceImplementationTwo<TCallback>(this global::Rocks.MethodAdornments<global::MockTests.IExplicitInterfaceImplementation, TCallback> @self, global::System.EventArgs @args)
						where TCallback : global::System.Delegate
					{
						@self.Handler.AddRaiseEvent(new("IExplicitInterfaceImplementationTwo_C", @args));
						return @self;
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IExplicitInterfaceImplementation_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}
