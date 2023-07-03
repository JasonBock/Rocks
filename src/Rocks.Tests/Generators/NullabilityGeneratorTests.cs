using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class NullabilityGeneratorTests
{
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
			new[] { (typeof(RockCreateGenerator), "IDestinationOfobject_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}