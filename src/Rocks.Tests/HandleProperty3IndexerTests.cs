using NUnit.Framework;
using Rocks.Exceptions;
using System;

namespace Rocks.Tests
{
	public static class HandleProperty3IndexerTests
	{
		[Test]
		public static void MakeWithGetIndexerProperty()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle(() => 44, () => 45, () => 46, (_, __, ___) => returnValue);

			var chunk = rock.Make();
			var propertyValue = chunk[44, 45, 46];

			Assert.That(propertyValue, Is.EqualTo(returnValue), nameof(propertyValue));
			rock.Verify();
		}

		[Test]
		public static void MakeWithGetIndexerPropertyAndGetNotUsed()
		{
			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle(() => 44, () => 45, () => 46, (_, __, ___) => Guid.NewGuid().ToString());

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetIndexerPropertyAndExpectedCallCount()
		{
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle(() => 44, () => 45, () => 46, (_, __, ___) => returnValue, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[44, 45, 46];
			propertyValue = chunk[44, 45, 46];

			Assert.That(propertyValue, Is.EqualTo(returnValue), nameof(propertyValue));
			rock.Verify();
		}

		[Test]
		public static void MakeWithGetIndexerPropertyAndExpectedCallCountAndGetNotUsed()
		{
			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle(() => 44, () => 45, () => 46, (_, __, ___) => Guid.NewGuid().ToString(), 2);

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetIndexerPropertyAndExpectedCallCountAndGetNotUsedEnough()
		{
			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle(() => 44, () => 45, () => 46, (_, __, ___) => Guid.NewGuid().ToString(), 2);

			var chunk = rock.Make();
			var propertyValue = chunk[44, 45, 46];

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithSetIndexerProperty()
		{
			var indexer1 = Guid.NewGuid();
			var indexer2 = Guid.NewGuid();
			var indexer3 = Guid.NewGuid();
			var indexerSetValue = Guid.NewGuid().ToString();
			string? setValue = null;

			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle<Guid, Guid, Guid, string>(() => indexer1, () => indexer2, () => indexer3, (i1, i2, i3, value) => setValue = value);

			var chunk = rock.Make();
			chunk[indexer1, indexer2, indexer3] = indexerSetValue;

			Assert.That(setValue, Is.EqualTo(indexerSetValue), nameof(setValue));
			rock.Verify();
		}

		[Test]
		public static void MakeWithSetIndexerPropertyAndSetNotUsed()
		{
			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle<Guid, Guid, Guid, string>(() => Guid.NewGuid(), () => Guid.NewGuid(), () => Guid.NewGuid(), (i1, i2, i3, value) => { });

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithSetIndexerPropertyAndExpectedCallCount()
		{
			var indexer1 = Guid.NewGuid();
			var indexer2 = Guid.NewGuid();
			var indexer3 = Guid.NewGuid();
			var indexerSetValue = Guid.NewGuid().ToString();
			string? setValue = null;

			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle<Guid, Guid, Guid, string>(() => indexer1, () => indexer2, () => indexer3, (i1, i2, i3, value) => setValue = value, 2);

			var chunk = rock.Make();
			chunk[indexer1, indexer2, indexer3] = indexerSetValue;
			chunk[indexer1, indexer2, indexer3] = indexerSetValue;

			Assert.That(setValue, Is.EqualTo(indexerSetValue), nameof(setValue));
			rock.Verify();
		}

		[Test]
		public static void MakeWithSetIndexerPropertyAndExpectedCallCountAndSetNotUsed()
		{
			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle<Guid, Guid, Guid, string>(() => Guid.NewGuid(), () => Guid.NewGuid(), () => Guid.NewGuid(), (i1, i2, i3, value) => { }, 2);

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithSetIndexerPropertyAndExpectedCallCountAndSetNotUsedEnough()
		{
			var indexer1 = Guid.NewGuid();
			var indexer2 = Guid.NewGuid();
			var indexer3 = Guid.NewGuid();
			var indexerSetValue = Guid.NewGuid().ToString();
			string? setValue = null;

			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle<Guid, Guid, Guid, string>(() => indexer1, () => indexer2, () => indexer3, (i1, i2, i3, value) => setValue = value, 2);

			var chunk = rock.Make();
			chunk[indexer1, indexer2, indexer3] = indexerSetValue;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetAndSetIndexerProperty()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexer3 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string? setValue = null;

			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle(() => indexer1, () => indexer2, () => indexer3, (_, __, ___) => returnValue, (i1, i2, i3, value) => setValue = value);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1, indexer2, indexer3];
			chunk[indexer1, indexer2, indexer3] = indexerSetValue;

			Assert.That(propertyValue, Is.EqualTo(returnValue), nameof(propertyValue));
			Assert.That(setValue, Is.EqualTo(indexerSetValue), nameof(setValue));
			rock.Verify();
		}

		[Test]
		public static void MakeWithGetAndSetIndexerPropertyAndGetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexer3 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle(() => indexer1, () => indexer2, () => indexer3, (_, __, ___) => returnValue, (i1, i2, i3, value) => { });

			var chunk = rock.Make();
			chunk[indexer1, indexer2, indexer3] = indexerSetValue;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetAndSetIndexerPropertyAndSetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexer3 = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle(() => indexer1, () => indexer2, () => indexer3, (_, __, ___) => returnValue, (i1, i2, i3, value) => { });

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1, indexer2, indexer3];

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetAndSetIndexerPropertyAndExpectedCallCount()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexer3 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();
			string? setValue = null;

			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle(() => indexer1, () => indexer2, () => indexer3, (_, __, ___) => returnValue, (i1, i2, i3, value) => setValue = value, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1, indexer2, indexer3];
			propertyValue = chunk[indexer1, indexer2, indexer3];
			chunk[indexer1, indexer2, indexer3] = indexerSetValue;
			chunk[indexer1, indexer2, indexer3] = indexerSetValue;

			Assert.That(propertyValue, Is.EqualTo(returnValue), nameof(propertyValue));
			Assert.That(setValue, Is.EqualTo(indexerSetValue), nameof(setValue));
			rock.Verify();
		}

		[Test]
		public static void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndGetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexer3 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle(() => indexer1, () => indexer2, () => indexer3, (_, __, ___) => returnValue, (i1, i2, i3, value) => { }, 2);

			var chunk = rock.Make();
			chunk[indexer1, indexer2, indexer3] = indexerSetValue;
			chunk[indexer1, indexer2, indexer3] = indexerSetValue;

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndGetNotUsedEnough()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexer3 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle(() => indexer1, () => indexer2, () => indexer3, (_, __, ___) => returnValue, (i1, i2, i3, value) => { }, 2);

			var chunk = rock.Make();
			chunk[indexer1, indexer2, indexer3] = indexerSetValue;
			chunk[indexer1, indexer2, indexer3] = indexerSetValue;
			var propertyValue = chunk[indexer1, indexer2, indexer3];

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndSetNotUsed()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexer3 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle(() => indexer1, () => indexer2, () => indexer3, (_, __, ___) => returnValue, (i1, i2, i3, value) => { }, 2);

			var chunk = rock.Make();
			var propertyValue = chunk[indexer1, indexer2, indexer3];
			propertyValue = chunk[indexer1, indexer2, indexer3];

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void MakeWithGetAndSetIndexerPropertyAndExpectedCallCountAndSetNotUsedEnough()
		{
			var indexer1 = Guid.NewGuid().ToString();
			var indexer2 = Guid.NewGuid().ToString();
			var indexer3 = Guid.NewGuid().ToString();
			var indexerSetValue = Guid.NewGuid().ToString();
			var returnValue = Guid.NewGuid().ToString();

			var rock = Rock.Create<IProperty3Indexer>();
			rock.Handle(() => indexer1, () => indexer2, () => indexer3, (_, __, ___) => returnValue, (i1, i2, i3, value) => { }, 2);

			var chunk = rock.Make();
			chunk[indexer1, indexer2, indexer3] = indexerSetValue;
			var propertyValue = chunk[indexer1, indexer2, indexer3];
			propertyValue = chunk[indexer1, indexer2, indexer3];

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}
	}

	public interface IProperty3Indexer
	{
		string this[int a, int b, int c] { get; }
		string this[Guid a, Guid b, Guid c] { set; }
		string this[string a, string b, string c] { get; set; }
	}
}
