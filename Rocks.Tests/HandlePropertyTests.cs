using NUnit.Framework;
using Rocks.Exceptions;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandlePropertyTests
	{
		[Test]
		public void MakeWithGetAndSetProperty()
		{
			var rock = Rock.Create<IProperties>();
			rock.HandleProperty(nameof(IProperties.GetterAndSetter));

			var chunk = rock.Make();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			var value = chunk.GetterAndSetter;

			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndGetNotUsed()
		{
			var rock = Rock.Create<IProperties>();
			rock.HandleProperty(nameof(IProperties.GetterAndSetter));

			var chunk = rock.Make();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndSetNotUsed()
		{
			var rock = Rock.Create<IProperties>();
			rock.HandleProperty(nameof(IProperties.GetterAndSetter));

			var chunk = rock.Make();
			var value = chunk.GetterAndSetter;

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndExpectedCallCount()
		{
			var rock = Rock.Create<IProperties>();
			rock.HandleProperty(nameof(IProperties.GetterAndSetter), 2);

			var chunk = rock.Make();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			chunk.GetterAndSetter = Guid.NewGuid().ToString();
			var value = chunk.GetterAndSetter;
			value = chunk.GetterAndSetter;

			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetPropertyAndExpectedCallCountAndGetNotUsedEnough()
		{
			var rock = Rock.Create<IProperties>();
			rock.HandleProperty(nameof(IProperties.GetterAndSetter), 2);

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
			rock.HandleProperty(nameof(IProperties.GetterAndSetter), 2);

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
			rock.HandleProperty(nameof(IProperties.GetterOnly), () => returnValue);

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
			rock.HandleProperty(nameof(IProperties.GetterOnly), () => returnValue);

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetHandlerAndExpectedCallCount()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.HandleProperty(nameof(IProperties.GetterOnly), () => returnValue, 2);

			var chunk = rock.Make();
			var value = chunk.GetterOnly;
			value = chunk.GetterOnly;

			Assert.AreEqual(returnValue, value);
			rock.Verify();
		}

		[Test]
		public void MakeWithGetHandlerAndExpectedCallCountAndGetNotUsed()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.HandleProperty(nameof(IProperties.GetterOnly), () => returnValue, 2);

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetHandlerAndExpectedCallCountAndGetNotUsedEnough()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.HandleProperty(nameof(IProperties.GetterOnly), () => returnValue, 2);

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
			rock.HandleProperty<string>(nameof(IProperties.SetterOnly), value => setValue = value);

			var chunk = rock.Make();
			chunk.SetterOnly = data;

			Assert.AreEqual(data, setValue);
			rock.Verify();
		}

		[Test]
		public void MakeWithSetHandlerAndSetNotUsed()
		{
			var data = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.HandleProperty<string>(nameof(IProperties.SetterOnly), value => setValue = value);

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithSetHandlerAndExpectedCallCount()
		{
			var data = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.HandleProperty<string>(nameof(IProperties.SetterOnly), value => setValue = value, 2);

			var chunk = rock.Make();
			chunk.SetterOnly = data;
			chunk.SetterOnly = data;

			Assert.AreEqual(data, setValue);
			rock.Verify();
		}

		[Test]
		public void MakeWithSetHandlerAndExpectedCallCountAndSetNotUsed()
		{
			var data = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.HandleProperty<string>(nameof(IProperties.SetterOnly), value => setValue = value, 2);

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithSetHandlerAndExpectedCallCountAndSetNotUsedEnough()
		{
			var data = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.HandleProperty<string>(nameof(IProperties.SetterOnly), value => setValue = value, 2);

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
			rock.HandleProperty<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value);

			var chunk = rock.Make();
			chunk.GetterAndSetter = data;
			var propertyValue = chunk.GetterAndSetter;

			Assert.AreEqual(setValue, data, "Setter");
			Assert.AreEqual(returnValue, propertyValue, "Getter");
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndGetNotUsed()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.HandleProperty<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value);

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
			rock.HandleProperty<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value);

			var chunk = rock.Make();
			var propertyValue = chunk.GetterAndSetter;

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetHandlersAndGetAndSetNotUsed()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.HandleProperty<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value);

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
			rock.HandleProperty<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2);

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
		public void MakeWithGetAndSetHandlersAndExpectedCallCountAndGetNotUsed()
		{
			var data = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperties>();
			rock.HandleProperty<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2);

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
			rock.HandleProperty<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2);

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
			rock.HandleProperty<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2);

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
			rock.HandleProperty<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2);

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
			rock.HandleProperty<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2);

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
			rock.HandleProperty<string>(nameof(IProperties.GetterAndSetter), () => returnValue, value => setValue = value, 2);

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
			rock.HandleProperty(() => new object[] { a, b, c });

			var chunk = rock.Make();
			chunk[a, b, c] = Guid.NewGuid().ToString();
			var propertyValue = chunk[a, b, c];

			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndGetNotUsed()
		{
			var a = 44;
			var b = Guid.NewGuid();
			var c = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.HandleProperty(() => new object[] { a, b, c });

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
			rock.HandleProperty(() => new object[] { a, b, c });

			var chunk = rock.Make();
			var propertyValue = chunk[a, b, c];

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWith1GetIndexerProperty()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.HandleProperty<int, string>(() => 44, _ => returnValue);

			var chunk = rock.Make();
			var propertyValue = chunk[44];

			Assert.AreEqual(returnValue, propertyValue, nameof(propertyValue));
			rock.Verify();
		}

		[Test]
		public void MakeWith1GetIndexerPropertyAndGetNotUsed()
		{
			var rock = Rock.Create<IProperties>();
			rock.HandleProperty<int, string>(() => 44, _ => Guid.NewGuid().ToString());

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWith1GetIndexerPropertyAndExpectedCallCount()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperties>();
			rock.HandleProperty<int, string>(() => 44, _ => returnValue, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[44];
			propertyValue = chunk[44];

			Assert.AreEqual(returnValue, propertyValue, nameof(propertyValue));
			rock.Verify();
		}

		[Test]
		public void MakeWith1GetIndexerPropertyAndExpectedCallCountAndGetNotUsed()
		{
			var rock = Rock.Create<IProperties>();
			rock.HandleProperty<int, string>(() => 44, _ => Guid.NewGuid().ToString(), 2);

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWith1GetIndexerPropertyAndExpectedCallCountAndGetNotUsedEnough()
		{
			var rock = Rock.Create<IProperties>();
			rock.HandleProperty<int, string>(() => 44, _ => Guid.NewGuid().ToString(), 2);

			var chunk = rock.Make();
			var propertyValue = chunk[44];

			Assert.Throws<VerificationException>(() => rock.Verify());
		}
	}

	public interface IProperties
	{
		string GetterOnly { get; }
		string SetterOnly { set; }
		string GetterAndSetter { get;  set; }
		string this[int a, Guid b, string c] { get; set; }
		string this[int a] { get; }
	}
}
