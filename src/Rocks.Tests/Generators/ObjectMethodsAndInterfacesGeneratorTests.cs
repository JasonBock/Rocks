using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ObjectMethodsAndInterfacesGeneratorTests
{
	[Test]
	public static async Task GenerateWithExactMatchesCreateAsync()
	{
		var code =
			"""
			using Rocks;

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
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfIMatchObjectOfobjectExtensions
				{
					internal static global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> ExplicitMethodsForIMatchObjectOfobject(this global::Rocks.Expectations.Expectations<global::MockTests.IMatchObject<object>> self) =>
						new(self);
					
					internal static global::MockTests.IMatchObject<object> Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IMatchObject<object>> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockIMatchObjectOfobject(self);
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIMatchObjectOfobject
						: global::MockTests.IMatchObject<object>
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockIMatchObjectOfobject(global::Rocks.Expectations.Expectations<global::MockTests.IMatchObject<object>> expectations) =>
							this.handlers = expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "bool global::MockTests.IMatchObject<object>.Equals(object? other)")]
						bool global::MockTests.IMatchObject<object>.Equals(object? other)
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								foreach (var methodHandler in methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(methodHandler.Expectations[0]).IsValid(other))
									{
										var result = methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, bool>>(methodHandler.Method)(other) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(methodHandler).ReturnValue;
										methodHandler.IncrementCallCount();
										return result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool global::MockTests.IMatchObject<object>.Equals(object? other)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for bool global::MockTests.IMatchObject<object>.Equals(object? other)");
						}
						
						[global::Rocks.MemberIdentifier(1, "bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? objA, object? objB)")]
						bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? objA, object? objB)
						{
							if (this.handlers.TryGetValue(1, out var methodHandlers))
							{
								foreach (var methodHandler in methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(methodHandler.Expectations[0]).IsValid(objA) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(methodHandler.Expectations[1]).IsValid(objB))
									{
										var result = methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, object?, bool>>(methodHandler.Method)(objA, objB) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(methodHandler).ReturnValue;
										methodHandler.IncrementCallCount();
										return result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? objA, object? objB)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? objA, object? objB)");
						}
						
						[global::Rocks.MemberIdentifier(2, "object global::MockTests.IMatchObject<object>.MemberwiseClone()")]
						object global::MockTests.IMatchObject<object>.MemberwiseClone()
						{
							if (this.handlers.TryGetValue(2, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								var result = methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object>>(methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<object>>(methodHandler).ReturnValue;
								methodHandler.IncrementCallCount();
								return result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for object global::MockTests.IMatchObject<object>.MemberwiseClone()");
						}
						
					}
				}
				
				internal static class ExplicitMethodExpectationsOfIMatchObjectOfobjectForIMatchObjectOfobjectExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> self, global::Rocks.Argument<object?> other)
					{
						global::System.ArgumentNullException.ThrowIfNull(other);
						return new global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, bool>, bool>(self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { other }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, object?, bool>, bool> ReferenceEquals(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> self, global::Rocks.Argument<object?> objA, global::Rocks.Argument<object?> objB)
					{
						global::System.ArgumentNullException.ThrowIfNull(objA);
						global::System.ArgumentNullException.ThrowIfNull(objB);
						return new global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, object?, bool>, bool>(self.Add<bool>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { objA, objB }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object>, object> MemberwiseClone(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> self) =>
						new global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object>, object>(self.Add<object>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IMatchObjectOfobject_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithExactMatchesMakeAsync()
	{
		var code =
			"""
			using Rocks;

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
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class MakeExpectationsOfIMatchObjectOfobjectExtensions
				{
					internal static global::MockTests.IMatchObject<object> Instance(this global::Rocks.MakeGeneration<global::MockTests.IMatchObject<object>> self) =>
						new RockIMatchObjectOfobject();
					
					private sealed class RockIMatchObjectOfobject
						: global::MockTests.IMatchObject<object>
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
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "IMatchObjectOfobject_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithDiffByReturnTypeMatchesCreateAsync()
	{
		var code =
			"""
			using Rocks;

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
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfIMatchObjectOfobjectExtensions
				{
					internal static global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> ExplicitMethodsForIMatchObjectOfobject(this global::Rocks.Expectations.Expectations<global::MockTests.IMatchObject<object>> self) =>
						new(self);
					
					internal static global::MockTests.IMatchObject<object> Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IMatchObject<object>> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockIMatchObjectOfobject(self);
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIMatchObjectOfobject
						: global::MockTests.IMatchObject<object>
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockIMatchObjectOfobject(global::Rocks.Expectations.Expectations<global::MockTests.IMatchObject<object>> expectations) =>
							this.handlers = expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "string global::MockTests.IMatchObject<object>.Equals(object? other)")]
						string global::MockTests.IMatchObject<object>.Equals(object? other)
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								foreach (var methodHandler in methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(methodHandler.Expectations[0]).IsValid(other))
									{
										var result = methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, string>>(methodHandler.Method)(other) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string>>(methodHandler).ReturnValue;
										methodHandler.IncrementCallCount();
										return result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for string global::MockTests.IMatchObject<object>.Equals(object? other)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for string global::MockTests.IMatchObject<object>.Equals(object? other)");
						}
						
						[global::Rocks.MemberIdentifier(1, "int global::MockTests.IMatchObject<object>.ReferenceEquals(object? objA, object? objB)")]
						int global::MockTests.IMatchObject<object>.ReferenceEquals(object? objA, object? objB)
						{
							if (this.handlers.TryGetValue(1, out var methodHandlers))
							{
								foreach (var methodHandler in methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(methodHandler.Expectations[0]).IsValid(objA) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(methodHandler.Expectations[1]).IsValid(objB))
									{
										var result = methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, object?, int>>(methodHandler.Method)(objA, objB) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(methodHandler).ReturnValue;
										methodHandler.IncrementCallCount();
										return result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for int global::MockTests.IMatchObject<object>.ReferenceEquals(object? objA, object? objB)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for int global::MockTests.IMatchObject<object>.ReferenceEquals(object? objA, object? objB)");
						}
						
						[global::Rocks.MemberIdentifier(2, "bool global::MockTests.IMatchObject<object>.MemberwiseClone()")]
						bool global::MockTests.IMatchObject<object>.MemberwiseClone()
						{
							if (this.handlers.TryGetValue(2, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								var result = methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<bool>>(methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(methodHandler).ReturnValue;
								methodHandler.IncrementCallCount();
								return result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for bool global::MockTests.IMatchObject<object>.MemberwiseClone()");
						}
						
					}
				}
				
				internal static class ExplicitMethodExpectationsOfIMatchObjectOfobjectForIMatchObjectOfobjectExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, string>, string> Equals(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> self, global::Rocks.Argument<object?> other)
					{
						global::System.ArgumentNullException.ThrowIfNull(other);
						return new global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, string>, string>(self.Add<string>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { other }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, object?, int>, int> ReferenceEquals(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> self, global::Rocks.Argument<object?> objA, global::Rocks.Argument<object?> objB)
					{
						global::System.ArgumentNullException.ThrowIfNull(objA);
						global::System.ArgumentNullException.ThrowIfNull(objB);
						return new global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, object?, int>, int>(self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { objA, objB }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<bool>, bool> MemberwiseClone(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> self) =>
						new global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<bool>, bool>(self.Add<bool>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IMatchObjectOfobject_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithDiffByReturnTypeMatchesMakeAsync()
	{
		var code =
			"""
			using Rocks;

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
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class MakeExpectationsOfIMatchObjectOfobjectExtensions
				{
					internal static global::MockTests.IMatchObject<object> Instance(this global::Rocks.MakeGeneration<global::MockTests.IMatchObject<object>> self) =>
						new RockIMatchObjectOfobject();
					
					private sealed class RockIMatchObjectOfobject
						: global::MockTests.IMatchObject<object>
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
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "IMatchObjectOfobject_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}