﻿using Microsoft.CodeAnalysis;

namespace Rocks.Descriptors;

internal static class ValueTypeInReturnValueDescriptor
{
	internal static SuppressionDescriptor Create() =>
		new(
			ValueTypeInReturnValueDescriptor.Id, 
			ValueTypeInReturnValueDescriptor.SuppressedId,
			ValueTypeInReturnValueDescriptor.Description);

	internal const string Description = "Suppress CA2012 on ReturnValue() invocations when parameter type is either ValueTask or ValueTask<>";
	internal const string Id = "ROCK15";
	internal const string SuppressedId = "CA2012";
}