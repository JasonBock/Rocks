using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Rocks.Analysis.Descriptors;
using Rocks.Analysis.Extensions;
using System.Collections.Immutable;

namespace Rocks.Analysis;

/// <summary>
/// A suppressor that will suppress CA2025 when
/// a <c>Task</c>-like type is passed to 
/// <c>IAdornments{TAdornments, THandler, TCallback, TReturnValue}.ReturnValue(TReturnValue)</c>/>,
/// <c>IAdornments{TAdornments, THandler, TCallback, TReturnValue}.Callback()</c>/>, or
/// <c>IAdornments{TAdornments, THandler, TCallback}.Callback()</c>/>, or
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class IDisposableInstancesIntoTasksSuppressor
	: DiagnosticSuppressor
{
	private static readonly SuppressionDescriptor descriptor =
		IDisposableInstancesIntoTasksDescriptor.Create();

	/// <summary>
	/// Gets an <see cref="ImmutableArray{SuppressionDescriptor}"/> instance
	/// containing all suppressed diagnostic IDs.
	/// </summary>
	public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions =>
		[descriptor];

	/// <summary>
	/// Reports targeted CA2025 suppressions
	/// </summary>
	/// <param name="context">A <see cref="SuppressionAnalysisContext"/> instance.</param>
	public override void ReportSuppressions(SuppressionAnalysisContext context)
	{
		foreach (var diagnostic in context.ReportedDiagnostics.Where(
			_ => _.Id == IDisposableInstancesIntoTasksDescriptor.SuppressedId))
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
						var semanticModel = context.GetSemanticModel(syntaxTree);

						var invocationExpressionNode = node.FindParent<InvocationExpressionSyntax>(
							node =>
							{
								var invocationMethod = semanticModel.GetSymbolInfo(
									node, context.CancellationToken).Symbol as IMethodSymbol;

								return invocationMethod is not null &&
									((invocationMethod.Name == "ReturnValue" &&
										SymbolEqualityComparer.Default.Equals(
											invocationMethod.ContainingType.ConstructedFrom,
											context.Compilation.GetTypeByMetadataName("Rocks.Adornments`4"))) ||
									invocationMethod.Name == "Callback" &&
										SymbolEqualityComparer.Default.Equals(
											invocationMethod.ContainingType.ConstructedFrom,
											context.Compilation.GetTypeByMetadataName("Rocks.Adornments`4")));
							});

						if (invocationExpressionNode is not null)
						{
							var taskInvocationNode = node.FindParent<InvocationExpressionSyntax>(_ => true)!;

							var taskInvocationMethod = semanticModel.GetSymbolInfo(
								taskInvocationNode, context.CancellationToken).Symbol as IMethodSymbol;
							var taskInvocationType = taskInvocationMethod!.ContainingType;

							if (taskInvocationType is not null &&
								(SymbolEqualityComparer.Default.Equals(
									taskInvocationType,
									context.Compilation.GetTypeByMetadataName("System.Threading.Tasks.ValueTask")) ||
								SymbolEqualityComparer.Default.Equals(
									taskInvocationType,
									context.Compilation.GetTypeByMetadataName("System.Threading.Tasks.ValueTask`1")) ||
								SymbolEqualityComparer.Default.Equals(
									taskInvocationType,
									context.Compilation.GetTypeByMetadataName("System.Threading.Tasks.Task")) ||
								SymbolEqualityComparer.Default.Equals(
									taskInvocationType,
									context.Compilation.GetTypeByMetadataName("System.Threading.Tasks.Task`1"))))
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