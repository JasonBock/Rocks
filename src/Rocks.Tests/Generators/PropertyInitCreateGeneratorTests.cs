using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class PropertyInitCreateGeneratorTests
{
	[Test]
	public static async Task GenerateWithInitAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			#nullable enable
			namespace MockTests
			{
				public interface ITest
				{
					int NonNullableValueType { get; init; }
					int? NullableValueType { get; init; }
					string NonNullableReferenceType { get; init; }
					string? NullableReferenceType { get; init; }
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<ITest>();
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
				internal static class CreateExpectationsOfITestExtensions
				{
					internal static global::Rocks.Expectations.PropertyExpectations<global::MockTests.ITest> Properties(this global::Rocks.Expectations.Expectations<global::MockTests.ITest> @self) =>
						new(@self);
					
					internal static global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.ITest> Getters(this global::Rocks.Expectations.PropertyExpectations<global::MockTests.ITest> @self) =>
						new(@self);
					
					internal sealed class ConstructorProperties
					{
						internal int NonNullableValueType { get; init; }
						internal int? NullableValueType { get; init; }
						internal string? NonNullableReferenceType { get; init; }
						internal string? NullableReferenceType { get; init; }
					}
					
					internal static global::MockTests.ITest Instance(this global::Rocks.Expectations.Expectations<global::MockTests.ITest> @self, ConstructorProperties? @constructorProperties)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							return @constructorProperties is null ?
								new RockITest(@self) :
								new RockITest(@self)
								{
									NonNullableValueType = @constructorProperties.NonNullableValueType,
									NullableValueType = @constructorProperties.NullableValueType,
									NonNullableReferenceType = @constructorProperties.NonNullableReferenceType!,
									NullableReferenceType = @constructorProperties.NullableReferenceType,
								};
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockITest
						: global::MockTests.ITest
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockITest(global::Rocks.Expectations.Expectations<global::MockTests.ITest> @expectations) =>
							this.handlers = @expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "get_NonNullableValueType()")]
						[global::Rocks.MemberIdentifier(1, "set_NonNullableValueType(@value)")]
						public int NonNullableValueType
						{
							get
							{
								if (this.handlers.TryGetValue(0, out var @methodHandlers))
								{
									var @methodHandler = @methodHandlers[0];
									var @result = @methodHandler.Method is not null ?
										global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int>>(@methodHandler.Method)() :
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
									@methodHandler.IncrementCallCount();
									return @result!;
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_NonNullableValueType())");
							}
							init { }
						}
						[global::Rocks.MemberIdentifier(2, "get_NullableValueType()")]
						[global::Rocks.MemberIdentifier(3, "set_NullableValueType(@value)")]
						public int? NullableValueType
						{
							get
							{
								if (this.handlers.TryGetValue(2, out var @methodHandlers))
								{
									var @methodHandler = @methodHandlers[0];
									var @result = @methodHandler.Method is not null ?
										global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int?>>(@methodHandler.Method)() :
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int?>>(@methodHandler).ReturnValue;
									@methodHandler.IncrementCallCount();
									return @result!;
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_NullableValueType())");
							}
							init { }
						}
						[global::Rocks.MemberIdentifier(4, "get_NonNullableReferenceType()")]
						[global::Rocks.MemberIdentifier(5, "set_NonNullableReferenceType(@value)")]
						public string NonNullableReferenceType
						{
							get
							{
								if (this.handlers.TryGetValue(4, out var @methodHandlers))
								{
									var @methodHandler = @methodHandlers[0];
									var @result = @methodHandler.Method is not null ?
										global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string>>(@methodHandler.Method)() :
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string>>(@methodHandler).ReturnValue;
									@methodHandler.IncrementCallCount();
									return @result!;
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_NonNullableReferenceType())");
							}
							init { }
						}
						[global::Rocks.MemberIdentifier(6, "get_NullableReferenceType()")]
						[global::Rocks.MemberIdentifier(7, "set_NullableReferenceType(@value)")]
						public string? NullableReferenceType
						{
							get
							{
								if (this.handlers.TryGetValue(6, out var @methodHandlers))
								{
									var @methodHandler = @methodHandlers[0];
									var @result = @methodHandler.Method is not null ?
										global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string?>>(@methodHandler.Method)() :
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string?>>(@methodHandler).ReturnValue;
									@methodHandler.IncrementCallCount();
									return @result!;
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_NullableReferenceType())");
							}
							init { }
						}
					}
				}
				
				internal static class PropertyGetterExpectationsOfITestExtensions
				{
					internal static global::Rocks.PropertyAdornments<global::MockTests.ITest, global::System.Func<int>, int> NonNullableValueType(this global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.ITest> @self) =>
						new global::Rocks.PropertyAdornments<global::MockTests.ITest, global::System.Func<int>, int>(@self.Add<int>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.PropertyAdornments<global::MockTests.ITest, global::System.Func<int?>, int?> NullableValueType(this global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.ITest> @self) =>
						new global::Rocks.PropertyAdornments<global::MockTests.ITest, global::System.Func<int?>, int?>(@self.Add<int?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.PropertyAdornments<global::MockTests.ITest, global::System.Func<string>, string> NonNullableReferenceType(this global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.ITest> @self) =>
						new global::Rocks.PropertyAdornments<global::MockTests.ITest, global::System.Func<string>, string>(@self.Add<string>(4, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.PropertyAdornments<global::MockTests.ITest, global::System.Func<string?>, string?> NullableReferenceType(this global::Rocks.Expectations.PropertyGetterExpectations<global::MockTests.ITest> @self) =>
						new global::Rocks.PropertyAdornments<global::MockTests.ITest, global::System.Func<string?>, string?>(@self.Add<string?>(6, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ITest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Ignore("Currently having a nullable issue with this ONE TEST.")]
	[Test]
	public static async Task GenerateWithRequiredAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public class Test
				{
					public virtual void Foo() { }
					public required string? Data { get; set; }
				}

				public static class RockTest
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
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.Test> @self) =>
						new(@self);
					
					internal sealed class ConstructorProperties
					{
						internal required string? Data { get; init; }
					}
					
					internal static global::MockTests.Test Instance(this global::Rocks.Expectations.Expectations<global::MockTests.Test> @self, ConstructorProperties @constructorProperties)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							return @constructorProperties is null ?
								throw new global::System.ArgumentNullException(nameof(@constructorProperties)) :
								new RockTest(@self)
								{
									Data = @constructorProperties.Data,
								};
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockTest
						: global::MockTests.Test
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockTest(global::Rocks.Expectations.Expectations<global::MockTests.Test> @expectations) =>
							this.handlers = @expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "bool Equals(object? @obj)")]
						public override bool Equals(object? @obj)
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(@methodHandler.Expectations[0]).IsValid(@obj))
									{
										var @result = @methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, bool>>(@methodHandler.Method)(obj) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(@methodHandler).ReturnValue;
										@methodHandler.IncrementCallCount();
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
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
								@methodHandler.IncrementCallCount();
								return @result!;
							}
							else
							{
								return base.GetHashCode();
							}
						}
						
						[global::Rocks.MemberIdentifier(2, "string? ToString()")]
						public override string? ToString()
						{
							if (this.handlers.TryGetValue(2, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string?>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string?>>(@methodHandler).ReturnValue;
								@methodHandler.IncrementCallCount();
								return @result!;
							}
							else
							{
								return base.ToString();
							}
						}
						
						[global::Rocks.MemberIdentifier(3, "void Foo()")]
						public override void Foo()
						{
							if (this.handlers.TryGetValue(3, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								if (@methodHandler.Method is not null)
								{
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action>(@methodHandler.Method)();
								}
								
								@methodHandler.IncrementCallCount();
							}
							else
							{
								base.Foo();
							}
						}
						
					}
				}
				
				internal static class MethodExpectationsOfTestExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> @self, global::Rocks.Argument<object?> @obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(@obj);
						return new global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Action> Foo(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Test> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Test, global::System.Action>(@self.Add(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "Test_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}