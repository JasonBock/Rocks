using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class PropertyInitCreateGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public interface ITest
	{
		int Bar { get; init; }
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<ITest>();
		}
	}
}";

		var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using Rocks.Expectations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

#nullable enable
namespace MockTests
{
	internal static class CreateExpectationsOfITestExtensions
	{
		internal static PropertyExpectations<ITest> Properties(this Expectations<ITest> self) =>
			new(self);
		
		internal static PropertyGetterExpectations<ITest> Getters(this PropertyExpectations<ITest> self) =>
			new(self.Expectations);
		
		internal static PropertyInitExpectations<ITest> Initializers(this PropertyExpectations<ITest> self) =>
			new(self.Expectations);
		
		internal static ITest Instance(this Expectations<ITest> self)
		{
			var mock = new RockITest(self);
			self.Mocks.Add(mock);
			return mock;
		}
		
		private sealed class RockITest
			: ITest, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockITest(Expectations<ITest> expectations) =>
				this.handlers = expectations.Handlers;
			
			[MemberIdentifier(0, ""get_Bar()"")]
			[MemberIdentifier(1, ""set_Bar(value)"")]
			public int Bar
			{
				get
				{
					if (this.handlers.TryGetValue(0, out var methodHandlers))
					{
						var methodHandler = methodHandlers[0];
						var result = methodHandler.Method is not null ?
							((Func<int>)methodHandler.Method)() :
							((HandlerInformation<int>)methodHandler).ReturnValue;
						methodHandler.IncrementCallCount();
						return result!;
					}
					
					throw new ExpectationException(""No handlers were found for get_Bar())"");
				}
				init
				{
					if (this.handlers.TryGetValue(1, out var methodHandlers))
					{
						var foundMatch = false;
						foreach (var methodHandler in methodHandlers)
						{
							if ((methodHandler.Expectations[0] as Argument<int>)?.IsValid(value) ?? false)
							{
								foundMatch = true;
								
								if (methodHandler.Method is not null)
								{
									((Action<int>)methodHandler.Method)(value);
								}
								
								if (!foundMatch)
								{
									throw new ExpectationException(""No handlers were found for set_Bar(value)"");
								}
								
								methodHandler.IncrementCallCount();
								break;
							}
						}
					}
					else
					{
						throw new ExpectationException(""No handlers were found for set_Bar(value)"");
					}
				}
			}
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class PropertyGetterExpectationsOfITestExtensions
	{
	}
	internal static class PropertyInitExpectationsOfITestExtensions
	{
		internal static PropertyAdornments<ITest, Action<int>> Bar(this PropertyInitExpectations<ITest> self, Argument<int> value) =>
			new PropertyAdornments<ITest, Action<int>>(self.Add(1, new List<Argument>(1) { value }));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ITest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}