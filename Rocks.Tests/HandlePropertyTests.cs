using NUnit.Framework;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandlePropertyTests
	{
		[Test]
		public void MakeWithGetAndSetProperty()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter));

			var chunk = rock.Make();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			var value = chunk.GetterAndSetter;

			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetPropertyButOnlyExpectGetter()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter), () => "44");

			var chunk = rock.Make();
			Assert.That(chunk.GetterAndSetter, Is.EqualTo("44"));

			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetPropertyButOnlyExpectSetter()
		{
			string value = null;
			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), _ => value = _);

			var chunk = rock.Make();
			chunk.GetterAndSetter = "44";
			Assert.That(value, Is.EqualTo("44"));

			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndEventRaisedOnGetter()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter))
				.RaisesOnGetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;

			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			var value = chunk.GetterAndSetter;

			Assert.That(eventRaisedCount, Is.EqualTo(1));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndEventRaisedOnSetter()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter))
				.RaisesOnSetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;

			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			var value = chunk.GetterAndSetter;

			Assert.That(eventRaisedCount, Is.EqualTo(1));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndEventRaisedOnGetterAndSetter()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter))
				.RaisesOnGetter(nameof(IProperties.TargetEvent), EventArgs.Empty)
				.RaisesOnSetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;

			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			var value = chunk.GetterAndSetter;

			Assert.That(eventRaisedCount, Is.EqualTo(2));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndGetNotUsed()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter));

			var chunk = rock.Make();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndSetNotUsed()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter));

			var chunk = rock.Make();
			var value = chunk.GetterAndSetter;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndExpectedCallCount()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter), 2);

			var chunk = rock.Make();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			var value = chunk.GetterAndSetter;
			value = chunk.GetterAndSetter;

			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndExpectedCallCountAndEventRaisedOnGetter()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter), 2)
				.RaisesOnGetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;

			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			var value = chunk.GetterAndSetter;
			value = chunk.GetterAndSetter;

			Assert.That(eventRaisedCount, Is.EqualTo(2));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndExpectedCallCountAndEventRaisedOnSetter()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter), 2)
				.RaisesOnSetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;

			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			var value = chunk.GetterAndSetter;
			value = chunk.GetterAndSetter;

			Assert.That(eventRaisedCount, Is.EqualTo(2));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndExpectedCallCountAndEventRaisedOnGetterAndSetter()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter), 2)
				.RaisesOnGetter(nameof(IProperties.TargetEvent), EventArgs.Empty)
				.RaisesOnSetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;

			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			var value = chunk.GetterAndSetter;
			value = chunk.GetterAndSetter;

			Assert.That(eventRaisedCount, Is.EqualTo(4));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndExpectedCallCountAndGetNotUsedEnough()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter), 2);

			var chunk = rock.Make();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			var value = chunk.GetterAndSetter;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndExpectedCallCountAndSetNotUsed()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter), 2);

			var chunk = rock.Make();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			var value = chunk.GetterAndSetter;
			value = chunk.GetterAndSetter;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetHandler()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterOnly), () => returnValue)
				.Raises(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			var value = chunk.GetterOnly;

			Assert.That(value, Is.EqualTo(returnValue));
			Assert.That(eventRaisedCount, Is.EqualTo(1));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetHandlerAndEventRaised()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterOnly), () => returnValue);

			var chunk = rock.Make();
			var value = chunk.GetterOnly;

			Assert.That(value, Is.EqualTo(returnValue));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetHandlerAndGetNotUsed()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterOnly), () => returnValue);

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetHandlerAndExpectedCallCount()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterOnly), () => returnValue, 2);

			var chunk = rock.Make();
			var value = chunk.GetterOnly;
			value = chunk.GetterOnly;

			Assert.That(value, Is.EqualTo(returnValue));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetHandlerAndEventRaisedAndExpectedCallCount()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterOnly), () => returnValue, 2)
				.Raises(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			var value = chunk.GetterOnly;
			value = chunk.GetterOnly;

			Assert.That(value, Is.EqualTo(returnValue));
			Assert.That(eventRaisedCount, Is.EqualTo(2));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetHandlerAndExpectedCallCountAndGetNotUsed()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterOnly), () => returnValue, 2);

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetHandlerAndExpectedCallCountAndGetNotUsedEnough()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterOnly), () => returnValue, 2);

			var chunk = rock.Make();
			var value = chunk.GetterOnly;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithSetHandler()
		{
			var data = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.SetterOnly), value => setValue = value);

			var chunk = rock.Make();
			chunk.SetterOnly = data;

			Assert.That(setValue, Is.EqualTo(data));
			rock.Verify();
		}

		[Test]
		public void MakeWithSetHandlerAndEventRaised()
		{
			var data = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.SetterOnly), value => setValue = value)
				.Raises(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.SetterOnly = data;

			Assert.That(setValue, Is.EqualTo(data));
			Assert.That(eventRaisedCount, Is.EqualTo(1));
			rock.Verify();
		}

		[Test]
		public void MakeWithSetHandlerAndSetNotUsed()
		{
			var data = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.SetterOnly), value => setValue = value);

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithSetHandlerAndExpectedCallCount()
		{
			var data = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.SetterOnly), value => setValue = value, 2);

			var chunk = rock.Make();
			chunk.SetterOnly = data;
			chunk.SetterOnly = data;

			Assert.That(setValue, Is.EqualTo(data));
			rock.Verify();
		}

		[Test]
		public void MakeWithSetHandlerAndEventRaisedAndExpectedCallCount()
		{
			var data = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.SetterOnly), value => setValue = value, 2)
				.Raises(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.SetterOnly = data;
			chunk.SetterOnly = data;

			Assert.That(setValue, Is.EqualTo(data));
			Assert.That(eventRaisedCount, Is.EqualTo(2));
			rock.Verify();
		}

		[Test]
		public void MakeWithSetHandlerAndExpectedCallCountAndSetNotUsed()
		{
			var data = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.SetterOnly), value => setValue = value, 2);

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithSetHandlerAndExpectedCallCountAndSetNotUsedEnough()
		{
			var data = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.SetterOnly), value => setValue = value, 2);

			var chunk = rock.Make();
			chunk.SetterOnly = data;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetHandlers()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value);

			var chunk = rock.Make();
			chunk.GetterAndSetter = data;
			var propertyValue = chunk.GetterAndSetter;

			Assert.That(data, Is.EqualTo(setValue), "Setter");
			Assert.That(propertyValue, Is.EqualTo(returnValue), "Getter");
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndEventRaisedOnGetter()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value)
				.RaisesOnGetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.GetterAndSetter = data;
			var propertyValue = chunk.GetterAndSetter;

			Assert.That(data, Is.EqualTo(setValue), "Setter");
			Assert.That(propertyValue, Is.EqualTo(returnValue), "Getter");
			Assert.That(eventRaisedCount, Is.EqualTo(1));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndEventRaisedOnSetter()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value)
				.RaisesOnSetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.GetterAndSetter = data;
			var propertyValue = chunk.GetterAndSetter;

			Assert.That(data, Is.EqualTo(setValue), "Setter");
			Assert.That(propertyValue, Is.EqualTo(returnValue), "Getter");
			Assert.That(eventRaisedCount, Is.EqualTo(1));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndEventRaisedOnGetterAndSetter()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value)
				.RaisesOnGetter(nameof(IProperties.TargetEvent), EventArgs.Empty)
				.RaisesOnSetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.GetterAndSetter = data;
			var propertyValue = chunk.GetterAndSetter;

			Assert.That(data, Is.EqualTo(setValue), "Setter");
			Assert.That(propertyValue, Is.EqualTo(returnValue), "Getter");
			Assert.That(eventRaisedCount, Is.EqualTo(2));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndGetNotUsed()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value);

			var chunk = rock.Make();
			chunk.GetterAndSetter = data;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndSetNotUsed()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value);

			var chunk = rock.Make();
			var propertyValue = chunk.GetterAndSetter;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndGetAndSetNotUsed()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => { });

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndExpectedCallCount()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2);

			var chunk = rock.Make();
			chunk.GetterAndSetter = data;
			chunk.GetterAndSetter = data;
			var propertyValue = chunk.GetterAndSetter;
			propertyValue = chunk.GetterAndSetter;

			Assert.That(data, Is.EqualTo(setValue), "Setter");
			Assert.That(propertyValue, Is.EqualTo(returnValue), "Getter");
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndEventRaisedOnGetterExpectedCallCount()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2)
				.RaisesOnGetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.GetterAndSetter = data;
			chunk.GetterAndSetter = data;
			var propertyValue = chunk.GetterAndSetter;
			propertyValue = chunk.GetterAndSetter;

			Assert.That(data, Is.EqualTo(setValue), "Setter");
			Assert.That(propertyValue, Is.EqualTo(returnValue), "Getter");
			Assert.That(eventRaisedCount, Is.EqualTo(2));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndEventRaisedOnSetterExpectedCallCount()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2)
				.RaisesOnSetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.GetterAndSetter = data;
			chunk.GetterAndSetter = data;
			var propertyValue = chunk.GetterAndSetter;
			propertyValue = chunk.GetterAndSetter;

			Assert.That(data, Is.EqualTo(setValue), "Setter");
			Assert.That(propertyValue, Is.EqualTo(returnValue), "Getter");
			Assert.That(eventRaisedCount, Is.EqualTo(2));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndEventRaisedOnGetterAndSetterExpectedCallCount()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2)
				.RaisesOnGetter(nameof(IProperties.TargetEvent), EventArgs.Empty)
				.RaisesOnSetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.GetterAndSetter = data;
			chunk.GetterAndSetter = data;
			var propertyValue = chunk.GetterAndSetter;
			propertyValue = chunk.GetterAndSetter;

			Assert.That(data, Is.EqualTo(setValue), "Setter");
			Assert.That(propertyValue, Is.EqualTo(returnValue), "Getter");
			Assert.That(eventRaisedCount, Is.EqualTo(4));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndExpectedCallCountAndGetNotUsed()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2);

			var chunk = rock.Make();
			chunk.GetterAndSetter = data;
			chunk.GetterAndSetter = data;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndExpectedCallCountAndGetNotUsedEnough()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2);

			var chunk = rock.Make();
			chunk.GetterAndSetter = data;
			chunk.GetterAndSetter = data;
			var propertyValue = chunk.GetterAndSetter;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndExpectedCallCountAndSetNotUsed()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2);

			var chunk = rock.Make();
			var propertyValue = chunk.GetterAndSetter;
			propertyValue = chunk.GetterAndSetter;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndExpectedCallCountAndSetNotUsedEnough()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2);

			var chunk = rock.Make();
			chunk.GetterAndSetter = data;
			var propertyValue = chunk.GetterAndSetter;
			propertyValue = chunk.GetterAndSetter;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndExpectedCallCountAndGetAndSetNotUsed()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2);

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndExpectedCallCountAndGetAndSetNotUsedEnough()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2);

			var chunk = rock.Make();
			chunk.GetterAndSetter = data;
			var propertyValue = chunk.GetterAndSetter;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetIndexerProperty()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(() => new object[] { a, b, c });

			var chunk = rock.Make();
			chunk[a, b, c] = Guid.NewGuid().ToString();
			var propertyValue = chunk[a, b, c];

			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndEventRaisedOnGetter()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(() => new object[] { a, b, c })
				.RaisesOnGetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk[a, b, c] = Guid.NewGuid().ToString();
			var propertyValue = chunk[a, b, c];

			Assert.That(eventRaisedCount, Is.EqualTo(1));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndEventRaisedOnSetter()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(() => new object[] { a, b, c })
				.RaisesOnSetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk[a, b, c] = Guid.NewGuid().ToString();
			var propertyValue = chunk[a, b, c];

			Assert.That(eventRaisedCount, Is.EqualTo(1));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndEventRaisedOnGetterAndSetter()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(() => new object[] { a, b, c })
				.RaisesOnGetter(nameof(IProperties.TargetEvent), EventArgs.Empty)
				.RaisesOnSetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk[a, b, c] = Guid.NewGuid().ToString();
			var propertyValue = chunk[a, b, c];

			Assert.That(eventRaisedCount, Is.EqualTo(2));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndGetNotUsed()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(() => new object[] { a, b, c });

			var chunk = rock.Make();
			chunk[a, b, c] = Guid.NewGuid().ToString();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndSetNotUsed()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(() => new object[] { a, b, c });

			var chunk = rock.Make();
			var propertyValue = chunk[a, b, c];

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCount()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(() => new object[] { a, b, c }, 2);

			var chunk = rock.Make();
			chunk[a, b, c] = Guid.NewGuid().ToString();
			chunk[a, b, c] = Guid.NewGuid().ToString();
			var propertyValue = chunk[a, b, c];
			propertyValue = chunk[a, b, c];

			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndEventRaisedOnGetterAndExpectedCallCount()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(() => new object[] { a, b, c }, 2)
				.RaisesOnGetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk[a, b, c] = Guid.NewGuid().ToString();
			chunk[a, b, c] = Guid.NewGuid().ToString();
			var propertyValue = chunk[a, b, c];
			propertyValue = chunk[a, b, c];

			Assert.That(eventRaisedCount, Is.EqualTo(2));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndEventRaisedOnSetterAndExpectedCallCount()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(() => new object[] { a, b, c }, 2)
				.RaisesOnSetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk[a, b, c] = Guid.NewGuid().ToString();
			chunk[a, b, c] = Guid.NewGuid().ToString();
			var propertyValue = chunk[a, b, c];
			propertyValue = chunk[a, b, c];

			Assert.That(eventRaisedCount, Is.EqualTo(2));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndEventRaisedOnGetterAndSetterAndExpectedCallCount()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(() => new object[] { a, b, c }, 2)
				.RaisesOnGetter(nameof(IProperties.TargetEvent), EventArgs.Empty)
				.RaisesOnSetter(nameof(IProperties.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk[a, b, c] = Guid.NewGuid().ToString();
			chunk[a, b, c] = Guid.NewGuid().ToString();
			var propertyValue = chunk[a, b, c];
			propertyValue = chunk[a, b, c];

			Assert.That(eventRaisedCount, Is.EqualTo(4));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndGetNotUsed()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(() => new object[] { a, b, c }, 2);

			var chunk = rock.Make();
			chunk[a, b, c] = Guid.NewGuid().ToString();
			chunk[a, b, c] = Guid.NewGuid().ToString();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndGetNotUsedEnough()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(() => new object[] { a, b, c }, 2);

			var chunk = rock.Make();
			chunk[a, b, c] = Guid.NewGuid().ToString();
			chunk[a, b, c] = Guid.NewGuid().ToString();
			var propertyValue = chunk[a, b, c];

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndSetNotUsed()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(() => new object[] { a, b, c }, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[a, b, c];
			propertyValue = chunk[a, b, c];

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndSetNotUsedEnough()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(() => new object[] { a, b, c }, 2);

			var chunk = rock.Make();
			chunk[a, b, c] = Guid.NewGuid().ToString();
			var propertyValue = chunk[a, b, c];
			propertyValue = chunk[a, b, c];

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void MakeWithALotOfIndexers()
		{
			var a0 = 0;
			var a1 = 1;
			var a2 = 2;
			var a3 = 3;
			var a4 = 4;

			var rock = Rock.Create<IHaveALotOfIndexers>();
			rock.Handle(() => new object[] { a0, a1, a2, a3, a4 });

			var chunk = rock.Make();
			chunk[a0, a1, a2, a3, a4] = Guid.NewGuid().ToString();
			var propertyValue = chunk[a0, a1, a2, a3, a4];

			rock.Verify();
		}

		[Test]
		public void MakeWithIndexerOnBaseType()
		{
			var rock = Rock.Create<ICustomList<int>>();
			rock.Handle(() => 0, _ => 22);

			var chunk = rock.Make();
			Assert.That(chunk[0], Is.EqualTo(22));

			rock.Verify();
		}

		[Test]
		public void MakeWithGenericReturnValue()
		{
			var rock = Rock.Create<IProperties<Guid>>();
			rock.Handle(nameof(IProperties<Guid>.Target));

			var chunk = rock.Make();
		}
	}

	public interface ICustomList<T>
		: IList<T>
	{ }

	public interface IProperties
	{
		event EventHandler TargetEvent;
		string GetterOnly { get; }
		string SetterOnly { set; }
		string GetterAndSetter { get;  set; }
		string this[int a, Guid b, string c] { get; set; }
	}

	public interface IProperties<T>
	{
		IEnumerable<T> Target { get; set; }
	}

	public interface IHaveALotOfIndexers
	{
		string this[int a0, int a1, int a2, int a3, int a4] { get; set; }
	}
}
