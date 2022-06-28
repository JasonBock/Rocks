using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class DoesNotReturnGeneratorTests
{
	[Test]
	public static async Task GenerateClassCreateAsync()
	{
		var code =
@"using Rocks;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MockTests
{
	public class ClassTest
	{
		[DoesNotReturn]
		public virtual void VoidMethod() => throw new NotSupportedException();

		[DoesNotReturn]
		public virtual int IntMethod() => throw new NotSupportedException();
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<ClassTest>();
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
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace MockTests
{
	internal static class CreateExpectationsOfClassTestExtensions
	{
		internal static MethodExpectations<ClassTest> Methods(this Expectations<ClassTest> self) =>
			new(self);
		
		internal static ClassTest Instance(this Expectations<ClassTest> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockClassTest(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockClassTest
			: ClassTest, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockClassTest(Expectations<ClassTest> expectations) =>
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
			
			[DoesNotReturn]
			[MemberIdentifier(3, ""void VoidMethod()"")]
			public override void VoidMethod()
			{
				if (this.handlers.TryGetValue(3, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					if (methodHandler.Method is not null)
					{
						((Action)methodHandler.Method)();
					}
					
					methodHandler.IncrementCallCount();
					throw new DoesNotReturnException();
				}
				else
				{
					base.VoidMethod();
				}
			}
			
			[DoesNotReturn]
			[MemberIdentifier(4, ""int IntMethod()"")]
			public override int IntMethod()
			{
				if (this.handlers.TryGetValue(4, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					_ = methodHandler.Method is not null ?
						((Func<int>)methodHandler.Method)() :
						((HandlerInformation<int>)methodHandler).ReturnValue;
					methodHandler.IncrementCallCount();
					throw new DoesNotReturnException();
				}
				else
				{
					return base.IntMethod();
				}
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfClassTestExtensions
	{
		internal static MethodAdornments<ClassTest, Func<object?, bool>, bool> Equals(this MethodExpectations<ClassTest> self, Argument<object?> obj) =>
			new MethodAdornments<ClassTest, Func<object?, bool>, bool>(self.Add<bool>(0, new List<Argument>(1) { obj }));
		internal static MethodAdornments<ClassTest, Func<int>, int> GetHashCode(this MethodExpectations<ClassTest> self) =>
			new MethodAdornments<ClassTest, Func<int>, int>(self.Add<int>(1, new List<Argument>()));
		internal static MethodAdornments<ClassTest, Func<string?>, string?> ToString(this MethodExpectations<ClassTest> self) =>
			new MethodAdornments<ClassTest, Func<string?>, string?>(self.Add<string?>(2, new List<Argument>()));
		internal static MethodAdornments<ClassTest, Action> VoidMethod(this MethodExpectations<ClassTest> self) =>
			new MethodAdornments<ClassTest, Action>(self.Add(3, new List<Argument>()));
		internal static MethodAdornments<ClassTest, Func<int>, int> IntMethod(this MethodExpectations<ClassTest> self) =>
			new MethodAdornments<ClassTest, Func<int>, int>(self.Add<int>(4, new List<Argument>()));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ClassTest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateClassMakeAsync()
	{
		var code =
@"using Rocks;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MockTests
{
	public class ClassTest
	{
		[DoesNotReturn]
		public virtual void VoidMethod() => throw new NotSupportedException();

		[DoesNotReturn]
		public virtual int IntMethod() => throw new NotSupportedException();
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<ClassTest>();
		}
	}
}";

		var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace MockTests
{
	internal static class MakeExpectationsOfClassTestExtensions
	{
		internal static ClassTest Instance(this MakeGeneration<ClassTest> self) =>
			new RockClassTest();
		
		private sealed class RockClassTest
			: ClassTest
		{
			public RockClassTest() { }
			
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
			[DoesNotReturn]
			public override void VoidMethod()
			{
				throw new DoesNotReturnException();
			}
			[DoesNotReturn]
			public override int IntMethod()
			{
				throw new DoesNotReturnException();
			}
		}
	}
}
";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "ClassTest_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateInterfaceCreateAsync()
	{
		var code =
@"using Rocks;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MockTests
{
	public interface IInterfaceTest
	{
		[DoesNotReturn]
		void VoidMethod();

		[DoesNotReturn]
		int IntMethod();
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<IInterfaceTest>();
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
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace MockTests
{
	internal static class CreateExpectationsOfIInterfaceTestExtensions
	{
		internal static MethodExpectations<IInterfaceTest> Methods(this Expectations<IInterfaceTest> self) =>
			new(self);
		
		internal static IInterfaceTest Instance(this Expectations<IInterfaceTest> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockIInterfaceTest(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockIInterfaceTest
			: IInterfaceTest, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockIInterfaceTest(Expectations<IInterfaceTest> expectations) =>
				this.handlers = expectations.Handlers;
			
			[DoesNotReturn]
			[MemberIdentifier(0, ""void VoidMethod()"")]
			public void VoidMethod()
			{
				if (this.handlers.TryGetValue(0, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					if (methodHandler.Method is not null)
					{
						((Action)methodHandler.Method)();
					}
					
					methodHandler.IncrementCallCount();
					throw new DoesNotReturnException();
				}
				else
				{
					throw new ExpectationException(""No handlers were found for void VoidMethod()"");
				}
			}
			
			[DoesNotReturn]
			[MemberIdentifier(1, ""int IntMethod()"")]
			public int IntMethod()
			{
				if (this.handlers.TryGetValue(1, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					_ = methodHandler.Method is not null ?
						((Func<int>)methodHandler.Method)() :
						((HandlerInformation<int>)methodHandler).ReturnValue;
					methodHandler.IncrementCallCount();
					throw new DoesNotReturnException();
				}
				
				throw new ExpectationException(""No handlers were found for int IntMethod()"");
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfIInterfaceTestExtensions
	{
		internal static MethodAdornments<IInterfaceTest, Action> VoidMethod(this MethodExpectations<IInterfaceTest> self) =>
			new MethodAdornments<IInterfaceTest, Action>(self.Add(0, new List<Argument>()));
		internal static MethodAdornments<IInterfaceTest, Func<int>, int> IntMethod(this MethodExpectations<IInterfaceTest> self) =>
			new MethodAdornments<IInterfaceTest, Func<int>, int>(self.Add<int>(1, new List<Argument>()));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IInterfaceTest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateInterfaceMakeAsync()
	{
		var code =
@"using Rocks;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MockTests
{
	public interface IInterfaceTest
	{
		[DoesNotReturn]
		void VoidMethod();

		[DoesNotReturn]
		int IntMethod();
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<IInterfaceTest>();
		}
	}
}";

		var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace MockTests
{
	internal static class MakeExpectationsOfIInterfaceTestExtensions
	{
		internal static IInterfaceTest Instance(this MakeGeneration<IInterfaceTest> self) =>
			new RockIInterfaceTest();
		
		private sealed class RockIInterfaceTest
			: IInterfaceTest
		{
			public RockIInterfaceTest() { }
			
			[DoesNotReturn]
			public void VoidMethod()
			{
				throw new DoesNotReturnException();
			}
			[DoesNotReturn]
			public int IntMethod()
			{
				throw new DoesNotReturnException();
			}
		}
	}
}
";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "IInterfaceTest_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}