using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Rocks.Extensions;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks
{
	[Generator]
	public sealed class RockGenerator
		: ISourceGenerator
	{
		private static (ImmutableList<Diagnostic> diagnostics, string? name, SourceText? text) GenerateMapping(ITypeSymbol typeToMock)
		{
			var diagnostics = ImmutableList.CreateBuilder<Diagnostic>();

			// TODO:
			// * Debate adding in IndentedTextWriter (possibly reading in .editorconfig whitespace choice)
			// * Create mock extension methods
			// * Create a "Make" returning a mock type
			// * Actually create the mock itself.
			// * Somehow "verify"

			var text = SourceText.From(
$@"public static class ExpectationsOf{typeToMock.Name}Extensions
{{
}}");
			return (diagnostics.ToImmutableList(), $"{typeToMock.Name}_Mock.g.cs", text);
		}

		public void Execute(SourceGeneratorContext context)
		{
			if (context.SyntaxReceiver is RockReceiver receiver)
			{
				var compilation = context.Compilation;

				foreach (var candidateInvocation in receiver.Candidates)
				{
					var model = compilation.GetSemanticModel(candidateInvocation.SyntaxTree);
					var invocationSymbol = (IMethodSymbol)model.GetSymbolInfo(candidateInvocation).Symbol!;

					var rockCreateSymbol = context.Compilation.GetTypeByMetadataName(typeof(Rock).FullName)!
						.GetMembers().Single(_ => _.Name == nameof(Rock.Create));

					if(rockCreateSymbol.Equals(invocationSymbol.ConstructedFrom, SymbolEqualityComparer.Default))
					{
						var typeToMock = invocationSymbol.TypeArguments[0];
						var containingCandidateType = candidateInvocation.FindParent<TypeDeclarationSyntax>();
						var containingCandidateSymbol = (ITypeSymbol)model.GetDeclaredSymbol(containingCandidateType)!;

						var (diagnostics, name, text) = RockGenerator.GenerateMapping(typeToMock);

						foreach (var diagnostic in diagnostics)
						{
							context.ReportDiagnostic(diagnostic);
						}

						if (name is not null && text is not null)
						{
							context.AddSource(name, text);
						}
					}
				}
			}
		}

		public void Initialize(InitializationContext context) => context.RegisterForSyntaxNotifications(() => new RockReceiver());
	}
}