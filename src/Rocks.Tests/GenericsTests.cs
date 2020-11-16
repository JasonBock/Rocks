using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rocks.Tests
{
	public static class GenericsTests
	{
		[Test]
		public static void HandleWithNoConstraints()
		{
			var expectationB = new Base();
			var argumentA = 0;
			var argumentB = default(Base);

			var rock = Rock.Create<IGenerics>();
			rock.Handle<int, Base, Base>(
				_ => _.TargetWithNoConstraints(1, expectationB),
				(a, b) => { argumentA = a; argumentB = b; return new Base(); });
			var chunk = rock.Make();
			var result = chunk.TargetWithNoConstraints(1, expectationB);

			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.Not.Null, nameof(argumentB));

			rock.Verify();
		}

		[Test]
		public static void TargetWithArgumentsAndReturnTypeUsingGeneric()
		{
			var rock = Rock.Create<IGenerics<int>>();
			rock.Handle(_ => _.Target(Arg.IsAny<IEnumerable<int>>()));

			var chunk = rock.Make();
			chunk.Target(new List<int>());

			rock.Verify();
		}

		[Test]
		public static void TargetWithNonTypeConstrains()
		{
			var expectationB = new Base();
			var argumentA = 0;
			var argumentB = default(Base);

			var rock = Rock.Create<IGenerics>();
			rock.Handle<int, Base, Base>(
				_ => _.TargetWithNonTypeConstrains(1, expectationB),
				(a, b) => { argumentA = a; argumentB = b; return new Base(); });
			var chunk = rock.Make();
			var result = chunk.TargetWithNonTypeConstrains(1, expectationB);

			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.Not.Null, nameof(argumentB));

			rock.Verify();
		}

		[Test]
		public static void TargetWithTypeConstraints()
		{
			var expectationB = new Base();
			var argumentA = 0;
			var argumentB = default(Base);

			var rock = Rock.Create<IGenerics>();
			rock.Handle<int, Base, Base>(
				_ => _.TargetWithTypeConstraints(1, expectationB),
				(a, b) => { argumentA = a; argumentB = b; return new Base(); });
			var chunk = rock.Make();
			var result = chunk.TargetWithTypeConstraints(1, expectationB);

			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.Not.Null, nameof(argumentB));

			rock.Verify();
		}

		[Test]
		public static void TargetWithMultipleConstraints()
		{
			var expectationA = new StringBuilder();
			var expectationB = new Base();
			var expectationC = new InheritingFromBase();
			var expectationD = Guid.NewGuid();
			var expectationE = new InheritingFromBase();

			var argumentA = default(StringBuilder);
			var argumentB = default(Base);
			var argumentC = default(InheritingFromBase);
			var argumentD = default(Guid);
			var argumentE = default(InheritingFromBase);

			var rock = Rock.Create<IGenerics>();
			rock.Handle<StringBuilder, Base, InheritingFromBase, Guid, InheritingFromBase>(
				_ => _.TargetWithMultipleConstraints(expectationA, expectationB, expectationC, expectationD, expectationE),
				(a, b, c, d, e) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; });
			var chunk = rock.Make();
			chunk.TargetWithMultipleConstraints(
				expectationA, expectationB, expectationC, expectationD, expectationE);

			Assert.That(argumentA, Is.Not.Null, nameof(argumentA));
			Assert.That(argumentB, Is.Not.Null, nameof(argumentB));
			Assert.That(argumentC, Is.Not.Null, nameof(argumentC));
			Assert.That(argumentD, Is.Not.EqualTo(Guid.Empty), nameof(argumentD));
			Assert.That(argumentE, Is.Not.Null, nameof(argumentE));

			rock.Verify();
		}
	}

	public class Base { }

	public interface IBase { }

	public class InheritingFromBase : IBase { }

	public interface IGenerics
	{
		U TargetWithNoConstraints<U>(int a, U b);
		U TargetWithNonTypeConstrains<U>(int a, U b) where U : class, new();
		U TargetWithTypeConstraints<U>(int a, U b) where U : Base;
		void TargetWithMultipleConstraints<U, V, W, X, Y>(U a, V b, W c, X d, Y e) where U : class, new() where V : Base where W : IBase where X : struct where Y : W;
	}

	public interface IGenerics<T>
	{
		IEnumerable<T> Target(IEnumerable<T> a);
	}
}
