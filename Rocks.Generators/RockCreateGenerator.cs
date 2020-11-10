using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Rocks.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks
{
	[Generator]
	public sealed class RockCreateGenerator
		: ISourceGenerator
	{
		private static (ImmutableArray<Diagnostic> diagnostics, string? name, SourceText? text) GenerateMapping(
			ITypeSymbol typeToMock, IAssemblySymbol containingAssemblySymbol, SemanticModel model, Compilation compilation)
		{
			var information = new MockInformation(typeToMock, containingAssemblySymbol, model, compilation);

			if (!information.Diagnostics.Any(_ => _.Severity == DiagnosticSeverity.Error))
			{
				var builder = new RockCreateBuilder(information);
				return (builder.Diagnostics, builder.Name, builder.Text);
			}
			else
			{
				return (information.Diagnostics, null, null);
			}
		}

		public void Execute(GeneratorExecutionContext context)
		{
			if (context.SyntaxReceiver is RockCreateReceiver receiver)
			{
				var compilation = context.Compilation;
				var typesToMock = new HashSet<ITypeSymbol>();

				foreach (var candidateInvocation in receiver.Candidates)
				{
					context.CancellationToken.ThrowIfCancellationRequested();
					var model = compilation.GetSemanticModel(candidateInvocation.SyntaxTree);
					var invocationSymbol = (IMethodSymbol)model.GetSymbolInfo(candidateInvocation).Symbol!;

					var rockCreateSymbol = compilation.GetTypeByMetadataName(typeof(Rock).FullName)!
						.GetMembers().Single(_ => _.Name == nameof(Rock.Create));

					if (rockCreateSymbol.Equals(invocationSymbol.ConstructedFrom, SymbolEqualityComparer.Default))
					{
						var typeToMock = invocationSymbol.TypeArguments[0];

						if(typesToMock.Add(typeToMock))
						{
							var containingCandidateType = candidateInvocation.FindParent<TypeDeclarationSyntax>();
							var containingAssemblyOfInvocationSymbol = (model.GetDeclaredSymbol(containingCandidateType)!).ContainingAssembly;

							var (diagnostics, name, text) = RockCreateGenerator.GenerateMapping(
								typeToMock, containingAssemblyOfInvocationSymbol, model, compilation);

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
		}

		public void Initialize(GeneratorInitializationContext context) => context.RegisterForSyntaxNotifications(() => new RockCreateReceiver());
	}
}