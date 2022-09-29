using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class AttributeGeneratorTests
{
	[Test]
	public static async Task GenerateWithGenericAttributeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Diagnostics.CodeAnalysis;

			namespace MockTests
			{
				[AttributeUsage(AttributeTargets.Method)]
				public sealed class MyAttribute<T>
					: Attribute { }

				public interface IHaveGenericAttribute
				{
					 [MyAttribute<string>]
					 void Foo();
				}
	
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IHaveGenericAttribute>();
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
				internal static class CreateExpectationsOfIHaveGenericAttributeExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveGenericAttribute> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveGenericAttribute> self) =>
						new(self);
					
					internal static global::MockTests.IHaveGenericAttribute Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveGenericAttribute> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockIHaveGenericAttribute(self);
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIHaveGenericAttribute
						: global::MockTests.IHaveGenericAttribute
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockIHaveGenericAttribute(global::Rocks.Expectations.Expectations<global::MockTests.IHaveGenericAttribute> expectations) =>
							this.handlers = expectations.Handlers;
						
						[global::MockTests.MyAttribute<string>]
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
				
				internal static class MethodExpectationsOfIHaveGenericAttributeExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IHaveGenericAttribute, global::System.Action> Foo(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveGenericAttribute> self) =>
						new global::Rocks.MethodAdornments<global::MockTests.IHaveGenericAttribute, global::System.Action>(self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IHaveGenericAttribute_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithMultipleAttributesOnMethodParameterAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Diagnostics.CodeAnalysis;

			namespace MockTests
			{
				[AttributeUsage(AttributeTargets.Parameter)]
				public sealed class ParameterOneAttribute
					: Attribute { }

				[AttributeUsage(AttributeTargets.Parameter)]
				public sealed class ParameterTwoAttribute
					: Attribute { }				

				public interface IHaveMultipleAttributes
				{
					void Foo([ParameterOne, ParameterTwo] string data);
				}
	
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<IHaveMultipleAttributes>();
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
				internal static class CreateExpectationsOfIHaveMultipleAttributesExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveMultipleAttributes> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveMultipleAttributes> self) =>
						new(self);
					
					internal static global::MockTests.IHaveMultipleAttributes Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveMultipleAttributes> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockIHaveMultipleAttributes(self);
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIHaveMultipleAttributes
						: global::MockTests.IHaveMultipleAttributes
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockIHaveMultipleAttributes(global::Rocks.Expectations.Expectations<global::MockTests.IHaveMultipleAttributes> expectations) =>
							this.handlers = expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "void Foo(string data)")]
						public void Foo([global::MockTests.ParameterOneAttribute, global::MockTests.ParameterTwoAttribute] string data)
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								var foundMatch = false;
								
								foreach (var methodHandler in methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(methodHandler.Expectations[0]).IsValid(data))
									{
										foundMatch = true;
										
										if (methodHandler.Method is not null)
										{
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<string>>(methodHandler.Method)(data);
										}
										
										methodHandler.IncrementCallCount();
										break;
									}
								}
								
								if (!foundMatch)
								{
									throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void Foo([global::MockTests.ParameterOneAttribute, global::MockTests.ParameterTwoAttribute] string data)");
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void Foo([global::MockTests.ParameterOneAttribute, global::MockTests.ParameterTwoAttribute] string data)");
							}
						}
						
					}
				}
				
				internal static class MethodExpectationsOfIHaveMultipleAttributesExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IHaveMultipleAttributes, global::System.Action<string>> Foo(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveMultipleAttributes> self, global::Rocks.Argument<string> data)
					{
						global::System.ArgumentNullException.ThrowIfNull(data);
						return new global::Rocks.MethodAdornments<global::MockTests.IHaveMultipleAttributes, global::System.Action<string>>(self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { data }));
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IHaveMultipleAttributes_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}