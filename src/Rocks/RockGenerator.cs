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
		static IncrementalValuesProvider<MockModelInformation> GetMockInformation(
			IncrementalGeneratorInitializationContext context) =>
			context.SyntaxProvider
				.ForAttributeWithMetadataName("Rocks.RockAttribute", (_, _) => true,
					(context, token) =>
					{
						var models = new List<MockModelInformation>(context.Attributes.Length);

						for (var i = 0; i < context.Attributes.Length; i++)
						{
							// Need to grab the type attribute values.
							var attributeClass = context.Attributes[i];
							var mockType = (attributeClass.ConstructorArguments[0].Value as ITypeSymbol)!;
							var buildType = (BuildType)attributeClass.ConstructorArguments[1].Value!;

							// We should only see an unbound generic type with the "typeof(...)" case,
							// but using "typeof(IService)" won't give an unbound generic, so
							// we'll check it in all cases.
							if (mockType is INamedTypeSymbol namedType &&
								namedType.IsUnboundGenericType)
							{
								mockType = mockType.OriginalDefinition;
							}

							if (!mockType.ContainsDiagnostics())
							{
								if (buildType.HasFlag(BuildType.Create))
								{
									var model = MockModel.Create(attributeClass.ApplicationSyntaxReference!.GetSyntax(token),
									  mockType, context.SemanticModel, BuildType.Create, true);

									if (model.Information is not null)
									{
										models.Add(model.Information);
									}
								}

								if (buildType.HasFlag(BuildType.Make))
								{
									var model = MockModel.Create(attributeClass.ApplicationSyntaxReference!.GetSyntax(token),
									  mockType, context.SemanticModel, BuildType.Make, true);

									if (model.Information is not null)
									{
										models.Add(model.Information);
									}
								}
							}
						}

						return models;
					})
				.SelectMany((names, _) => names);

		context.RegisterTypes();

		var mockTypes = GetMockInformation(context);

		context.RegisterSourceOutput(mockTypes.Collect(),
			(context, source) => RockGenerator.CreateOutput(source, context));
	}

	private static void CreateOutput(ImmutableArray<MockModelInformation> mocks, SourceProductionContext context)
	{
		var projections = new HashSet<TypeReferenceModel>();

		foreach (var mock in mocks.Distinct())
		{
			foreach (var projection in mock.Type.Projections)
			{
				projections.Add(projection);
			}

			if (mock.BuildType.HasFlag(BuildType.Create))
			{
				var builder = new RockCreateBuilder(mock.Type);
				context.AddSource(builder.Name, builder.Text);
			}

			if (mock.BuildType.HasFlag(BuildType.Make))
			{
				var builder = new RockMakeBuilder(mock.Type);
				context.AddSource(builder.Name, builder.Text);
			}
		}

		if (projections.Count > 0)
		{
			var projectionFileNames = new HashSet<string>();

			foreach (var projection in projections)
			{
				var builder = new RockProjectionBuilder(projection);

				if (projectionFileNames.Add(builder.Name))
				{
					context.AddSource(builder.Name, builder.Text);
				}
			}
		}
	}
}