using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class NullabilityGeneratorTests
{
	[Test]
	public static async Task GenerateWhenPropertyInShimNeedsNullForgivingAsync()
	{
		var code =
			"""
			using Rocks;

			#nullable enable

			public interface IReadOnlyNavigationBase
			{
				IReadOnlyNavigationBase? Inverse { get; }
			}

			public interface IReadOnlySkipNavigation
				: IReadOnlyNavigationBase
			{
				new IReadOnlySkipNavigation Inverse { get; }

				IReadOnlyNavigationBase IReadOnlyNavigationBase.Inverse
				{
					get => Inverse;
				}
			}

			public interface IConventionSkipNavigation 
				: IReadOnlySkipNavigation
			{
				new IConventionSkipNavigation? Inverse
				{
					get => (IConventionSkipNavigation?)((IReadOnlySkipNavigation)this).Inverse;
				}
			}

			public static class Test
			{
				public static void Go() => Rock.Create<IConventionSkipNavigation>();
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfIConventionSkipNavigationExtensions
			{
				internal static global::Rocks.Expectations.PropertyExpectations<global::IConventionSkipNavigation> Properties(this global::Rocks.Expectations.Expectations<global::IConventionSkipNavigation> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.PropertyGetterExpectations<global::IConventionSkipNavigation> Getters(this global::Rocks.Expectations.PropertyExpectations<global::IConventionSkipNavigation> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.ExplicitPropertyExpectations<global::IConventionSkipNavigation, global::IReadOnlySkipNavigation> ExplicitPropertiesForIReadOnlySkipNavigation(this global::Rocks.Expectations.Expectations<global::IConventionSkipNavigation> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.ExplicitPropertyGetterExpectations<global::IConventionSkipNavigation, global::IReadOnlySkipNavigation> Getters(this global::Rocks.Expectations.ExplicitPropertyExpectations<global::IConventionSkipNavigation, global::IReadOnlySkipNavigation> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.ExplicitPropertyExpectations<global::IConventionSkipNavigation, global::IReadOnlyNavigationBase> ExplicitPropertiesForIReadOnlyNavigationBase(this global::Rocks.Expectations.Expectations<global::IConventionSkipNavigation> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.ExplicitPropertyGetterExpectations<global::IConventionSkipNavigation, global::IReadOnlyNavigationBase> Getters(this global::Rocks.Expectations.ExplicitPropertyExpectations<global::IConventionSkipNavigation, global::IReadOnlyNavigationBase> @self) =>
					new(@self);
				
				internal static global::IConventionSkipNavigation Instance(this global::Rocks.Expectations.Expectations<global::IConventionSkipNavigation> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						var @mock = new RockIConventionSkipNavigation(@self);
						@self.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockIConventionSkipNavigation
					: global::IConventionSkipNavigation
				{
					private readonly global::IConventionSkipNavigation shimForIConventionSkipNavigation;
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockIConventionSkipNavigation(global::Rocks.Expectations.Expectations<global::IConventionSkipNavigation> @expectations)
					{
						(this.handlers, this.shimForIConventionSkipNavigation) = (@expectations.Handlers, new ShimIConventionSkipNavigation396255620100734449241449954173443346123272207515(this));
					}
					
					[global::Rocks.MemberIdentifier(0, "get_Inverse()")]
					public global::IConventionSkipNavigation? Inverse
					{
						get
						{
							if (this.handlers.TryGetValue(0, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									((global::System.Func<global::IConventionSkipNavigation?>)@methodHandler.Method)() :
									((global::Rocks.HandlerInformation<global::IConventionSkipNavigation?>)@methodHandler).ReturnValue;
								return @result!;
							}
							else
							{
								return this.shimForIConventionSkipNavigation.Inverse;
							}
						}
					}
					[global::Rocks.MemberIdentifier(1, "global::IReadOnlySkipNavigation.get_Inverse()")]
					global::IReadOnlySkipNavigation global::IReadOnlySkipNavigation.Inverse
					{
						get
						{
							if (this.handlers.TryGetValue(1, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									((global::System.Func<global::IReadOnlySkipNavigation>)@methodHandler.Method)() :
									((global::Rocks.HandlerInformation<global::IReadOnlySkipNavigation>)@methodHandler).ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::IReadOnlySkipNavigation.get_Inverse())");
						}
					}
					[global::Rocks.MemberIdentifier(2, "global::IReadOnlyNavigationBase.get_Inverse()")]
					global::IReadOnlyNavigationBase? global::IReadOnlyNavigationBase.Inverse
					{
						get
						{
							if (this.handlers.TryGetValue(2, out var @methodHandlers))
							{
								var @methodHandler = @methodHandlers[0];
								@methodHandler.IncrementCallCount();
								var @result = @methodHandler.Method is not null ?
									((global::System.Func<global::IReadOnlyNavigationBase?>)@methodHandler.Method)() :
									((global::Rocks.HandlerInformation<global::IReadOnlyNavigationBase?>)@methodHandler).ReturnValue;
								return @result!;
							}
							
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::IReadOnlyNavigationBase.get_Inverse())");
						}
					}
					
					private sealed class ShimIConventionSkipNavigation396255620100734449241449954173443346123272207515
						: global::IConventionSkipNavigation
					{
						private readonly RockIConventionSkipNavigation mock;
						
						public ShimIConventionSkipNavigation396255620100734449241449954173443346123272207515(RockIConventionSkipNavigation @mock) =>
							this.mock = @mock;
						
						global::IReadOnlySkipNavigation global::IReadOnlySkipNavigation.Inverse
						{
							get => ((global::IConventionSkipNavigation)this.mock).Inverse!;
						}
						
						global::IReadOnlyNavigationBase? global::IReadOnlyNavigationBase.Inverse
						{
							get => ((global::IConventionSkipNavigation)this.mock).Inverse!;
						}
					}
				}
			}
			
			internal static class PropertyGetterExpectationsOfIConventionSkipNavigationExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::IConventionSkipNavigation, global::System.Func<global::IConventionSkipNavigation?>, global::IConventionSkipNavigation?> Inverse(this global::Rocks.Expectations.PropertyGetterExpectations<global::IConventionSkipNavigation> @self) =>
					new global::Rocks.PropertyAdornments<global::IConventionSkipNavigation, global::System.Func<global::IConventionSkipNavigation?>, global::IConventionSkipNavigation?>(@self.Add<global::IConventionSkipNavigation?>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			internal static class ExplicitPropertyGetterExpectationsOfIConventionSkipNavigationForIReadOnlySkipNavigationExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::IConventionSkipNavigation, global::System.Func<global::IReadOnlySkipNavigation>, global::IReadOnlySkipNavigation> Inverse(this global::Rocks.Expectations.ExplicitPropertyGetterExpectations<global::IConventionSkipNavigation, global::IReadOnlySkipNavigation> @self) =>
					new global::Rocks.PropertyAdornments<global::IConventionSkipNavigation, global::System.Func<global::IReadOnlySkipNavigation>, global::IReadOnlySkipNavigation>(@self.Add<global::IReadOnlySkipNavigation>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			internal static class ExplicitPropertyGetterExpectationsOfIConventionSkipNavigationForIReadOnlyNavigationBaseExtensions
			{
				internal static global::Rocks.PropertyAdornments<global::IConventionSkipNavigation, global::System.Func<global::IReadOnlyNavigationBase?>, global::IReadOnlyNavigationBase?> Inverse(this global::Rocks.Expectations.ExplicitPropertyGetterExpectations<global::IConventionSkipNavigation, global::IReadOnlyNavigationBase> @self) =>
					new global::Rocks.PropertyAdornments<global::IConventionSkipNavigation, global::System.Func<global::IReadOnlyNavigationBase?>, global::IReadOnlyNavigationBase?>(@self.Add<global::IReadOnlyNavigationBase?>(2, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IConventionSkipNavigation_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task GenerateWhenTargetIsInterfaceAndMethodIsConstrainedByTypeParameterThatIsAssignedAsync()
	{
		var code =
			"""
			using Rocks;

			public interface IDestination<TDestination>
			{
				void As<T>() where T : TDestination;
			}

			public static class Test
			{
				public static void Go() => Rock.Create<IDestination<object>>();
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfIDestinationOfobjectExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::IDestination<object>> Methods(this global::Rocks.Expectations.Expectations<global::IDestination<object>> @self) =>
					new(@self);
				
				internal static global::IDestination<object> Instance(this global::Rocks.Expectations.Expectations<global::IDestination<object>> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						var @mock = new RockIDestinationOfobject(@self);
						@self.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockIDestinationOfobject
					: global::IDestination<object>
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockIDestinationOfobject(global::Rocks.Expectations.Expectations<global::IDestination<object>> @expectations)
					{
						this.handlers = @expectations.Handlers;
					}
					
					[global::Rocks.MemberIdentifier(0, "void As<T>()")]
					public void As<T>()
						where T : notnull
					{
						if (this.handlers.TryGetValue(0, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							@methodHandler.IncrementCallCount();
							if (@methodHandler.Method is not null && @methodHandler.Method is global::System.Action @method)
							{
								@method();
							}
						}
						else
						{
							throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void As<T>()");
						}
					}
					
				}
			}
			
			internal static class MethodExpectationsOfIDestinationOfobjectExtensions
			{
				internal static global::Rocks.MethodAdornments<global::IDestination<object>, global::System.Action> As<T>(this global::Rocks.Expectations.MethodExpectations<global::IDestination<object>> @self) where T : notnull =>
					new global::Rocks.MethodAdornments<global::IDestination<object>, global::System.Action>(@self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IDestinationobject_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}