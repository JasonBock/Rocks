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
		static IncrementalValuesProvider<MockModelInformation> GetInformation(
			IncrementalGeneratorInitializationContext context, string attributeName, BuildType buildType, bool isGeneric) =>
			context.SyntaxProvider
				.ForAttributeWithMetadataName(attributeName, (_, _) => true,
					(context, token) =>
					{
						var models = new List<MockModelInformation>(context.Attributes.Length);

						for (var i = 0; i < context.Attributes.Length; i++)
						{
							// Need to grab the type attribute value.
							// Note that I'm assuming there will be at least one generic parameter.
							// If someone creates an attribute to mess with this...
							// well, I guess I could an add a diagnostic that informs users
							// that the attribute was invalid.
							var attributeClass = context.Attributes[i];
							var typeToMock = isGeneric ?
								attributeClass.AttributeClass!.TypeArguments[0] :
								(attributeClass.ConstructorArguments[0].Value as ITypeSymbol)!;

							// We should only see an unbound generic type with the "typeof(...)" case,
							// but using "typeof(IService)" won't give an unbound generic, so
							// we'll check it in all cases.
							if (typeToMock is INamedTypeSymbol namedType &&
								namedType.IsUnboundGenericType)
							{
								typeToMock = typeToMock.OriginalDefinition;
							}

							if (!typeToMock.ContainsDiagnostics())
							{
								var model = MockModel.Create(attributeClass.ApplicationSyntaxReference!.GetSyntax(token),
								  typeToMock, context.SemanticModel, buildType, true);

								if (model.Information is not null)
								{
									models.Add(model.Information);
								}
							}
						}

						return models;
					})
				.SelectMany((names, _) => names);

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

		var mockCreateFromGenericTypes = GetInformation(context, "Rocks.RockCreateAttribute`1", BuildType.Create, true);
		var mockMakeFromGenericTypes = GetInformation(context, "Rocks.RockMakeAttribute`1", BuildType.Make, true);
		var mockCreateFromNonGenericTypes = GetInformation(context, "Rocks.RockCreateAttribute", BuildType.Create, false);
		var mockMakeFromNonGenericTypes = GetInformation(context, "Rocks.RockMakeAttribute", BuildType.Make, false);

		var mockTypes = GetMockInformation(context);

		// TODO: Is there some way I could combine all of these so I make one CreateOutput() call?
		context.RegisterSourceOutput(mockCreateFromGenericTypes.Collect(),
			(context, source) => RockGenerator.CreateOutput(source, context));
		context.RegisterSourceOutput(mockMakeFromGenericTypes.Collect(),
			(context, source) => RockGenerator.CreateOutput(source, context));
		context.RegisterSourceOutput(mockCreateFromNonGenericTypes.Collect(),
			(context, source) => RockGenerator.CreateOutput(source, context));
		context.RegisterSourceOutput(mockMakeFromNonGenericTypes.Collect(),
			(context, source) => RockGenerator.CreateOutput(source, context));

		context.RegisterSourceOutput(mockTypes.Collect(),
			(context, source) => RockGenerator.CreateOutput(source, context));
	}

	private static void CreateOutput(ImmutableArray<MockModelInformation> mocks, SourceProductionContext context)
	{
		foreach (var mock in mocks.Distinct())
		{
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
	}
}