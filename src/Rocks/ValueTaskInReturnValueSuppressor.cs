using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Rocks.Descriptors;
using System.Collections.Immutable;

namespace Rocks;

/// <summary>
/// A suppressor that will suppress CA2012 when
/// <see cref="ValueTask"/> or <see cref="ValueTask{TResult}"/> is passed to 
/// <see cref="IAdornments{TAdornments, THandler, TCallback, TReturnValue}.ReturnValue(TReturnValue)"/>.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ValueTaskInReturnValueSuppressor
	: DiagnosticSuppressor
{
	/// <summary>
	/// Gets an <see cref="ImmutableArray{SuppressionDescriptor}"/> instance
	/// containing all suppressed diagnostic IDs.
	/// </summary>
	public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions =>
		[ValueTypeInReturnValueDescriptor.Create()];

	/// <summary>
	/// Reports targeted CA2012 suppressions
	/// </summary>
	/// <param name="context">A <see cref="SuppressionAnalysisContext"/> instance.</param>
	public override void ReportSuppressions(SuppressionAnalysisContext context)
	{
		foreach (var diagnostic in context.ReportedDiagnostics.Where(
			_ => _.Id == ValueTypeInReturnValueDescriptor.SuppressedId))
		{
			var location = diagnostic.Location;
			var syntaxTree = location.SourceTree!;
			var root = syntaxTree.GetRoot(context.CancellationToken);
			var textSpan = location.SourceSpan;
			var node = root.FindNode(textSpan);

			if (node is ArgumentSyntax argument)
			{

			}
		}
	}
}