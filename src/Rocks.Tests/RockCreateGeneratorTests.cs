using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;
using Rocks.Diagnostics;

namespace Rocks.Tests;

public static class RockCreateGeneratorTests
{
	[Test]
	public static async Task GenerateWhenInvocationIsNotRockCreateAsync()
	{
		var code =
			"""
			using Rocks;

			public interface ITest { }

			public static class Rocker
			{
				public static void Create<T>() { }
			}

			public static class Invoker
			{
				public static void Invoke()
				{
					Rocker.Create<ITest>();
				}
			}
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			Enumerable.Empty<(Type, string, string)>(),
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenTargetTypeContainsCompilerGeneratedMembersAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			
			#nullable enable
			namespace MockTests
			{
				public interface IContainNullableReferences
				{
					string? DoSomething(string? a, string b);
				}
				
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IContainNullableReferences>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfIContainNullableReferencesExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.IContainNullableReferences> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.IContainNullableReferences> self) =>
						new(self);
					
					internal static global::MockTests.IContainNullableReferences Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IContainNullableReferences> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockIContainNullableReferences(self);
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIContainNullableReferences
						: global::MockTests.IContainNullableReferences
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockIContainNullableReferences(global::Rocks.Expectations.Expectations<global::MockTests.IContainNullableReferences> expectations) =>
							this.handlers = expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "string? DoSomething(string? a, string b)")]
						public string? DoSomething(string? a, string b)
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								foreach (var methodHandler in methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string?>>(methodHandler.Expectations[0]).IsValid(a) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(methodHandler.Expectations[1]).IsValid(b))
									{
										var result = methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string?, string, string?>>(methodHandler.Method)(a, b) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string?>>(methodHandler).ReturnValue;
										methodHandler.IncrementCallCount();
										return result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for string? DoSomething(string? a, string b)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for string? DoSomething(string? a, string b)");
						}
						
					}
				}
				
				internal static class MethodExpectationsOfIContainNullableReferencesExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IContainNullableReferences, global::System.Func<string?, string, string?>, string?> DoSomething(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IContainNullableReferences> self, global::Rocks.Argument<string?> a, global::Rocks.Argument<string> b)
					{
						global::System.ArgumentNullException.ThrowIfNull(a);
						global::System.ArgumentNullException.ThrowIfNull(b);
						return new global::Rocks.MethodAdornments<global::MockTests.IContainNullableReferences, global::System.Func<string?, string, string?>, string?>(self.Add<string?>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { a, b }));
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IContainNullableReferences_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenTargetTypeIsValidAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public interface ITest
				{
					void Foo();
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
			#nullable enable
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfITestExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.ITest> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.ITest> self) =>
						new(self);
					
					internal static global::MockTests.ITest Instance(this global::Rocks.Expectations.Expectations<global::MockTests.ITest> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockITest(self);
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
						
						public RockITest(global::Rocks.Expectations.Expectations<global::MockTests.ITest> expectations) =>
							this.handlers = expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "void Foo()")]
						public void Foo()
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								if (methodHandler.Method is not null)
								{
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action>(methodHandler.Method)();
								}
								
								methodHandler.IncrementCallCount();
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void Foo()");
							}
						}
						
					}
				}
				
				internal static class MethodExpectationsOfITestExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.ITest, global::System.Action> Foo(this global::Rocks.Expectations.MethodExpectations<global::MockTests.ITest> self) =>
						new global::Rocks.MethodAdornments<global::MockTests.ITest, global::System.Action>(self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ITest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenTargetTypeIsInGlobalNamespaceAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			public static class Runner
			{
				public static void Run() 
				{
					var rock = Rock.Create<ITest>();
				}
			}

			public interface ITest
			{
				void Foo();
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			internal static class CreateExpectationsOfITestExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::ITest> Methods(this global::Rocks.Expectations.Expectations<global::ITest> self) =>
					new(self);
				
				internal static global::ITest Instance(this global::Rocks.Expectations.Expectations<global::ITest> self)
				{
					if (!self.WasInstanceInvoked)
					{
						self.WasInstanceInvoked = true;
						return new RockITest(self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockITest
					: global::ITest
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockITest(global::Rocks.Expectations.Expectations<global::ITest> expectations) =>
						this.handlers = expectations.Handlers;
					
					[global::Rocks.MemberIdentifier(0, "void Foo()")]
					public void Foo()
					{
						if (this.handlers.TryGetValue(0, out var methodHandlers))
						{
							var methodHandler = methodHandlers[0];
							if (methodHandler.Method is not null)
							{
								global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action>(methodHandler.Method)();
							}
							
							methodHandler.IncrementCallCount();
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void Foo()");
						}
					}
					
				}
			}
			
			internal static class MethodExpectationsOfITestExtensions
			{
				internal static global::Rocks.MethodAdornments<global::ITest, global::System.Action> Foo(this global::Rocks.Expectations.MethodExpectations<global::ITest> self) =>
					new global::Rocks.MethodAdornments<global::ITest, global::System.Action>(self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ITest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenTargetTypeIsValidForRockRepositoryAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public interface ITest
				{
					void Foo();
				}

				public static class Test
				{
					public static void Generate()
					{
						var repository = new RockRepository();
						var rock = repository.Create<ITest>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfITestExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.ITest> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.ITest> self) =>
						new(self);
					
					internal static global::MockTests.ITest Instance(this global::Rocks.Expectations.Expectations<global::MockTests.ITest> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockITest(self);
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
						
						public RockITest(global::Rocks.Expectations.Expectations<global::MockTests.ITest> expectations) =>
							this.handlers = expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "void Foo()")]
						public void Foo()
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								if (methodHandler.Method is not null)
								{
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action>(methodHandler.Method)();
								}
								
								methodHandler.IncrementCallCount();
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void Foo()");
							}
						}
						
					}
				}
				
				internal static class MethodExpectationsOfITestExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.ITest, global::System.Action> Foo(this global::Rocks.Expectations.MethodExpectations<global::MockTests.ITest> self) =>
						new global::Rocks.MethodAdornments<global::MockTests.ITest, global::System.Action>(self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ITest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenInvocationExistsInTopLevelStatementsAsync()
	{
		var code =
			"""
			using MockTests;
			using Rocks;
			using System;

			var rock = Rock.Create<ITest>();

			namespace MockTests
			{
				public interface ITest
				{
					void Foo();
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfITestExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.ITest> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.ITest> self) =>
						new(self);
					
					internal static global::MockTests.ITest Instance(this global::Rocks.Expectations.Expectations<global::MockTests.ITest> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockITest(self);
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
						
						public RockITest(global::Rocks.Expectations.Expectations<global::MockTests.ITest> expectations) =>
							this.handlers = expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "void Foo()")]
						public void Foo()
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								if (methodHandler.Method is not null)
								{
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action>(methodHandler.Method)();
								}
								
								methodHandler.IncrementCallCount();
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void Foo()");
							}
						}
						
					}
				}
				
				internal static class MethodExpectationsOfITestExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.ITest, global::System.Action> Foo(this global::Rocks.Expectations.MethodExpectations<global::MockTests.ITest> self) =>
						new global::Rocks.MethodAdornments<global::MockTests.ITest, global::System.Action>(self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ITest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>(), OutputKind.ConsoleApplication).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenTargetTypeIsInvalidAsync()
	{
		var code =
			"""
			using Rocks;

			namespace MockTests
			{
				public interface ITest { }

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<ITest>();
					}
				}
			}
			""";

		var diagnostic = new DiagnosticResult(TypeHasNoMockableMembersDiagnostic.Id, DiagnosticSeverity.Error)
			.WithSpan(5, 19, 5, 24);
		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			Enumerable.Empty<(Type, string, string)>(),
			new[] { diagnostic }).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenTargetTypeHasDiagnosticsAsync()
	{
		var code =
			"""
			using Rocks;

			namespace MockTests
			{
				public interface ITest 
				{ 
					// Note the missing semicolon
					void Foo()
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

		var diagnostic = new DiagnosticResult("CS1002", DiagnosticSeverity.Error)
			.WithSpan(8, 13, 8, 13);
		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			Enumerable.Empty<(Type, string, string)>(),
			new[] { diagnostic }).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenTargetTypeIsValidButOtherCodeHasDiagnosticsAsync()
	{
		var code =
			"""
			using Rocks;

			namespace MockTests
			{
				public interface ITest 
				{ 
					void Foo();
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<ITest>();
					}
			// Note the missing closing brace
				}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			namespace MockTests
			{
				internal static class CreateExpectationsOfITestExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.ITest> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.ITest> self) =>
						new(self);
					
					internal static global::MockTests.ITest Instance(this global::Rocks.Expectations.Expectations<global::MockTests.ITest> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockITest(self);
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
						
						public RockITest(global::Rocks.Expectations.Expectations<global::MockTests.ITest> expectations) =>
							this.handlers = expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "void Foo()")]
						public void Foo()
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								if (methodHandler.Method is not null)
								{
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action>(methodHandler.Method)();
								}
								
								methodHandler.IncrementCallCount();
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void Foo()");
							}
						}
						
					}
				}
				
				internal static class MethodExpectationsOfITestExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.ITest, global::System.Action> Foo(this global::Rocks.Expectations.MethodExpectations<global::MockTests.ITest> self) =>
						new global::Rocks.MethodAdornments<global::MockTests.ITest, global::System.Action>(self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		var diagnostic = new DiagnosticResult("CS1513", DiagnosticSeverity.Error)
			.WithSpan(17, 3, 17, 3);
		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ITest_Rock_Create.g.cs", generatedCode) },
			new[] { diagnostic }).ConfigureAwait(false);
	}
}