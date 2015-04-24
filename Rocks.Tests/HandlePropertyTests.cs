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

			Assert.AreEqual(1, eventRaisedCount);
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

			Assert.AreEqual(1, eventRaisedCount);
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

			Assert.AreEqual(2, eventRaisedCount);
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndGetNotUsed()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter));

			var chunk = rock.Make();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndSetNotUsed()
		{
			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterAndSetter));

			var chunk = rock.Make();
			var value = chunk.GetterAndSetter;

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.AreEqual(2, eventRaisedCount);
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

			Assert.AreEqual(2, eventRaisedCount);
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

			Assert.AreEqual(4, eventRaisedCount);
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.AreEqual(returnValue, value);
			Assert.AreEqual(1, eventRaisedCount);
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

			Assert.AreEqual(returnValue, value);
			rock.Verify();
		}

		[Test]
		public void MakeWithGetHandlerAndGetNotUsed()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterOnly), () => returnValue);

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.AreEqual(returnValue, value);
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

			Assert.AreEqual(returnValue, value);
			Assert.AreEqual(2, eventRaisedCount);
			rock.Verify();
		}

		[Test]
		public void MakeWithGetHandlerAndExpectedCallCountAndGetNotUsed()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterOnly), () => returnValue, 2);

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetHandlerAndExpectedCallCountAndGetNotUsedEnough()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle(nameof(IProperties.GetterOnly), () => returnValue, 2);

			var chunk = rock.Make();
			var value = chunk.GetterOnly;

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.AreEqual(data, setValue);
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

			Assert.AreEqual(data, setValue);
			Assert.AreEqual(1, eventRaisedCount);
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.AreEqual(data, setValue);
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

			Assert.AreEqual(data, setValue);
			Assert.AreEqual(2, eventRaisedCount);
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.AreEqual(setValue, data, "Setter");
			Assert.AreEqual(returnValue, propertyValue, "Getter");
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

			Assert.AreEqual(setValue, data, "Setter");
			Assert.AreEqual(returnValue, propertyValue, "Getter");
			Assert.AreEqual(1, eventRaisedCount);
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

			Assert.AreEqual(setValue, data, "Setter");
			Assert.AreEqual(returnValue, propertyValue, "Getter");
			Assert.AreEqual(1, eventRaisedCount);
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

			Assert.AreEqual(setValue, data, "Setter");
			Assert.AreEqual(returnValue, propertyValue, "Getter");
			Assert.AreEqual(2, eventRaisedCount);
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndGetAndSetNotUsed()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.Handle<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => { });

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.AreEqual(setValue, data, "Setter");
			Assert.AreEqual(returnValue, propertyValue, "Getter");
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

			Assert.AreEqual(setValue, data, "Setter");
			Assert.AreEqual(returnValue, propertyValue, "Getter");
			Assert.AreEqual(2, eventRaisedCount);
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

			Assert.AreEqual(setValue, data, "Setter");
			Assert.AreEqual(returnValue, propertyValue, "Getter");
			Assert.AreEqual(2, eventRaisedCount);
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

			Assert.AreEqual(setValue, data, "Setter");
			Assert.AreEqual(returnValue, propertyValue, "Getter");
			Assert.AreEqual(4, eventRaisedCount);
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.AreEqual(1, eventRaisedCount);
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

			Assert.AreEqual(1, eventRaisedCount);
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

			Assert.AreEqual(2, eventRaisedCount);
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.AreEqual(2, eventRaisedCount);
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

			Assert.AreEqual(2, eventRaisedCount);
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

			Assert.AreEqual(4, eventRaisedCount);
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.Throws<VerificationException>(() => rock.Verify());
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

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGenericReturnValue()
		{
			var rock = Rock.Create<IProperties<Guid>>();
			rock.Handle(nameof(IProperties<Guid>.Target));

			var chunk = rock.Make();
		}
	}

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
}
