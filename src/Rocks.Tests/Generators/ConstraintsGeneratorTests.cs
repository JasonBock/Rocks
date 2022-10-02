using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ConstraintsGeneratorTests
{
	[Test]
	public static async Task GenerateProperNumberOfConstraintsAsync()
	{
		var code =
			"""
			using Rocks;
			
			public interface IValue<TValue>
			  where TValue : IValue<TValue> { }

			public class Value<TValue>
			  where TValue : unmanaged, IValue<TValue> { }

			public interface IUnmanagedValue
			{
			    void Use<TValue>(Value<TValue> value)
			        where TValue : unmanaged, IValue<TValue>;
			}

			public static class Test
			{
			  public static void Run()
			  {
			    var expectations = Rock.Create<IUnmanagedValue>();
			  }
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfIUnmanagedValueExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::IUnmanagedValue> Methods(this global::Rocks.Expectations.Expectations<global::IUnmanagedValue> @self) =>
					new(@self);
				
				internal static global::IUnmanagedValue Instance(this global::Rocks.Expectations.Expectations<global::IUnmanagedValue> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						return new RockIUnmanagedValue(@self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockIUnmanagedValue
					: global::IUnmanagedValue
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockIUnmanagedValue(global::Rocks.Expectations.Expectations<global::IUnmanagedValue> @expectations) =>
						this.handlers = @expectations.Handlers;
					
					[global::Rocks.MemberIdentifier(0, "void Use<TValue>(global::Value<TValue> @value)")]
					public void Use<TValue>(global::Value<TValue> @value)
						where TValue : unmanaged, global::IValue<TValue>
					{
						if (this.handlers.TryGetValue(0, out var @methodHandlers))
						{
							var @foundMatch = false;
							
							foreach (var @methodHandler in @methodHandlers)
							{
								if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<global::Value<TValue>>>(@methodHandler.Expectations[0]).IsValid(@value))
								{
									@foundMatch = true;
									
									if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action<global::Value<TValue>> @method)
									{
										@method(@value);
									}
									
									@methodHandler.IncrementCallCount();
									break;
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void Use<TValue>(global::Value<TValue> @value)");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void Use<TValue>(global::Value<TValue> @value)");
						}
					}
					
				}
			}
			
			internal static class MethodExpectationsOfIUnmanagedValueExtensions
			{
				internal static global::Rocks.MethodAdornments<global::IUnmanagedValue, global::System.Action<global::Value<TValue>>> Use<TValue>(this global::Rocks.Expectations.MethodExpectations<global::IUnmanagedValue> @self, global::Rocks.Argument<global::Value<TValue>> @value) where TValue : unmanaged, global::IValue<TValue>
				{
					global::System.ArgumentNullException.ThrowIfNull(@value);
					return new global::Rocks.MethodAdornments<global::IUnmanagedValue, global::System.Action<global::Value<TValue>>>(@self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IUnmanagedValue_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace MockTests
			{
				public class Thing { }

				public abstract class Thing<T>
					: Thing where T : class
				{
					public abstract Thing<TTarget> As<TTarget>() where TTarget : class;
				}
	
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<Thing<string>>();
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
				internal static class CreateExpectationsOfThingOfstringExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.Thing<string>> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.Thing<string>> @self) =>
						new(@self);
					
					internal static global::MockTests.Thing<string> Instance(this global::Rocks.Expectations.Expectations<global::MockTests.Thing<string>> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							return new RockThingOfstring(@self);
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockThingOfstring
						: global::MockTests.Thing<string>
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockThingOfstring(global::Rocks.Expectations.Expectations<global::MockTests.Thing<string>> @expectations) =>
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
						
						[global::Rocks.MemberIdentifier(3, "global::MockTests.Thing<TTarget> As<TTarget>()")]
						public override global::MockTests.Thing<TTarget> As<TTarget>()
							where TTarget : class
						{
							if (this.handlers.TryGetValue(3, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<global::MockTests.Thing<TTarget>>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<global::MockTests.Thing<TTarget>>>(@methodHandler).ReturnValue;
								@methodHandler.IncrementCallCount();
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::MockTests.Thing<TTarget> As<TTarget>()");
						}
						
					}
				}
				
				internal static class MethodExpectationsOfThingOfstringExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Thing<string>> @self, global::Rocks.Argument<object?> @obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(@obj);
						return new global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
					}
					internal static global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Thing<string>> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Thing<string>> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
					internal static global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<global::MockTests.Thing<TTarget>>, global::MockTests.Thing<TTarget>> As<TTarget>(this global::Rocks.Expectations.MethodExpectations<global::MockTests.Thing<string>> @self) where TTarget : class =>
						new global::Rocks.MethodAdornments<global::MockTests.Thing<string>, global::System.Func<global::MockTests.Thing<TTarget>>, global::MockTests.Thing<TTarget>>(@self.Add<global::MockTests.Thing<TTarget>>(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ThingOfstring_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}