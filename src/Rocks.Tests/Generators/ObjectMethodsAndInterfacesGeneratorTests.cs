using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ObjectMethodsAndInterfacesGeneratorTests
{
	[Test]
	public static async Task GenerateWithExactMatchesCreateAsync()
	{
		var code =
@"using Rocks;

#nullable enable

namespace MockTests
{
	public interface IMatchObject<T>
	{
		bool Equals(T? other);
		bool ReferenceEquals(T? objA, T? objB);
		T MemberwiseClone();
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<IMatchObject<object>>();
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
	internal static class CreateExpectationsOfIMatchObjectOfobjectExtensions
	{
		internal static ExplicitMethodExpectations<IMatchObject<object>, IMatchObject<object>> ExplicitMethodsForIMatchObjectOfobject(this Expectations<IMatchObject<object>> self) =>
			new(self);
		
		internal static IMatchObject<object> Instance(this Expectations<IMatchObject<object>> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockIMatchObjectOfobject(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockIMatchObjectOfobject
			: IMatchObject<object>, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockIMatchObjectOfobject(Expectations<IMatchObject<object>> expectations) =>
				this.handlers = expectations.Handlers;
			
			[MemberIdentifier(0, ""bool IMatchObject<object>.Equals(object? other)"")]
			bool IMatchObject<object>.Equals(object? other)
			{
				if (this.handlers.TryGetValue(0, out var methodHandlers))
				{
					foreach (var methodHandler in methodHandlers)
					{
						if (((methodHandler.Expectations[0] as Argument<object?>)?.IsValid(other) ?? false))
						{
							var result = methodHandler.Method is not null ?
								((Func<object?, bool>)methodHandler.Method)(other) :
								((HandlerInformation<bool>)methodHandler).ReturnValue;
							methodHandler.IncrementCallCount();
							return result!;
						}
					}
					
					throw new ExpectationException(""No handlers match for bool IMatchObject<object>.Equals(object? other)"");
				}
				
				throw new ExpectationException(""No handlers were found for bool IMatchObject<object>.Equals(object? other)"");
			}
			
			[MemberIdentifier(1, ""bool IMatchObject<object>.ReferenceEquals(object? objA, object? objB)"")]
			bool IMatchObject<object>.ReferenceEquals(object? objA, object? objB)
			{
				if (this.handlers.TryGetValue(1, out var methodHandlers))
				{
					foreach (var methodHandler in methodHandlers)
					{
						if (((methodHandler.Expectations[0] as Argument<object?>)?.IsValid(objA) ?? false) &&
							((methodHandler.Expectations[1] as Argument<object?>)?.IsValid(objB) ?? false))
						{
							var result = methodHandler.Method is not null ?
								((Func<object?, object?, bool>)methodHandler.Method)(objA, objB) :
								((HandlerInformation<bool>)methodHandler).ReturnValue;
							methodHandler.IncrementCallCount();
							return result!;
						}
					}
					
					throw new ExpectationException(""No handlers match for bool IMatchObject<object>.ReferenceEquals(object? objA, object? objB)"");
				}
				
				throw new ExpectationException(""No handlers were found for bool IMatchObject<object>.ReferenceEquals(object? objA, object? objB)"");
			}
			
			[MemberIdentifier(2, ""object IMatchObject<object>.MemberwiseClone()"")]
			object IMatchObject<object>.MemberwiseClone()
			{
				if (this.handlers.TryGetValue(2, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method is not null ?
						((Func<object>)methodHandler.Method)() :
						((HandlerInformation<object>)methodHandler).ReturnValue;
					methodHandler.IncrementCallCount();
					return result!;
				}
				
				throw new ExpectationException(""No handlers were found for object IMatchObject<object>.MemberwiseClone()"");
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class ExplicitMethodExpectationsOfIMatchObjectOfobjectForIMatchObjectOfobjectExtensions
	{
		internal static MethodAdornments<IMatchObject<object>, Func<object?, bool>, bool> Equals(this ExplicitMethodExpectations<IMatchObject<object>, IMatchObject<object>> self, Argument<object?> other) =>
			new MethodAdornments<IMatchObject<object>, Func<object?, bool>, bool>(self.Add<bool>(0, new List<Argument>(1) { other }));
		internal static MethodAdornments<IMatchObject<object>, Func<object?, object?, bool>, bool> ReferenceEquals(this ExplicitMethodExpectations<IMatchObject<object>, IMatchObject<object>> self, Argument<object?> objA, Argument<object?> objB) =>
			new MethodAdornments<IMatchObject<object>, Func<object?, object?, bool>, bool>(self.Add<bool>(1, new List<Argument>(2) { objA, objB }));
		internal static MethodAdornments<IMatchObject<object>, Func<object>, object> MemberwiseClone(this ExplicitMethodExpectations<IMatchObject<object>, IMatchObject<object>> self) =>
			new MethodAdornments<IMatchObject<object>, Func<object>, object>(self.Add<object>(2, new List<Argument>()));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IMatchObjectOfobject_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithExactMatchesMakeAsync()
	{
		var code =
@"using Rocks;

#nullable enable

namespace MockTests
{
	public interface IMatchObject<T>
	{
		bool Equals(T? other);
		bool ReferenceEquals(T? objA, T? objB);
		T MemberwiseClone();
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<IMatchObject<object>>();
		}
	}
}";

		var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

#nullable enable
namespace MockTests
{
	internal static class MakeExpectationsOfIMatchObjectOfobjectExtensions
	{
		internal static IMatchObject<object> Instance(this MakeGeneration<IMatchObject<object>> self) =>
			new RockIMatchObjectOfobject();
		
		private sealed class RockIMatchObjectOfobject
			: IMatchObject<object>
		{
			public RockIMatchObjectOfobject() { }
			
			bool IMatchObject<object>.Equals(object? other)
			{
				return default!;
			}
			bool IMatchObject<object>.ReferenceEquals(object? objA, object? objB)
			{
				return default!;
			}
			object IMatchObject<object>.MemberwiseClone()
			{
				return default!;
			}
		}
	}
}
";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "IMatchObjectOfobject_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithDiffByReturnTypeMatchesCreateAsync()
	{
		var code =
@"using Rocks;

#nullable enable

namespace MockTests
{
	public interface IMatchObject<T>
	{
		string Equals(T? other);
		int ReferenceEquals(T? objA, T? objB);
		bool MemberwiseClone();
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<IMatchObject<object>>();
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
	internal static class CreateExpectationsOfIMatchObjectOfobjectExtensions
	{
		internal static ExplicitMethodExpectations<IMatchObject<object>, IMatchObject<object>> ExplicitMethodsForIMatchObjectOfobject(this Expectations<IMatchObject<object>> self) =>
			new(self);
		
		internal static IMatchObject<object> Instance(this Expectations<IMatchObject<object>> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockIMatchObjectOfobject(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockIMatchObjectOfobject
			: IMatchObject<object>, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockIMatchObjectOfobject(Expectations<IMatchObject<object>> expectations) =>
				this.handlers = expectations.Handlers;
			
			[MemberIdentifier(0, ""string IMatchObject<object>.Equals(object? other)"")]
			string IMatchObject<object>.Equals(object? other)
			{
				if (this.handlers.TryGetValue(0, out var methodHandlers))
				{
					foreach (var methodHandler in methodHandlers)
					{
						if (((methodHandler.Expectations[0] as Argument<object?>)?.IsValid(other) ?? false))
						{
							var result = methodHandler.Method is not null ?
								((Func<object?, string>)methodHandler.Method)(other) :
								((HandlerInformation<string>)methodHandler).ReturnValue;
							methodHandler.IncrementCallCount();
							return result!;
						}
					}
					
					throw new ExpectationException(""No handlers match for string IMatchObject<object>.Equals(object? other)"");
				}
				
				throw new ExpectationException(""No handlers were found for string IMatchObject<object>.Equals(object? other)"");
			}
			
			[MemberIdentifier(1, ""int IMatchObject<object>.ReferenceEquals(object? objA, object? objB)"")]
			int IMatchObject<object>.ReferenceEquals(object? objA, object? objB)
			{
				if (this.handlers.TryGetValue(1, out var methodHandlers))
				{
					foreach (var methodHandler in methodHandlers)
					{
						if (((methodHandler.Expectations[0] as Argument<object?>)?.IsValid(objA) ?? false) &&
							((methodHandler.Expectations[1] as Argument<object?>)?.IsValid(objB) ?? false))
						{
							var result = methodHandler.Method is not null ?
								((Func<object?, object?, int>)methodHandler.Method)(objA, objB) :
								((HandlerInformation<int>)methodHandler).ReturnValue;
							methodHandler.IncrementCallCount();
							return result!;
						}
					}
					
					throw new ExpectationException(""No handlers match for int IMatchObject<object>.ReferenceEquals(object? objA, object? objB)"");
				}
				
				throw new ExpectationException(""No handlers were found for int IMatchObject<object>.ReferenceEquals(object? objA, object? objB)"");
			}
			
			[MemberIdentifier(2, ""bool IMatchObject<object>.MemberwiseClone()"")]
			bool IMatchObject<object>.MemberwiseClone()
			{
				if (this.handlers.TryGetValue(2, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method is not null ?
						((Func<bool>)methodHandler.Method)() :
						((HandlerInformation<bool>)methodHandler).ReturnValue;
					methodHandler.IncrementCallCount();
					return result!;
				}
				
				throw new ExpectationException(""No handlers were found for bool IMatchObject<object>.MemberwiseClone()"");
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class ExplicitMethodExpectationsOfIMatchObjectOfobjectForIMatchObjectOfobjectExtensions
	{
		internal static MethodAdornments<IMatchObject<object>, Func<object?, string>, string> Equals(this ExplicitMethodExpectations<IMatchObject<object>, IMatchObject<object>> self, Argument<object?> other) =>
			new MethodAdornments<IMatchObject<object>, Func<object?, string>, string>(self.Add<string>(0, new List<Argument>(1) { other }));
		internal static MethodAdornments<IMatchObject<object>, Func<object?, object?, int>, int> ReferenceEquals(this ExplicitMethodExpectations<IMatchObject<object>, IMatchObject<object>> self, Argument<object?> objA, Argument<object?> objB) =>
			new MethodAdornments<IMatchObject<object>, Func<object?, object?, int>, int>(self.Add<int>(1, new List<Argument>(2) { objA, objB }));
		internal static MethodAdornments<IMatchObject<object>, Func<bool>, bool> MemberwiseClone(this ExplicitMethodExpectations<IMatchObject<object>, IMatchObject<object>> self) =>
			new MethodAdornments<IMatchObject<object>, Func<bool>, bool>(self.Add<bool>(2, new List<Argument>()));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IMatchObjectOfobject_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithDiffByReturnTypeMatchesMakeAsync()
	{
		var code =
@"using Rocks;

#nullable enable

namespace MockTests
{
	public interface IMatchObject<T>
	{
		string Equals(T? other);
		int ReferenceEquals(T? objA, T? objB);
		bool MemberwiseClone();
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<IMatchObject<object>>();
		}
	}
}";

		var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

#nullable enable
namespace MockTests
{
	internal static class MakeExpectationsOfIMatchObjectOfobjectExtensions
	{
		internal static IMatchObject<object> Instance(this MakeGeneration<IMatchObject<object>> self) =>
			new RockIMatchObjectOfobject();
		
		private sealed class RockIMatchObjectOfobject
			: IMatchObject<object>
		{
			public RockIMatchObjectOfobject() { }
			
			string IMatchObject<object>.Equals(object? other)
			{
				return default!;
			}
			int IMatchObject<object>.ReferenceEquals(object? objA, object? objB)
			{
				return default!;
			}
			bool IMatchObject<object>.MemberwiseClone()
			{
				return default!;
			}
		}
	}
}
";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "IMatchObjectOfobject_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}