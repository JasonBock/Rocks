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
			using MockTests;
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
				internal static class CreateExpectationsOfIHaveGenericAttributeExtensions
				{
					internal static MethodExpectations<IHaveGenericAttribute> Methods(this Expectations<IHaveGenericAttribute> self) =>
						new(self);
					
					internal static IHaveGenericAttribute Instance(this Expectations<IHaveGenericAttribute> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockIHaveGenericAttribute(self);
						}
						else
						{
							throw new NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIHaveGenericAttribute
						: IHaveGenericAttribute
					{
						private readonly Dictionary<int, List<HandlerInformation>> handlers;
						
						public RockIHaveGenericAttribute(Expectations<IHaveGenericAttribute> expectations) =>
							this.handlers = expectations.Handlers;
						
						[My<string>]
						[MemberIdentifier(0, "void Foo()")]
						public void Foo()
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								if (methodHandler.Method is not null)
								{
									Unsafe.As<Action>(methodHandler.Method)();
								}
								
								methodHandler.IncrementCallCount();
							}
							else
							{
								throw new ExpectationException("No handlers were found for void Foo()");
							}
						}
						
					}
				}
				
				internal static class MethodExpectationsOfIHaveGenericAttributeExtensions
				{
					internal static MethodAdornments<IHaveGenericAttribute, Action> Foo(this MethodExpectations<IHaveGenericAttribute> self) =>
						new MethodAdornments<IHaveGenericAttribute, Action>(self.Add(0, new List<Argument>()));
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
			using MockTests;
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
				internal static class CreateExpectationsOfIHaveMultipleAttributesExtensions
				{
					internal static MethodExpectations<IHaveMultipleAttributes> Methods(this Expectations<IHaveMultipleAttributes> self) =>
						new(self);
					
					internal static IHaveMultipleAttributes Instance(this Expectations<IHaveMultipleAttributes> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockIHaveMultipleAttributes(self);
						}
						else
						{
							throw new NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIHaveMultipleAttributes
						: IHaveMultipleAttributes
					{
						private readonly Dictionary<int, List<HandlerInformation>> handlers;
						
						public RockIHaveMultipleAttributes(Expectations<IHaveMultipleAttributes> expectations) =>
							this.handlers = expectations.Handlers;
						
						[MemberIdentifier(0, "void Foo(string data)")]
						public void Foo([ParameterOne, ParameterTwo] string data)
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								var foundMatch = false;
								
								foreach (var methodHandler in methodHandlers)
								{
									if (Unsafe.As<Argument<string>>(methodHandler.Expectations[0]).IsValid(data))
									{
										foundMatch = true;
										
										if (methodHandler.Method is not null)
										{
											Unsafe.As<Action<string>>(methodHandler.Method)(data);
										}
										
										methodHandler.IncrementCallCount();
										break;
									}
								}
								
								if (!foundMatch)
								{
									throw new ExpectationException("No handlers match for void Foo([ParameterOne, ParameterTwo] string data)");
								}
							}
							else
							{
								throw new ExpectationException("No handlers were found for void Foo([ParameterOne, ParameterTwo] string data)");
							}
						}
						
					}
				}
				
				internal static class MethodExpectationsOfIHaveMultipleAttributesExtensions
				{
					internal static MethodAdornments<IHaveMultipleAttributes, Action<string>> Foo(this MethodExpectations<IHaveMultipleAttributes> self, Argument<string> data)
					{
						ArgumentNullException.ThrowIfNull(data);
						return new MethodAdornments<IHaveMultipleAttributes, Action<string>>(self.Add(0, new List<Argument>(1) { data }));
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IHaveMultipleAttributes_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}