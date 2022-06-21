using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class VirtualsWithImplementationsGeneratorTests
{
	[Test]
	public static async Task GenerateForMethodWithParamsReturnsVoidAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public class VoidMethodWithParams
	{
		public virtual void CallMe(params string[] values) { }
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<VoidMethodWithParams>();
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
	internal static class CreateExpectationsOfVoidMethodWithParamsExtensions
	{
		internal static MethodExpectations<VoidMethodWithParams> Methods(this Expectations<VoidMethodWithParams> self) =>
			new(self);
		
		internal static VoidMethodWithParams Instance(this Expectations<VoidMethodWithParams> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockVoidMethodWithParams(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockVoidMethodWithParams
			: VoidMethodWithParams, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockVoidMethodWithParams(Expectations<VoidMethodWithParams> expectations) =>
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
			
			[MemberIdentifier(3, ""void CallMe(params string[] values)"")]
			public override void CallMe(params string[] values)
			{
				if (this.handlers.TryGetValue(3, out var methodHandlers))
				{
					var foundMatch = false;
					
					foreach (var methodHandler in methodHandlers)
					{
						if (((methodHandler.Expectations[0] as Argument<string[]>)?.IsValid(values) ?? false))
						{
							foundMatch = true;
							
							if (methodHandler.Method is not null)
							{
								((Action<string[]>)methodHandler.Method)(values);
							}
							
							methodHandler.IncrementCallCount();
							break;
						}
					}
					
					if (!foundMatch)
					{
						throw new ExpectationException(""No handlers match for void CallMe(params string[] values))"");
					}
				}
				else
				{
					base.CallMe(values);
				}
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfVoidMethodWithParamsExtensions
	{
		internal static MethodAdornments<VoidMethodWithParams, Func<object?, bool>, bool> Equals(this MethodExpectations<VoidMethodWithParams> self, Argument<object?> obj) =>
			new MethodAdornments<VoidMethodWithParams, Func<object?, bool>, bool>(self.Add<bool>(0, new List<Argument>(1) { obj }));
		internal static MethodAdornments<VoidMethodWithParams, Func<int>, int> GetHashCode(this MethodExpectations<VoidMethodWithParams> self) =>
			new MethodAdornments<VoidMethodWithParams, Func<int>, int>(self.Add<int>(1, new List<Argument>()));
		internal static MethodAdornments<VoidMethodWithParams, Func<string?>, string?> ToString(this MethodExpectations<VoidMethodWithParams> self) =>
			new MethodAdornments<VoidMethodWithParams, Func<string?>, string?>(self.Add<string?>(2, new List<Argument>()));
		internal static MethodAdornments<VoidMethodWithParams, Action<string[]>> CallMe(this MethodExpectations<VoidMethodWithParams> self, Argument<string[]> values) =>
			new MethodAdornments<VoidMethodWithParams, Action<string[]>>(self.Add(3, new List<Argument>(1) { values }));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "VoidMethodWithParams_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateForMethodWithParamsReturnsValueAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public class ValueMethodWithParams
	{
		public virtual int CallMe(params string[] values) => default;
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<ValueMethodWithParams>();
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
	internal static class CreateExpectationsOfValueMethodWithParamsExtensions
	{
		internal static MethodExpectations<ValueMethodWithParams> Methods(this Expectations<ValueMethodWithParams> self) =>
			new(self);
		
		internal static ValueMethodWithParams Instance(this Expectations<ValueMethodWithParams> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockValueMethodWithParams(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockValueMethodWithParams
			: ValueMethodWithParams, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockValueMethodWithParams(Expectations<ValueMethodWithParams> expectations) =>
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
			
			[MemberIdentifier(3, ""int CallMe(params string[] values)"")]
			public override int CallMe(params string[] values)
			{
				if (this.handlers.TryGetValue(3, out var methodHandlers))
				{
					foreach (var methodHandler in methodHandlers)
					{
						if (((methodHandler.Expectations[0] as Argument<string[]>)?.IsValid(values) ?? false))
						{
							var result = methodHandler.Method is not null ?
								((Func<string[], int>)methodHandler.Method)(values) :
								((HandlerInformation<int>)methodHandler).ReturnValue;
							methodHandler.IncrementCallCount();
							return result!;
						}
					}
					
					throw new ExpectationException(""No handlers match for int CallMe(params string[] values)"");
				}
				else
				{
					return base.CallMe(values);
				}
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfValueMethodWithParamsExtensions
	{
		internal static MethodAdornments<ValueMethodWithParams, Func<object?, bool>, bool> Equals(this MethodExpectations<ValueMethodWithParams> self, Argument<object?> obj) =>
			new MethodAdornments<ValueMethodWithParams, Func<object?, bool>, bool>(self.Add<bool>(0, new List<Argument>(1) { obj }));
		internal static MethodAdornments<ValueMethodWithParams, Func<int>, int> GetHashCode(this MethodExpectations<ValueMethodWithParams> self) =>
			new MethodAdornments<ValueMethodWithParams, Func<int>, int>(self.Add<int>(1, new List<Argument>()));
		internal static MethodAdornments<ValueMethodWithParams, Func<string?>, string?> ToString(this MethodExpectations<ValueMethodWithParams> self) =>
			new MethodAdornments<ValueMethodWithParams, Func<string?>, string?>(self.Add<string?>(2, new List<Argument>()));
		internal static MethodAdornments<ValueMethodWithParams, Func<string[], int>, int> CallMe(this MethodExpectations<ValueMethodWithParams> self, Argument<string[]> values) =>
			new MethodAdornments<ValueMethodWithParams, Func<string[], int>, int>(self.Add<int>(3, new List<Argument>(1) { values }));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ValueMethodWithParams_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

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
			private readonly IHaveImplementation shimForIHaveImplementation;
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockIHaveImplementation(Expectations<IHaveImplementation> expectations) =>
				(this.handlers, this.shimForIHaveImplementation) = (expectations.Handlers, new ShimRockIHaveImplementation(this));
			
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
					this.shimForIHaveImplementation.Foo();
				}
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
			
			private sealed class ShimRockIHaveImplementation
				: IHaveImplementation
			{
				private readonly RockIHaveImplementation mock;
				
				public ShimRockIHaveImplementation(RockIHaveImplementation mock) =>
					this.mock = mock;
			}
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
			private readonly IHaveImplementation shimForIHaveImplementation;
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockIHaveImplementation(Expectations<IHaveImplementation> expectations) =>
				(this.handlers, this.shimForIHaveImplementation) = (expectations.Handlers, new ShimRockIHaveImplementation(this));
			
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
				else
				{
					return this.shimForIHaveImplementation.Foo();
				}
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
			
			private sealed class ShimRockIHaveImplementation
				: IHaveImplementation
			{
				private readonly RockIHaveImplementation mock;
				
				public ShimRockIHaveImplementation(RockIHaveImplementation mock) =>
					this.mock = mock;
			}
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