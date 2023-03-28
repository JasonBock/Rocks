﻿using NUnit.Framework;

namespace Rocks.IntegrationTests;

public sealed class ClassEventArgs
	: EventArgs
{ }

public class ClassGenericEvents<T>
	where T : EventArgs
{
	public virtual void Foo() { }
#pragma warning disable CA1070 // Do not declare event fields as virtual
#pragma warning disable CS0067
   public virtual event EventHandler<T>? MyEvent;
#pragma warning restore CS0067
#pragma warning restore CA1070 // Do not declare event fields as virtual
}

public static class ClassGenericEventsTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = Rock.Create<ClassGenericEvents<ClassEventArgs>>();
		expectations.Methods().Foo().RaisesMyEvent(new ClassEventArgs());

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.Foo();

		expectations.Verify();

		Assert.That(wasEventRaised, Is.True);
	}
}