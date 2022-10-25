using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ClassConstructorCreateGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public class BaseCtor
				{
					 public BaseCtor(int a, ref string b, out string c, params string[] d) { c = "c"; }

					 public virtual void Foo() { }
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<BaseCtor>();
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
				internal static class CreateExpectationsOfBaseCtorExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.BaseCtor> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.BaseCtor> @self) =>
						new(@self);
					
					internal static global::MockTests.BaseCtor Instance(this global::Rocks.Expectations.Expectations<global::MockTests.BaseCtor> @self, int @a, ref string @b, out string @c, params string[] @d)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							return new RockBaseCtor(@self, @a, ref @b, out @c, @d);
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockBaseCtor
						: global::MockTests.BaseCtor
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockBaseCtor(global::Rocks.Expectations.Expectations<global::MockTests.BaseCtor> @expectations, int @a, ref string @b, out string @c, params string[] @d)
							: base(@a, ref @b, out @c, @d)
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
										var @result = @methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, bool>>(@methodHandler.Method)(@obj) :
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
				
				internal static class MethodExpectationsOfBaseCtorExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.BaseCtor, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::MockTests.BaseCtor> @self, global::Rocks.Argument<object?> @obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(@obj);
						return new global::Rocks.MethodAdornments<global::MockTests.BaseCtor, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.BaseCtor, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::MockTests.BaseCtor> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.BaseCtor, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.BaseCtor, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::MockTests.BaseCtor> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.BaseCtor, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.BaseCtor, global::System.Action> Foo(this global::Rocks.Expectations.MethodExpectations<global::MockTests.BaseCtor> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.BaseCtor, global::System.Action>(@self.Add(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "BaseCtor_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}