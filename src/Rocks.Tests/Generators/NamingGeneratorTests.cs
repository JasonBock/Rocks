using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class NamingGeneratorTests
{
	[Test]
	public static async Task GenerateWithMultipleNamespacesAsync()
	{
		var code =
			"""
			using Rocks;
			
			namespace Namespace1
			{
			  public class Thing { }
			  public class Stuff { }
			}

			namespace Namespace2
			{
			  public class Thing { }
			}

			public interface IUsesThing
			{
			  void Use(Namespace2.Thing thing, Namespace1.Stuff stuff);
			}

			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Create<IUsesThing>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfIUsesThingExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::IUsesThing> Methods(this global::Rocks.Expectations.Expectations<global::IUsesThing> self) =>
					new(self);
				
				internal static global::IUsesThing Instance(this global::Rocks.Expectations.Expectations<global::IUsesThing> self)
				{
					if (!self.WasInstanceInvoked)
					{
						self.WasInstanceInvoked = true;
						return new RockIUsesThing(self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockIUsesThing
					: global::IUsesThing
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockIUsesThing(global::Rocks.Expectations.Expectations<global::IUsesThing> expectations) =>
						this.handlers = expectations.Handlers;
					
					[global::Rocks.MemberIdentifier(0, "void Use(global::Namespace2.Thing thing, global::Namespace1.Stuff stuff)")]
					public void Use(global::Namespace2.Thing thing, global::Namespace1.Stuff stuff)
					{
						if (this.handlers.TryGetValue(0, out var methodHandlers))
						{
							var foundMatch = false;
							
							foreach (var methodHandler in methodHandlers)
							{
								if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<global::Namespace2.Thing>>(methodHandler.Expectations[0]).IsValid(thing) &&
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<global::Namespace1.Stuff>>(methodHandler.Expectations[1]).IsValid(stuff))
								{
									foundMatch = true;
									
									if (methodHandler.Method is not null)
									{
										global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<global::Namespace2.Thing, global::Namespace1.Stuff>>(methodHandler.Method)(thing, stuff);
									}
									
									methodHandler.IncrementCallCount();
									break;
								}
							}
							
							if (!foundMatch)
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void Use(global::Namespace2.Thing thing, global::Namespace1.Stuff stuff)");
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void Use(global::Namespace2.Thing thing, global::Namespace1.Stuff stuff)");
						}
					}
					
				}
			}
			
			internal static class MethodExpectationsOfIUsesThingExtensions
			{
				internal static global::Rocks.MethodAdornments<global::IUsesThing, global::System.Action<global::Namespace2.Thing, global::Namespace1.Stuff>> Use(this global::Rocks.Expectations.MethodExpectations<global::IUsesThing> self, global::Rocks.Argument<global::Namespace2.Thing> thing, global::Rocks.Argument<global::Namespace1.Stuff> stuff)
				{
					global::System.ArgumentNullException.ThrowIfNull(thing);
					global::System.ArgumentNullException.ThrowIfNull(stuff);
					return new global::Rocks.MethodAdornments<global::IUsesThing, global::System.Action<global::Namespace2.Thing, global::Namespace1.Stuff>>(self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { thing, stuff }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IUsesThing_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithArrayAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			public class MethodInformation { }

			public interface IUseMethodInformation
			{
				MethodInformation[] Methods { get; }
			}

			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Create<IUseMethodInformation>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfIUseMethodInformationExtensions
			{
				internal static global::Rocks.Expectations.PropertyExpectations<global::IUseMethodInformation> Properties(this global::Rocks.Expectations.Expectations<global::IUseMethodInformation> self) =>
					new(self);
				
				internal static global::Rocks.Expectations.PropertyGetterExpectations<global::IUseMethodInformation> Getters(this global::Rocks.Expectations.PropertyExpectations<global::IUseMethodInformation> self) =>
					new(self);
				
				internal static global::IUseMethodInformation Instance(this global::Rocks.Expectations.Expectations<global::IUseMethodInformation> self)
				{
					if (!self.WasInstanceInvoked)
					{
						self.WasInstanceInvoked = true;
						return new RockIUseMethodInformation(self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockIUseMethodInformation
					: global::IUseMethodInformation
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockIUseMethodInformation(global::Rocks.Expectations.Expectations<global::IUseMethodInformation> expectations) =>
						this.handlers = expectations.Handlers;
					
					[global::Rocks.MemberIdentifier(0, "get_Methods()")]
					public global::MethodInformation[] Methods
					{
						get
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								var result = methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<global::MethodInformation[]>>(methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<global::MethodInformation[]>>(methodHandler).ReturnValue;
								methodHandler.IncrementCallCount();
								return result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_Methods())");
						}
					}
				}
			}
			
			internal static class PropertyGetterExpectationsOfIUseMethodInformationExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::IUseMethodInformation, global::System.Func<global::MethodInformation[]>, global::MethodInformation[]> Methods(this global::Rocks.Expectations.PropertyGetterExpectations<global::IUseMethodInformation> self) =>
					new global::Rocks.PropertyAdornments<global::IUseMethodInformation, global::System.Func<global::MethodInformation[]>, global::MethodInformation[]>(self.Add<global::MethodInformation[]>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IUseMethodInformation_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithNestedTypeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			public interface IOperation
			{
				public struct OperationList { }

				OperationList Operations { get; }
			}

			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Create<IOperation>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfIOperationExtensions
			{
				internal static global::Rocks.Expectations.PropertyExpectations<global::IOperation> Properties(this global::Rocks.Expectations.Expectations<global::IOperation> self) =>
					new(self);
				
				internal static global::Rocks.Expectations.PropertyGetterExpectations<global::IOperation> Getters(this global::Rocks.Expectations.PropertyExpectations<global::IOperation> self) =>
					new(self);
				
				internal static global::IOperation Instance(this global::Rocks.Expectations.Expectations<global::IOperation> self)
				{
					if (!self.WasInstanceInvoked)
					{
						self.WasInstanceInvoked = true;
						return new RockIOperation(self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockIOperation
					: global::IOperation
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockIOperation(global::Rocks.Expectations.Expectations<global::IOperation> expectations) =>
						this.handlers = expectations.Handlers;
					
					[global::Rocks.MemberIdentifier(0, "get_Operations()")]
					public global::IOperation.OperationList Operations
					{
						get
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								var methodHandler = methodHandlers[0];
								var result = methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<global::IOperation.OperationList>>(methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<global::IOperation.OperationList>>(methodHandler).ReturnValue;
								methodHandler.IncrementCallCount();
								return result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_Operations())");
						}
					}
				}
			}
			
			internal static class PropertyGetterExpectationsOfIOperationExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::IOperation, global::System.Func<global::IOperation.OperationList>, global::IOperation.OperationList> Operations(this global::Rocks.Expectations.PropertyGetterExpectations<global::IOperation> self) =>
					new global::Rocks.PropertyAdornments<global::IOperation, global::System.Func<global::IOperation.OperationList>, global::IOperation.OperationList>(self.Add<global::IOperation.OperationList>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IOperation_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithConstraintAsync()
	{
		var code =
			"""
			using Rocks;
			using System;

			namespace Namespace1
			{
				public interface IConstraint { }
			}

			namespace Namespace2
			{
				public interface IUseConstraint
				{
					void Foo<T>(T value) where T : Namespace1.IConstraint;
				}
			}

			namespace MockTest
			{
				public static class Test
				{
					public static void Go()
					{
						var expectations = Rock.Create<Namespace2.IUseConstraint>();
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
			
			namespace Namespace2
			{
				internal static class CreateExpectationsOfIUseConstraintExtensions
				{
					internal static global::Rocks.Expectations.MethodExpectations<global::Namespace2.IUseConstraint> Methods(this global::Rocks.Expectations.Expectations<global::Namespace2.IUseConstraint> self) =>
						new(self);
					
					internal static global::Namespace2.IUseConstraint Instance(this global::Rocks.Expectations.Expectations<global::Namespace2.IUseConstraint> self)
					{
						if (!self.WasInstanceInvoked)
						{
							self.WasInstanceInvoked = true;
							return new RockIUseConstraint(self);
						}
						else
						{
							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
						}
					}
					
					private sealed class RockIUseConstraint
						: global::Namespace2.IUseConstraint
					{
						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
						public RockIUseConstraint(global::Rocks.Expectations.Expectations<global::Namespace2.IUseConstraint> expectations) =>
							this.handlers = expectations.Handlers;
						
						[global::Rocks.MemberIdentifier(0, "void Foo<T>(T value)")]
						public void Foo<T>(T value)
							where T : global::Namespace1.IConstraint
						{
							if (this.handlers.TryGetValue(0, out var methodHandlers))
							{
								var foundMatch = false;
								
								foreach (var methodHandler in methodHandlers)
								{
									if (((methodHandler.Expectations[0] as global::Rocks.Argument<T>)?.IsValid(value) ?? false))
									{
										foundMatch = true;
										
										if (methodHandler.Method is not null && methodHandler.Method is global::System.Action<T> method)
										{
											method(value);
										}
										
										methodHandler.IncrementCallCount();
										break;
									}
								}
								
								if (!foundMatch)
								{
									throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void Foo<T>(T value)");
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void Foo<T>(T value)");
							}
						}
						
					}
				}
				
				internal static class MethodExpectationsOfIUseConstraintExtensions
				{
					internal static global::Rocks.MethodAdornments<global::Namespace2.IUseConstraint, global::System.Action<T>> Foo<T>(this global::Rocks.Expectations.MethodExpectations<global::Namespace2.IUseConstraint> self, global::Rocks.Argument<T> value) where T : global::Namespace1.IConstraint
					{
						global::System.ArgumentNullException.ThrowIfNull(value);
						return new global::Rocks.MethodAdornments<global::Namespace2.IUseConstraint, global::System.Action<T>>(self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { value }));
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IUseConstraint_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}