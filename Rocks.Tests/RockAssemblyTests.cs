using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Rocks.Exceptions;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class RockAssemblyTests
	{
		[Test]
		public void CreateMocksFromGeneratedAssembly()
		{
			var assembly = new RockAssembly(RockAssemblyTests.Create(),
				new Options(CodeFileOptions.Create));
		}

		private static Assembly Create()
		{
			var trees = new[] { RockAssemblyTests.Class1, RockAssemblyTests.Interface1, RockAssemblyTests.Class3 }
				.Select(_ => SyntaxFactory.ParseSyntaxTree(_));
			var compilation = CSharpCompilation.Create(nameof(RockAssemblyTests),
				options: new CSharpCompilationOptions(
					OutputKind.DynamicallyLinkedLibrary),
				syntaxTrees: trees,
				references: new[]
				{
					MetadataReference.CreateFromAssembly(typeof(object).Assembly),
				});

			var fileName = $"{nameof(RockAssemblyTests)}.dll";

         using (var assemblyStream = new FileStream(fileName, FileMode.Create))
			{
				var results = compilation.Emit(assemblyStream);

				if (!results.Success)
				{
					throw new CompilationException(results.Diagnostics);
				}
			}

			return Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), fileName));
		}

		public const string Class1 =
@"using System;

namespace TestAssembly
{
	public class Class1
	{
		public virtual void Method1() { }
		public virtual string Method2(int a) { return default(string); }
		public virtual Guid Method3 (string a, int b) { return default(Guid); }
	}
}";

		public const string Interface1 =
@"using System;

namespace TestAssembly.Contracts
{
	public interface Interface1<T>
	{
		void Method1();
		string Method2(T a);
		Guid Method3(string a, int b);
	}
}";

		public const string Class3 =
@"using System;

namespace TestAssembly.Extensions
{
	public static class Class3
	{
		public static void Method1(this string @this) { }
	}
}";
   }
}
