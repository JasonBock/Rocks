using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class NonPublicMembersGeneratorTests
{
	[Test]
	public static async Task CreateWithProtectedVirtualMembersAsync()
	{
		var code =
@"using Rocks;
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
}";

		var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using Rocks.Expectations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

#nullable enable
namespace MockTests
{
	internal static class CreateExpectationsOfTestExtensions
	{
		internal static MethodExpectations<Test> Methods(this Expectations<Test> self) =>
			new(self);
		
		internal static PropertyExpectations<Test> Properties(this Expectations<Test> self) =>
			new(self);
		
		internal static PropertyGetterExpectations<Test> Getters(this PropertyExpectations<Test> self) =>
			new(self.Expectations);
		
		internal static PropertySetterExpectations<Test> Setters(this PropertyExpectations<Test> self) =>
			new(self.Expectations);
		
		internal static Test Instance(this Expectations<Test> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockTest(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockTest
			: Test, IMockWithEvents
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockTest(Expectations<Test> expectations) =>
				this.handlers = expectations.Handlers;
			
			[MemberIdentifier(0, ""bool Equals(object? obj)"")]
			public override bool Equals(object? obj)
			{
				if (this.handlers.TryGetValue(0, out var methodHandlers))
				{
					foreach (var methodHandler in methodHandlers)
					{
						if (((methodHandler.Expectations[0] as Argument<object?>)?.IsValid(obj) ?? false))
						{
							var result = methodHandler.Method is not null ?
								((Func<object?, bool>)methodHandler.Method)(obj) :
								((HandlerInformation<bool>)methodHandler).ReturnValue;
							methodHandler.RaiseEvents(this);
							methodHandler.IncrementCallCount();
							return result!;
						}
					}
					
					throw new ExpectationException(""No handlers match for bool Equals(object? obj)"");
				}
				else
				{
					return base.Equals(obj);
				}
			}
			
			[MemberIdentifier(1, ""int GetHashCode()"")]
			public override int GetHashCode()
			{
				if (this.handlers.TryGetValue(1, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method is not null ?
						((Func<int>)methodHandler.Method)() :
						((HandlerInformation<int>)methodHandler).ReturnValue;
					methodHandler.RaiseEvents(this);
					methodHandler.IncrementCallCount();
					return result!;
				}
				else
				{
					return base.GetHashCode();
				}
			}
			
			[MemberIdentifier(2, ""string? ToString()"")]
			public override string? ToString()
			{
				if (this.handlers.TryGetValue(2, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method is not null ?
						((Func<string?>)methodHandler.Method)() :
						((HandlerInformation<string?>)methodHandler).ReturnValue;
					methodHandler.RaiseEvents(this);
					methodHandler.IncrementCallCount();
					return result!;
				}
				else
				{
					return base.ToString();
				}
			}
			
			[MemberIdentifier(3, ""void ProtectedMethod()"")]
			protected override void ProtectedMethod()
			{
				if (this.handlers.TryGetValue(3, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					if (methodHandler.Method is not null)
					{
						((Action)methodHandler.Method)();
					}
					
					methodHandler.RaiseEvents(this);
					methodHandler.IncrementCallCount();
				}
				else
				{
					base.ProtectedMethod();
				}
			}
			
			[MemberIdentifier(4, ""get_ProtectedProperty()"")]
			[MemberIdentifier(5, ""set_ProtectedProperty(value)"")]
			protected override string ProtectedProperty
			{
				get
				{
					if (this.handlers.TryGetValue(4, out var methodHandlers))
					{
						var methodHandler = methodHandlers[0];
						var result = methodHandler.Method is not null ?
							((Func<string>)methodHandler.Method)() :
							((HandlerInformation<string>)methodHandler).ReturnValue;
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
							if ((methodHandler.Expectations[0] as Argument<string>)?.IsValid(value) ?? false)
							{
								foundMatch = true;
								
								if (methodHandler.Method is not null)
								{
									((Action<string>)methodHandler.Method)(value);
								}
								
								if (!foundMatch)
								{
									throw new ExpectationException(""No handlers match for set_ProtectedProperty(value)"");
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
			protected override event EventHandler? ProtectedEvent;
			#pragma warning restore CS0067
			
			void IMockWithEvents.Raise(string fieldName, EventArgs args)
			{
				var thisType = this.GetType();
				var eventDelegate = (MulticastDelegate)thisType.GetField(fieldName, 
					BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(this)!;
				
				if (eventDelegate is not null)
				{
					foreach (var handler in eventDelegate.GetInvocationList())
					{
						handler.Method.Invoke(handler.Target, new object[]{this, args});
					}
				}
			}
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfTestExtensions
	{
		internal static MethodAdornments<Test, Func<object?, bool>, bool> Equals(this MethodExpectations<Test> self, Argument<object?> obj) =>
			new MethodAdornments<Test, Func<object?, bool>, bool>(self.Add<bool>(0, new List<Argument>(1) { obj }));
		internal static MethodAdornments<Test, Func<int>, int> GetHashCode(this MethodExpectations<Test> self) =>
			new MethodAdornments<Test, Func<int>, int>(self.Add<int>(1, new List<Argument>()));
		internal static MethodAdornments<Test, Func<string?>, string?> ToString(this MethodExpectations<Test> self) =>
			new MethodAdornments<Test, Func<string?>, string?>(self.Add<string?>(2, new List<Argument>()));
		internal static MethodAdornments<Test, Action> ProtectedMethod(this MethodExpectations<Test> self) =>
			new MethodAdornments<Test, Action>(self.Add(3, new List<Argument>()));
	}
	
	internal static class PropertyGetterExpectationsOfTestExtensions
	{
		internal static PropertyAdornments<Test, Func<string>, string> ProtectedProperty(this PropertyGetterExpectations<Test> self) =>
			new PropertyAdornments<Test, Func<string>, string>(self.Add<string>(4, new List<Argument>()));
	}
	internal static class PropertySetterExpectationsOfTestExtensions
	{
		internal static PropertyAdornments<Test, Action<string>> ProtectedProperty(this PropertySetterExpectations<Test> self, Argument<string> value) =>
			new PropertyAdornments<Test, Action<string>>(self.Add(5, new List<Argument>(1) { value }));
	}
	
	internal static class MethodAdornmentsOfTestExtensions
	{
		internal static MethodAdornments<Test, TCallback, TReturn> RaisesProtectedEvent<TCallback, TReturn>(this MethodAdornments<Test, TCallback, TReturn> self, EventArgs args)
			where TCallback : Delegate
		{
			self.Handler.AddRaiseEvent(new(""ProtectedEvent"", args));
			return self;
		}
		internal static MethodAdornments<Test, TCallback> RaisesProtectedEvent<TCallback>(this MethodAdornments<Test, TCallback> self, EventArgs args)
			where TCallback : Delegate
		{
			self.Handler.AddRaiseEvent(new(""ProtectedEvent"", args));
			return self;
		}
	}
	
	internal static class PropertyAdornmentsOfTestExtensions
	{
		internal static PropertyAdornments<Test, TCallback, TReturn> RaisesProtectedEvent<TCallback, TReturn>(this PropertyAdornments<Test, TCallback, TReturn> self, EventArgs args)
			where TCallback : Delegate
		{
			self.Handler.AddRaiseEvent(new(""ProtectedEvent"", args));
			return self;
		}
		internal static PropertyAdornments<Test, TCallback> RaisesProtectedEvent<TCallback>(this PropertyAdornments<Test, TCallback> self, EventArgs args)
			where TCallback : Delegate
		{
			self.Handler.AddRaiseEvent(new(""ProtectedEvent"", args));
			return self;
		}
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "Test_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task CreateWithNonPublicAbstractMembersAsync()
	{
		var code =
@"using Rocks;
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
}";

		var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using Rocks.Expectations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

#nullable enable
namespace MockTests
{
	internal static class CreateExpectationsOfTestExtensions
	{
		internal static MethodExpectations<Test> Methods(this Expectations<Test> self) =>
			new(self);
		
		internal static PropertyExpectations<Test> Properties(this Expectations<Test> self) =>
			new(self);
		
		internal static PropertyGetterExpectations<Test> Getters(this PropertyExpectations<Test> self) =>
			new(self.Expectations);
		
		internal static PropertySetterExpectations<Test> Setters(this PropertyExpectations<Test> self) =>
			new(self.Expectations);
		
		internal static Test Instance(this Expectations<Test> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockTest(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockTest
			: Test, IMockWithEvents
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockTest(Expectations<Test> expectations) =>
				this.handlers = expectations.Handlers;
			
			[MemberIdentifier(0, ""bool Equals(object? obj)"")]
			public override bool Equals(object? obj)
			{
				if (this.handlers.TryGetValue(0, out var methodHandlers))
				{
					foreach (var methodHandler in methodHandlers)
					{
						if (((methodHandler.Expectations[0] as Argument<object?>)?.IsValid(obj) ?? false))
						{
							var result = methodHandler.Method is not null ?
								((Func<object?, bool>)methodHandler.Method)(obj) :
								((HandlerInformation<bool>)methodHandler).ReturnValue;
							methodHandler.RaiseEvents(this);
							methodHandler.IncrementCallCount();
							return result!;
						}
					}
					
					throw new ExpectationException(""No handlers match for bool Equals(object? obj)"");
				}
				else
				{
					return base.Equals(obj);
				}
			}
			
			[MemberIdentifier(1, ""int GetHashCode()"")]
			public override int GetHashCode()
			{
				if (this.handlers.TryGetValue(1, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method is not null ?
						((Func<int>)methodHandler.Method)() :
						((HandlerInformation<int>)methodHandler).ReturnValue;
					methodHandler.RaiseEvents(this);
					methodHandler.IncrementCallCount();
					return result!;
				}
				else
				{
					return base.GetHashCode();
				}
			}
			
			[MemberIdentifier(2, ""string? ToString()"")]
			public override string? ToString()
			{
				if (this.handlers.TryGetValue(2, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method is not null ?
						((Func<string?>)methodHandler.Method)() :
						((HandlerInformation<string?>)methodHandler).ReturnValue;
					methodHandler.RaiseEvents(this);
					methodHandler.IncrementCallCount();
					return result!;
				}
				else
				{
					return base.ToString();
				}
			}
			
			[MemberIdentifier(3, ""void ProtectedMethod()"")]
			protected override void ProtectedMethod()
			{
				if (this.handlers.TryGetValue(3, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					if (methodHandler.Method is not null)
					{
						((Action)methodHandler.Method)();
					}
					
					methodHandler.RaiseEvents(this);
					methodHandler.IncrementCallCount();
				}
				else
				{
					throw new ExpectationException(""No handlers were found for void ProtectedMethod()"");
				}
			}
			
			[MemberIdentifier(4, ""get_ProtectedProperty()"")]
			[MemberIdentifier(5, ""set_ProtectedProperty(value)"")]
			protected override string ProtectedProperty
			{
				get
				{
					if (this.handlers.TryGetValue(4, out var methodHandlers))
					{
						var methodHandler = methodHandlers[0];
						var result = methodHandler.Method is not null ?
							((Func<string>)methodHandler.Method)() :
							((HandlerInformation<string>)methodHandler).ReturnValue;
						methodHandler.RaiseEvents(this);
						methodHandler.IncrementCallCount();
						return result!;
					}
					
					throw new ExpectationException(""No handlers were found for get_ProtectedProperty())"");
				}
				set
				{
					if (this.handlers.TryGetValue(5, out var methodHandlers))
					{
						var foundMatch = false;
						foreach (var methodHandler in methodHandlers)
						{
							if ((methodHandler.Expectations[0] as Argument<string>)?.IsValid(value) ?? false)
							{
								foundMatch = true;
								
								if (methodHandler.Method is not null)
								{
									((Action<string>)methodHandler.Method)(value);
								}
								
								if (!foundMatch)
								{
									throw new ExpectationException(""No handlers match for set_ProtectedProperty(value)"");
								}
								
								methodHandler.RaiseEvents(this);
								methodHandler.IncrementCallCount();
								break;
							}
						}
					}
					else
					{
						throw new ExpectationException(""No handlers were found for set_ProtectedProperty(value)"");
					}
				}
			}
			
			#pragma warning disable CS0067
			protected override event EventHandler? ProtectedEvent;
			#pragma warning restore CS0067
			
			void IMockWithEvents.Raise(string fieldName, EventArgs args)
			{
				var thisType = this.GetType();
				var eventDelegate = (MulticastDelegate)thisType.GetField(fieldName, 
					BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(this)!;
				
				if (eventDelegate is not null)
				{
					foreach (var handler in eventDelegate.GetInvocationList())
					{
						handler.Method.Invoke(handler.Target, new object[]{this, args});
					}
				}
			}
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfTestExtensions
	{
		internal static MethodAdornments<Test, Func<object?, bool>, bool> Equals(this MethodExpectations<Test> self, Argument<object?> obj) =>
			new MethodAdornments<Test, Func<object?, bool>, bool>(self.Add<bool>(0, new List<Argument>(1) { obj }));
		internal static MethodAdornments<Test, Func<int>, int> GetHashCode(this MethodExpectations<Test> self) =>
			new MethodAdornments<Test, Func<int>, int>(self.Add<int>(1, new List<Argument>()));
		internal static MethodAdornments<Test, Func<string?>, string?> ToString(this MethodExpectations<Test> self) =>
			new MethodAdornments<Test, Func<string?>, string?>(self.Add<string?>(2, new List<Argument>()));
		internal static MethodAdornments<Test, Action> ProtectedMethod(this MethodExpectations<Test> self) =>
			new MethodAdornments<Test, Action>(self.Add(3, new List<Argument>()));
	}
	
	internal static class PropertyGetterExpectationsOfTestExtensions
	{
		internal static PropertyAdornments<Test, Func<string>, string> ProtectedProperty(this PropertyGetterExpectations<Test> self) =>
			new PropertyAdornments<Test, Func<string>, string>(self.Add<string>(4, new List<Argument>()));
	}
	internal static class PropertySetterExpectationsOfTestExtensions
	{
		internal static PropertyAdornments<Test, Action<string>> ProtectedProperty(this PropertySetterExpectations<Test> self, Argument<string> value) =>
			new PropertyAdornments<Test, Action<string>>(self.Add(5, new List<Argument>(1) { value }));
	}
	
	internal static class MethodAdornmentsOfTestExtensions
	{
		internal static MethodAdornments<Test, TCallback, TReturn> RaisesProtectedEvent<TCallback, TReturn>(this MethodAdornments<Test, TCallback, TReturn> self, EventArgs args)
			where TCallback : Delegate
		{
			self.Handler.AddRaiseEvent(new(""ProtectedEvent"", args));
			return self;
		}
		internal static MethodAdornments<Test, TCallback> RaisesProtectedEvent<TCallback>(this MethodAdornments<Test, TCallback> self, EventArgs args)
			where TCallback : Delegate
		{
			self.Handler.AddRaiseEvent(new(""ProtectedEvent"", args));
			return self;
		}
	}
	
	internal static class PropertyAdornmentsOfTestExtensions
	{
		internal static PropertyAdornments<Test, TCallback, TReturn> RaisesProtectedEvent<TCallback, TReturn>(this PropertyAdornments<Test, TCallback, TReturn> self, EventArgs args)
			where TCallback : Delegate
		{
			self.Handler.AddRaiseEvent(new(""ProtectedEvent"", args));
			return self;
		}
		internal static PropertyAdornments<Test, TCallback> RaisesProtectedEvent<TCallback>(this PropertyAdornments<Test, TCallback> self, EventArgs args)
			where TCallback : Delegate
		{
			self.Handler.AddRaiseEvent(new(""ProtectedEvent"", args));
			return self;
		}
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "Test_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task MakeWithNonPublicVirtualMenbersAsync()
	{
		var code =
@"using Rocks;
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
}";

		var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

#nullable enable
namespace MockTests
{
	internal static class MakeExpectationsOfTestExtensions
	{
		internal static Test Instance(this MakeGeneration<Test> self) =>
			new RockTest();
		
		private sealed class RockTest
			: Test
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
			protected override event EventHandler? ProtectedEvent;
			#pragma warning restore CS0067
		}
	}
}
";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "Test_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task MakeWithNonPublicAbstractMembersAsync()
	{
		var code =
@"using Rocks;
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
}";

		var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

#nullable enable
namespace MockTests
{
	internal static class MakeExpectationsOfTestExtensions
	{
		internal static Test Instance(this MakeGeneration<Test> self) =>
			new RockTest();
		
		private sealed class RockTest
			: Test
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
			protected override event EventHandler? ProtectedEvent;
			#pragma warning restore CS0067
		}
	}
}
";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "Test_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}