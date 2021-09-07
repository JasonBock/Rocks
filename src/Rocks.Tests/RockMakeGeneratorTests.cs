using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;
using Rocks.Diagnostics;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rocks.Tests
{
	public static class RockMakeGeneratorTests
	{
		[Test]
		public static async Task GenerateWhenValueTaskOfTIsReturnedAsync()
		{
			var code =
@"using Rocks;
using System;
using System.Threading.Tasks;

namespace MockTests
{
	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<ITest>();
		}
	}

	public interface ITest
	{
		ValueTask<T> Foo<T>();
	}
}";

			var generatedCode =
@"using MockTests;
using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

#nullable enable
namespace MockTests
{
	internal static class MakeExpectationsOfITestExtensions
	{
		internal static ITest Instance(this MakeGeneration<ITest> self) =>
			new RockITest();
		
		private sealed class RockITest
			: ITest
		{
			public RockITest() { }
			
			public ValueTask<T> Foo<T>()
			{
				return new ValueTask<T>(default(T)!);
			}
		}
	}
}
";

			await TestAssistants.RunAsync<RockMakeGenerator>(code,
				new[] { (typeof(RockMakeGenerator), "ITest_Rock_Make.g.cs", generatedCode) },
				Enumerable.Empty<DiagnosticResult>());
		}

		[Test]
		public static async Task GenerateWhenTargetTypeIsValidAsync()
		{
			var code =
@"using Rocks;
using System;
using System.Threading.Tasks;

namespace MockTests
{
	public interface ITest
	{
		unsafe int* Foo(int* value);
		unsafe delegate*<int, void> FooMethod(delegate*<int, void> value);
		unsafe int* Data { get; set; }
		unsafe delegate*<int, void> DataMethod { get; set; }
		unsafe int* this[int* a] { get; set; }
		unsafe delegate*<int, void> this[delegate*<int, void> a] { get; set; }
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<ITest>();
		}
	}
}";

			var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

#nullable enable
namespace MockTests
{
	internal static class MakeExpectationsOfITestExtensions
	{
		internal static ITest Instance(this MakeGeneration<ITest> self) =>
			new RockITest();
		
		private sealed class RockITest
			: ITest
		{
			public RockITest() { }
			
			public unsafe int* Foo(int* value)
			{
				return default!;
			}
			public unsafe delegate*<int, void> FooMethod(delegate*<int, void> value)
			{
				return default!;
			}
			public unsafe int* Data
			{
				get => default!;
				set { }
			}
			public unsafe delegate*<int, void> DataMethod
			{
				get => default!;
				set { }
			}
			public unsafe int* this[int* a]
			{
				get => default!;
				set { }
			}
			public unsafe delegate*<int, void> this[delegate*<int, void> a]
			{
				get => default!;
				set { }
			}
		}
	}
}
";

			await TestAssistants.RunAsync<RockMakeGenerator>(code,
				new[] { (typeof(RockMakeGenerator), "ITest_Rock_Make.g.cs", generatedCode) },
				Enumerable.Empty<DiagnosticResult>());
		}

		[Test]
		public static async Task GenerateWhenInvocationExistsInTopLevelStatementsAsync()
		{
			var code =
@"using MockTests;
using Rocks;
using System;

var rock = Rock.Make<ITest>();

namespace MockTests
{
	public interface ITest
	{
		void Foo();
	}
}";

			var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

#nullable enable
namespace MockTests
{
	internal static class MakeExpectationsOfITestExtensions
	{
		internal static ITest Instance(this MakeGeneration<ITest> self) =>
			new RockITest();
		
		private sealed class RockITest
			: ITest
		{
			public RockITest() { }
			
			public void Foo()
			{
			}
		}
	}
}
";

			await TestAssistants.RunAsync<RockMakeGenerator>(code,
				new[] { (typeof(RockMakeGenerator), "ITest_Rock_Make.g.cs", generatedCode) },
				Enumerable.Empty<DiagnosticResult>(), OutputKind.ConsoleApplication);
		}

		[Test]
		public static async Task GenerateWhenTargetTypeIsInvalidAsync()
		{
			var code =
@"using Rocks;

namespace MockTests
{
	public sealed class InvalidTarget { }

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<InvalidTarget>();
		}
	}
}";

			var diagnostic = new DiagnosticResult(CannotMockSealedTypeDiagnostic.Id, DiagnosticSeverity.Error)
				.WithSpan(5, 22, 5, 35);
			await TestAssistants.RunAsync<RockMakeGenerator>(code,
				Enumerable.Empty<(Type, string, string)>(),
				new[] { diagnostic });
		}

		[Test]
		public static async Task GenerateWhenTargetTypeHasDiagnosticsAsync()
		{
			var code =
@"using Rocks;

namespace MockTests
{
	public interface ITest 
	{ 
		// Note the missing semicolon
		void Foo()
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<ITest>();
		}
	}
}";

			var diagnostic = new DiagnosticResult("CS1002", DiagnosticSeverity.Error)
				.WithSpan(8, 13, 8, 13);
			await TestAssistants.RunAsync<RockMakeGenerator>(code,
				Enumerable.Empty<(Type, string, string)>(),
				new[] { diagnostic });
		}

		[Test]
		public static async Task GenerateWhenTargetTypeIsValidButOtherCodeHasDiagnosticsAsync()
		{
			var code =
@"using Rocks;

namespace MockTests
{
	public interface ITest 
	{ 
		void Foo();
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<ITest>();
		}
// Note the missing closing brace
	}";

			var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

#nullable enable
namespace MockTests
{
	internal static class MakeExpectationsOfITestExtensions
	{
		internal static ITest Instance(this MakeGeneration<ITest> self) =>
			new RockITest();
		
		private sealed class RockITest
			: ITest
		{
			public RockITest() { }
			
			public void Foo()
			{
			}
		}
	}
}
";

			var diagnostic = new DiagnosticResult("CS1513", DiagnosticSeverity.Error)
				.WithSpan(17, 3, 17, 3);
			await TestAssistants.RunAsync<RockMakeGenerator>(code,
				new[] { (typeof(RockMakeGenerator), "ITest_Rock_Make.g.cs", generatedCode) },
				new[] { diagnostic });
		}
	}
}