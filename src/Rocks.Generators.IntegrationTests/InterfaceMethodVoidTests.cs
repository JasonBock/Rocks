using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests
{
	public interface IInterfaceMethodVoidTests
	{
		void NoParameters();
		void OneParameter(int a);
		void MultipleParameters(int a, string b);
	}

	public static class InterfaceMethodVoidTests
	{
		[Test]
		public static void VerifyWithNoParameters()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().NoParameters();

			var chunk = rock.Instance();
			chunk.NoParameters();

			rock.Verify();
		}

		[Test]
		public static void VerifyWithNoParametersAndCallback()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().NoParameters();

			var chunk = rock.Instance();
			chunk.NoParameters();

			rock.Verify();
		}

		[Test]
		public static void VerifyWithNoParametersNoExpectationSet()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.NoParameters(), Throws.TypeOf<ExpectationException>());
			});
		}

		[Test]
		public static void VerifyWithNoParametersExpectationsNotMet()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().NoParameters();

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
			});
		}

		[Test]
		public static void VerifyWithNoParametersMultipleCalls()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().NoParameters().CallCount(2);

			var chunk = rock.Instance();
			chunk.NoParameters();
			chunk.NoParameters();

			rock.Verify();
		}

		[Test]
		public static void VerifyWithNoParametersMultipleCallsExpectationsNotMet()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().NoParameters().CallCount(2);

			var chunk = rock.Instance();
			chunk.NoParameters();

			Assert.Multiple(() =>
			{
				Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
			});
		}

		[Test]
		public static void VerifyWithOneParameter()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().OneParameter(3);

			var chunk = rock.Instance();
			chunk.NoParameters();

			rock.Verify();
		}

		[Test]
		public static void VerifyWithOneParameterNoExpectationSet()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.OneParameter(3), Throws.TypeOf<ExpectationException>());
			});
		}

		[Test]
		public static void VerifyWithOneParameterExpectationsNotMet()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().OneParameter(3);

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
			});
		}

		[Test]
		public static void VerifyWithOneParameterMultipleCalls()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().OneParameter(3).CallCount(2);

			var chunk = rock.Instance();
			chunk.OneParameter(3);
			chunk.OneParameter(3);

			rock.Verify();
		}

		[Test]
		public static void VerifyWithOneParameterMultipleCallsExpectationsNotMet()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().OneParameter(3).CallCount(2);

			var chunk = rock.Instance();
			chunk.OneParameter(3);

			Assert.Multiple(() =>
			{
				Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
			});
		}
	}
}