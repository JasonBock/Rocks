using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class NullableAnnotationTests
{
	[Test]
	public static async Task GenerateWhenParameterWithNullDefaultIsNotAnnotatedAsync()
	{
		var code =
			"""
			using Rocks;

			public class NeedNullable
			{
			    public virtual void Foo(object initializationData = null) { }
			}
			
			public static class Test
			{
				public static void Generate()
				{
					var rock = Rock.Create<NeedNullable>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfNeedNullableExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::NeedNullable> Methods(this global::Rocks.Expectations.Expectations<global::NeedNullable> @self) =>
					new(@self);
				
				internal static global::NeedNullable Instance(this global::Rocks.Expectations.Expectations<global::NeedNullable> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						return new RockNeedNullable(@self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockNeedNullable
					: global::NeedNullable
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockNeedNullable(global::Rocks.Expectations.Expectations<global::NeedNullable> @expectations) =>
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
					
					[global::Rocks.MemberIdentifier(3, "void Foo(object @initializationData)")]
					public override void Foo(object @initializationData = null)
					{
						if (this.handlers.TryGetValue(3, out var @methodHandlers))
						{
							var @foundMatch = false;
							
							foreach (var @methodHandler in @methodHandlers)
							{
								if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object>>(@methodHandler.Expectations[0]).IsValid(@initializationData))
								{
									@foundMatch = true;
									
									if (@methodHandler.Method is not null)
									{
										global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<object>>(@methodHandler.Method)(@initializationData);
									}
									
									@methodHandler.IncrementCallCount();
									break;
								}
							}
							
							if (!@foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void Foo(object @initializationData = null)");
							}
						}
						else
						{
							base.Foo(@initializationData);
						}
					}
					
				}
			}
			
			internal static class MethodExpectationsOfNeedNullableExtensions
			{
				internal static global::Rocks.MethodAdornments<global::NeedNullable, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::NeedNullable> @self, global::Rocks.Argument<object?> @obj)
				{
					global::System.ArgumentNullException.ThrowIfNull(@obj);
					return new global::Rocks.MethodAdornments<global::NeedNullable, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
				}
				internal static global::Rocks.MethodAdornments<global::NeedNullable, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::NeedNullable> @self) =>
					new global::Rocks.MethodAdornments<global::NeedNullable, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::NeedNullable, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::NeedNullable> @self) =>
					new global::Rocks.MethodAdornments<global::NeedNullable, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::NeedNullable, global::System.Action<object>> Foo(this global::Rocks.Expectations.MethodExpectations<global::NeedNullable> @self, global::Rocks.Argument<object> @initializationData)
				{
					global::System.ArgumentNullException.ThrowIfNull(@initializationData);
					return new global::Rocks.MethodAdornments<global::NeedNullable, global::System.Action<object>>(@self.Add(3, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @initializationData.Transform(null) }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "NeedNullable_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}