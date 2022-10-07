//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis;
//using NUnit.Framework;
//using Microsoft.CodeAnalysis.Testing;

//namespace Rocks.Tests.Generators;

//public static class VisibilityGeneratorTests
//{
//	[Test]
//	public static async Task GenerateCreateWhenClassHasInternalAbstractMemberAsync()
//	{
//		var internalCode =
//			"""
//			namespace InternalStuff;

//			public abstract class InternalTargets
//			{
//				public abstract void VisibleWork();
//				internal abstract void Work();
//			}
//			""";

//		var internalSyntaxTree = CSharpSyntaxTree.ParseText(internalCode);
//		var internalReferences = AppDomain.CurrentDomain.GetAssemblies()
//			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
//			.Select(_ =>
//			{
//				var location = _.Location;
//				return MetadataReference.CreateFromFile(location);
//			});
//		var internalCompilation = CSharpCompilation.Create(
//			"InternalStuff", new SyntaxTree[] { internalSyntaxTree },
//			internalReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
//		using var internalStream = new MemoryStream();
//		internalCompilation.Emit(internalStream);
//		internalStream.Position = 0;

//		var referencingCode =
//			"""
//			using InternalStuff;
//			using Rocks;

//			public static class Test
//			{
//				public static void Generate() => Rock.Create<InternalTargets>();
//			}
//			""";

//		var referencingGeneratedCode =
//			"""
//			using Rocks.Extensions;
//			using System.Collections.Generic;
//			using System.Collections.Immutable;
//			#nullable enable
			
//			namespace InternalStuff
//			{
//				internal static class CreateExpectationsOfInternalTargetsExtensions
//				{
//					internal static global::Rocks.Expectations.MethodExpectations<global::InternalStuff.InternalTargets> Methods(this global::Rocks.Expectations.Expectations<global::InternalStuff.InternalTargets> @self) =>
//						new(@self);
					
//					internal static global::InternalStuff.InternalTargets Instance(this global::Rocks.Expectations.Expectations<global::InternalStuff.InternalTargets> @self)
//					{
//						if (!@self.WasInstanceInvoked)
//						{
//							@self.WasInstanceInvoked = true;
//							return new RockInternalTargets(@self);
//						}
//						else
//						{
//							throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
//						}
//					}
					
//					private sealed class RockInternalTargets
//						: global::InternalStuff.InternalTargets
//					{
//						private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
						
//						public RockInternalTargets(global::Rocks.Expectations.Expectations<global::InternalStuff.InternalTargets> @expectations) =>
//							this.handlers = @expectations.Handlers;
						
//						[global::Rocks.MemberIdentifier(0, "string? ToString()")]
//						public override string? ToString()
//						{
//							if (this.handlers.TryGetValue(0, out var @methodHandlers))
//							{
//								var @methodHandler = @methodHandlers[0];
//								var @result = @methodHandler.Method is not null ?
//									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string?>>(@methodHandler.Method)() :
//									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string?>>(@methodHandler).ReturnValue;
//								@methodHandler.IncrementCallCount();
//								return @result!;
//							}
//							else
//							{
//								return base.ToString();
//							}
//						}
						
//						[global::Rocks.MemberIdentifier(1, "bool Equals(object? @obj)")]
//						public override bool Equals(object? @obj)
//						{
//							if (this.handlers.TryGetValue(1, out var @methodHandlers))
//							{
//								foreach (var @methodHandler in @methodHandlers)
//								{
//									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<object?>>(@methodHandler.Expectations[0]).IsValid(@obj))
//									{
//										var @result = @methodHandler.Method is not null ?
//											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, bool>>(@methodHandler.Method)(@obj) :
//											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(@methodHandler).ReturnValue;
//										@methodHandler.IncrementCallCount();
//										return @result!;
//									}
//								}
								
//								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for bool Equals(object? @obj)");
//							}
//							else
//							{
//								return base.Equals(@obj);
//							}
//						}
						
//						[global::Rocks.MemberIdentifier(2, "int GetHashCode()")]
//						public override int GetHashCode()
//						{
//							if (this.handlers.TryGetValue(2, out var @methodHandlers))
//							{
//								var @methodHandler = @methodHandlers[0];
//								var @result = @methodHandler.Method is not null ?
//									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int>>(@methodHandler.Method)() :
//									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
//								@methodHandler.IncrementCallCount();
//								return @result!;
//							}
//							else
//							{
//								return base.GetHashCode();
//							}
//						}
						
//						[global::Rocks.MemberIdentifier(3, "void VisibleWork()")]
//						public override void VisibleWork()
//						{
//							if (this.handlers.TryGetValue(3, out var @methodHandlers))
//							{
//								var @methodHandler = @methodHandlers[0];
//								if (@methodHandler.Method is not null)
//								{
//									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action>(@methodHandler.Method)();
//								}
								
//								@methodHandler.IncrementCallCount();
//							}
//							else
//							{
//								throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void VisibleWork()");
//							}
//						}
						
//					}
//				}
				
//				internal static class MethodExpectationsOfInternalTargetsExtensions
//				{
//					internal static global::Rocks.MethodAdornments<global::InternalStuff.InternalTargets, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::InternalStuff.InternalTargets> @self) =>
//						new global::Rocks.MethodAdornments<global::InternalStuff.InternalTargets, global::System.Func<string?>, string?>(@self.Add<string?>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
//					internal static global::Rocks.MethodAdornments<global::InternalStuff.InternalTargets, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::InternalStuff.InternalTargets> @self, global::Rocks.Argument<object?> @obj)
//					{
//						global::System.ArgumentNullException.ThrowIfNull(@obj);
//						return new global::Rocks.MethodAdornments<global::InternalStuff.InternalTargets, global::System.Func<object?, bool>, bool>(@self.Add<bool>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
//					}
//					internal static global::Rocks.MethodAdornments<global::InternalStuff.InternalTargets, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::InternalStuff.InternalTargets> @self) =>
//						new global::Rocks.MethodAdornments<global::InternalStuff.InternalTargets, global::System.Func<int>, int>(@self.Add<int>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
//					internal static global::Rocks.MethodAdornments<global::InternalStuff.InternalTargets, global::System.Action> VisibleWork(this global::Rocks.Expectations.MethodExpectations<global::InternalStuff.InternalTargets> @self) =>
//						new global::Rocks.MethodAdornments<global::InternalStuff.InternalTargets, global::System.Action>(@self.Add(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
//				}
//			}
			
//			""";

//		var additionalReferences = internalReferences.ToArray()
//			.Concat(new[]
//			{
//				MetadataReference.CreateFromStream(internalStream)
//			});

//		await TestAssistants.RunAsync<RockCreateGenerator>(referencingCode,
//			new[] { (typeof(RockCreateGenerator), "InternalTargets_Rock_Create.g.cs", referencingGeneratedCode) },
//			Enumerable.Empty<DiagnosticResult>(),
//			additionalReferences: additionalReferences).ConfigureAwait(false);
//	}

//	[Test]
//	public static async Task GenerateCreateWhenClassHasInternalVirtualMemberAsync()
//	{
//		await TestAssistants.RunAsync<RockCreateGenerator>(referencingCode,
//			new[] { (typeof(RockCreateGenerator), "InternalTargets_Rock_Create.g.cs", referencingGeneratedCode) },
//			Enumerable.Empty<DiagnosticResult>(),
//			additionalReferences: additionalReferences).ConfigureAwait(false);
//	}

//	[Test]
//	public static async Task GenerateCreateWhenInterfaceHasInternalMemberAsync()
//	{
//		await TestAssistants.RunAsync<RockCreateGenerator>(referencingCode,
//			new[] { (typeof(RockCreateGenerator), "InternalTargets_Rock_Create.g.cs", referencingGeneratedCode) },
//			Enumerable.Empty<DiagnosticResult>(),
//			additionalReferences: additionalReferences).ConfigureAwait(false);
//	}

//	[Test]
//	public static async Task GenerateMakeWhenClassHasInternalAbstractMemberAsync()
//	{
//		await TestAssistants.RunAsync<RockCreateGenerator>(referencingCode,
//			new[] { (typeof(RockCreateGenerator), "InternalTargets_Rock_Create.g.cs", referencingGeneratedCode) },
//			Enumerable.Empty<DiagnosticResult>(),
//			additionalReferences: additionalReferences).ConfigureAwait(false);
//	}

//	[Test]
//	public static async Task GenerateMakeWhenClassHasInternalVirtualMemberAsync()
//	{
//		await TestAssistants.RunAsync<RockCreateGenerator>(referencingCode,
//			new[] { (typeof(RockCreateGenerator), "InternalTargets_Rock_Create.g.cs", referencingGeneratedCode) },
//			Enumerable.Empty<DiagnosticResult>(),
//			additionalReferences: additionalReferences).ConfigureAwait(false);
//	}

//	[Test]
//	public static async Task GenerateMakeWhenInterfaceHasInternalMemberAsync()
//	{
//		await TestAssistants.RunAsync<RockCreateGenerator>(referencingCode,
//			new[] { (typeof(RockCreateGenerator), "InternalTargets_Rock_Create.g.cs", referencingGeneratedCode) },
//			Enumerable.Empty<DiagnosticResult>(),
//			additionalReferences: additionalReferences).ConfigureAwait(false);
//	}
//}