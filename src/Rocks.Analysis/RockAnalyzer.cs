using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using Rocks.Analysis.Descriptors;
using Rocks.Analysis.Models;
using System.Collections.Immutable;

namespace Rocks.Analysis;

/// <summary>
/// An analyzer that looks for invalid <c>RockAttribute</c> usage.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class RockAnalyzer
	: DiagnosticAnalyzer
{
	/// <summary>
	/// Initializes the analyzer.
	/// </summary>
	/// <param name="context">An <see cref="AnalysisContext"/> instance.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is <see langword="null"/>.</exception>
	public override void Initialize(AnalysisContext context)
	{
		if (context is null)
		{
			throw new ArgumentNullException(nameof(context));
		}

		context.ConfigureGeneratedCodeAnalysis(
			GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
		context.EnableConcurrentExecution();

		context.RegisterCompilationStartAction(compilationContext =>
		{
			var rockAttributeSymbol = compilationContext.Compilation.GetTypeByMetadataName(
				Constants.RockAttributeName)!;
			var rockPartialAttributeSymbol = compilationContext.Compilation.GetTypeByMetadataName(
				Constants.RockPartialAttributeName)!;

			compilationContext.RegisterOperationAction(operationContext =>
			{
				RockAnalyzer.AnalyzeAttribute(
					operationContext, rockAttributeSymbol);
				RockAnalyzer.AnalyzeAttribute(
					operationContext, rockPartialAttributeSymbol);
			}, OperationKind.Attribute);
		});
	}

	// Both [Rock] and [RockPartial] take the same number of constructor arguments,
	// so this works.
	private static void AnalyzeAttribute(
		OperationAnalysisContext context, INamedTypeSymbol rockAttributeSymbol)
	{
		if (context.Operation is IAttributeOperation { Operation: IObjectCreationOperation attribute })
		{
			var attributeType = attribute.Constructor?.ContainingType;

			if (attributeType is not null &&
				SymbolEqualityComparer.Default.Equals(attributeType, rockAttributeSymbol))
			{
				var mockTypeSymbol = (attribute.Arguments[0].Value as ITypeOfOperation)!.TypeOperand.OriginalDefinition;
				var buildType = (BuildType)attribute.Arguments[1].Value!.ConstantValue.Value!;

				if (buildType.HasFlag(BuildType.Create))
				{
					var model = MockModel.Create(context.Operation.Syntax,
					  mockTypeSymbol, null, new ModelContext(context.Operation.SemanticModel!), 
					  BuildType.Create, true);

					if (model.Information is null)
					{
						foreach (var diagnostic in model.Diagnostics)
						{
							context.ReportDiagnostic(diagnostic);
						}
					}
				}

				if (buildType.HasFlag(BuildType.Make))
				{
					var model = MockModel.Create(context.Operation.Syntax,
					  mockTypeSymbol, null, new ModelContext(context.Operation.SemanticModel!), 
					  BuildType.Make, true);

					if (model.Information is null)
					{
						foreach (var diagnostic in model.Diagnostics)
						{
							context.ReportDiagnostic(diagnostic);
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Gets an array of supported diagnostics from this analyzer.
	/// </summary>
	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
		[
			CannotMockObsoleteTypeDescriptor.Create(),
			CannotMockSealedTypeDescriptor.Create(),
			CannotMockSpecialTypesDescriptor.Create(),
			InterfaceHasStaticAbstractMembersDescriptor.Create(),
			MemberUsesObsoleteTypeDescriptor.Create(),
			TypeHasInaccessibleAbstractMembersDescriptor.Create(),
			TypeHasNoAccessibleConstructorsDescriptor.Create(),
			TypeHasNoMockableMembersDescriptor.Create(),
			TypeIsClosedGenericDescriptor.Create(),
		];
}