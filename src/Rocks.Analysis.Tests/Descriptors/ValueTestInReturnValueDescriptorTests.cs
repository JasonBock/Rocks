using NUnit.Framework;
using Rocks.Analysis.Descriptors;
using System.Globalization;

namespace Rocks.Analysis.Tests.Descriptors;

public static class ValueTestInReturnValueDescriptorTests
{
	[Test]
	public static void Create()
	{
		var descriptor = ValueTaskInReturnValueDescriptor.Create();

		Assert.Multiple(() =>
		{
			Assert.That(descriptor.Id, Is.EqualTo(ValueTaskInReturnValueDescriptor.Id));
			Assert.That(descriptor.Justification.ToString(CultureInfo.InvariantCulture), 
				Is.EqualTo(ValueTaskInReturnValueDescriptor.Description));
			Assert.That(descriptor.SuppressedDiagnosticId,
				Is.EqualTo(ValueTaskInReturnValueDescriptor.SuppressedId));
		});
	}
}