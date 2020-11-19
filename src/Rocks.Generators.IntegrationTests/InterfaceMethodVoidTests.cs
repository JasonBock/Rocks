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
		public static void MockWithNoParameters()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().NoParameters();

			var chunk = rock.Instance();
			chunk.NoParameters();

			rock.Verify();
		}

		[Test]
		public static void MockWithNoParametersMultipleCalls()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().NoParameters().CallCount(2);

			var chunk = rock.Instance();
			chunk.NoParameters();
			chunk.NoParameters();

			rock.Verify();
		}

		[Test]
		public static void MockWithNoParametersMultipleCallsNotMet()
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
		public static void MockWithNoParametersAndCallback()
		{
			var wasCallbackInvoked = false;

			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().NoParameters().Callback(() => wasCallbackInvoked = true);

			var chunk = rock.Instance();
			chunk.NoParameters();

			Assert.Multiple(() =>
			{
				Assert.That(wasCallbackInvoked, Is.True);
			});
			rock.Verify();
		}

		[Test]
		public static void MockWithNoParametersNoExpectationSet()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.NoParameters(), Throws.TypeOf<ExpectationException>());
			});
		}

		[Test]
		public static void MockWithNoParametersExpectationsNotMet()
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
		public static void MockWithOneParameter()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().OneParameter(3);

			var chunk = rock.Instance();
			chunk.OneParameter(3);

			rock.Verify();
		}

		[Test]
		public static void MockWithOneParameterWithCallback()
		{
			var aValue = 0;
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().OneParameter(3).Callback(a => aValue = a);

			var chunk = rock.Instance();
			chunk.OneParameter(3);

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(aValue, Is.EqualTo(3));
			});
		}

		[Test]
		public static void MockWithOneParameterArgExpectationNotMet()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
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
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().MultipleParameters(3, "b");

			var chunk = rock.Instance();
			chunk.MultipleParameters(3, "b");

			rock.Verify();
		}

		[Test]
		public static void MockWithMultipleParametersWithCallback()
		{
			var aValue = 0;
			var bValue = string.Empty;
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().MultipleParameters(3, "b").Callback((a, b) => (aValue, bValue) = (a, b));

			var chunk = rock.Instance();
			chunk.MultipleParameters(3, "b");

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(aValue, Is.EqualTo(3));
				Assert.That(bValue, Is.EqualTo("b"));
			});
		}

		[Test]
		public static void MockWithMultipleParametersArgExpectationNotMet()
		{
			var rock = Rock.Create<IInterfaceMethodVoidTests>();
			rock.Methods().MultipleParameters(3, "b");

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.MultipleParameters(3, "a"), Throws.TypeOf<ExpectationException>());
			});
		}
	}
}