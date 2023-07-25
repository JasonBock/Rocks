using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ShimBuilderGeneratorTests
{
	[Test]
	public static async Task CreateWhenDuplicatesOccurAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Linq;
			using System.Collections.Generic;

			public interface IReadOnlyProperty 
			{ 
				Type ClrType { get; }
			}

			public interface IProperty
				: IReadOnlyProperty { }

			public interface IReadOnlyKey
			{
				bool IsPrimaryKey() => true;

				IReadOnlyList<IReadOnlyProperty> Properties { get; }
			}

			public interface IKey
				: IReadOnlyKey
			{
				Type GetKeyType() => Properties.Count > 1 ? typeof(object[]) : Properties.First().ClrType;

				new IReadOnlyList<IProperty> Properties { get; }
			}

			public interface IRuntimeKey 
				: IKey { }

			public static class Test
			{
				public static void Go() => Rock.Create<IRuntimeKey>();
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfIRuntimeKeyExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::IRuntimeKey> Methods(this global::Rocks.Expectations.Expectations<global::IRuntimeKey> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertyExpectations<global::IRuntimeKey> Properties(this global::Rocks.Expectations.Expectations<global::IRuntimeKey> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertyGetterExpectations<global::IRuntimeKey> Getters(this global::Rocks.Expectations.PropertyExpectations<global::IRuntimeKey> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.ExplicitPropertyExpectations<global::IRuntimeKey, global::IReadOnlyKey> ExplicitPropertiesForIReadOnlyKey(this global::Rocks.Expectations.Expectations<global::IRuntimeKey> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.ExplicitPropertyGetterExpectations<global::IRuntimeKey, global::IReadOnlyKey> Getters(this global::Rocks.Expectations.ExplicitPropertyExpectations<global::IRuntimeKey, global::IReadOnlyKey> @self) =>
					new(@self);
				
				internal static global::IRuntimeKey Instance(this global::Rocks.Expectations.Expectations<global::IRuntimeKey> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						var @mock = new RockIRuntimeKey(@self);
						@self.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockIRuntimeKey
					: global::IRuntimeKey
				{
					private readonly global::IKey shimForIKey;
					private readonly global::IReadOnlyKey shimForIReadOnlyKey;
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockIRuntimeKey(global::Rocks.Expectations.Expectations<global::IRuntimeKey> @expectations)
					{
						(this.handlers, this.shimForIKey, this.shimForIReadOnlyKey) = (@expectations.Handlers, new ShimIKey55018818661256234156060084750235742359064106137(this), new ShimIReadOnlyKey550014593303283198411825442751878980310579223223(this));
					}
					
					[global::Rocks.MemberIdentifier(0, "global::System.Type GetKeyType()")]
					public global::System.Type GetKeyType()
					{
						if (this.handlers.TryGetValue(0, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							@methodHandler.IncrementCallCount();
							var @result = @methodHandler.Method is not null ?
								((global::System.Func<global::System.Type>)@methodHandler.Method)() :
								((global::Rocks.HandlerInformation<global::System.Type>)@methodHandler).ReturnValue;
							return @result!;
						}
						else
						{
							return this.shimForIKey.GetKeyType();
						}
					}
					
					[global::Rocks.MemberIdentifier(1, "bool IsPrimaryKey()")]
					public bool IsPrimaryKey()
					{
						if (this.handlers.TryGetValue(1, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							@methodHandler.IncrementCallCount();
							var @result = @methodHandler.Method is not null ?
								((global::System.Func<bool>)@methodHandler.Method)() :
								((global::Rocks.HandlerInformation<bool>)@methodHandler).ReturnValue;
							return @result!;
						}
						else
						{
							return this.shimForIReadOnlyKey.IsPrimaryKey();
						}
					}
					
					[global::Rocks.MemberIdentifier(2, "get_Properties()")]
					public global::System.Collections.Generic.IReadOnlyList<global::IProperty> Properties
					{
						get
						{
							if (this.handlers.TryGetValue(2, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									((global::System.Func<global::System.Collections.Generic.IReadOnlyList<global::IProperty>>)@methodHandler.Method)() :
									((global::Rocks.HandlerInformation<global::System.Collections.Generic.IReadOnlyList<global::IProperty>>)@methodHandler).ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for get_Properties())");
						}
					}
					[global::Rocks.MemberIdentifier(3, "global::IReadOnlyKey.get_Properties()")]
					global::System.Collections.Generic.IReadOnlyList<global::IReadOnlyProperty> global::IReadOnlyKey.Properties
					{
						get
						{
							if (this.handlers.TryGetValue(3, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									((global::System.Func<global::System.Collections.Generic.IReadOnlyList<global::IReadOnlyProperty>>)@methodHandler.Method)() :
									((global::Rocks.HandlerInformation<global::System.Collections.Generic.IReadOnlyList<global::IReadOnlyProperty>>)@methodHandler).ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::IReadOnlyKey.get_Properties())");
						}
					}
					
					private sealed class ShimIKey55018818661256234156060084750235742359064106137
						: global::IKey
					{
						private readonly RockIRuntimeKey mock;
						
						public ShimIKey55018818661256234156060084750235742359064106137(RockIRuntimeKey @mock) =>
							this.mock = @mock;
						
						public global::System.Collections.Generic.IReadOnlyList<global::IProperty> Properties
						{
							get => ((global::IKey)this.mock).Properties!;
						}
						
						global::System.Collections.Generic.IReadOnlyList<global::IReadOnlyProperty> global::IReadOnlyKey.Properties
						{
							get => ((global::IKey)this.mock).Properties!;
						}
					}
					
					private sealed class ShimIReadOnlyKey550014593303283198411825442751878980310579223223
						: global::IReadOnlyKey
					{
						private readonly RockIRuntimeKey mock;
						
						public ShimIReadOnlyKey550014593303283198411825442751878980310579223223(RockIRuntimeKey @mock) =>
							this.mock = @mock;
						
						public global::System.Collections.Generic.IReadOnlyList<global::IReadOnlyProperty> Properties
						{
							get => ((global::IReadOnlyKey)this.mock).Properties!;
						}
					}
				}
			}
			
			internal static class MethodExpectationsOfIRuntimeKeyExtensions
			{
				internal static global::Rocks.MethodAdornments<global::IRuntimeKey, global::System.Func<global::System.Type>, global::System.Type> GetKeyType(this global::Rocks.Expectations.MethodExpectations<global::IRuntimeKey> @self) =>
					new global::Rocks.MethodAdornments<global::IRuntimeKey, global::System.Func<global::System.Type>, global::System.Type>(@self.Add<global::System.Type>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::IRuntimeKey, global::System.Func<bool>, bool> IsPrimaryKey(this global::Rocks.Expectations.MethodExpectations<global::IRuntimeKey> @self) =>
					new global::Rocks.MethodAdornments<global::IRuntimeKey, global::System.Func<bool>, bool>(@self.Add<bool>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			internal static class PropertyGetterExpectationsOfIRuntimeKeyExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::IRuntimeKey, global::System.Func<global::System.Collections.Generic.IReadOnlyList<global::IProperty>>, global::System.Collections.Generic.IReadOnlyList<global::IProperty>> Properties(this global::Rocks.Expectations.PropertyGetterExpectations<global::IRuntimeKey> @self) =>
					new global::Rocks.PropertyAdornments<global::IRuntimeKey, global::System.Func<global::System.Collections.Generic.IReadOnlyList<global::IProperty>>, global::System.Collections.Generic.IReadOnlyList<global::IProperty>>(@self.Add<global::System.Collections.Generic.IReadOnlyList<global::IProperty>>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			internal static class ExplicitPropertyGetterExpectationsOfIRuntimeKeyForIReadOnlyKeyExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::IRuntimeKey, global::System.Func<global::System.Collections.Generic.IReadOnlyList<global::IReadOnlyProperty>>, global::System.Collections.Generic.IReadOnlyList<global::IReadOnlyProperty>> Properties(this global::Rocks.Expectations.ExplicitPropertyGetterExpectations<global::IRuntimeKey, global::IReadOnlyKey> @self) =>
					new global::Rocks.PropertyAdornments<global::IRuntimeKey, global::System.Func<global::System.Collections.Generic.IReadOnlyList<global::IReadOnlyProperty>>, global::System.Collections.Generic.IReadOnlyList<global::IReadOnlyProperty>>(@self.Add<global::System.Collections.Generic.IReadOnlyList<global::IReadOnlyProperty>>(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IRuntimeKey_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

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
								((global::System.Func<int>)@methodHandler.Method)() :
								((global::Rocks.HandlerInformation<int>)@methodHandler).ReturnValue;
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
								((global::System.Func<int>)@methodHandler.Method)() :
								((global::Rocks.HandlerInformation<int>)@methodHandler).ReturnValue;
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
									((global::System.Func<int>)@methodHandler.Method)() :
									((global::Rocks.HandlerInformation<int>)@methodHandler).ReturnValue;
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
									((global::System.Func<int>)@methodHandler.Method)() :
									((global::Rocks.HandlerInformation<int>)@methodHandler).ReturnValue;
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
									if (((global::Rocks.Argument<string>)@methodHandler.Expectations[0]).IsValid(@dimKey) &&
										((global::Rocks.Argument<int>)@methodHandler.Expectations[1]).IsValid(@dimValue))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											((global::System.Func<string, int, int>)@methodHandler.Method)(@dimKey, @dimValue) :
											((global::Rocks.HandlerInformation<int>)@methodHandler).ReturnValue;
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
									if (((global::Rocks.Argument<string>)@methodHandler.Expectations[0]).IsValid(@notDimKey))
									{
										@methodHandler.IncrementCallCount();
										var @result = @methodHandler.Method is not null ?
											((global::System.Func<string, int>)@methodHandler.Method)(@notDimKey) :
											((global::Rocks.HandlerInformation<int>)@methodHandler).ReturnValue;
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
							((global::IHaveDims)this.mock).IAmNotADim();
						
						public int NotDim
						{
							get => ((global::IHaveDims)this.mock).NotDim!;
						}
						
						public int this[string @notDimKey]
						{
							get => ((global::IHaveDims)this.mock)[@notDimKey];
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