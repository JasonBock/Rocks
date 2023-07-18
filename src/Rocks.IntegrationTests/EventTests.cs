using NUnit.Framework;

namespace Rocks.IntegrationTests;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public class NotEventArgs { }

public interface IUseNotEventArgs
{
	void A();
	event EventHandler<NotEventArgs> NotEvent;
}
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix

public static class EventTests
{
	[Test]
	public static void Create()
	{
		var eventArgs = new NotEventArgs();
		NotEventArgs? mockEventArgs = null;

		var expectations = Rock.Create<IUseNotEventArgs>();
		expectations.Methods().A().RaisesNotEvent(eventArgs);

		var mock = expectations.Instance();
		mock.NotEvent += (sender, args) => mockEventArgs = args;
		mock.A();

		Assert.That(mockEventArgs, Is.SameAs(eventArgs));
		expectations.Verify();
	}
}