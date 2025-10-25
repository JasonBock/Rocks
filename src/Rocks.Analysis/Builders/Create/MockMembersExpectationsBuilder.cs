using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class MockMembersExpectationsBuilder
{
	internal static void Build(IndentedTextWriter writer, TypeMockModel type, 
		string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		// expectationsSource will either be "this" or "this.parent"

		// First, generate methods, properties and indexers that are not explicit.
		var methods = type.Methods.Where(
			method => method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No);
		MethodExpectationsBuilder.Build(writer, type, methods, expectationsFullyQualifiedName, "this", adornmentsFQNsPipeline);

		var properties = type.Properties.Where(
			method => method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No);
		PropertyExpectationsBuilder.Build(writer, type, properties, expectationsFullyQualifiedName, "this", adornmentsFQNsPipeline);

		// Then, get all of the type names that need explicit implementation.

		// For each explicit implementation type, generate methods, properties, and indexers.



		//MethodExpectationsBuilder.Build(writer, mockType, expectationsFQN, adornmentsPipeline);
		//PropertyExpectationsBuilder.Build(writer, mockType, expectationsFQN, adornmentsPipeline);
	}
}