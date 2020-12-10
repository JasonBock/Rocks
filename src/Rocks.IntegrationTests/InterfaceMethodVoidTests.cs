using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests
{
	public interface IInterfaceMethodVoid
	{
		void NoParameters();
		void OneParameter(int a);
		void MultipleParameters(int a, string b);
	}

	public static class InterfaceMethodVoidTests
	{
		[Test]
		public static void CreateWithNoParameters()
		{
			var rock = Rock.Create<IInterfaceMethodVoid>();
			rock.Methods().NoParameters();

			var chunk = rock.Instance();
			chunk.NoParameters();

			rock.Verify();
		}

		[Test]
		public static void MakeWithNoParameters()
		{
			var chunk = Rock.Make<IInterfaceMethodVoid>().Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.NoParameters(), Throws.Nothing);
			});
		}

		[Test]
		public static void CreateWithNoParametersMultipleCalls()
		{
			var rock = Rock.Create<IInterfaceMethodVoid>();
			rock.Methods().NoParameters().CallCount(2);

			var chunk = rock.Instance();
			chunk.NoParameters();
			chunk.NoParameters();

			rock.Verify();
		}

		[Test]
		public static void CreateWithNoParametersMultipleCallsNotMet()
		{
			var rock = Rock.Create<IInterfaceMethodVoid>();
			rock.Methods().NoParameters().CallCount(2);

			var chunk = rock.Instance();
			chunk.NoParameters();

			Assert.Multiple(() =>
			{
				Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
			});
		}

		[Test]
		public static void CreateWithNoParametersAndCallback()
		{
			var wasCallbackInvoked = false;

			var rock = Rock.Create<IInterfaceMethodVoid>();
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
		public static void CreateWithNoParametersNoExpectationSet()
		{
			var rock = Rock.Create<IInterfaceMethodVoid>();

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.NoParameters(), Throws.TypeOf<ExpectationException>());
			});
		}

		[Test]
		public static void CreateWithNoParametersExpectationsNotMet()
		{
			var rock = Rock.Create<IInterfaceMethodVoid>();
			rock.Methods().NoParameters();

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
			});
		}

		[Test]
		public static void CreateWithOneParameter()
		{
			var rock = Rock.Create<IInterfaceMethodVoid>();
			rock.Methods().OneParameter(3);

			var chunk = rock.Instance();
			chunk.OneParameter(3);

			rock.Verify();
		}

		[Test]
		public static void MakeWithOneParameter()
		{
			var chunk = Rock.Make<IInterfaceMethodVoid>().Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.OneParameter(3), Throws.Nothing);
			});
		}

		[Test]
		public static void CreateWithOneParameterWithCallback()
		{
			var aValue = 0;
			var rock = Rock.Create<IInterfaceMethodVoid>();
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
		public static void CreateWithOneParameterArgExpectationNotMet()
		{
			var rock = Rock.Create<IInterfaceMethodVoid>();
			rock.Methods().OneParameter(3);

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.OneParameter(1), Throws.TypeOf<ExpectationException>());
			});
		}

		[Test]
		public static void CreateWithMultipleParameters()
		{
			var rock = Rock.Create<IInterfaceMethodVoid>();
			rock.Methods().MultipleParameters(3, "b");

			var chunk = rock.Instance();
			chunk.MultipleParameters(3, "b");

			rock.Verify();
		}

		[Test]
		public static void MakeWithMultipleParameters()
		{
			var chunk = Rock.Make<IInterfaceMethodVoid>().Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.MultipleParameters(3, "b"), Throws.Nothing);
			});
		}

		[Test]
		public static void CreateWithMultipleParametersWithCallback()
		{
			var aValue = 0;
			var bValue = string.Empty;
			var rock = Rock.Create<IInterfaceMethodVoid>();
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
		public static void CreateWithMultipleParametersArgExpectationNotMet()
		{
			var rock = Rock.Create<IInterfaceMethodVoid>();
			rock.Methods().MultipleParameters(3, "b");

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.MultipleParameters(3, "a"), Throws.TypeOf<ExpectationException>());
			});
		}
	}
}