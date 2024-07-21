using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using Rocks.Descriptors;
using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks;

/// <summary>
/// An analyzer that looks for invalid <see cref="RockCreateAttribute{T}"/>
/// or <see cref="RockMakeAttribute{T}"/> usage.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class RockAnalyzer
	: DiagnosticAnalyzer
{
	/// <summary>
	/// Initializes the analyer.
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
			// TODO: This needs to include the non-generic versions
			// as well as the upcoming new RockAttribute.
			var createAttributeSymbol = compilationContext.Compilation.GetTypeByMetadataName(
				typeof(RockCreateAttribute<>).FullName)!;
			var makeAttributeSymbol = compilationContext.Compilation.GetTypeByMetadataName(
				typeof(RockMakeAttribute<>).FullName)!;

			compilationContext.RegisterOperationAction(operationContext =>
			{
				RockAnalyzer.AnalyzeAttributes(operationContext,
					createAttributeSymbol, makeAttributeSymbol);
			}, OperationKind.Attribute);
		});
	}

	private static void AnalyzeAttributes(OperationAnalysisContext context,
		INamedTypeSymbol createAttributeSymbol, INamedTypeSymbol makeAttributeSymbol)
	{
		if (context.Operation is IAttributeOperation { Operation: IObjectCreationOperation attribute })
		{
			var attributeType = attribute.Constructor?.ContainingType;

			if (attributeType is not null)
			{
				// What's being given to us is the closed generic. That's why
				// OriginalDefinition is used.
				var buildType =
					SymbolEqualityComparer.Default.Equals(createAttributeSymbol, attributeType.OriginalDefinition) ?
						BuildType.Create :
						SymbolEqualityComparer.Default.Equals(makeAttributeSymbol, attributeType.OriginalDefinition) ?
							BuildType.Make : null as BuildType?;

				if (buildType is not null)
				{
					var mockTypeSymbol = attributeType.TypeArguments[0];

					if (mockTypeSymbol is not null)
					{
						var model = MockModel.Create(context.Operation.Syntax,
						  mockTypeSymbol, context.Operation.SemanticModel!, buildType.Value, true);

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
	}

	/// <summary>
	/// Gets an array of supported diagnostics from this analyzer.
	/// </summary>
	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
		[
		   CannotMockObsoleteTypeDescriptor.Create(),
		   CannotMockSealedTypeDescriptor.Create(),
		   CannotMockSpecialTypesDescriptor.Create(),
		   DuplicateConstructorsDescriptor.Create(),
		   InterfaceHasStaticAbstractMembersDescriptor.Create(),
		   MemberUsesObsoleteTypeDescriptor.Create(),
		   TypeHasInaccessibleAbstractMembersDescriptor.Create(),
		   TypeHasMatchWithNonVirtualDescriptor.Create(),
		   TypeHasNoAccessibleConstructorsDescriptor.Create(),
		   TypeHasNoMockableMembersDescriptor.Create(),
		   TypeErrorDescriptor.Create(),
		];
}