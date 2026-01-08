using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Rocks.Analysis.Descriptors;
using Rocks.Analysis.Extensions;
using System.Collections.Immutable;

namespace Rocks.Analysis;

/// <summary>
/// A suppressor that will suppress CA2012 when
/// <see cref="ValueTask"/> or <see cref="ValueTask{TResult}"/> is passed to 
/// <c>IAdornments{TAdornments, THandler, TCallback, TReturnValue}.ReturnValue(TReturnValue)</c>/>.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ValueTaskInReturnValueSuppressor
	: DiagnosticSuppressor
{
	private static readonly SuppressionDescriptor descriptor =
		ValueTaskInReturnValueDescriptor.Create();

	/// <summary>
	/// Gets an <see cref="ImmutableArray{SuppressionDescriptor}"/> instance
	/// containing all suppressed diagnostic IDs.
	/// </summary>
	public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions =>
		[descriptor];

	/// <summary>
	/// Reports targeted CA2012 suppressions
	/// </summary>
	/// <param name="context">A <see cref="SuppressionAnalysisContext"/> instance.</param>
	public override void ReportSuppressions(SuppressionAnalysisContext context)
	{
		foreach (var diagnostic in context.ReportedDiagnostics.Where(
			_ => _.Id == ValueTaskInReturnValueDescriptor.SuppressedId))
		{
			var location = diagnostic.Location;

			if (location != Location.None && location.IsInSource)
			{
				var syntaxTree = location.SourceTree;

				if (syntaxTree is not null)
				{
					var root = syntaxTree.GetRoot(context.CancellationToken);
					var textSpan = location.SourceSpan;
					var node = root.FindNode(textSpan);

					if (node is ArgumentSyntax argument)
					{
						var invocationExpressionNode = node.FindParent<InvocationExpressionSyntax>();

						if (invocationExpressionNode is not null)
						{
							var semanticModel = context.GetSemanticModel(syntaxTree);
							var invocationMethod = semanticModel.GetSymbolInfo(
								invocationExpressionNode, context.CancellationToken).Symbol as IMethodSymbol;

							if (invocationMethod is not null &&
								invocationMethod.Name == "ReturnValue" &&
								SymbolEqualityComparer.Default.Equals(
									invocationMethod.ContainingType.ConstructedFrom,
									context.Compilation.GetTypeByMetadataName("Rocks.Adornments`4")))
							{
								var constructedParameterType = (invocationMethod.Parameters[0].Type as INamedTypeSymbol)?.ConstructedFrom;

								if (constructedParameterType is not null &&
									(SymbolEqualityComparer.Default.Equals(
										constructedParameterType,
										context.Compilation.GetTypeByMetadataName("System.Threading.Tasks.ValueTask")) ||
									SymbolEqualityComparer.Default.Equals(
										constructedParameterType,
										context.Compilation.GetTypeByMetadataName("System.Threading.Tasks.ValueTask`1"))))
								{
									var suppression = Suppression.Create(
										descriptor, diagnostic);
									context.ReportSuppression(suppression);
								}
							}
						}
					}
				}
			}
		}
	}
}