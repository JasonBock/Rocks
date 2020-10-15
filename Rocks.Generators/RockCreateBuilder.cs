using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace Rocks
{
	internal sealed class RockCreateBuilder
	{
		private readonly MockInformation information;

		internal RockCreateBuilder(MockInformation information)
		{
			this.information = information;
			(this.Diagnostics, this.Name, this.Text) = this.Build();
		}

		private (ImmutableArray<Diagnostic>, string, SourceText) Build()
		{
			/* TODO:
			
			* Can we read .editorconfig to figure out the space/tab + indention

using ...;

namespace {typeToMock.Namespace}
{
	internal static class ExpectationsFor{typeToMock.Name}
	{
		// If methods...
		internal static MethodExpectations<{typeToMock.Name}> Methods(this Expectations<{typeToMock.Name}> self) =>
			new MethodExpectations<{typeToMock.Name}>(self);

		// If properties...

		// If events ...

		// If constructors...
		internal static {typeToMock.Name} Instance(this Expectations<{typeToMock.Name}> self, ...) =>
			new Rock{typeToMock.Name}(

		private sealed class Rock{typeToMock.Name}
			: {typeToMock.Name}, IMock
		{
			private readonly Expectations<{typeToMock.Name}> expectations;

			// Implement constructors...
			public {typeToMock.Name}(Expectations<{typeToMock.Name}> expectations, ...)
			{
				this.expectations = expectations;
				...
			}

			// Implement members...
		}
	}

	// If methods...
	internal static class MethodExpectationsFor{typeToMock.Name}
	{
		internal static void Foo(this MethodExpectations<{typeToMock.Name}> self, Arg<int> a) =>
			self.AddExpectations(...);
	}
}

			var x = Rock.Create<IA>();
			x.Methods().Foo(3);
			x.Properties().
			x.Events().

			var rock = x.Instance(...);
			rock.Foo(3);

			*/

			var text = SourceText.From(
$@"public static class ExpectationsOf{this.information.TypeToMock.Name}Extensions
{{
}}", Encoding.UTF8);
			return (this.information.Diagnostics, $"{this.information.TypeToMock.Name}_Mock.g.cs", text);
		}

		public ImmutableArray<Diagnostic> Diagnostics { get; private set; }
		public string Name { get; private set; }
		public SourceText Text { get; private set; }
	}
}