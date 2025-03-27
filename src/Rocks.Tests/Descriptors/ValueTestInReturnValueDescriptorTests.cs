using NUnit.Framework;
using Rocks.Descriptors;
using System.Globalization;

namespace Rocks.Tests.Descriptors;

public static class ValueTestInReturnValueDescriptorTests
{
	[Test]
	public static void Create()
	{
		var descriptor = ValueTestInReturnValueDescriptor.Create();

		Assert.Multiple(() =>
		{
			Assert.That(descriptor.Id, Is.EqualTo(ValueTestInReturnValueDescriptor.Id));
			Assert.That(descriptor.Justification.ToString(CultureInfo.InvariantCulture), 
				Is.EqualTo(ValueTestInReturnValueDescriptor.Description));
			Assert.That(descriptor.SuppressedDiagnosticId,
				Is.EqualTo(ValueTestInReturnValueDescriptor.SuppressedId));
		});
	}
}