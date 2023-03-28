using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class AttributeGeneratorTests
{
	[Test]
	public static async Task CreateWithTupleNamesAsync()
	{
		var tupleCode =
			"""
			public interface IUseTuples
			{
				(nint Display, nuint Window)? X11 { get; }
			}
			""";
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location) as MetadataReference);

		var tupleSyntaxTree = CSharpSyntaxTree.ParseText(tupleCode);
		var tupleCompilation = CSharpCompilation.Create("internal", new SyntaxTree[] { tupleSyntaxTree },
			references,
			new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		using var tupleStream = new MemoryStream();
		tupleCompilation.Emit(tupleStream);
		tupleStream.Position = 0;
		var tupleReference = MetadataReference.CreateFromStream(tupleStream);

		var code =
			"""
			using Rocks;

			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Create<IUseTuples>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfIUseTuplesExtensions
			{
				internal static global::Rocks.Expectations.PropertyExpectations<global::IUseTuples> Properties(this global::Rocks.Expectations.Expectations<global::IUseTuples> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertyGetterExpectations<global::IUseTuples> Getters(this global::Rocks.Expectations.PropertyExpectations<global::IUseTuples> @self) =>
					new(@self);
				
				internal static global::IUseTuples Instance(this global::Rocks.Expectations.Expectations<global::IUseTuples> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						var @mock = new RockIUseTuples(@self);
						@self.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockIUseTuples
					: global::IUseTuples
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockIUseTuples(global::Rocks.Expectations.Expectations<global::IUseTuples> @expectations)
					{
						this.handlers = @expectations.Handlers;
					}
					
					
					[global::Rocks.MemberIdentifier(0, "get_X11()")]
					public (nint Display, nuint Window)? X11
					{
						get
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<(nint Display, nuint Window)?>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<(nint Display, nuint Window)?>>(@methodHandler).ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_X11())");
						}
					}
				}
			}
			
			internal static class PropertyGetterExpectationsOfIUseTuplesExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::IUseTuples, global::System.Func<(nint Display, nuint Window)?>, (nint Display, nuint Window)?> X11(this global::Rocks.Expectations.PropertyGetterExpectations<global::IUseTuples> @self) =>
					new global::Rocks.PropertyAdornments<global::IUseTuples, global::System.Func<(nint Display, nuint Window)?>, (nint Display, nuint Window)?>(@self.Add<(nint Display, nuint Window)?>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			""";
		
		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IUseTuples_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>(),
			additionalReferences: references.Concat(new[] { tupleReference as MetadataReference })).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWithDynamicAttributeAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Runtime.CompilerServices;

			public class HaveDynamic
			{
				[return: Dynamic]
				protected virtual dynamic CreateDynamicRecord() => default;
			}

			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Create<HaveDynamic>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfHaveDynamicExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::HaveDynamic> Methods(this global::Rocks.Expectations.Expectations<global::HaveDynamic> @self) =>
					new(@self);
				
				internal static global::HaveDynamic Instance(this global::Rocks.Expectations.Expectations<global::HaveDynamic> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						var @mock = new RockHaveDynamic(@self);
						@self.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockHaveDynamic
					: global::HaveDynamic
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockHaveDynamic(global::Rocks.Expectations.Expectations<global::HaveDynamic> @expectations)
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
									@methodHandler.IncrementCallCount();
									var @result = @methodHandler.Method is not null ?
										global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, bool>>(@methodHandler.Method)(@obj) :
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(@methodHandler).ReturnValue;
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
							@methodHandler.IncrementCallCount();
							var @result = @methodHandler.Method is not null ?
								global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int>>(@methodHandler.Method)() :
								global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
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
							@methodHandler.IncrementCallCount();
							var @result = @methodHandler.Method is not null ?
								global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string?>>(@methodHandler.Method)() :
								global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string?>>(@methodHandler).ReturnValue;
							return @result!;
						}
						else
						{
							return base.ToString();
						}
					}
					
					
					[global::Rocks.MemberIdentifier(3, "dynamic CreateDynamicRecord()")]
					protected override dynamic CreateDynamicRecord()
					{
						if (this.handlers.TryGetValue(3, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							@methodHandler.IncrementCallCount();
							var @result = @methodHandler.Method is not null ?
								global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<dynamic>>(@methodHandler.Method)() :
								global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<dynamic>>(@methodHandler).ReturnValue;
							return @result!;
						}
						else
						{
							return base.CreateDynamicRecord();
						}
					}
					
				}
			}
			
			internal static class MethodExpectationsOfHaveDynamicExtensions
			{
				internal static global::Rocks.MethodAdornments<global::HaveDynamic, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::HaveDynamic> @self, global::Rocks.Argument<object?> @obj)
				{
					global::System.ArgumentNullException.ThrowIfNull(@obj);
					return new global::Rocks.MethodAdornments<global::HaveDynamic, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
				}
				internal static global::Rocks.MethodAdornments<global::HaveDynamic, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::HaveDynamic> @self) =>
					new global::Rocks.MethodAdornments<global::HaveDynamic, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::HaveDynamic, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::HaveDynamic> @self) =>
					new global::Rocks.MethodAdornments<global::HaveDynamic, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::HaveDynamic, global::System.Func<dynamic>, dynamic> CreateDynamicRecord(this global::Rocks.Expectations.MethodExpectations<global::HaveDynamic> @self) =>
					new global::Rocks.MethodAdornments<global::HaveDynamic, global::System.Func<dynamic>, dynamic>(@self.Add<dynamic>(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			""";

		// The diagnostic is coming from the method definition in code,
		// which we have to have to ensure the generator doesn't emit [Dynamic],
		// so it's expected to get CS1970.
		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "HaveDynamic_Rock_Create.g.cs", generatedCode) },
			new[] { DiagnosticResult.CompilerError("CS1970").WithSpan(7, 11, 7, 18) }).ConfigureAwait(false);
	}

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
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveGenericAttribute> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveGenericAttribute> @self) =>
						new(@self);
					
					internal static global::MockTests.IHaveGenericAttribute Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveGenericAttribute> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockIHaveGenericAttribute(@self);
							@self.MockType = @mock.GetType();
							return @mock;
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
						
						public RockIHaveGenericAttribute(global::Rocks.Expectations.Expectations<global::MockTests.IHaveGenericAttribute> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
						[global::MockTests.MyAttribute<string>]
						[global::Rocks.MemberIdentifier(0, "void Foo()")]
						public void Foo()
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								if (@methodHandler.Method is not null)
								{
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action>(@methodHandler.Method)();
								}
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
					internal static global::Rocks.MethodAdornments<global::MockTests.IHaveGenericAttribute, global::System.Action> Foo(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveGenericAttribute> @self) =>
						new global::Rocks.MethodAdornments<global::MockTests.IHaveGenericAttribute, global::System.Action>(@self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
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
					internal static global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveMultipleAttributes> Methods(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveMultipleAttributes> @self) =>
						new(@self);
					
					internal static global::MockTests.IHaveMultipleAttributes Instance(this global::Rocks.Expectations.Expectations<global::MockTests.IHaveMultipleAttributes> @self)
					{
						if (!@self.WasInstanceInvoked)
						{
							@self.WasInstanceInvoked = true;
							var @mock = new RockIHaveMultipleAttributes(@self);
							@self.MockType = @mock.GetType();
							return @mock;
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
						
						public RockIHaveMultipleAttributes(global::Rocks.Expectations.Expectations<global::MockTests.IHaveMultipleAttributes> @expectations)
						{
							this.handlers = @expectations.Handlers;
						}
						
						[global::Rocks.MemberIdentifier(0, "void Foo(string @data)")]
						public void Foo([global::MockTests.ParameterOneAttribute, global::MockTests.ParameterTwoAttribute] string @data)
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								var @foundMatch = false;
								
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(@methodHandler.Expectations[0]).IsValid(@data))
									{
										@foundMatch = true;
										
										@methodHandler.IncrementCallCount();
										if (@methodHandler.Method is not null)
										{
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<string>>(@methodHandler.Method)(@data);
										}
										break;
									}
								}
								
								if (!@foundMatch)
								{
									throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void Foo([global::MockTests.ParameterOneAttribute, global::MockTests.ParameterTwoAttribute] string @data)");
								}
							}
							else
							{
								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void Foo([global::MockTests.ParameterOneAttribute, global::MockTests.ParameterTwoAttribute] string @data)");
							}
						}
						
					}
				}
				
				internal static class MethodExpectationsOfIHaveMultipleAttributesExtensions
				{
					internal static global::Rocks.MethodAdornments<global::MockTests.IHaveMultipleAttributes, global::System.Action<string>> Foo(this global::Rocks.Expectations.MethodExpectations<global::MockTests.IHaveMultipleAttributes> @self, global::Rocks.Argument<string> @data)
					{
						global::System.ArgumentNullException.ThrowIfNull(@data);
						return new global::Rocks.MethodAdornments<global::MockTests.IHaveMultipleAttributes, global::System.Action<string>>(@self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @data }));
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IHaveMultipleAttributes_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}