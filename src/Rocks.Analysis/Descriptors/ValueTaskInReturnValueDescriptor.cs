using Microsoft.CodeAnalysis;

namespace Rocks.Analysis.Descriptors;

internal static class ValueTaskInReturnValueDescriptor
{
	internal static SuppressionDescriptor Create() =>
		new(
			ValueTaskInReturnValueDescriptor.Id, 
			ValueTaskInReturnValueDescriptor.SuppressedId,
			ValueTaskInReturnValueDescriptor.Description);

	internal const string Description = "Suppress CA2012 on ReturnValue() invocations when parameter type is either ValueTask or ValueTask<>";
	internal const string Id = "ROCK15";
	internal const string SuppressedId = "CA2012";
}