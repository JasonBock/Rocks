using NUnit.Framework;
using Rocks.Construction;
using System;

namespace Rocks.Tests.Construction
{
	[TestFixture]
	public sealed class MethodInformationTests
	{
		[Test]
		public void Create()
		{
			var containsDelegateConditions = true;
			var delegateCast = Guid.NewGuid().ToString("N");
         var description = Guid.NewGuid().ToString("N");
			var descriptionWithOverride = Guid.NewGuid().ToString("N");

			var information = new MethodInformation(containsDelegateConditions,
				delegateCast, description, descriptionWithOverride);

			Assert.That(information.ContainsDelegateConditions, Is.EqualTo(containsDelegateConditions),
				nameof(information.ContainsDelegateConditions));
			Assert.That(information.DelegateCast, Is.EqualTo(delegateCast),
				nameof(information.DelegateCast));
			Assert.That(information.Description, Is.EqualTo(description),
				nameof(information.Description));
			Assert.That(information.DescriptionWithOverride, Is.EqualTo(descriptionWithOverride),
				nameof(information.DescriptionWithOverride));
		}
	}
}
