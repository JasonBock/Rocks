using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ExplicitImplementationGeneratorTests
{
	[Test]
	public static async Task CreateWithExplicitImplementationAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Collections.Generic;
			
			public interface ISetup { }

			public interface ISetupList
				: IEnumerable<ISetup> { }

			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Create<ISetupList>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfISetupListExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::ISetupList> Methods(this global::Rocks.Expectations.Expectations<global::ISetupList> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.ExplicitMethodExpectations<global::ISetupList, global::System.Collections.IEnumerable> ExplicitMethodsForIEnumerable(this global::Rocks.Expectations.Expectations<global::ISetupList> @self) =>
					new(@self);
				
				internal static global::ISetupList Instance(this global::Rocks.Expectations.Expectations<global::ISetupList> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						return new RockISetupList(@self);
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockISetupList
					: global::ISetupList
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockISetupList(global::Rocks.Expectations.Expectations<global::ISetupList> @expectations)
					{
						this.handlers = @expectations.Handlers;
					}
					
					
					[global::Rocks.MemberIdentifier(0, "global::System.Collections.Generic.IEnumerator<global::ISetup> GetEnumerator()")]
					public global::System.Collections.Generic.IEnumerator<global::ISetup> GetEnumerator()
					{
						if (this.handlers.TryGetValue(0, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							var @result = @methodHandler.Method is not null ?
								global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<global::System.Collections.Generic.IEnumerator<global::ISetup>>>(@methodHandler.Method)() :
								global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<global::System.Collections.Generic.IEnumerator<global::ISetup>>>(@methodHandler).ReturnValue;
							@methodHandler.IncrementCallCount();
							return @result!;
						}
						
						throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::System.Collections.Generic.IEnumerator<global::ISetup> GetEnumerator()");
					}
					
					[global::Rocks.MemberIdentifier(1, "global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()")]
					global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
					{
						if (this.handlers.TryGetValue(1, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							var @result = @methodHandler.Method is not null ?
								global::System.Runtime.CompilerServices.Unsafe.As<global::System.Func<global::System.Collections.IEnumerator>>(@methodHandler.Method)() :
								global::System.Runtime.CompilerServices.Unsafe.As<global::Rocks.HandlerInformation<global::System.Collections.IEnumerator>>(@methodHandler).ReturnValue;
							@methodHandler.IncrementCallCount();
							return @result!;
						}
						
						throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()");
					}
					
				}
			}
			
			internal static class MethodExpectationsOfISetupListExtensions
			{
				internal static global::Rocks.MethodAdornments<global::ISetupList, global::System.Func<global::System.Collections.Generic.IEnumerator<global::ISetup>>, global::System.Collections.Generic.IEnumerator<global::ISetup>> GetEnumerator(this global::Rocks.Expectations.MethodExpectations<global::ISetupList> @self) =>
					new global::Rocks.MethodAdornments<global::ISetupList, global::System.Func<global::System.Collections.Generic.IEnumerator<global::ISetup>>, global::System.Collections.Generic.IEnumerator<global::ISetup>>(@self.Add<global::System.Collections.Generic.IEnumerator<global::ISetup>>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			internal static class ExplicitMethodExpectationsOfISetupListForIEnumerableExtensions
			{
				internal static global::Rocks.MethodAdornments<global::ISetupList, global::System.Func<global::System.Collections.IEnumerator>, global::System.Collections.IEnumerator> GetEnumerator(this global::Rocks.Expectations.ExplicitMethodExpectations<global::ISetupList, global::System.Collections.IEnumerable> @self) =>
					new global::Rocks.MethodAdornments<global::ISetupList, global::System.Func<global::System.Collections.IEnumerator>, global::System.Collections.IEnumerator>(@self.Add<global::System.Collections.IEnumerator>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ISetupList_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task MakeWithExplicitImplementationAsync()
	{
		var code =
			"""
			using Rocks;
			using System;
			using System.Collections.Generic;
			
			public interface ISetup { }

			public interface ISetupList
				: IEnumerable<ISetup> { }

			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Make<ISetupList>();
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			internal static class MakeExpectationsOfISetupListExtensions
			{
				internal static global::ISetupList Instance(this global::Rocks.MakeGeneration<global::ISetupList> @self)
				{
					return new RockISetupList();
				}
				
				private sealed class RockISetupList
					: global::ISetupList
				{
					public RockISetupList()
					{
					}
					
					
					public global::System.Collections.Generic.IEnumerator<global::ISetup> GetEnumerator()
					{
						return default!;
					}
					global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
					{
						return default!;
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "ISetupList_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}