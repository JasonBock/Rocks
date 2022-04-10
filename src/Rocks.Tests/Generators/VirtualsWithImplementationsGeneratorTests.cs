using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class VirtualsWithImplementationsGeneratorTests
{
	[Test]
	public static async Task GenerateForInterfaceReturnsVoidAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public interface IHaveImplementation
	{
		void Foo() { }
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<IHaveImplementation>();
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

#nullable enable
namespace MockTests
{
	internal static class CreateExpectationsOfIHaveImplementationExtensions
	{
		internal static MethodExpectations<IHaveImplementation> Methods(this Expectations<IHaveImplementation> self) =>
			new(self);
		
		internal static IHaveImplementation Instance(this Expectations<IHaveImplementation> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockIHaveImplementation(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockIHaveImplementation
			: IHaveImplementation, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockIHaveImplementation(Expectations<IHaveImplementation> expectations) =>
				this.handlers = expectations.Handlers;
			
			[MemberIdentifier(0, ""void Foo()"")]
			public void Foo()
			{
				if (this.handlers.TryGetValue(0, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					if (methodHandler.Method is not null)
					{
						((Action)methodHandler.Method)();
					}
					
					methodHandler.IncrementCallCount();
				}
				else
				{
					throw new ExpectationException(""No handlers were found for void Foo()"");
				}
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfIHaveImplementationExtensions
	{
		internal static MethodAdornments<IHaveImplementation, Action> Foo(this MethodExpectations<IHaveImplementation> self) =>
			new MethodAdornments<IHaveImplementation, Action>(self.Add(0, new List<Argument>()));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IHaveImplementation_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateForInterfaceReturnsValueAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public interface IHaveImplementation
	{
		int Foo() => 3;
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<IHaveImplementation>();
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

#nullable enable
namespace MockTests
{
	internal static class CreateExpectationsOfIHaveImplementationExtensions
	{
		internal static MethodExpectations<IHaveImplementation> Methods(this Expectations<IHaveImplementation> self) =>
			new(self);
		
		internal static IHaveImplementation Instance(this Expectations<IHaveImplementation> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockIHaveImplementation(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockIHaveImplementation
			: IHaveImplementation, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockIHaveImplementation(Expectations<IHaveImplementation> expectations) =>
				this.handlers = expectations.Handlers;
			
			[MemberIdentifier(0, ""int Foo()"")]
			public int Foo()
			{
				if (this.handlers.TryGetValue(0, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method is not null ?
						((Func<int>)methodHandler.Method)() :
						((HandlerInformation<int>)methodHandler).ReturnValue;
					methodHandler.IncrementCallCount();
					return result!;
				}
				
				throw new ExpectationException(""No handlers were found for int Foo()"");
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfIHaveImplementationExtensions
	{
		internal static MethodAdornments<IHaveImplementation, Func<int>, int> Foo(this MethodExpectations<IHaveImplementation> self) =>
			new MethodAdornments<IHaveImplementation, Func<int>, int>(self.Add<int>(0, new List<Argument>()));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IHaveImplementation_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateForClassReturnsVoidAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public class HaveImplementation
	{
		public virtual void Foo() { }
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<HaveImplementation>();
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

#nullable enable
namespace MockTests
{
	internal static class CreateExpectationsOfHaveImplementationExtensions
	{
		internal static MethodExpectations<HaveImplementation> Methods(this Expectations<HaveImplementation> self) =>
			new(self);
		
		internal static HaveImplementation Instance(this Expectations<HaveImplementation> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockHaveImplementation(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockHaveImplementation
			: HaveImplementation, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockHaveImplementation(Expectations<HaveImplementation> expectations) =>
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
					methodHandler.IncrementCallCount();
					return result!;
				}
				else
				{
					return base.ToString();
				}
			}
			
			[MemberIdentifier(3, ""void Foo()"")]
			public override void Foo()
			{
				if (this.handlers.TryGetValue(3, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					if (methodHandler.Method is not null)
					{
						((Action)methodHandler.Method)();
					}
					
					methodHandler.IncrementCallCount();
				}
				else
				{
					base.Foo();
				}
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfHaveImplementationExtensions
	{
		internal static MethodAdornments<HaveImplementation, Func<object?, bool>, bool> Equals(this MethodExpectations<HaveImplementation> self, Argument<object?> obj) =>
			new MethodAdornments<HaveImplementation, Func<object?, bool>, bool>(self.Add<bool>(0, new List<Argument>(1) { obj }));
		internal static MethodAdornments<HaveImplementation, Func<int>, int> GetHashCode(this MethodExpectations<HaveImplementation> self) =>
			new MethodAdornments<HaveImplementation, Func<int>, int>(self.Add<int>(1, new List<Argument>()));
		internal static MethodAdornments<HaveImplementation, Func<string?>, string?> ToString(this MethodExpectations<HaveImplementation> self) =>
			new MethodAdornments<HaveImplementation, Func<string?>, string?>(self.Add<string?>(2, new List<Argument>()));
		internal static MethodAdornments<HaveImplementation, Action> Foo(this MethodExpectations<HaveImplementation> self) =>
			new MethodAdornments<HaveImplementation, Action>(self.Add(3, new List<Argument>()));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "HaveImplementation_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateForClassReturnsValueAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public class HaveImplementation
	{
		public virtual int Foo() => 3;
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<HaveImplementation>();
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

#nullable enable
namespace MockTests
{
	internal static class CreateExpectationsOfHaveImplementationExtensions
	{
		internal static MethodExpectations<HaveImplementation> Methods(this Expectations<HaveImplementation> self) =>
			new(self);
		
		internal static HaveImplementation Instance(this Expectations<HaveImplementation> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockHaveImplementation(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockHaveImplementation
			: HaveImplementation, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockHaveImplementation(Expectations<HaveImplementation> expectations) =>
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
					methodHandler.IncrementCallCount();
					return result!;
				}
				else
				{
					return base.ToString();
				}
			}
			
			[MemberIdentifier(3, ""int Foo()"")]
			public override int Foo()
			{
				if (this.handlers.TryGetValue(3, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method is not null ?
						((Func<int>)methodHandler.Method)() :
						((HandlerInformation<int>)methodHandler).ReturnValue;
					methodHandler.IncrementCallCount();
					return result!;
				}
				else
				{
					return base.Foo();
				}
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfHaveImplementationExtensions
	{
		internal static MethodAdornments<HaveImplementation, Func<object?, bool>, bool> Equals(this MethodExpectations<HaveImplementation> self, Argument<object?> obj) =>
			new MethodAdornments<HaveImplementation, Func<object?, bool>, bool>(self.Add<bool>(0, new List<Argument>(1) { obj }));
		internal static MethodAdornments<HaveImplementation, Func<int>, int> GetHashCode(this MethodExpectations<HaveImplementation> self) =>
			new MethodAdornments<HaveImplementation, Func<int>, int>(self.Add<int>(1, new List<Argument>()));
		internal static MethodAdornments<HaveImplementation, Func<string?>, string?> ToString(this MethodExpectations<HaveImplementation> self) =>
			new MethodAdornments<HaveImplementation, Func<string?>, string?>(self.Add<string?>(2, new List<Argument>()));
		internal static MethodAdornments<HaveImplementation, Func<int>, int> Foo(this MethodExpectations<HaveImplementation> self) =>
			new MethodAdornments<HaveImplementation, Func<int>, int>(self.Add<int>(3, new List<Argument>()));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "HaveImplementation_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}