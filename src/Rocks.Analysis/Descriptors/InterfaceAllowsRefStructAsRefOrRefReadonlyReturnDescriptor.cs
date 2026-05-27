using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class InterfaceAllowsRefStructAsRefOrRefReadonlyReturnDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(InterfaceAllowsRefStructAsRefOrRefReadonlyReturnDescriptor.Id, InterfaceAllowsRefStructAsRefOrRefReadonlyReturnDescriptor.Title,
			InterfaceAllowsRefStructAsRefOrRefReadonlyReturnDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				InterfaceAllowsRefStructAsRefOrRefReadonlyReturnDescriptor.Id, InterfaceAllowsRefStructAsRefOrRefReadonlyReturnDescriptor.Title));

	internal const string Id = "ROCK15";
	internal const string Message = "The type {0} allows ref structs in type parameters that are used as ref or ref readonly return values";
	internal const string Title = "Interface Allows Ref Structs As Ref or Ref Readonly Return Values";
}