using Microsoft.CodeAnalysis;

namespace Rocks.Analysis.Descriptors;

internal static class IDisposableInstancesIntoTasksDescriptor
{
	internal static SuppressionDescriptor Create() =>
		new(
			DescriptorIdentifiers.IDisposableInstancesIntoTasksId,
			IDisposableInstancesIntoTasksDescriptor.SuppressedId,
			IDisposableInstancesIntoTasksDescriptor.Description);

	internal const string Description = "Suppress CA2025 on ReturnValue() and Callback() invocations";
	internal const string SuppressedId = "CA2025";
}