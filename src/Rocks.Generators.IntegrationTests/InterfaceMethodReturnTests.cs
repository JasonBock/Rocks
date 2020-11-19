using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests
{
	public interface IInterfaceMethodReturnTests
	{
		int NoParameters();
		int OneParameter(int a);
		int MultipleParameters(int a, string b);
	}

	public static class InterfaceMethodReturnTests
	{
		[Test]
		public static void VerifyWithNoParameters()
		{
			var rock = Rock.Create<IInterfaceMethodReturnTests>();
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
		public static void VerifyWithNoParametersWithReturn()
		{
			var rock = Rock.Create<IInterfaceMethodReturnTests>();
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
		public static void VerifyWithNoParametersMultipleCalls()
		{
			var rock = Rock.Create<IInterfaceMethodReturnTests>();
			rock.Methods().NoParameters().CallCount(2);

			var chunk = rock.Instance();
			chunk.NoParameters();
			chunk.NoParameters();

			rock.Verify();
		}

		[Test]
		public static void VerifyWithNoParametersMultipleCallsNotMet()
		{
			var rock = Rock.Create<IInterfaceMethodReturnTests>();
			rock.Methods().NoParameters().CallCount(2);

			var chunk = rock.Instance();
			chunk.NoParameters();

			Assert.Multiple(() =>
			{
				Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
			});
		}

		[Test]
		public static void VerifyWithNoParametersAndCallback()
		{
			var rock = Rock.Create<IInterfaceMethodReturnTests>();
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
		public static void VerifyWithNoParametersNoExpectationSet()
		{
			var rock = Rock.Create<IInterfaceMethodReturnTests>();

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.NoParameters(), Throws.TypeOf<ExpectationException>());
			});
		}

		[Test]
		public static void VerifyWithNoParametersExpectationsNotMet()
		{
			var rock = Rock.Create<IInterfaceMethodReturnTests>();
			rock.Methods().NoParameters();

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
			});
		}

		[Test]
		public static void VerifyWithOneParameter()
		{
			var rock = Rock.Create<IInterfaceMethodReturnTests>();
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
		public static void VerifyWithOneParameterWithReturn()
		{
			var rock = Rock.Create<IInterfaceMethodReturnTests>();
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
		public static void VerifyWithOneParameterWithCallback()
		{
			var rock = Rock.Create<IInterfaceMethodReturnTests>();
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
		public static void VerifyWithOneParameterArgExpectationNotMet()
		{
			var rock = Rock.Create<IInterfaceMethodReturnTests>();
			rock.Methods().OneParameter(3);

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.OneParameter(1), Throws.TypeOf<ExpectationException>());
			});
		}

		[Test]
		public static void VerifyWithMultipleParameters()
		{
			var rock = Rock.Create<IInterfaceMethodReturnTests>();
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
		public static void VerifyWithMultipleParametersWithReturn()
		{
			var rock = Rock.Create<IInterfaceMethodReturnTests>();
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
		public static void VerifyWithMultipleParametersWithCallback()
		{
			var aValue = 0;
			var bValue = string.Empty;
			var rock = Rock.Create<IInterfaceMethodReturnTests>();
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
		public static void VerifyWithMultipleParametersArgExpectationNotMet()
		{
			var rock = Rock.Create<IInterfaceMethodReturnTests>();
			rock.Methods().MultipleParameters(3, "b");

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.MultipleParameters(3, "a"), Throws.TypeOf<ExpectationException>());
			});
		}
	}
}