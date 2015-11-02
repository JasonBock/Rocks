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

			Assert.AreEqual(containsDelegateConditions, information.ContainsDelegateConditions,
				nameof(information.ContainsDelegateConditions));
			Assert.AreEqual(delegateCast, information.DelegateCast,
				nameof(information.DelegateCast));
			Assert.AreEqual(description, information.Description,
				nameof(information.Description));
			Assert.AreEqual(descriptionWithOverride, information.DescriptionWithOverride,
				nameof(information.DescriptionWithOverride));
		}
	}
}
