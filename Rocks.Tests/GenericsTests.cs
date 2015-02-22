using NUnit.Framework;
using System;
using System.Text;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class GenericsTests
	{
		[Test]
		public void HandleWithNoConstraints()
		{
			var argumentA = 0;
			var argumentB = default(Base);

			var rock = Rock.Create<IGenerics>();
			rock.HandleFunc<int, Base, Base>(
				_ => _.TargetWithNoConstraints(default(int), default(Base)),
				(a, b) => { argumentA = a; argumentB = b; return new Base(); });
			var chunk = rock.Make();
			var result = chunk.TargetWithNoConstraints<Base>(1, new Base());

			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.IsNotNull(argumentB, nameof(argumentB));

			rock.Verify();
		}

		[Test]
		public void TargetWithNonTypeConstrains()
		{
			var argumentA = 0;
			var argumentB = default(Base);

			var rock = Rock.Create<IGenerics>();
			rock.HandleFunc<int, Base, Base>(
				_ => _.TargetWithNonTypeConstrains(default(int), default(Base)),
				(a, b) => { argumentA = a; argumentB = b; return new Base(); });
			var chunk = rock.Make();
			var result = chunk.TargetWithNonTypeConstrains<Base>(1, new Base());

			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.IsNotNull(argumentB, nameof(argumentB));

			rock.Verify();
		}

		[Test]
		public void TargetWithTypeConstraints()
		{
			var argumentA = 0;
			var argumentB = default(Base);

			var rock = Rock.Create<IGenerics>();
			rock.HandleFunc<int, Base, Base>(
				_ => _.TargetWithTypeConstraints(default(int), default(Base)),
				(a, b) => { argumentA = a; argumentB = b; return new Base(); });
			var chunk = rock.Make();
			var result = chunk.TargetWithTypeConstraints<Base>(1, new Base());

			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.IsNotNull(argumentB, nameof(argumentB));

			rock.Verify();
		}

		[Test]
		public void TargetWithMultipleConstraints()
		{
			var argumentA = default(StringBuilder);
			var argumentB = default(Base);
			var argumentC = default(InheritingFromBase);
			var argumentD = default(Guid);
			var argumentE = default(InheritingFromBase);

			var rock = Rock.Create<IGenerics>();
			rock.HandleAction<StringBuilder, Base, InheritingFromBase, Guid, InheritingFromBase>(
				_ => _.TargetWithMultipleConstraints(default(StringBuilder), default(Base), default(InheritingFromBase), default(Guid), default(InheritingFromBase)),
				(a, b, c, d, e) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; });
			var chunk = rock.Make();
			chunk.TargetWithMultipleConstraints<StringBuilder, Base, InheritingFromBase, Guid, InheritingFromBase>(
				new StringBuilder(), new Base(), new InheritingFromBase(), Guid.NewGuid(), new InheritingFromBase());

			Assert.IsNotNull(argumentA, nameof(argumentB));
			Assert.IsNotNull(argumentB, nameof(argumentB));
			Assert.IsNotNull(argumentC, nameof(argumentC));
			Assert.AreNotEqual(Guid.Empty, argumentD, nameof(argumentD));
			Assert.IsNotNull(argumentE, nameof(argumentE));

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
}
