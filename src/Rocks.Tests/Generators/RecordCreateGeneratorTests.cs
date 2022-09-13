using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class RecordCreateGeneratorTests
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
				public record RecordTest
				{
					public RecordTest() { }

					public virtual void Foo() { }
				}

				public static class Test
				{
					public static void Generate()
					{
						var rock = Rock.Create<RecordTest>();
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
			using System.Text;
			
			#nullable enable
			namespace MockTests
			{
				internal static class CreateExpectationsOfRecordTestExtensions
				{
					internal static MethodExpectations<RecordTest> Methods(this Expectations<RecordTest> self) =>
						new(self);
					
					internal static PropertyExpectations<RecordTest> Properties(this Expectations<RecordTest> self) =>
						new(self);
					
					internal static PropertyGetterExpectations<RecordTest> Getters(this PropertyExpectations<RecordTest> self) =>
						new(self);
					
					internal static RecordTest Instance(this Expectations<RecordTest> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockRecordTest(self);
						}
						else
						{
							throw new NewMockInstanceException("Can only create a new mock once.");
						}
					}
					internal static RecordTest Instance(this Expectations<RecordTest> self, RecordTest original)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockRecordTest(self, original);
						}
						else
						{
							throw new NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed record RockRecordTest
						: RecordTest
					{
						private readonly Dictionary<int, List<HandlerInformation>> handlers;
						
						public RockRecordTest(Expectations<RecordTest> expectations) =>
							this.handlers = expectations.Handlers;
						public RockRecordTest(Expectations<RecordTest> expectations, RecordTest original)
							: base(original) =>
								this.handlers = expectations.Handlers;
						
						[MemberIdentifier(2, "void Foo()")]
						public override void Foo()
						{
							if (this.handlers.TryGetValue(2, out var methodHandlers))
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
						
						[MemberIdentifier(3, "string ToString()")]
						public override string ToString()
						{
							if (this.handlers.TryGetValue(3, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								var result = methodHandler.Method is not null ?
									((Func<string>)methodHandler.Method)() :
									((HandlerInformation<string>)methodHandler).ReturnValue;
								methodHandler.IncrementCallCount();
								return result!;
							}
							else
							{
								return base.ToString();
							}
						}
						
						[MemberIdentifier(4, "bool PrintMembers(StringBuilder builder)")]
						protected override bool PrintMembers(StringBuilder builder)
						{
							if (this.handlers.TryGetValue(4, out var methodHandlers))
							{
								foreach (var methodHandler in methodHandlers)
								{
									if (((Argument<StringBuilder>)methodHandler.Expectations[0]).IsValid(builder))
									{
										var result = methodHandler.Method is not null ?
											((Func<StringBuilder, bool>)methodHandler.Method)(builder) :
											((HandlerInformation<bool>)methodHandler).ReturnValue;
										methodHandler.IncrementCallCount();
										return result!;
									}
								}
								
								throw new ExpectationException("No handlers match for bool PrintMembers(StringBuilder builder)");
							}
							else
							{
								return base.PrintMembers(builder);
							}
						}
						
						[MemberIdentifier(5, "int GetHashCode()")]
						public override int GetHashCode()
						{
							if (this.handlers.TryGetValue(5, out var methodHandlers))
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
						
						[MemberIdentifier(6, "get_EqualityContract()")]
						protected override Type EqualityContract
						{
							get
							{
								if (this.handlers.TryGetValue(6, out var methodHandlers))
								{
									var methodHandler = methodHandlers[0];
									var result = methodHandler.Method is not null ?
										((Func<Type>)methodHandler.Method)() :
										((HandlerInformation<Type>)methodHandler).ReturnValue;
									methodHandler.IncrementCallCount();
									return result!;
								}
								else
								{
									return base.EqualityContract;
								}
							}
						}
					}
				}
				
				internal static class MethodExpectationsOfRecordTestExtensions
				{
					internal static MethodAdornments<RecordTest, Action> Foo(this MethodExpectations<RecordTest> self) =>
						new MethodAdornments<RecordTest, Action>(self.Add(2, new List<Argument>()));
					internal static MethodAdornments<RecordTest, Func<string>, string> ToString(this MethodExpectations<RecordTest> self) =>
						new MethodAdornments<RecordTest, Func<string>, string>(self.Add<string>(3, new List<Argument>()));
					internal static MethodAdornments<RecordTest, Func<StringBuilder, bool>, bool> PrintMembers(this MethodExpectations<RecordTest> self, Argument<StringBuilder> builder)
					{
						ArgumentNullException.ThrowIfNull(builder);
						return new MethodAdornments<RecordTest, Func<StringBuilder, bool>, bool>(self.Add<bool>(4, new List<Argument>(1) { builder }));
					}
					internal static MethodAdornments<RecordTest, Func<int>, int> GetHashCode(this MethodExpectations<RecordTest> self) =>
						new MethodAdornments<RecordTest, Func<int>, int>(self.Add<int>(5, new List<Argument>()));
				}
				
				internal static class PropertyGetterExpectationsOfRecordTestExtensions
				{
					internal static PropertyAdornments<RecordTest, Func<Type>, Type> EqualityContract(this PropertyGetterExpectations<RecordTest> self) =>
						new PropertyAdornments<RecordTest, Func<Type>, Type>(self.Add<Type>(6, new List<Argument>()));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "RecordTest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}