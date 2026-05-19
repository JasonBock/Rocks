using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rocks.Analysis.IntegrationTests.ExceptionTestTypes;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public interface IThrowException
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
{
	void ThrowIt();
}

public sealed class ThrownException
	: Exception
{
	public const string DefaultMessage = nameof(ThrownException);

	public ThrownException()
		: base(ThrownException.DefaultMessage) { }

	public ThrownException(string message)
		: base(message) { }

	public ThrownException(string message, Exception innerException)
		: base(message, innerException) { }
}

internal static class ExceptionTests
{
	[Test]
	public static void ThrowUsingGeneric()
	{
		using var context = new RockContext();
		var throwsExpectations = context.Create<IThrowExceptionCreateExpectations>();
		throwsExpectations.Setups.ThrowIt().Throws<ThrownException>();

		var mock = throwsExpectations.Instance();
		Assert.That(() => mock.ThrowIt(),
			Throws.TypeOf<ThrownException>().With.Message.EqualTo(ThrownException.DefaultMessage));
	}

	[Test]
	public static void ThrowUsingInstance()
	{
		const string message = "My message";

		using var context = new RockContext();
		var throwsExpectations = context.Create<IThrowExceptionCreateExpectations>();
		throwsExpectations.Setups.ThrowIt().Throws(new ThrownException(message));

		var mock = throwsExpectations.Instance();
		Assert.That(() => mock.ThrowIt(),
			Throws.TypeOf<ThrownException>().With.Message.EqualTo(message));
	}
}