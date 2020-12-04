using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Rocks.Configuration;
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
			ITypeSymbol typeToMock, IAssemblySymbol containingAssemblySymbol, SemanticModel model, 
			ConfigurationValues configurationValues)
		{
			var information = new MockInformation(typeToMock, containingAssemblySymbol, model, configurationValues);

			if (!information.Diagnostics.Any(_ => _.Severity == DiagnosticSeverity.Error))
			{
				var builder = new RockCreateBuilder(information, configurationValues);
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
				var typesToMock = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);

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
							var configurationValues = new ConfigurationValues(context, candidateInvocation.SyntaxTree);
							var (diagnostics, name, text) = RockCreateGenerator.GenerateMapping(
								typeToMock, compilation.Assembly, model, configurationValues);

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

		public void Initialize(GeneratorInitializationContext context) => 
			context.RegisterForSyntaxNotifications(() => new RockCreateReceiver());
	}
}