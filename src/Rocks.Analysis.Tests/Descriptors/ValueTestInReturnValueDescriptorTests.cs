using NUnit.Framework;
using Rocks.Analysis.Descriptors;
using System.Globalization;

namespace Rocks.Analysis.Tests.Descriptors;

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