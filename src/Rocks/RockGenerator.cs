using Microsoft.CodeAnalysis;
using Rocks.Builders.Create;
using Rocks.Builders.Make;
using Rocks.Extensions;
using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks;

[Generator]
internal sealed class RockGenerator
	: IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var mockTypes = context.SyntaxProvider
			.ForAttributeWithMetadataName("Rocks.RockAttribute`1", (_, _) => true,
				(context, token) =>
				{
					var models = new List<MockModelV4>(context.Attributes.Length);

					for (var i = 0; i < context.Attributes.Length; i++)
					{
						// Need to grab the BuildType and type attribute value
						// Note that I'm assuming there will be at least one generic parameter
						// as well as a constructor that takes one BuildType argument.
						// If someone creates an attribute to mess with this...
						// well, I guess I could an add a diagnostic that informs users
						// that the attribute was invalid.
						var attributeClass = context.Attributes[i];
						var typeToMock = attributeClass.AttributeClass!.TypeArguments[0];
						var buildType = (BuildType)attributeClass.ConstructorArguments[0].Value!;

						if (!typeToMock.ContainsDiagnostics())
						{
							models.Add(MockModelV4.Create(attributeClass.ApplicationSyntaxReference!.GetSyntax(token),
								typeToMock, context.SemanticModel, buildType, true));
						}
					}

					return models;
				})
			.SelectMany((names, _) => names);

		context.RegisterSourceOutput(mockTypes.Collect(),
			(context, source) => RockGenerator.CreateOutput(source, context));
	}

	private static void CreateOutput(ImmutableArray<MockModelV4> mocks, SourceProductionContext context)
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
					var builder = new RockCreateBuilderV4(mock.Type);
					context.AddSource(builder.Name, builder.Text);
				}
				else if (mock.BuildType == BuildType.Make)
				{
					var builder = new RockMakeBuilderV4(mock.Type);
					context.AddSource(builder.Name, builder.Text);
				}
			}
		}
	}
}