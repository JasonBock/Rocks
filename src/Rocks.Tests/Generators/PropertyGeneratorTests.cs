using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class PropertyGeneratorTests
{
	[Test]
	public static async Task CreateWithMixedVisibilityAsync()
	{
		var code =
			"""
			using System;
			using Rocks;

			#nullable enable

			public class MixedProperties
			{
				public virtual string? PublicGetPrivateSet { get; private set; }
				public virtual string? PublicGetProtectedSet { get; protected set; }
				public virtual string? PrivateGetPublicSet { private get; set; }
				public virtual string? ProtectedGetPublicSet { protected get; set; }
			}

			public static class Test
			{
				public static void Generate()
				{
					var rock = Rock.Create<MixedProperties>();
				}
			}			
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfMixedPropertiesExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::MixedProperties> Methods(this global::Rocks.Expectations.Expectations<global::MixedProperties> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertyExpectations<global::MixedProperties> Properties(this global::Rocks.Expectations.Expectations<global::MixedProperties> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertyGetterExpectations<global::MixedProperties> Getters(this global::Rocks.Expectations.PropertyExpectations<global::MixedProperties> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertySetterExpectations<global::MixedProperties> Setters(this global::Rocks.Expectations.PropertyExpectations<global::MixedProperties> @self) =>
					new(@self);
				
				internal static global::MixedProperties Instance(this global::Rocks.Expectations.Expectations<global::MixedProperties> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						return new RockMixedProperties(@self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockMixedProperties
					: global::MixedProperties
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockMixedProperties(global::Rocks.Expectations.Expectations<global::MixedProperties> @expectations)
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
									var @result = @methodHandler.Method is not null ?
										global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<object?, bool>>(@methodHandler.Method)(@obj) :
										global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<bool>>(@methodHandler).ReturnValue;
									@methodHandler.IncrementCallCount();
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
							var @result = @methodHandler.Method is not null ?
								global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<int>>(@methodHandler.Method)() :
								global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<int>>(@methodHandler).ReturnValue;
							@methodHandler.IncrementCallCount();
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
							var @result = @methodHandler.Method is not null ?
								global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string?>>(@methodHandler.Method)() :
								global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string?>>(@methodHandler).ReturnValue;
							@methodHandler.IncrementCallCount();
							return @result!;
						}
						else
						{
							return base.ToString();
						}
					}
					
					[global::Rocks.MemberIdentifier(3, "get_PublicGetPrivateSet()")]
					public override string? PublicGetPrivateSet
					{
						get
						{
							if (this.handlers.TryGetValue(3, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string?>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string?>>(@methodHandler).ReturnValue;
								@methodHandler.IncrementCallCount();
								return @result!;
							}
							else
							{
								return base.PublicGetPrivateSet;
							}
						}
					}
					[global::Rocks.MemberIdentifier(5, "get_PublicGetProtectedSet()")]
					[global::Rocks.MemberIdentifier(6, "set_PublicGetProtectedSet(@value)")]
					public override string? PublicGetProtectedSet
					{
						get
						{
							if (this.handlers.TryGetValue(5, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string?>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string?>>(@methodHandler).ReturnValue;
								@methodHandler.IncrementCallCount();
								return @result!;
							}
							else
							{
								return base.PublicGetProtectedSet;
							}
						}
						protected set
						{
							if (this.handlers.TryGetValue(6, out var @methodHandlers))
							{
								var @foundMatch = false;
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string?>>(@methodHandler.Expectations[0]).IsValid(@value))
									{
										@foundMatch = true;
										
										if (@methodHandler.Method is not null)
										{
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<string?>>(@methodHandler.Method)(@value);
										}
										
										if (!@foundMatch)
										{
											throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_PublicGetProtectedSet(@value)");
										}
										
										@methodHandler.IncrementCallCount();
										break;
									}
								}
							}
							else
							{
								base.PublicGetProtectedSet = @value;
							}
						}
					}
					[global::Rocks.MemberIdentifier(7, "set_PrivateGetPublicSet(@value)")]
					public override string? PrivateGetPublicSet
					{
						set
						{
							if (this.handlers.TryGetValue(7, out var @methodHandlers))
							{
								var @foundMatch = false;
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string?>>(@methodHandler.Expectations[0]).IsValid(@value))
									{
										@foundMatch = true;
										
										if (@methodHandler.Method is not null)
										{
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<string?>>(@methodHandler.Method)(@value);
										}
										
										if (!@foundMatch)
										{
											throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_PrivateGetPublicSet(@value)");
										}
										
										@methodHandler.IncrementCallCount();
										break;
									}
								}
							}
							else
							{
								base.PrivateGetPublicSet = @value;
							}
						}
					}
					[global::Rocks.MemberIdentifier(9, "get_ProtectedGetPublicSet()")]
					[global::Rocks.MemberIdentifier(10, "set_ProtectedGetPublicSet(@value)")]
					public override string? ProtectedGetPublicSet
					{
						protected get
						{
							if (this.handlers.TryGetValue(9, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								var @result = @methodHandler.Method is not null ?
									global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<string?>>(@methodHandler.Method)() :
									global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<string?>>(@methodHandler).ReturnValue;
								@methodHandler.IncrementCallCount();
								return @result!;
							}
							else
							{
								return base.ProtectedGetPublicSet;
							}
						}
						set
						{
							if (this.handlers.TryGetValue(10, out var @methodHandlers))
							{
								var @foundMatch = false;
								foreach (var @methodHandler in @methodHandlers)
								{
									if (global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.Argument<string?>>(@methodHandler.Expectations[0]).IsValid(@value))
									{
										@foundMatch = true;
										
										if (@methodHandler.Method is not null)
										{
											global::System.Runtime.CompilerServices.Unsafe.As<global::System.Action<string?>>(@methodHandler.Method)(@value);
										}
										
										if (!@foundMatch)
										{
											throw new global::Rocks.Exceptions.ExpectationException("No handlers match for set_ProtectedGetPublicSet(@value)");
										}
										
										@methodHandler.IncrementCallCount();
										break;
									}
								}
							}
							else
							{
								base.ProtectedGetPublicSet = @value;
							}
						}
					}
				}
			}
			
			internal static class MethodExpectationsOfMixedPropertiesExtensions
			{
				internal static global::Rocks.MethodAdornments<global::MixedProperties, global::System.Func<object?, bool>, bool> Equals(this global::Rocks.Expectations.MethodExpectations<global::MixedProperties> @self, global::Rocks.Argument<object?> @obj)
				{
					global::System.ArgumentNullException.ThrowIfNull(@obj);
					return new global::Rocks.MethodAdornments<global::MixedProperties, global::System.Func<object?, bool>, bool>(@self.Add<bool>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @obj }));
				}
				internal static global::Rocks.MethodAdornments<global::MixedProperties, global::System.Func<int>, int> GetHashCode(this global::Rocks.Expectations.MethodExpectations<global::MixedProperties> @self) =>
					new global::Rocks.MethodAdornments<global::MixedProperties, global::System.Func<int>, int>(@self.Add<int>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.MethodAdornments<global::MixedProperties, global::System.Func<string?>, string?> ToString(this global::Rocks.Expectations.MethodExpectations<global::MixedProperties> @self) =>
					new global::Rocks.MethodAdornments<global::MixedProperties, global::System.Func<string?>, string?>(@self.Add<string?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			internal static class PropertyGetterExpectationsOfMixedPropertiesExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::MixedProperties, global::System.Func<string?>, string?> PublicGetPrivateSet(this global::Rocks.Expectations.PropertyGetterExpectations<global::MixedProperties> @self) =>
					new global::Rocks.PropertyAdornments<global::MixedProperties, global::System.Func<string?>, string?>(@self.Add<string?>(3, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.PropertyAdornments<global::MixedProperties, global::System.Func<string?>, string?> PublicGetProtectedSet(this global::Rocks.Expectations.PropertyGetterExpectations<global::MixedProperties> @self) =>
					new global::Rocks.PropertyAdornments<global::MixedProperties, global::System.Func<string?>, string?>(@self.Add<string?>(5, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
				internal static global::Rocks.PropertyAdornments<global::MixedProperties, global::System.Func<string?>, string?> ProtectedGetPublicSet(this global::Rocks.Expectations.PropertyGetterExpectations<global::MixedProperties> @self) =>
					new global::Rocks.PropertyAdornments<global::MixedProperties, global::System.Func<string?>, string?>(@self.Add<string?>(9, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			internal static class PropertySetterExpectationsOfMixedPropertiesExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::MixedProperties, global::System.Action<string?>> PublicGetProtectedSet(this global::Rocks.Expectations.PropertySetterExpectations<global::MixedProperties> @self, global::Rocks.Argument<string?> @value) =>
					new global::Rocks.PropertyAdornments<global::MixedProperties, global::System.Action<string?>>(@self.Add(6, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
				internal static global::Rocks.PropertyAdornments<global::MixedProperties, global::System.Action<string?>> PrivateGetPublicSet(this global::Rocks.Expectations.PropertySetterExpectations<global::MixedProperties> @self, global::Rocks.Argument<string?> @value) =>
					new global::Rocks.PropertyAdornments<global::MixedProperties, global::System.Action<string?>>(@self.Add(8, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
				internal static global::Rocks.PropertyAdornments<global::MixedProperties, global::System.Action<string?>> ProtectedGetPublicSet(this global::Rocks.Expectations.PropertySetterExpectations<global::MixedProperties> @self, global::Rocks.Argument<string?> @value) =>
					new global::Rocks.PropertyAdornments<global::MixedProperties, global::System.Action<string?>>(@self.Add(10, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "MixedProperties_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task MakeWithMixedVisibilityAsync()
	{
		var code =
			"""
			using System;
			using Rocks;

			#nullable enable

			public class MixedProperties
			{
				public virtual string? PublicGetPrivateSet { get; private set; }
				public virtual string? PublicGetProtectedSet { get; protected set; }
				public virtual string? PrivateGetPublicSet { private get; set; }
				public virtual string? ProtectedGetPublicSet { protected get; set; }
			}

			public static class Test
			{
				public static void Generate()
				{
					var rock = Rock.Make<MixedProperties>();
				}
			}			
			""";

		var generatedCode =
			"""
			#nullable enable
			
			internal static class MakeExpectationsOfMixedPropertiesExtensions
			{
				internal static global::MixedProperties Instance(this global::Rocks.MakeGeneration<global::MixedProperties> @self) =>
					new RockMixedProperties();
				
				private sealed class RockMixedProperties
					: global::MixedProperties
				{
					public RockMixedProperties() { }
					
					public override bool Equals(object? @obj)
					{
						return default!;
					}
					public override int GetHashCode()
					{
						return default!;
					}
					public override string? ToString()
					{
						return default!;
					}
					public override string? PublicGetPrivateSet
					{
						get => default!;
					}
					public override string? PublicGetProtectedSet
					{
						get => default!;
						protected set { }
					}
					public override string? PrivateGetPublicSet
					{
						set { }
					}
					public override string? ProtectedGetPublicSet
					{
						protected get => default!;
						set { }
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "MixedProperties_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}