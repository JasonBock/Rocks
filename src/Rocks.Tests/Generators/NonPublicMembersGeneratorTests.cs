using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class NonPublicMembersGeneratorTests
{
	[Test]
	public static async Task CreateWithProtectedVirtualMembersAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public class Test
				{
					protected virtual void ProtectedMethod() { }
					protected virtual string ProtectedProperty { get; set; }
					protected virtual event EventHandler ProtectedEvent;
				}

				public static class TestUser
				{
					public static void Generate()
					{
						var rock = Rock.Create<Test>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfTestExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.Test> self) =>
						new(self);
					
					internal static global::Rocks.Expectations.PropertyExpectations<global::MockTests.Test> Properties(this global::Rocks.Expectations.Expectations<global::MockTests.Test> self) =>
						new(self);
					
					internal static global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.Test> Getters(this global::Rocks.Expectations.PropertyExpectations<global::MockTests.Test> self) =>
						new(self);
					
					internal static global::Rocks.Expectations.PropertySetterExpectations<global::MockTests.Test> Setters(this global::Rocks.Expectations.PropertyExpectations<global::MockTests.Test> self) =>
						new(self);
					
					internal static global::MockTests.Test Instance(this global::Rocks.Expectations.Expectations<global::MockTests.Test> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockTest(self);
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockTest
						: global::MockTests.Test, global::Rocks.IRaiseEvents
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockTest(global::Rocks.Expectations.Expectations<global::MockTests.Test> expectations) =>
							this.handlers = expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "bool Equals(object? obj)")]
						public override bool Equals(object? obj)
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								foreach (var methodHandler in methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(methodHandler.Expectations[0]).IsValid(obj))
									{
										var result = methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, bool>>(methodHandler.Method)(obj) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(methodHandler).ReturnValue;
										methodHandler.RaiseEvents(this);
										methodHandler.IncrementCallCount();
										return result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool Equals(object? obj)");
							}
							else
							{
								return base.Equals(obj);
							}
						}
						
						[global::Rocks.MemberIdentifier(1, "int GetHashCode()")]
						public override int GetHashCode()
						{
							if (this.handlers.TryGetValue(1, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								var result = methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int>>(methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(methodHandler).ReturnValue;
								methodHandler.RaiseEvents(this);
								methodHandler.IncrementCallCount();
								return result!;
							}
							else
							{
								return base.GetHashCode();
							}
						}
						
						[global::Rocks.MemberIdentifier(2, "string? ToString()")]
						public override string? ToString()
						{
							if (this.handlers.TryGetValue(2, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								var result = methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string?>>(methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string?>>(methodHandler).ReturnValue;
								methodHandler.RaiseEvents(this);
								methodHandler.IncrementCallCount();
								return result!;
							}
							else
							{
								return base.ToString();
							}
						}
						
						[global::Rocks.MemberIdentifier(3, "void ProtectedMethod()")]
						protected override void ProtectedMethod()
						{
							if (this.handlers.TryGetValue(3, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								if (methodHandler.Method is not null)
								{
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action>(methodHandler.Method)();
								}
								
								methodHandler.RaiseEvents(this);
								methodHandler.IncrementCallCount();
							}
							else
							{
								base.ProtectedMethod();
							}
						}
						
						[global::Rocks.MemberIdentifier(4, "get_ProtectedProperty()")]
						[global::Rocks.MemberIdentifier(5, "set_ProtectedProperty(value)")]
						protected override string ProtectedProperty
						{
							get
							{
								if (this.handlers.TryGetValue(4, out var methodHandlers))
								{
									var methodHandler = methodHandlers[0];
									var result = methodHandler.Method is not null ?
										global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string>>(methodHandler.Method)() :
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string>>(methodHandler).ReturnValue;
									methodHandler.RaiseEvents(this);
									methodHandler.IncrementCallCount();
									return result!;
								}
								else
								{
									return base.ProtectedProperty;
								}
							}
							set
							{
								if (this.handlers.TryGetValue(5, out var methodHandlers))
								{
									var foundMatch = false;
									foreach (var methodHandler in methodHandlers)
									{
										if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(methodHandler.Expectations[0]).IsValid(value))
										{
											foundMatch = true;
											
											if (methodHandler.Method is not null)
											{
												global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<string>>(methodHandler.Method)(value);
											}
											
											if (!foundMatch)
											{
												throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_ProtectedProperty(value)");
											}
											
											methodHandler.RaiseEvents(this);
											methodHandler.IncrementCallCount();
											break;
										}
									}
								}
								else
								{
									base.ProtectedProperty = value;
								}
							}
						}
						
						#pragma warning disable CS0067
						protected override event global::System.EventHandler? ProtectedEvent;
						#pragma warning restore CS0067
						
						void global::Rocks.IRaiseEvents.Raise(string fieldName, global::System.EventArgs args)
						{
							var thisType = this.GetType();
							var eventDelegate = (global::System.MulticastDelegate)thisType.GetField(fieldName, 
								global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.NonPublic)!.GetValue(this)!;
							
							if (eventDelegate is not null)
							{
								foreach (var handler in eventDelegate.GetInvocationList())
								{
									handler.Method.Invoke(handler.Target, new object[]{this, args});
								}
							}
						}
					}
				}
				
				internal static class MethodExpectationsOfTestExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> self, global::Rocks.Argument<object?> obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(obj);
						return new global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<object?, bool>, bool>(self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { obj }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<int>, int>(self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<string?>, string?>(self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Action> ProtectedMethod(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Action>(self.Add(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				
				internal static class PropertyGetterExpectationsOfTestExtensions
				{
					internal static global::Rocks.PropertyAdornments<Test, global::System.Func<string>, string> ProtectedProperty(this global::Rocks.Expectations.PropertyGetterExpectations<Test> self) =>
						new global::Rocks.PropertyAdornments<Test, global::System.Func<string>, string>(self.Add<string>(4, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				internal static class PropertySetterExpectationsOfTestExtensions
				{
					internal static global::Rocks.PropertyAdornments<Test, global::System.Action<string>> ProtectedProperty(this global::Rocks.Expectations.PropertySetterExpectations<Test> self, global::Rocks.Argument<string> value) =>
						new global::Rocks.PropertyAdornments<Test, global::System.Action<string>>(self.Add(5, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { value }));
				}
				
				internal static class MethodAdornmentsOfTestExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, TCallback, TReturn> RaisesProtectedEvent<TCallback, TReturn>(this global::Rocks.MethodAdornments<global::MockTests.Test, TCallback, TReturn> self, global::System.EventArgs args)
						where TCallback : global::System.Delegate
					{
						self.Handler.AddRaiseEvent(new("ProtectedEvent", args));
						return self;
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, TCallback> RaisesProtectedEvent<TCallback>(this global::Rocks.MethodAdornments<global::MockTests.Test, TCallback> self, global::System.EventArgs args)
						where TCallback : global::System.Delegate
					{
						self.Handler.AddRaiseEvent(new("ProtectedEvent", args));
						return self;
					}
				}
				
				internal static class PropertyAdornmentsOfTestExtensions
				{
					internal static global::Rocks.PropertyAdornments<global::MockTests.Test, TCallback, TReturn> RaisesProtectedEvent<TCallback, TReturn>(this global::Rocks.PropertyAdornments<global::MockTests.Test, TCallback, TReturn> self, global::System.EventArgs args)
						where TCallback : global::System.Delegate
					{
						self.Handler.AddRaiseEvent(new("ProtectedEvent", args));
						return self;
					}
					internal static global::Rocks.PropertyAdornments<global::MockTests.Test, TCallback> RaisesProtectedEvent<TCallback>(this global::Rocks.PropertyAdornments<global::MockTests.Test, TCallback> self, global::System.EventArgs args)
						where TCallback : global::System.Delegate
					{
						self.Handler.AddRaiseEvent(new("ProtectedEvent", args));
						return self;
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "Test_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task CreateWithNonPublicAbstractMembersAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public abstract class Test
				{
					protected abstract void ProtectedMethod();
					protected abstract string ProtectedProperty { get; set; }
					protected abstract event EventHandler ProtectedEvent;
				}

				public static class TestUser
				{
					public static void Generate()
					{
						var rock = Rock.Create<Test>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfTestExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.Test> self) =>
						new(self);
					
					internal static global::Rocks.Expectations.PropertyExpectations<global::MockTests.Test> Properties(this global::Rocks.Expectations.Expectations<global::MockTests.Test> self) =>
						new(self);
					
					internal static global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.Test> Getters(this global::Rocks.Expectations.PropertyExpectations<global::MockTests.Test> self) =>
						new(self);
					
					internal static global::Rocks.Expectations.PropertySetterExpectations<global::MockTests.Test> Setters(this global::Rocks.Expectations.PropertyExpectations<global::MockTests.Test> self) =>
						new(self);
					
					internal static global::MockTests.Test Instance(this global::Rocks.Expectations.Expectations<global::MockTests.Test> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockTest(self);
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockTest
						: global::MockTests.Test, global::Rocks.IRaiseEvents
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockTest(global::Rocks.Expectations.Expectations<global::MockTests.Test> expectations) =>
							this.handlers = expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "bool Equals(object? obj)")]
						public override bool Equals(object? obj)
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								foreach (var methodHandler in methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(methodHandler.Expectations[0]).IsValid(obj))
									{
										var result = methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, bool>>(methodHandler.Method)(obj) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(methodHandler).ReturnValue;
										methodHandler.RaiseEvents(this);
										methodHandler.IncrementCallCount();
										return result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool Equals(object? obj)");
							}
							else
							{
								return base.Equals(obj);
							}
						}
						
						[global::Rocks.MemberIdentifier(1, "int GetHashCode()")]
						public override int GetHashCode()
						{
							if (this.handlers.TryGetValue(1, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								var result = methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int>>(methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(methodHandler).ReturnValue;
								methodHandler.RaiseEvents(this);
								methodHandler.IncrementCallCount();
								return result!;
							}
							else
							{
								return base.GetHashCode();
							}
						}
						
						[global::Rocks.MemberIdentifier(2, "string? ToString()")]
						public override string? ToString()
						{
							if (this.handlers.TryGetValue(2, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								var result = methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string?>>(methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string?>>(methodHandler).ReturnValue;
								methodHandler.RaiseEvents(this);
								methodHandler.IncrementCallCount();
								return result!;
							}
							else
							{
								return base.ToString();
							}
						}
						
						[global::Rocks.MemberIdentifier(3, "void ProtectedMethod()")]
						protected override void ProtectedMethod()
						{
							if (this.handlers.TryGetValue(3, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								if (methodHandler.Method is not null)
								{
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action>(methodHandler.Method)();
								}
								
								methodHandler.RaiseEvents(this);
								methodHandler.IncrementCallCount();
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void ProtectedMethod()");
							}
						}
						
						[global::Rocks.MemberIdentifier(4, "get_ProtectedProperty()")]
						[global::Rocks.MemberIdentifier(5, "set_ProtectedProperty(value)")]
						protected override string ProtectedProperty
						{
							get
							{
								if (this.handlers.TryGetValue(4, out var methodHandlers))
								{
									var methodHandler = methodHandlers[0];
									var result = methodHandler.Method is not null ?
										global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string>>(methodHandler.Method)() :
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string>>(methodHandler).ReturnValue;
									methodHandler.RaiseEvents(this);
									methodHandler.IncrementCallCount();
									return result!;
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_ProtectedProperty())");
							}
							set
							{
								if (this.handlers.TryGetValue(5, out var methodHandlers))
								{
									var foundMatch = false;
									foreach (var methodHandler in methodHandlers)
									{
										if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(methodHandler.Expectations[0]).IsValid(value))
										{
											foundMatch = true;
											
											if (methodHandler.Method is not null)
											{
												global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<string>>(methodHandler.Method)(value);
											}
											
											if (!foundMatch)
											{
												throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_ProtectedProperty(value)");
											}
											
											methodHandler.RaiseEvents(this);
											methodHandler.IncrementCallCount();
											break;
										}
									}
								}
								else
								{
									throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for set_ProtectedProperty(value)");
								}
							}
						}
						
						#pragma warning disable CS0067
						protected override event global::System.EventHandler? ProtectedEvent;
						#pragma warning restore CS0067
						
						void global::Rocks.IRaiseEvents.Raise(string fieldName, global::System.EventArgs args)
						{
							var thisType = this.GetType();
							var eventDelegate = (global::System.MulticastDelegate)thisType.GetField(fieldName, 
								global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.NonPublic)!.GetValue(this)!;
							
							if (eventDelegate is not null)
							{
								foreach (var handler in eventDelegate.GetInvocationList())
								{
									handler.Method.Invoke(handler.Target, new object[]{this, args});
								}
							}
						}
					}
				}
				
				internal static class MethodExpectationsOfTestExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> self, global::Rocks.Argument<object?> obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(obj);
						return new global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<object?, bool>, bool>(self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { obj }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<int>, int>(self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<string?>, string?>(self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Action> ProtectedMethod(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Action>(self.Add(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				
				internal static class PropertyGetterExpectationsOfTestExtensions
				{
					internal static global::Rocks.PropertyAdornments<Test, global::System.Func<string>, string> ProtectedProperty(this global::Rocks.Expectations.PropertyGetterExpectations<Test> self) =>
						new global::Rocks.PropertyAdornments<Test, global::System.Func<string>, string>(self.Add<string>(4, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
				internal static class PropertySetterExpectationsOfTestExtensions
				{
					internal static global::Rocks.PropertyAdornments<Test, global::System.Action<string>> ProtectedProperty(this global::Rocks.Expectations.PropertySetterExpectations<Test> self, global::Rocks.Argument<string> value) =>
						new global::Rocks.PropertyAdornments<Test, global::System.Action<string>>(self.Add(5, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { value }));
				}
				
				internal static class MethodAdornmentsOfTestExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, TCallback, TReturn> RaisesProtectedEvent<TCallback, TReturn>(this global::Rocks.MethodAdornments<global::MockTests.Test, TCallback, TReturn> self, global::System.EventArgs args)
						where TCallback : global::System.Delegate
					{
						self.Handler.AddRaiseEvent(new("ProtectedEvent", args));
						return self;
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, TCallback> RaisesProtectedEvent<TCallback>(this global::Rocks.MethodAdornments<global::MockTests.Test, TCallback> self, global::System.EventArgs args)
						where TCallback : global::System.Delegate
					{
						self.Handler.AddRaiseEvent(new("ProtectedEvent", args));
						return self;
					}
				}
				
				internal static class PropertyAdornmentsOfTestExtensions
				{
					internal static global::Rocks.PropertyAdornments<global::MockTests.Test, TCallback, TReturn> RaisesProtectedEvent<TCallback, TReturn>(this global::Rocks.PropertyAdornments<global::MockTests.Test, TCallback, TReturn> self, global::System.EventArgs args)
						where TCallback : global::System.Delegate
					{
						self.Handler.AddRaiseEvent(new("ProtectedEvent", args));
						return self;
					}
					internal static global::Rocks.PropertyAdornments<global::MockTests.Test, TCallback> RaisesProtectedEvent<TCallback>(this global::Rocks.PropertyAdornments<global::MockTests.Test, TCallback> self, global::System.EventArgs args)
						where TCallback : global::System.Delegate
					{
						self.Handler.AddRaiseEvent(new("ProtectedEvent", args));
						return self;
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "Test_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task MakeWithNonPublicVirtualMenbersAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public abstract class Test
				{
					protected abstract void ProtectedMethod();
					protected abstract string ProtectedProperty { get; set; }
					protected abstract event EventHandler ProtectedEvent;
				}

				public static class TestUser
				{
					public static void Generate()
					{
						var rock = Rock.Make<Test>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class MakeExpectationsOfTestExtensions
				{
					internal static global::MockTests.Test Instance(this global::Rocks.MakeGeneration<global::MockTests.Test> self) =>
						new RockTest();
					
					private sealed class RockTest
						: global::MockTests.Test
					{
						public RockTest() { }
						
						public override bool Equals(object? obj)
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
						protected override void ProtectedMethod()
						{
						}
						protected override string ProtectedProperty
						{
							get => default!;
							set { }
						}
						
						#pragma warning disable CS0067
						protected override event global::System.EventHandler? ProtectedEvent;
						#pragma warning restore CS0067
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "Test_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task MakeWithNonPublicAbstractMembersAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public abstract class Test
				{
					protected virtual void ProtectedMethod() { }
					protected virtual string ProtectedProperty { get; set; }
					protected virtual event EventHandler ProtectedEvent;
				}

				public static class TestUser
				{
					public static void Generate()
					{
						var rock = Rock.Make<Test>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class MakeExpectationsOfTestExtensions
				{
					internal static global::MockTests.Test Instance(this global::Rocks.MakeGeneration<global::MockTests.Test> self) =>
						new RockTest();
					
					private sealed class RockTest
						: global::MockTests.Test
					{
						public RockTest() { }
						
						public override bool Equals(object? obj)
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
						protected override void ProtectedMethod()
						{
						}
						protected override string ProtectedProperty
						{
							get => default!;
							set { }
						}
						
						#pragma warning disable CS0067
						protected override event global::System.EventHandler? ProtectedEvent;
						#pragma warning restore CS0067
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "Test_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}