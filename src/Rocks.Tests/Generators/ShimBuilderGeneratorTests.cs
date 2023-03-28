using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ShimBuilderGeneratorTests
{
	[Test]
	public static async Task GenerateAbstractCreateAsync()
	{
		var code =
			"""
			using Rocks;

			public interface IDoNotHaveDims
			{
				int IAmNotADim();
				int NotDim { get; }
				int this[string notDimKey] { get; }
			}
			
			public interface IHaveDims
				: IDoNotHaveDims
			{
				int IAmADim() => 2;
				int AmADim { get => 2; }
				int this[string dimKey, int dimValue] { get => dimKey.GetHashCode() + dimValue; }
			}
			
			public static class Test
			{
				public static void Go() => Rock.Create<IHaveDims>();
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfIHaveDimsExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::IHaveDims> Methods(this global::Rocks.Expectations.Expectations<global::IHaveDims> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertyExpectations<global::IHaveDims> Properties(this global::Rocks.Expectations.Expectations<global::IHaveDims> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertyGetterExpectations<global::IHaveDims> Getters(this global::Rocks.Expectations.PropertyExpectations<global::IHaveDims> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.IndexerExpectations<global::IHaveDims> Indexers(this global::Rocks.Expectations.Expectations<global::IHaveDims> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.IndexerGetterExpectations<global::IHaveDims> Getters(this global::Rocks.Expectations.IndexerExpectations<global::IHaveDims> @self) =>
					new(@self);
				
				internal static global::IHaveDims Instance(this global::Rocks.Expectations.Expectations<global::IHaveDims> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						var @mock = new RockIHaveDims(@self);
						@self.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockIHaveDims
					: global::IHaveDims
				{
					private readonly global::IHaveDims shimForIHaveDims;
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockIHaveDims(global::Rocks.Expectations.Expectations<global::IHaveDims> @expectations)
					{
						(this.handlers, this.shimForIHaveDims) = (@expectations.Handlers, new ShimIHaveDims531557381186657891604647139828315705947108387206(this));
					}
					
					[global::Rocks.MemberIdentifier(0, "int IAmADim()")]
					public int IAmADim()
					{
						if (this.handlers.TryGetValue(0, out var @methodHandlers))
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
							return this.shimForIHaveDims.IAmADim();
						}
					}
					
					[global::Rocks.MemberIdentifier(1, "int IAmNotADim()")]
					public int IAmNotADim()
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
						
						throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for int IAmNotADim()");
					}
					
					[global::Rocks.MemberIdentifier(2, "get_AmADim()")]
					public int AmADim
					{
						get
						{
							if (this.handlers.TryGetValue(2, out var @methodHandlers))
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
								return this.shimForIHaveDims.AmADim;
							}
						}
					}
					[global::Rocks.MemberIdentifier(4, "get_NotDim()")]
					public int NotDim
					{
						get
						{
							if (this.handlers.TryGetValue(4, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_NotDim())");
						}
					}
					[global::Rocks.MemberIdentifier(3, "this[string @dimKey, int @dimValue]")]
					public int this[string @dimKey, int @dimValue]
					{
						get
						{
							if (this.handlers.TryGetValue(3, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(@methodHandler.Expectations[0]).IsValid(@dimKey) &&
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<int>>(@methodHandler.Expectations[1]).IsValid(@dimValue))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string, int, int>>(@methodHandler.Method)(@dimKey, @dimValue) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[string @dimKey, int @dimValue]");
							}
							else
							{
								return this.shimForIHaveDims[@dimKey, @dimValue];
							}
						}
					}
					[global::Rocks.MemberIdentifier(5, "this[string @notDimKey]")]
					public int this[string @notDimKey]
					{
						get
						{
							if (this.handlers.TryGetValue(5, out var @methodHandlers))
							{
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string>>(@methodHandler.Expectations[0]).IsValid(@notDimKey))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string, int>>(@methodHandler.Method)(@notDimKey) :
											global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
										return @result!;
									}
								}
								
								throw new global::Rocks.Exceptions.ExpectationException("No handlers match for this[string @notDimKey]");
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for this[string @notDimKey])");
						}
					}
					
					private sealed class ShimIHaveDims531557381186657891604647139828315705947108387206
						: global::IHaveDims
					{
						private readonly RockIHaveDims mock;
						
						public ShimIHaveDims531557381186657891604647139828315705947108387206(RockIHaveDims @mock) =>
							this.mock = @mock;
						
						public int IAmNotADim() =>
							global::System.Runtime.CompilerServices.Unsafe.As<global::IHaveDims>(this.mock).IAmNotADim();
						
						public int NotDim
						{
							get => global::System.Runtime.CompilerServices.Unsafe.As<global::IHaveDims>(this.mock).NotDim;
						}
						
						public int this[string @notDimKey]
						{
							get => global::System.Runtime.CompilerServices.Unsafe.As<global::IHaveDims>(this.mock)[@notDimKey];
						}
					}
				}
			}
			
			internal static class MethodExpectationsOfIHaveDimsExtensions
			{
				internal static global::Rocks.MethodAdornments<global::IHaveDims, global::System.Func<int>, int> IAmADim(this global::Rocks.Expectations.MethodExpectations<global::IHaveDims> @self) =>
					new global::Rocks.MethodAdornments<global::IHaveDims, global::System.Func<int>, int>(@self.Add<int>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::IHaveDims, global::System.Func<int>, int> IAmNotADim(this global::Rocks.Expectations.MethodExpectations<global::IHaveDims> @self) =>
					new global::Rocks.MethodAdornments<global::IHaveDims, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			internal static class PropertyGetterExpectationsOfIHaveDimsExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::IHaveDims, global::System.Func<int>, int> AmADim(this global::Rocks.Expectations.PropertyGetterExpectations<global::IHaveDims> @self) =>
					new global::Rocks.PropertyAdornments<global::IHaveDims, global::System.Func<int>, int>(@self.Add<int>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.PropertyAdornments<global::IHaveDims, global::System.Func<int>, int> NotDim(this global::Rocks.Expectations.PropertyGetterExpectations<global::IHaveDims> @self) =>
					new global::Rocks.PropertyAdornments<global::IHaveDims, global::System.Func<int>, int>(@self.Add<int>(4, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			internal static class IndexerGetterExpectationsOfIHaveDimsExtensions
			{
				internal static global::Rocks.IndexerAdornments<global::IHaveDims, global::System.Func<string, int, int>, int> This(this global::Rocks.Expectations.IndexerGetterExpectations<global::IHaveDims> @self, global::Rocks.Argument<string> @dimKey, global::Rocks.Argument<int> @dimValue)
				{
					global::System.ArgumentNullException.ThrowIfNull(@dimKey);
					global::System.ArgumentNullException.ThrowIfNull(@dimValue);
					return new global::Rocks.IndexerAdornments<global::IHaveDims, global::System.Func<string, int, int>, int>(@self.Add<int>(3, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { @dimKey, @dimValue }));
				}
				internal static global::Rocks.IndexerAdornments<global::IHaveDims, global::System.Func<string, int>, int> This(this global::Rocks.Expectations.IndexerGetterExpectations<global::IHaveDims> @self, global::Rocks.Argument<string> @notDimKey)
				{
					global::System.ArgumentNullException.ThrowIfNull(@notDimKey);
					return new global::Rocks.IndexerAdornments<global::IHaveDims, global::System.Func<string, int>, int>(@self.Add<int>(5, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @notDimKey }));
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IHaveDims_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}