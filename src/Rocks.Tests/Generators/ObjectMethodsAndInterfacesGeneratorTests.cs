using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ObjectMethodsAndInterfacesGeneratorTests
{
	[Test]
	public static async Task CreateWhenObjectMethodIsHiddenAsStaticAsync()
	{
		var code =
			"""
			using Rocks;

			#nullable enable

			namespace MockTests
			{
				public class StaticToString
				{
					protected static new string ToString() => "c";   
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<StaticToString>();
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
				internal static class CreateExpectationsOfStaticToStringExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.StaticToString> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.StaticToString> @self) =>
						new(@self);
					
					internal static global::MockTests.StaticToString Instance(this global::Rocks.Expectations.Expectations<global::MockTests.StaticToString> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockStaticToString(@self);
							@self.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockStaticToString
						: global::MockTests.StaticToString
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockStaticToString(global::Rocks.Expectations.Expectations<global::MockTests.StaticToString> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
						[global::Rocks.MemberIdentifier(0, "bool Equals(object? @obj)")]
						public override bool Equals(object? @obj)
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(@methodHandler.Expectations[0]).IsValid(@obj))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, bool>>(@methodHandler.Method)(@obj) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(@methodHandler).ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool Equals(object? @obj)");
							}
							else
							{
								return base.Equals(@obj);
							}
						}
						
						[global::Rocks.MemberIdentifier(1, "int GetHashCode()")]
						public override int GetHashCode()
						{
							if (this.handlers.TryGetValue(1, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
								return @result!;
							}
							else
							{
								return base.GetHashCode();
							}
						}
						
					}
				}
				
				internal static class MethodExpectationsOfStaticToStringExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.StaticToString, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::MockTests.StaticToString> @self, global::Rocks.Argument<object?> @obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(@obj);
						return new global::Rocks.MethodAdornments<global::MockTests.StaticToString, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.StaticToString, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::MockTests.StaticToString> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.StaticToString, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGeneratorV3>(code,
			new[] { (typeof(RockCreateGeneratorV3), "StaticToString_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task MakeWhenObjectMethodIsHiddenAsStaticAsync()
	{
		var code =
			"""
			using Rocks;

			#nullable enable

			namespace MockTests
			{
				public class StaticToString
				{
					protected static new string ToString() => "c";   
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Make<StaticToString>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class MakeExpectationsOfStaticToStringExtensions
				{
					internal static global::MockTests.StaticToString Instance(this global::Rocks.MakeGeneration<global::MockTests.StaticToString> @self)
					{
						return new RockStaticToString();
					}
					
					private sealed class RockStaticToString
						: global::MockTests.StaticToString
					{
						public RockStaticToString()
						{
						}
						
						public override bool Equals(object? @obj)
						{
							return default!;
						}
						public override int GetHashCode()
						{
							return default!;
						}
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGeneratorV3>(code,
			new[] { (typeof(RockMakeGeneratorV3), "StaticToString_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

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
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfIMatchObjectOfobjectExtensions
				{
					internal static global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> ExplicitMethodsForIMatchObjectOfobject(this global::Rocks.Expectations.Expectations<global::MockTests.IMatchObject<object>> @self) =>
						new(@self);
					
					internal static global::MockTests.IMatchObject<object> Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IMatchObject<object>> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockIMatchObjectOfobject(@self);
							@self.MockType = @mock.GetType();
							return @mock;
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
						
						public RockIMatchObjectOfobject(global::Rocks.Expectations.Expectations<global::MockTests.IMatchObject<object>> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
						[global::Rocks.MemberIdentifier(0, "bool global::MockTests.IMatchObject<object>.Equals(object? @other)")]
						bool global::MockTests.IMatchObject<object>.Equals(object? @other)
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(@methodHandler.Expectations[0]).IsValid(@other))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, bool>>(@methodHandler.Method)(@other) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(@methodHandler).ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool global::MockTests.IMatchObject<object>.Equals(object? @other)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for bool global::MockTests.IMatchObject<object>.Equals(object? @other)");
						}
						
						[global::Rocks.MemberIdentifier(1, "bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)")]
						bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)
						{
							if (this.handlers.TryGetValue(1, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(@methodHandler.Expectations[0]).IsValid(@objA) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(@methodHandler.Expectations[1]).IsValid(@objB))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, object?, bool>>(@methodHandler.Method)(@objA, @objB) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(@methodHandler).ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)");
						}
						
						[global::Rocks.MemberIdentifier(2, "object global::MockTests.IMatchObject<object>.MemberwiseClone()")]
						object global::MockTests.IMatchObject<object>.MemberwiseClone()
						{
							if (this.handlers.TryGetValue(2, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<object>>(@methodHandler).ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for object global::MockTests.IMatchObject<object>.MemberwiseClone()");
						}
						
					}
				}
				
				internal static class ExplicitMethodExpectationsOfIMatchObjectOfobjectForIMatchObjectOfobjectExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> @self, global::Rocks.Argument<object?> @other)
					{
						global::System.ArgumentNullException.ThrowIfNull(@other);
						return new global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @other }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, object?, bool>, bool> ReferenceEquals(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> @self, global::Rocks.Argument<object?> @objA, global::Rocks.Argument<object?> @objB)
					{
						global::System.ArgumentNullException.ThrowIfNull(@objA);
						global::System.ArgumentNullException.ThrowIfNull(@objB);
						return new global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, object?, bool>, bool>(@self.Add<bool>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { @objA, @objB }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object>, object> MemberwiseClone(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object>, object>(@self.Add<object>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGeneratorV3>(code,
			new[] { (typeof(RockCreateGeneratorV3), "IMatchObjectOfobject_Rock_Create.g.cs", generatedCode) },
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
					internal static global::MockTests.IMatchObject<object> Instance(this global::Rocks.MakeGeneration<global::MockTests.IMatchObject<object>> @self)
					{
						return new RockIMatchObjectOfobject();
					}
					
					private sealed class RockIMatchObjectOfobject
						: global::MockTests.IMatchObject<object>
					{
						public RockIMatchObjectOfobject()
						{
						}
						
						bool global::MockTests.IMatchObject<object>.Equals(object? @other)
						{
							return default!;
						}
						bool global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)
						{
							return default!;
						}
						object global::MockTests.IMatchObject<object>.MemberwiseClone()
						{
							return default!;
						}
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGeneratorV3>(code,
			new[] { (typeof(RockMakeGeneratorV3), "IMatchObjectOfobject_Rock_Make.g.cs", generatedCode) },
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
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfIMatchObjectOfobjectExtensions
				{
					internal static global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> ExplicitMethodsForIMatchObjectOfobject(this global::Rocks.Expectations.Expectations<global::MockTests.IMatchObject<object>> @self) =>
						new(@self);
					
					internal static global::MockTests.IMatchObject<object> Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IMatchObject<object>> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockIMatchObjectOfobject(@self);
							@self.MockType = @mock.GetType();
							return @mock;
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
						
						public RockIMatchObjectOfobject(global::Rocks.Expectations.Expectations<global::MockTests.IMatchObject<object>> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
						[global::Rocks.MemberIdentifier(0, "string global::MockTests.IMatchObject<object>.Equals(object? @other)")]
						string global::MockTests.IMatchObject<object>.Equals(object? @other)
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(@methodHandler.Expectations[0]).IsValid(@other))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, string>>(@methodHandler.Method)(@other) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string>>(@methodHandler).ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for string global::MockTests.IMatchObject<object>.Equals(object? @other)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for string global::MockTests.IMatchObject<object>.Equals(object? @other)");
						}
						
						[global::Rocks.MemberIdentifier(1, "int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)")]
						int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)
						{
							if (this.handlers.TryGetValue(1, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(@methodHandler.Expectations[0]).IsValid(@objA) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(@methodHandler.Expectations[1]).IsValid(@objB))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, object?, int>>(@methodHandler.Method)(@objA, @objB) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)");
						}
						
						[global::Rocks.MemberIdentifier(2, "bool global::MockTests.IMatchObject<object>.MemberwiseClone()")]
						bool global::MockTests.IMatchObject<object>.MemberwiseClone()
						{
							if (this.handlers.TryGetValue(2, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<bool>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(@methodHandler).ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for bool global::MockTests.IMatchObject<object>.MemberwiseClone()");
						}
						
					}
				}
				
				internal static class ExplicitMethodExpectationsOfIMatchObjectOfobjectForIMatchObjectOfobjectExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, string>, string> Equals(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> @self, global::Rocks.Argument<object?> @other)
					{
						global::System.ArgumentNullException.ThrowIfNull(@other);
						return new global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, string>, string>(@self.Add<string>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @other }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, object?, int>, int> ReferenceEquals(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> @self, global::Rocks.Argument<object?> @objA, global::Rocks.Argument<object?> @objB)
					{
						global::System.ArgumentNullException.ThrowIfNull(@objA);
						global::System.ArgumentNullException.ThrowIfNull(@objB);
						return new global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<object?, object?, int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { @objA, @objB }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<bool>, bool> MemberwiseClone(this global::Rocks.Expectations.ExplicitMethodExpectations<global::MockTests.IMatchObject<object>, global::MockTests.IMatchObject<object>> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.IMatchObject<object>, global::System.Func<bool>, bool>(@self.Add<bool>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGeneratorV3>(code,
			new[] { (typeof(RockCreateGeneratorV3), "IMatchObjectOfobject_Rock_Create.g.cs", generatedCode) },
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
					internal static global::MockTests.IMatchObject<object> Instance(this global::Rocks.MakeGeneration<global::MockTests.IMatchObject<object>> @self)
					{
						return new RockIMatchObjectOfobject();
					}
					
					private sealed class RockIMatchObjectOfobject
						: global::MockTests.IMatchObject<object>
					{
						public RockIMatchObjectOfobject()
						{
						}
						
						string global::MockTests.IMatchObject<object>.Equals(object? @other)
						{
							return default!;
						}
						int global::MockTests.IMatchObject<object>.ReferenceEquals(object? @objA, object? @objB)
						{
							return default!;
						}
						bool global::MockTests.IMatchObject<object>.MemberwiseClone()
						{
							return default!;
						}
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGeneratorV3>(code,
			new[] { (typeof(RockMakeGeneratorV3), "IMatchObjectOfobject_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}