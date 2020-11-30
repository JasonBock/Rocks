using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests
{
	public abstract class AbstractClassMethodReturn
	{
		public abstract int NoParameters();
		public abstract int OneParameter(int a);
		public abstract int MultipleParameters(int a, string b);
	}

	public static class AbstractClassMethodReturnTests
	{
		[Test]
		public static void MockWithNoParameters()
		{
			var rock = Rock.Create<AbstractClassMethodReturn>();
			rock.Methods().NoParameters();

			var chunk = rock.Instance();
			var value = chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void MockWithNoParametersWithReturn()
		{
			var rock = Rock.Create<AbstractClassMethodReturn>();
			rock.Methods().NoParameters().Returns(3);

			var chunk = rock.Instance();
			var value = chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(3));
			});
		}

		[Test]
		public static void MockWithNoParametersMultipleCalls()
		{
			var rock = Rock.Create<AbstractClassMethodReturn>();
			rock.Methods().NoParameters().CallCount(2);

			var chunk = rock.Instance();
			chunk.NoParameters();
			chunk.NoParameters();

			rock.Verify();
		}

		[Test]
		public static void MockWithNoParametersMultipleCallsNotMet()
		{
			var rock = Rock.Create<AbstractClassMethodReturn>();
			rock.Methods().NoParameters().CallCount(2);

			var chunk = rock.Instance();
			chunk.NoParameters();

			Assert.Multiple(() =>
			{
				Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
			});
		}

		[Test]
		public static void MockWithNoParametersAndCallback()
		{
			var rock = Rock.Create<AbstractClassMethodReturn>();
			rock.Methods().NoParameters().Callback(() => 3);

			var chunk = rock.Instance();
			var value = chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(3));
			});
		}

		[Test]
		public static void MockWithNoParametersNoExpectationSet()
		{
			var rock = Rock.Create<AbstractClassMethodReturn>();

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.NoParameters(), Throws.TypeOf<ExpectationException>());
			});
		}

		[Test]
		public static void MockWithNoParametersExpectationsNotMet()
		{
			var rock = Rock.Create<AbstractClassMethodReturn>();
			rock.Methods().NoParameters();

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
			});
		}

		[Test]
		public static void MockWithOneParameter()
		{
			var rock = Rock.Create<AbstractClassMethodReturn>();
			rock.Methods().OneParameter(3);

			var chunk = rock.Instance();
			var value = chunk.OneParameter(3);

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void MockWithOneParameterWithReturn()
		{
			var rock = Rock.Create<AbstractClassMethodReturn>();
			rock.Methods().OneParameter(3).Returns(3);

			var chunk = rock.Instance();
			var value = chunk.OneParameter(3);

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(3));
			});
		}

		[Test]
		public static void MockWithOneParameterWithCallback()
		{
			var rock = Rock.Create<AbstractClassMethodReturn>();
			rock.Methods().OneParameter(3).Callback(_ => 3);

			var chunk = rock.Instance();
			var value = chunk.OneParameter(3);

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(3));
			});
		}

		[Test]
		public static void MockWithOneParameterArgExpectationNotMet()
		{
			var rock = Rock.Create<AbstractClassMethodReturn>();
			rock.Methods().OneParameter(3);

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.OneParameter(1), Throws.TypeOf<ExpectationException>());
			});
		}

		[Test]
		public static void MockWithMultipleParameters()
		{
			var rock = Rock.Create<AbstractClassMethodReturn>();
			rock.Methods().MultipleParameters(3, "b");

			var chunk = rock.Instance();
			var value = chunk.MultipleParameters(3, "b");

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void MockWithMultipleParametersWithReturn()
		{
			var rock = Rock.Create<AbstractClassMethodReturn>();
			rock.Methods().MultipleParameters(3, "b").Returns(3);

			var chunk = rock.Instance();
			var value = chunk.MultipleParameters(3, "b");

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(3));
			});
		}

		[Test]
		public static void MockWithMultipleParametersWithCallback()
		{
			var aValue = 0;
			var bValue = string.Empty;
			var rock = Rock.Create<AbstractClassMethodReturn>();
			rock.Methods().MultipleParameters(3, "b").Callback((a, b) =>
			{
				(aValue, bValue) = (a, b);
				return 3;
			});

			var chunk = rock.Instance();
			var value = chunk.MultipleParameters(3, "b");

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(aValue, Is.EqualTo(3));
				Assert.That(bValue, Is.EqualTo("b"));
				Assert.That(value, Is.EqualTo(3));
			});
		}

		[Test]
		public static void MockWithMultipleParametersArgExpectationNotMet()
		{
			var rock = Rock.Create<AbstractClassMethodReturn>();
			rock.Methods().MultipleParameters(3, "b");

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.MultipleParameters(3, "a"), Throws.TypeOf<ExpectationException>());
			});
		}
	}
}