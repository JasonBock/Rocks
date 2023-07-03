using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ExplicitImplementationGeneratorTests
{
	[Test]
	public static async Task CreateWithDifferenceInReturnTypeAsync()
	{
		var code =
			"""
			using Rocks;
			
			public interface IIterator
			{
				void Iterate();
			}

			public interface IIterator<out T>
				: IIterator
			{
				new T Iterate();
			}
			
			public interface IIterable
			{
				IIterator GetIterator();
			}

			public interface IIterable<out T>
				: IIterable
			{
				new IIterator<T> GetIterator();
			}

			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Create<IIterable<string>>();
				}
			}
			""";

		var generatedCode =
			"""
			using Rocks.Extensions;
			using System.Collections.Generic;
			using System.Collections.Immutable;
			#nullable enable
			
			internal static class CreateExpectationsOfIIterableOfstringExtensions
			{
				internal static global::Rocks.Expectations.MethodExpectations<global::IIterable<string>> Methods(this global::Rocks.Expectations.Expectations<global::IIterable<string>> @self) =>
					new(@self);
				
				internal static global::Rocks.Expectations.ExplicitMethodExpectations<global::IIterable<string>, global::IIterable> ExplicitMethodsForIIterable(this global::Rocks.Expectations.Expectations<global::IIterable<string>> @self) =>
					new(@self);
				
				internal static global::IIterable<string> Instance(this global::Rocks.Expectations.Expectations<global::IIterable<string>> @self)
				{
					if (!@self.WasInstanceInvoked)
					{
						@self.WasInstanceInvoked = true;
						var @mock = new RockIIterableOfstring(@self);
						@self.MockType = @mock.GetType();
						return @mock;
					}
					else
					{
						throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
					}
				}
				
				private sealed class RockIIterableOfstring
					: global::IIterable<string>
				{
					private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
					
					public RockIIterableOfstring(global::Rocks.Expectations.Expectations<global::IIterable<string>> @expectations)
					{
						this.handlers = @expectations.Handlers;
					}
					
					[global::Rocks.MemberIdentifier(0, "global::IIterator<string> GetIterator()")]
					public global::IIterator<string> GetIterator()
					{
						if (this.handlers.TryGetValue(0, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							@methodHandler.IncrementCallCount();
							var @result = @methodHandler.Method is not null ?
								((global::System.Func<global::IIterator<string>>)@methodHandler.Method)() :
								((global::Rocks.HandlerInformation<global::IIterator<string>>)@methodHandler).ReturnValue;
							return @result!;
						}
						
						throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::IIterator<string> GetIterator()");
					}
					
					[global::Rocks.MemberIdentifier(1, "global::IIterator global::IIterable.GetIterator()")]
					global::IIterator global::IIterable.GetIterator()
					{
						if (this.handlers.TryGetValue(1, out var @methodHandlers))
						{
							var @methodHandler = @methodHandlers[0];
							@methodHandler.IncrementCallCount();
							var @result = @methodHandler.Method is not null ?
								((global::System.Func<global::IIterator>)@methodHandler.Method)() :
								((global::Rocks.HandlerInformation<global::IIterator>)@methodHandler).ReturnValue;
							return @result!;
						}
						
						throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for global::IIterator global::IIterable.GetIterator()");
					}
					
				}
			}
			
			internal static class MethodExpectationsOfIIterableOfstringExtensions
			{
				internal static global::Rocks.MethodAdornments<global::IIterable<string>, global::System.Func<global::IIterator<string>>, global::IIterator<string>> GetIterator(this global::Rocks.Expectations.MethodExpectations<global::IIterable<string>> @self) =>
					new global::Rocks.MethodAdornments<global::IIterable<string>, global::System.Func<global::IIterator<string>>, global::IIterator<string>>(@self.Add<global::IIterator<string>>(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			internal static class ExplicitMethodExpectationsOfIIterableOfstringForIIterableExtensions
			{
				internal static global::Rocks.MethodAdornments<global::IIterable<string>, global::System.Func<global::IIterator>, global::IIterator> GetIterator(this global::Rocks.Expectations.ExplicitMethodExpectations<global::IIterable<string>, global::IIterable> @self) =>
					new global::Rocks.MethodAdornments<global::IIterable<string>, global::System.Func<global::IIterator>, global::IIterator>(@self.Add<global::IIterator>(1, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			}
			
			""";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "IIterableOfstring_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

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
						var @mock = new RockISetupList(@self);
						@self.MockType = @mock.GetType();
						return @mock;
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
							@methodHandler.IncrementCallCount();
							var @result = @methodHandler.Method is not null ?
								((global::System.Func<global::System.Collections.Generic.IEnumerator<global::ISetup>>)@methodHandler.Method)() :
								((global::Rocks.HandlerInformation<global::System.Collections.Generic.IEnumerator<global::ISetup>>)@methodHandler).ReturnValue;
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
							@methodHandler.IncrementCallCount();
							var @result = @methodHandler.Method is not null ?
								((global::System.Func<global::System.Collections.IEnumerator>)@methodHandler.Method)() :
								((global::Rocks.HandlerInformation<global::System.Collections.IEnumerator>)@methodHandler).ReturnValue;
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

	[Test]
	public static async Task MakeWithDifferenceInReturnTypeAsync()
	{
		var code =
			"""
			using Rocks;
			
			public interface IIterator
			{
				void Iterate();
			}

			public interface IIterator<out T>
				: IIterator
			{
				new T Iterate();
			}
			
			public interface IIterable
			{
				IIterator GetIterator();
			}

			public interface IIterable<out T>
				: IIterable
			{
				new IIterator<T> GetIterator();
			}

			public static class Test
			{
				public static void Go()
				{
					var expectations = Rock.Make<IIterable<string>>();
				}
			}
			""";

		var generatedCode =
			"""
			#nullable enable
			
			internal static class MakeExpectationsOfIIterableOfstringExtensions
			{
				internal static global::IIterable<string> Instance(this global::Rocks.MakeGeneration<global::IIterable<string>> @self)
				{
					return new RockIIterableOfstring();
				}
				
				private sealed class RockIIterableOfstring
					: global::IIterable<string>
				{
					public RockIIterableOfstring()
					{
					}
					
					public global::IIterator<string> GetIterator()
					{
						return default!;
					}
					global::IIterator global::IIterable.GetIterator()
					{
						return default!;
					}
				}
			}
			
			""";

		await TestAssistants.RunAsync<RockMakeGenerator>(code,
			new[] { (typeof(RockMakeGenerator), "IIterableOfstring_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}