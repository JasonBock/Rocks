using Microsoft.CodeAnalysis;

namespace Rocks.Analysis.Descriptors;

internal static class DisposableInstancesFromExpectationsDescriptor
{
	internal static SuppressionDescriptor Create() =>
		new(
			DisposableInstancesFromExpectationsDescriptor.Id,
			DisposableInstancesFromExpectationsDescriptor.SuppressedId,
			DisposableInstancesFromExpectationsDescriptor.Description);

	internal const string Description = "Suppress CA2000 on Instance() invocations";
	internal const string Id = "ROCK17";
	internal const string SuppressedId = "CA2000";
}