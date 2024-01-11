﻿using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;
using Rocks.Diagnostics;

namespace Rocks.Tests.Generators;

public static class ConstructorGeneratorTests
{
	[Test]
	public static async Task GenerateWhenNoAccessibleConstructorsExistAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<Constructable>]

			#nullable enable

			public class Constructable
			{
				private Constructable() { }

				public virtual object GetValue() => new();			
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			Enumerable.Empty<(Type, string, string)>(),
			new[] { DiagnosticResult.CompilerError(TypeHasNoAccessibleConstructorsDiagnostic.Id).WithSpan(3, 12, 3, 37) }).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenOnlyConstructorsIsObsoleteAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<Constructable>]

			#nullable enable

			public class Constructable
			{
				[Obsolete("Old", true)]
				public Constructable() { }

				public virtual object GetValue() => new();			
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			Enumerable.Empty<(Type, string, string)>(),
			new[] { DiagnosticResult.CompilerError(TypeHasNoAccessibleConstructorsDiagnostic.Id).WithSpan(4, 12, 4, 37) }).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenTypeArgumentsCreateDuplicateConstructorsAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<AnyOf<object, object>>]

			#nullable enable

			public class AnyOf<T1, T2>
			{
				public AnyOf(T1 value) { }

				public AnyOf(T2 value) { }

				public virtual object GetValue() => new();			
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			Enumerable.Empty<(Type, string, string)>(),
			new[] { DiagnosticResult.CompilerError(DuplicateConstructorsDiagnostic.Id).WithSpan(3, 12, 3, 45) }).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenTypeArgumentsDoNotCreateDuplicateConstructorsAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<AnyOf<string, int>>]

			#nullable enable

			public class AnyOf<T1, T2>
			{
				public AnyOf(T1 value) { }

				public AnyOf(T2 value) { }

				public virtual object GetValue() => new();			
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			internal sealed class AnyOfOfstring_intCreateExpectations
				: global::Rocks.Expectations
			{
				#pragma warning disable CS8618
				
				internal sealed class Handler0
					: global::Rocks.Handler<global::System.Func<object?, bool>, bool>
				{
					public global::Rocks.Argument<object?> @obj { get; set; }
				}
				
				internal sealed class Handler1
					: global::Rocks.Handler<global::System.Func<int>, int>
				{ }
				
				internal sealed class Handler2
					: global::Rocks.Handler<global::System.Func<string?>, string?>
				{ }
				
				internal sealed class Handler3
					: global::Rocks.Handler<global::System.Func<object>, object>
				{ }
				
				#pragma warning restore CS8618
				
				private readonly global::System.Collections.Generic.List<global::AnyOfOfstring_intCreateExpectations.Handler0> @handlers0 = new();
				private readonly global::System.Collections.Generic.List<global::AnyOfOfstring_intCreateExpectations.Handler1> @handlers1 = new();
				private readonly global::System.Collections.Generic.List<global::AnyOfOfstring_intCreateExpectations.Handler2> @handlers2 = new();
				private readonly global::System.Collections.Generic.List<global::AnyOfOfstring_intCreateExpectations.Handler3> @handlers3 = new();
				
				public override void Verify()
				{
					if (this.WasInstanceInvoked)
					{
						var failures = new global::System.Collections.Generic.List<string>();
				
						failures.AddRange(this.Verify(handlers0));
						failures.AddRange(this.Verify(handlers1));
						failures.AddRange(this.Verify(handlers2));
						failures.AddRange(this.Verify(handlers3));
				
						if (failures.Count > 0)
						{
							throw new global::Rocks.Exceptions.VerificationException(failures);
						}
					}
				}
				
				private sealed class RockAnyOfOfstring_int
					: global::AnyOf<string, int>
				{
					public RockAnyOfOfstring_int(global::AnyOfOfstring_intCreateExpectations @expectations, string @value)
						: base(@value)
					{
						this.Expectations = @expectations;
					}
					public RockAnyOfOfstring_int(global::AnyOfOfstring_intCreateExpectations @expectations, int @value)
						: base(@value)
					{
						this.Expectations = @expectations;
					}
					
					[global::Rocks.MemberIdentifier(0, "bool Equals(object? @obj)")]
					public override bool Equals(object? @obj)
					{
						if (this.Expectations.handlers0.Count > 0)
						{
							foreach (var @handler in this.Expectations.handlers0)
							{
								if (@handler.@obj.IsValid(@obj!))
								{
									@handler.CallCount++;
									var @result = @handler.Callback is not null ?
										@handler.Callback(@obj!) : @handler.ReturnValue;
									return @result!;
								}
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool Equals(object? @obj)");
						}
						else
						{
							return base.Equals(obj: @obj!);
						}
					}
					
					[global::Rocks.MemberIdentifier(1, "int GetHashCode()")]
					public override int GetHashCode()
					{
						if (this.Expectations.handlers1.Count > 0)
						{
							var @handler = this.Expectations.handlers1[0];
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
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
						if (this.Expectations.handlers2.Count > 0)
						{
							var @handler = this.Expectations.handlers2[0];
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
							return @result!;
						}
						else
						{
							return base.ToString();
						}
					}
					
					[global::Rocks.MemberIdentifier(3, "object GetValue()")]
					public override object GetValue()
					{
						if (this.Expectations.handlers3.Count > 0)
						{
							var @handler = this.Expectations.handlers3[0];
							@handler.CallCount++;
							var @result = @handler.Callback is not null ?
								@handler.Callback() : @handler.ReturnValue;
							return @result!;
						}
						else
						{
							return base.GetValue();
						}
					}
					
					private global::AnyOfOfstring_intCreateExpectations Expectations { get; }
				}
				
				internal sealed class AnyOfOfstring_intMethodExpectations
				{
					internal AnyOfOfstring_intMethodExpectations(global::AnyOfOfstring_intCreateExpectations expectations) =>
						this.Expectations = expectations;
					
					internal global::Rocks.Adornments<global::AnyOfOfstring_intCreateExpectations.Handler0, global::System.Func<object?, bool>, bool> Equals(global::Rocks.Argument<object?> @obj)
					{
						global::System.ArgumentNullException.ThrowIfNull(@obj);
						
						var handler = new global::AnyOfOfstring_intCreateExpectations.Handler0
						{
							@obj = @obj,
						};
						
						this.Expectations.handlers0.Add(handler);
						return new(handler);
					}
					
					internal new global::Rocks.Adornments<global::AnyOfOfstring_intCreateExpectations.Handler1, global::System.Func<int>, int> GetHashCode()
					{
						var handler = new global::AnyOfOfstring_intCreateExpectations.Handler1();
						this.Expectations.handlers1.Add(handler);
						return new(handler);
					}
					
					internal new global::Rocks.Adornments<global::AnyOfOfstring_intCreateExpectations.Handler2, global::System.Func<string?>, string?> ToString()
					{
						var handler = new global::AnyOfOfstring_intCreateExpectations.Handler2();
						this.Expectations.handlers2.Add(handler);
						return new(handler);
					}
					
					internal global::Rocks.Adornments<global::AnyOfOfstring_intCreateExpectations.Handler3, global::System.Func<object>, object> GetValue()
					{
						var handler = new global::AnyOfOfstring_intCreateExpectations.Handler3();
						this.Expectations.handlers3.Add(handler);
						return new(handler);
					}
					
					private global::AnyOfOfstring_intCreateExpectations Expectations { get; }
				}
				
				internal global::AnyOfOfstring_intCreateExpectations.AnyOfOfstring_intMethodExpectations Methods { get; }
				
				internal AnyOfOfstring_intCreateExpectations() =>
					(this.Methods) = (new(this));
				
				internal global::AnyOf<string, int> Instance(string @value)
				{
					if (!this.WasInstanceInvoked)
					{
						this.WasInstanceInvoked = true;
						var @mock = new RockAnyOfOfstring_int(this, @value);
						this.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				internal global::AnyOf<string, int> Instance(int @value)
				{
					if (!this.WasInstanceInvoked)
					{
						this.WasInstanceInvoked = true;
						var @mock = new RockAnyOfOfstring_int(this, @value);
						this.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "AnyOfstring, int_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}
}