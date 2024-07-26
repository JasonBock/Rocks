﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using Rocks.Descriptors;
using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks;

/// <summary>
/// An analyzer that looks for invalid <see cref="RockAttribute"/> usage.
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
				typeof(RockAttribute).FullName)!;

			compilationContext.RegisterOperationAction(operationContext =>
			{
				RockAnalyzer.AnalyzeAttribute(
					operationContext, rockAttributeSymbol);
			}, OperationKind.Attribute);
		});
	}

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
					  mockTypeSymbol, context.Operation.SemanticModel!, BuildType.Create, true);

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
					  mockTypeSymbol, context.Operation.SemanticModel!, BuildType.Make, true);

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