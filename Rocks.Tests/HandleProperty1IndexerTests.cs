using NUnit.Framework;
using Rocks.Exceptions;
using System;

namespace Rocks.Tests
{
	public static class HandleProperty1IndexerTests
	{
		[Test]
		public static void MakeWithGetIndexerProperty()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle(() => 44, _ => returnValue);

			var chunk = rock.Make();
			var propertyValue = chunk[44];

			Assert.That(propertyValue, Is.EqualTo(returnValue), nameof(propertyValue));
			rock.Verify();
		}

		[Test]
		public static void MakeWithGetIndexerPropertyAndGetNotUsed()
		{
			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle(() => 44, _ => Guid.NewGuid().ToString());

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetIndexerPropertyAndExpectedCallCount()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle(() => 44, _ => returnValue, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[44];
			propertyValue = chunk[44];

			Assert.That(propertyValue, Is.EqualTo(returnValue), nameof(propertyValue));
			rock.Verify();
		}

		[Test]
		public static void MakeWithGetIndexerPropertyAndExpectedCallCountAndGetNotUsed()
		{
			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle(() => 44, _ => Guid.NewGuid().ToString(), 2);

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetIndexerPropertyAndExpectedCallCountAndGetNotUsedEnough()
		{
			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle(() => 44, _ => Guid.NewGuid().ToString(), 2);

			var chunk = rock.Make();
			var propertyValue = chunk[44];

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithSetIndexerProperty()
		{
			var indexer1 = Guid.NewGuid();
			var indexerSetValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle<Guid, string>(() => indexer1, (i1, value) => setValue = value);

			var chunk = rock.Make();
			chunk[indexer1] = indexerSetValue;

			Assert.That(setValue, Is.EqualTo(indexerSetValue), nameof(setValue));
			rock.Verify();
		}

		[Test]
		public static void MakeWithSetIndexerPropertyAndSetNotUsed()
		{
			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle<Guid, string>(() => Guid.NewGuid(), (i1, value) => { });

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithSetIndexerPropertyAndExpectedCallCount()
		{
			var indexer1 = Guid.NewGuid();
			var indexerSetValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle<Guid, string>(() => indexer1, (i1, value) => setValue = value, 2);

			var chunk = rock.Make();
			chunk[indexer1] = indexerSetValue;
			chunk[indexer1] = indexerSetValue;

			Assert.That(setValue, Is.EqualTo(indexerSetValue), nameof(setValue));
			rock.Verify();
		}

		[Test]
		public static void MakeWithSetIndexerPropertyAndExpectedCallCountAndSetNotUsed()
		{
			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle<Guid, string>(() => Guid.NewGuid(), (i1, value) => { }, 2);

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithSetIndexerPropertyAndExpectedCallCountAndSetNotUsedEnough()
		{
			var indexer1 = Guid.NewGuid();
			string propertyValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle<Guid, string>(() => indexer1, (i1, value) => propertyValue = value, 2);

			var chunk = rock.Make();
			chunk[indexer1] = indexer1.ToString();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetAndSetIndexerProperty()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle(() => indexer1, _ => returnValue, (i1, value) => setValue = value);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1];
			chunk[indexer1] = indexerSetValue;

			Assert.That(propertyValue, Is.EqualTo(returnValue), nameof(propertyValue));
			Assert.That(setValue, Is.EqualTo(indexerSetValue), nameof(setValue));
			rock.Verify();
		}

		[Test]
		public static void MakeWithGetAndSetIndexerPropertyAndGetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle(() => indexer1, _ => returnValue, (i1, value) => setValue = value);

			var chunk = rock.Make();
			chunk[indexer1] = indexerSetValue;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetAndSetIndexerPropertyAndSetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle(() => indexer1, _ => returnValue, (i1, value) => setValue = value);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1];

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetAndSetIndexerPropertyAndExpectedCallCount()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle(() => indexer1, _ => returnValue, (i1, value) => setValue = value, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1];
			propertyValue = chunk[indexer1];
			chunk[indexer1] = indexerSetValue;
			chunk[indexer1] = indexerSetValue;

			Assert.That(propertyValue, Is.EqualTo(returnValue), nameof(propertyValue));
			Assert.That(setValue, Is.EqualTo(indexerSetValue), nameof(setValue));
			rock.Verify();
		}

		[Test]
		public static void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndGetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle(() => indexer1, _ => returnValue, (i1, value) => setValue = value, 2);

			var chunk = rock.Make();
			chunk[indexer1] = indexerSetValue;
			chunk[indexer1] = indexerSetValue;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndGetNotUsedEnough()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle(() => indexer1, _ => returnValue, (i1, value) => setValue = value, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1];
			chunk[indexer1] = indexerSetValue;
			chunk[indexer1] = indexerSetValue;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndSetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle(() => indexer1, _ => returnValue, (i1, value) => setValue = value, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1];
			propertyValue = chunk[indexer1];

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndSetNotUsedEnough()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string setValue = null;

			var rock = Rock.Create<IProperty1Indexer>();
			rock.Handle(() => indexer1, _ => returnValue, (i1, value) => setValue = value, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1];
			propertyValue = chunk[indexer1];
			chunk[indexer1] = indexerSetValue;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}
	}

	public interface IProperty1Indexer
	{
		string this[int a] { get; }
		string this[Guid a] { set; }
		string this[string a] { get; set; }
	}
}
