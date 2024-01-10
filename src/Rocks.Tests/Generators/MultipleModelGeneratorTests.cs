﻿using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class MultipleModelGeneratorTests
{
	[Test]
	public static async Task GenerateWithMultipleCreatesAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockCreate<MockTests.ITarget>]
			[assembly: RockCreate<MockTests.ITarget>]
			
			namespace MockTests
			{
				public interface ITarget
				{
					string Retrieve(int value);
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			
			namespace MockTests
			{
				internal sealed class ITargetCreateExpectations
					: global::Rocks.Expectations
				{
					#pragma warning disable CS8618
					
					internal sealed class Handler0
						: global::Rocks.Handler<global::System.Func<int, string>, string>
					{
						public global::Rocks.Argument<int> @value { get; set; }
					}
					
					#pragma warning restore CS8618
					
					private readonly global::System.Collections.Generic.List<global::MockTests.ITargetCreateExpectations.Handler0> @handlers0 = new();
					
					public override void Verify()
					{
						if (this.WasInstanceInvoked)
						{
							var failures = new global::System.Collections.Generic.List<string>();
					
							failures.AddRange(this.Verify(handlers0));
					
							if (failures.Count > 0)
							{
								throw new global::Rocks.Exceptions.VerificationException(failures);
							}
						}
					}
					
					private sealed class RockITarget
						: global::MockTests.ITarget
					{
						public RockITarget(global::MockTests.ITargetCreateExpectations @expectations)
						{
							this.Expectations = @expectations;
						}
						
						[global::Rocks.MemberIdentifier(0, "string Retrieve(int @value)")]
						public string Retrieve(int @value)
						{
							if (this.Expectations.handlers0.Count > 0)
							{
								foreach (var @handler in this.Expectations.handlers0)
								{
									if (@handler.@value.IsValid(@value!))
									{
										@handler.CallCount++;
										var @result = @handler.Callback is not null ?
											@handler.Callback(@value!) : @handler.ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for string Retrieve(int @value)");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for string Retrieve(int @value)");
						}
						
						private global::MockTests.ITargetCreateExpectations Expectations { get; }
					}
					
					internal sealed class ITargetMethodExpectations
					{
						internal ITargetMethodExpectations(global::MockTests.ITargetCreateExpectations expectations) =>
							this.Expectations = expectations;
						
						internal global::Rocks.Adornments<global::MockTests.ITargetCreateExpectations.Handler0, global::System.Func<int, string>, string> Retrieve(global::Rocks.Argument<int> @value)
						{
							global::System.ArgumentNullException.ThrowIfNull(@value);
							
							var handler = new global::MockTests.ITargetCreateExpectations.Handler0
							{
								@value = @value,
							};
							
							this.Expectations.handlers0.Add(handler);
							return new(handler);
						}
						
						private global::MockTests.ITargetCreateExpectations Expectations { get; }
					}
					
					internal global::MockTests.ITargetCreateExpectations.ITargetMethodExpectations Methods { get; }
					
					internal ITargetCreateExpectations() =>
						(this.Methods) = (new(this));
					
					internal global::MockTests.ITarget Instance()
					{
						if (!this.WasInstanceInvoked)
						{
							this.WasInstanceInvoked = true;
							var @mock = new RockITarget(this);
							this.MockType = @mock.GetType();
							return @mock;
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.ITarget_Rock_Create.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithMultipleMakeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			[assembly: RockMake<MockTests.ITarget>]
			[assembly: RockMake<MockTests.ITarget>]
			
			namespace MockTests
			{
				public interface ITarget
				{
					string Retrieve(int value);
				}
			}
			""";

		var generatedCode =
			"""
			// <auto-generated/>
			
			#nullable enable
			
			namespace MockTests
			{
				internal sealed class ITargetMakeExpectations
				{
					internal global::MockTests.ITarget Instance()
					{
						return new RockITarget();
					}
					
					private sealed class RockITarget
						: global::MockTests.ITarget
					{
						public RockITarget()
						{
						}
						
						public string Retrieve(int @value)
						{
							return default!;
						}
					}
				}
			}
			""";

		await TestAssistants.RunAsync<RockAttributeGenerator>(code,
			new[] { (typeof(RockAttributeGenerator), "MockTests.ITarget_Rock_Make.g.cs", generatedCode) },
			[]).ConfigureAwait(false);
	}
}