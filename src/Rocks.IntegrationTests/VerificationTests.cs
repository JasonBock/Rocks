﻿using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests;

public interface IData
{
	string Value { get; set; }
}

public static class VerificationTests
{
	[Test]
	public static void VerifyWhenPropertyExpectationIsNotMet()
	{
		var expectations = Rock.Create<IData>();
		expectations.Properties().Setters().Value("3");

		_ = expectations.Instance();

		Assert.That(expectations.Verify, 
			Throws.TypeOf<VerificationException>()
				.And.Message.EqualTo(
					"The following verification failure(s) occured: Type: Rocks.IntegrationTests.IData, mock type: Rocks.IntegrationTests.CreateExpectationsOfIDataExtensions+RockIData, member: set_Value(@value), message: The expected call count is incorrect. Expected: 1, received: 0."));
	}
}