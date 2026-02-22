using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Rocks.Analysis.Descriptors;
using Rocks.Analysis.Extensions;
using System.Collections.Immutable;

namespace Rocks.Analysis;

/// <summary>
/// A suppressor that will suppress CA2000 when
/// an <see cref="IDisposable" />-based reference is returned from an <c>Instance(...)</c> call
/// on an <c>Expectations</c>-based instance.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DisposableInstancesFromExpectationsSuppressor
	: DiagnosticSuppressor
{
	private static readonly SuppressionDescriptor descriptor =
		DisposableInstancesFromExpectationsDescriptor.Create();

	/// <summary>
	/// Gets an <see cref="ImmutableArray{SuppressionDescriptor}"/> instance
	/// containing all suppressed diagnostic IDs.
	/// </summary>
	public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions =>
		[descriptor];

	/// <summary>
	/// Reports targeted CA2000 suppressions
	/// </summary>
	/// <param name="context">A <see cref="SuppressionAnalysisContext"/> instance.</param>
	public override void ReportSuppressions(SuppressionAnalysisContext context)
	{
		foreach (var diagnostic in context.ReportedDiagnostics.Where(
			_ => _.Id == DisposableInstancesFromExpectationsDescriptor.SuppressedId))
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

					if (node is InvocationExpressionSyntax invocation)
					{
						var semanticModel = context.GetSemanticModel(syntaxTree);
						var invocationMethod = semanticModel.GetSymbolInfo(
							node, context.CancellationToken).Symbol as IMethodSymbol;

						if (invocationMethod is not null && invocationMethod.Name == "Instance" &&
							context.Compilation.GetTypeByMetadataName("Rocks.Expectations") is ITypeSymbol expectationsType &&
							invocationMethod.ContainingType.DerivesFrom(expectationsType))
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