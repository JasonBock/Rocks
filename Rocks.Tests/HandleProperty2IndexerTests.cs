using NUnit.Framework;
using Rocks.Exceptions;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleProperty2IndexerTests
	{
		[Test]
		public void MakeWithGetIndexerProperty()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle(() => 44, () => 45, (_, __) => returnValue);

			var chunk = rock.Make();
			var propertyValue = chunk[44, 45];

			Assert.AreEqual(returnValue, propertyValue, nameof(propertyValue));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetIndexerPropertyAndGetNotUsed()
		{
			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle(() => 44, () => 45, (_, __) => Guid.NewGuid().ToString());

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetIndexerPropertyAndExpectedCallCount()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle(() => 44, () => 45, (_, __) => returnValue, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[44, 45];
			propertyValue = chunk[44, 45];

			Assert.AreEqual(returnValue, propertyValue, nameof(propertyValue));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetIndexerPropertyAndExpectedCallCountAndGetNotUsed()
		{
			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle(() => 44, () => 45, (_, __) => Guid.NewGuid().ToString(), 2);

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetIndexerPropertyAndExpectedCallCountAndGetNotUsedEnough()
		{
			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle(() => 44, () => 45, (_, __) => Guid.NewGuid().ToString(), 2);

			var chunk = rock.Make();
			var propertyValue = chunk[44, 45];

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithSetIndexerProperty()
		{
			var indexer1 = Guid.NewGuid();
			var indexer2 = Guid.NewGuid();
			var indexerSetValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle<Guid, Guid, string>(() => indexer1, () => indexer2, (i1, i2, value) => setValue = value);

			var chunk = rock.Make();
			chunk[indexer1, indexer2] = indexerSetValue;

			Assert.AreEqual(indexerSetValue, setValue, nameof(setValue));
			rock.Verify();
		}

		[Test]
		public void MakeWithSetIndexerPropertyAndSetNotUsed()
		{
			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle<Guid, Guid, string>(() => Guid.NewGuid(), () => Guid.NewGuid(), (i1, i2, value) => { });

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithSetIndexerPropertyAndExpectedCallCount()
		{
			var indexer1 = Guid.NewGuid();
			var indexer2 = Guid.NewGuid();
			var indexerSetValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle<Guid, Guid, string>(() => indexer1, () => indexer2, (i1, i2, value) => setValue = value, 2);

			var chunk = rock.Make();
			chunk[indexer1, indexer2] = indexerSetValue;
			chunk[indexer1, indexer2] = indexerSetValue;

			Assert.AreEqual(indexerSetValue, setValue, nameof(setValue));
			rock.Verify();
		}

		[Test]
		public void MakeWithSetIndexerPropertyAndExpectedCallCountAndSetNotUsed()
		{
			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle<Guid, Guid, string>(() => Guid.NewGuid(), () => Guid.NewGuid(), (i1, i2, value) => { }, 2);

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithSetIndexerPropertyAndExpectedCallCountAndSetNotUsedEnough()
		{
			var indexer1 = Guid.NewGuid();
			var indexer2 = Guid.NewGuid();
			var indexerSetValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle<Guid, Guid, string>(() => indexer1, () => indexer2, (i1, i2, value) => setValue = value, 2);

			var chunk = rock.Make();
			chunk[indexer1, indexer2] = indexerSetValue;

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetIndexerProperty()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle(() => indexer1, () => indexer2, (_, __) => returnValue, (i1, i2, value) => setValue = value);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1, indexer2];
			chunk[indexer1, indexer2] = indexerSetValue;

			Assert.AreEqual(returnValue, propertyValue, nameof(propertyValue));
			Assert.AreEqual(indexerSetValue, setValue, nameof(setValue));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndGetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle(() => indexer1, () => indexer2, (_, __) => returnValue, (i1, i2, value) => { });

			var chunk = rock.Make();
			chunk[indexer1, indexer2] = indexerSetValue;

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndSetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle(() => indexer1, () => indexer2, (_, __) => returnValue, (i1, i2, value) => { });

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1, indexer2];

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCount()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle(() => indexer1, () => indexer2, (_, __) => returnValue, (i1, i2, value) => setValue = value, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1, indexer2];
			propertyValue = chunk[indexer1, indexer2];
			chunk[indexer1, indexer2] = indexerSetValue;
			chunk[indexer1, indexer2] = indexerSetValue;

			Assert.AreEqual(returnValue, propertyValue, nameof(propertyValue));
			Assert.AreEqual(indexerSetValue, setValue, nameof(setValue));
			rock.Verify();
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndGetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle(() => indexer1, () => indexer2, (_, __) => returnValue, (i1, i2, value) => { }, 2);

			var chunk = rock.Make();
			chunk[indexer1, indexer2] = indexerSetValue;
			chunk[indexer1, indexer2] = indexerSetValue;

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndGetNotUsedEnough()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle(() => indexer1, () => indexer2, (_, __) => returnValue, (i1, i2, value) => { }, 2);

			var chunk = rock.Make();
			chunk[indexer1, indexer2] = indexerSetValue;
			chunk[indexer1, indexer2] = indexerSetValue;
			var propertyValue = chunk[indexer1, indexer2];

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndSetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle(() => indexer1, () => indexer2, (_, __) => returnValue, (i1, i2, value) => { }, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1, indexer2];
			propertyValue = chunk[indexer1, indexer2];

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndSetNotUsedEnough()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty2Indexer>();
			rock.Handle(() => indexer1, () => indexer2, (_, __) => returnValue, (i1, i2, value) => { }, 2);

			var chunk = rock.Make();
			chunk[indexer1, indexer2] = indexerSetValue;
			var propertyValue = chunk[indexer1, indexer2];
			propertyValue = chunk[indexer1, indexer2];

			Assert.Throws<VerificationException>(() => rock.Verify());
		}
	}

	public interface IProperty2Indexer
	{
		string this[int a, int b] { get; }
		string this[Guid a, Guid b] { set; }
		string this[string a, string b] { get; set; }
	}
}
