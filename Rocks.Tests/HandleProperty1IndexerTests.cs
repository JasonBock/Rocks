using NUnit.Framework;
using Rocks.Exceptions;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleProperty1IndexerTests
	{
		[Test]
		public void MakeWithGetIndexerProperty()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty(() => 44, _ => returnValue);

			var chunk = rock.Make();
			var propertyValue = chunk[44];

			Assert.AreEqual(returnValue, propertyValue, nameof(propertyValue));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetIndexerPropertyAndGetNotUsed()
		{
			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty(() => 44, _ => Guid.NewGuid().ToString());

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetIndexerPropertyAndExpectedCallCount()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty(() => 44, _ => returnValue, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[44];
			propertyValue = chunk[44];

			Assert.AreEqual(returnValue, propertyValue, nameof(propertyValue));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetIndexerPropertyAndExpectedCallCountAndGetNotUsed()
		{
			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty(() => 44, _ => Guid.NewGuid().ToString(), 2);

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetIndexerPropertyAndExpectedCallCountAndGetNotUsedEnough()
		{
			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty(() => 44, _ => Guid.NewGuid().ToString(), 2);

			var chunk = rock.Make();
			var propertyValue = chunk[44];

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithSetIndexerProperty()
		{
			var indexer1 = Guid.NewGuid();
			var indexer1SetValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty<Guid, string>(() => indexer1, (i1, value) => setValue = value);

			var chunk = rock.Make();
			chunk[indexer1] = indexer1SetValue;

			Assert.AreEqual(indexer1SetValue, setValue, nameof(setValue));
			rock.Verify();
		}

		[Test]
		public void MakeWithSetIndexerPropertyAndSetNotUsed()
		{
			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty<Guid, string>(() => Guid.NewGuid(), (i1, value) => { });

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithSetIndexerPropertyAndExpectedCallCount()
		{
			var indexer1 = Guid.NewGuid();
			var indexer1SetValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty<Guid, string>(() => indexer1, (i1, value) => setValue = value, 2);

			var chunk = rock.Make();
			chunk[indexer1] = indexer1SetValue;
			chunk[indexer1] = indexer1SetValue;

			Assert.AreEqual(indexer1SetValue, setValue, nameof(setValue));
			rock.Verify();
		}

		[Test]
		public void MakeWithSetIndexerPropertyAndExpectedCallCountAndSetNotUsed()
		{
			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty<Guid, string>(() => Guid.NewGuid(), (i1, value) => { }, 2);

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithSetIndexerPropertyAndExpectedCallCountAndSetNotUsedEnough()
		{
			var indexer1 = Guid.NewGuid();
			string propertyValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty<Guid, string>(() => indexer1, (i1, value) => propertyValue = value, 2);

			var chunk = rock.Make();
			chunk[indexer1] = indexer1.ToString();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetIndexerProperty()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer1SetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty(() => indexer1, _ => returnValue, (i1, value) => setValue = value);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1];
			chunk[indexer1] = indexer1SetValue;

			Assert.AreEqual(returnValue, propertyValue, nameof(propertyValue));
			Assert.AreEqual(indexer1SetValue, setValue, nameof(setValue));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndGetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer1SetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty(() => indexer1, _ => returnValue, (i1, value) => setValue = value);

			var chunk = rock.Make();
			chunk[indexer1] = indexer1SetValue;

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndSetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer1SetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty(() => indexer1, _ => returnValue, (i1, value) => setValue = value);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1];

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCount()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer1SetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty(() => indexer1, _ => returnValue, (i1, value) => setValue = value, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1];
			propertyValue = chunk[indexer1];
			chunk[indexer1] = indexer1SetValue;
			chunk[indexer1] = indexer1SetValue;

			Assert.AreEqual(returnValue, propertyValue, nameof(propertyValue));
			Assert.AreEqual(indexer1SetValue, setValue, nameof(setValue));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndGetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer1SetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty(() => indexer1, _ => returnValue, (i1, value) => setValue = value, 2);

			var chunk = rock.Make();
			chunk[indexer1] = indexer1SetValue;
			chunk[indexer1] = indexer1SetValue;

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndGetNotUsedEnough()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer1SetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty(() => indexer1, _ => returnValue, (i1, value) => setValue = value, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1];
			chunk[indexer1] = indexer1SetValue;
			chunk[indexer1] = indexer1SetValue;

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndSetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer1SetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty(() => indexer1, _ => returnValue, (i1, value) => setValue = value, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1];
			propertyValue = chunk[indexer1];

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndSetNotUsedEnough()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer1SetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.HandleProperty(() => indexer1, _ => returnValue, (i1, value) => setValue = value, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1];
			propertyValue = chunk[indexer1];
			chunk[indexer1] = indexer1SetValue;

			Assert.Throws<VerificationException>(() => rock.Verify());
		}
	}

	public interface IProperty1Indexer
	{
		string this[int a] { get; }
		string this[Guid a] { set; }
		string this[string a] { get; set; }
	}
}
