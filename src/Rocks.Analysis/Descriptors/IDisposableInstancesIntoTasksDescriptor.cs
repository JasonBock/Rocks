using Microsoft.CodeAnalysis;

namespace Rocks.Analysis.Descriptors;

internal static class IDisposableInstancesIntoTasksDescriptor
{
	internal static SuppressionDescriptor Create() =>
		new(
			Id,
			SuppressedId,
			Description);

	internal const string Description = "Suppress CA2025 on ReturnValue() and Callback() invocations";
	internal const string Id = "ROCK16";
	internal const string SuppressedId = "CA2025";
}