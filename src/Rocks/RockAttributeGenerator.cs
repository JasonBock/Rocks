using Microsoft.CodeAnalysis;
using Rocks.Builders.Create;
using Rocks.Builders.Make;
using Rocks.Extensions;
using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks;

[Generator]
internal sealed class RockAttributeGenerator
	: IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var mockCreateTypes = context.SyntaxProvider
			.ForAttributeWithMetadataName("Rocks.RockCreateAttribute`1", (_, _) => true,
				(context, token) =>
				{
					var models = new List<MockModel>(context.Attributes.Length);

					for (var i = 0; i < context.Attributes.Length; i++)
					{
						// Need to grab the type attribute value.
						// Note that I'm assuming there will be at least one generic parameter.
						// If someone creates an attribute to mess with this...
						// well, I guess I could an add a diagnostic that informs users
						// that the attribute was invalid.
						var attributeClass = context.Attributes[i];
						var typeToMock = attributeClass.AttributeClass!.TypeArguments[0];

						if (!typeToMock.ContainsDiagnostics())
						{
							models.Add(MockModel.Create(attributeClass.ApplicationSyntaxReference!.GetSyntax(token),
								typeToMock, context.SemanticModel, BuildType.Create, true));
						}
					}

					return models;
				})
			.SelectMany((names, _) => names);
		var mockMakeTypes = context.SyntaxProvider
			.ForAttributeWithMetadataName("Rocks.RockMakeAttribute`1", (_, _) => true,
				(context, token) =>
				{
					var models = new List<MockModel>(context.Attributes.Length);

					for (var i = 0; i < context.Attributes.Length; i++)
					{
						// Need to grab the type attribute value.
						// Note that I'm assuming there will be at least one generic parameter.
						// If someone creates an attribute to mess with this...
						// well, I guess I could an add a diagnostic that informs users
						// that the attribute was invalid.
						var attributeClass = context.Attributes[i];
						var typeToMock = attributeClass.AttributeClass!.TypeArguments[0];

						if (!typeToMock.ContainsDiagnostics())
						{
							models.Add(MockModel.Create(attributeClass.ApplicationSyntaxReference!.GetSyntax(token),
								typeToMock, context.SemanticModel, BuildType.Make, true));
						}
					}

					return models;
				})
			.SelectMany((names, _) => names);

		context.RegisterSourceOutput(mockCreateTypes.Collect(),
			(context, source) => RockAttributeGenerator.CreateOutput(source, context));
		context.RegisterSourceOutput(mockMakeTypes.Collect(),
			(context, source) => RockAttributeGenerator.CreateOutput(source, context));
	}

	private static void CreateOutput(ImmutableArray<MockModel> mocks, SourceProductionContext context)
	{
		foreach (var mock in mocks.Distinct())
		{
			foreach (var diagnostic in mock.Diagnostics)
			{
				context.ReportDiagnostic(diagnostic);
			}

			if (mock.Type is not null)
			{
				if (mock.BuildType == BuildType.Create)
				{
					var builder = new RockCreateBuilder(mock.Type);
					context.AddSource(builder.Name, builder.Text);
				}
				else if (mock.BuildType == BuildType.Make)
				{
					var builder = new RockMakeBuilder(mock.Type);
					context.AddSource(builder.Name, builder.Text);
				}
			}
		}
	}
}