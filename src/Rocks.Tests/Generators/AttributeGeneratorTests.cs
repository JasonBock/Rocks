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
									((Action)methodHandler.Method)();
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
				public interface IAllow
				{
					 [AllowNull]
					 string NewLine { get; set; }
				}
	
				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Make<IAllow>();
					}
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks;
			using Rocks.Exceptions;
			using System;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			using System.Diagnostics.CodeAnalysis;
			
			#nullable enable
			namespace MockTests
			{
				internal static class MakeExpectationsOfIAllowExtensions
				{
					internal static IAllow Instance(this MakeGeneration<IAllow> self) =>
						new RockIAllow();
					
					private sealed class RockIAllow
						: IAllow
					{
						public RockIAllow() { }
						
						[AllowNull]
						public string NewLine
						{
							get => default!;
							set { }
						}
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "IAllow_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}