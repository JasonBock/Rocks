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
			using Rocks;
			using Rocks.Exceptions;
			using Rocks.Expectations;
			using System;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			using System.Runtime.CompilerServices;
			
			#nullable enable
			namespace MockTests
			{
				internal static class CreateExpectationsOfITestExtensions
				{
					internal static PropertyExpectations<ITest> Properties(this Expectations<ITest> self) =>
						new(self);
					
					internal static PropertyGetterExpectations<ITest> Getters(this PropertyExpectations<ITest> self) =>
						new(self);
					
					public sealed class ConstructorProperties
					{
						public int NonNullableValueType { get; init; }
						public int? NullableValueType { get; init; }
						public string? NonNullableReferenceType { get; init; }
						public string? NullableReferenceType { get; init; }
					}
					
					internal static ITest Instance(this Expectations<ITest> self, ConstructorProperties? constructorProperties)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return constructorProperties is null ?
								new RockITest(self) :
								new RockITest(self)
								{
									NonNullableValueType = constructorProperties.NonNullableValueType,
									NullableValueType = constructorProperties.NullableValueType,
									NonNullableReferenceType = constructorProperties.NonNullableReferenceType!,
									NullableReferenceType = constructorProperties.NullableReferenceType,
								};
						}
						else
						{
							throw new NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockITest
						: ITest
					{
						private readonly Dictionary<int, List<HandlerInformation>> handlers;
						
						public RockITest(Expectations<ITest> expectations) =>
							this.handlers = expectations.Handlers;
						
						[MemberIdentifier(0, "get_NonNullableValueType()")]
						[MemberIdentifier(1, "set_NonNullableValueType(value)")]
						public int NonNullableValueType
						{
							get
							{
								if (this.handlers.TryGetValue(0, out var methodHandlers))
								{
									var methodHandler = methodHandlers[0];
									var result = methodHandler.Method is not null ?
										Unsafe.As<Func<int>>(methodHandler.Method)() :
										Unsafe.As<HandlerInformation<int>>(methodHandler).ReturnValue;
									methodHandler.IncrementCallCount();
									return result!;
								}
								
								throw new ExpectationException("No handlers were found for get_NonNullableValueType())");
							}
							init { }
						}
						[MemberIdentifier(2, "get_NullableValueType()")]
						[MemberIdentifier(3, "set_NullableValueType(value)")]
						public int? NullableValueType
						{
							get
							{
								if (this.handlers.TryGetValue(2, out var methodHandlers))
								{
									var methodHandler = methodHandlers[0];
									var result = methodHandler.Method is not null ?
										Unsafe.As<Func<int?>>(methodHandler.Method)() :
										Unsafe.As<HandlerInformation<int?>>(methodHandler).ReturnValue;
									methodHandler.IncrementCallCount();
									return result!;
								}
								
								throw new ExpectationException("No handlers were found for get_NullableValueType())");
							}
							init { }
						}
						[MemberIdentifier(4, "get_NonNullableReferenceType()")]
						[MemberIdentifier(5, "set_NonNullableReferenceType(value)")]
						public string NonNullableReferenceType
						{
							get
							{
								if (this.handlers.TryGetValue(4, out var methodHandlers))
								{
									var methodHandler = methodHandlers[0];
									var result = methodHandler.Method is not null ?
										Unsafe.As<Func<string>>(methodHandler.Method)() :
										Unsafe.As<HandlerInformation<string>>(methodHandler).ReturnValue;
									methodHandler.IncrementCallCount();
									return result!;
								}
								
								throw new ExpectationException("No handlers were found for get_NonNullableReferenceType())");
							}
							init { }
						}
						[MemberIdentifier(6, "get_NullableReferenceType()")]
						[MemberIdentifier(7, "set_NullableReferenceType(value)")]
						public string? NullableReferenceType
						{
							get
							{
								if (this.handlers.TryGetValue(6, out var methodHandlers))
								{
									var methodHandler = methodHandlers[0];
									var result = methodHandler.Method is not null ?
										Unsafe.As<Func<string?>>(methodHandler.Method)() :
										Unsafe.As<HandlerInformation<string?>>(methodHandler).ReturnValue;
									methodHandler.IncrementCallCount();
									return result!;
								}
								
								throw new ExpectationException("No handlers were found for get_NullableReferenceType())");
							}
							init { }
						}
					}
				}
				
				internal static class PropertyGetterExpectationsOfITestExtensions
				{
					internal static PropertyAdornments<ITest, Func<int>, int> NonNullableValueType(this PropertyGetterExpectations<ITest> self) =>
						new PropertyAdornments<ITest, Func<int>, int>(self.Add<int>(0, new List<Argument>()));
					internal static PropertyAdornments<ITest, Func<int?>, int?> NullableValueType(this PropertyGetterExpectations<ITest> self) =>
						new PropertyAdornments<ITest, Func<int?>, int?>(self.Add<int?>(2, new List<Argument>()));
					internal static PropertyAdornments<ITest, Func<string>, string> NonNullableReferenceType(this PropertyGetterExpectations<ITest> self) =>
						new PropertyAdornments<ITest, Func<string>, string>(self.Add<string>(4, new List<Argument>()));
					internal static PropertyAdornments<ITest, Func<string?>, string?> NullableReferenceType(this PropertyGetterExpectations<ITest> self) =>
						new PropertyAdornments<ITest, Func<string?>, string?>(self.Add<string?>(6, new List<Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ITest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	//[Ignore("Currently cannot run this test until the required property feature is fully supported by the compiler.")]
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
					public required string Data { get; set; }
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
			using Rocks;
			using Rocks.Exceptions;
			using Rocks.Expectations;
			using System;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			#nullable enable
			namespace MockTests
			{
				internal static class CreateExpectationsOfTestExtensions
				{
					internal static MethodExpectations<Test> Methods(this Expectations<Test> self) =>
						new(self);
					
					public sealed class ConstructorProperties
					{
						public required string Data { get; init; }
					}
					
					internal static Test Instance(this Expectations<Test> self, ConstructorProperties constructorProperties)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return constructorProperties is null ?
								throw new ArgumentNullException(nameof(constructorProperties)) :
								new RockTest(self)
								{
									Data = constructorProperties.Data,
								};
						}
						else
						{
							throw new NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockTest
						: Test
					{
						private readonly Dictionary<int, List<HandlerInformation>> handlers;
						
						public RockTest(Expectations<Test> expectations) =>
							this.handlers = expectations.Handlers;
						
						[MemberIdentifier(0, "bool Equals(object? obj)")]
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
								
								throw new ExpectationException("No handlers match for bool Equals(object? obj)");
							}
							else
							{
								return base.Equals(obj);
							}
						}
						
						[MemberIdentifier(1, "int GetHashCode()")]
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
						
						[MemberIdentifier(2, "string? ToString()")]
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
						
						[MemberIdentifier(3, "void Foo()")]
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
					internal static MethodAdornments<Test, Action> Foo(this MethodExpectations<Test> self) =>
						new MethodAdornments<Test, Action>(self.Add(3, new List<Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "Test_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}