using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests
{
	public unsafe interface IHavePointers
	{
		void PointerParameter(int* value);
		int* PointerReturn();
	}

	public unsafe static class PointerTests
	{
		[Test]
		public static void CreateWithParameterNoReturn()
		{
			var value = 10;
			var pValue = &value;

			var rock = Rock.Create<IHavePointers>();
			rock.Methods().PointerParameter(ArgumentForintPointer.Is(pValue));

			var chunk = rock.Instance();
			chunk.PointerParameter(pValue);

			rock.Verify();
		}

		[Test]
		public static void CreateWithParameterNoReturnUsingImplicitConversion()
		{
			var value = 10;
			var pValue = &value;

			var rock = Rock.Create<IHavePointers>();
			rock.Methods().PointerParameter(pValue);

			var chunk = rock.Instance();
			chunk.PointerParameter(pValue);

			rock.Verify();
		}

		[Test]
		public static void CreateWithParameterNoReturnExpectationsNotMet()
		{
			var value = 10;
			var pValue = &value;
			var otherValue = 10;
			var pOtherValue = &otherValue;

			var rock = Rock.Create<IHavePointers>();
			rock.Methods().PointerParameter(pValue);

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.PointerParameter(pOtherValue), Throws.TypeOf<ExpectationException>());
			});
		}

		[Test]
		public static void CreateWithParameterWithCallbackNoReturn()
		{
			unsafe static void PointerParameterCallback(int* callbackValue) => *callbackValue = 20;

			var value = 10;
			var pValue = &value;

			var rock = Rock.Create<IHavePointers>();
			rock.Methods().PointerParameter(pValue).Callback(PointerParameterCallback);

			var chunk = rock.Instance();
			chunk.PointerParameter(pValue);

			rock.Verify();

			Assert.That(value, Is.EqualTo(20));
		}

		[Test]
		public static void CreateWithReturn()
		{
			var rock = Rock.Create<IHavePointers>();
			rock.Methods().PointerReturn();

			var chunk = rock.Instance();
			chunk.PointerReturn();

			rock.Verify();
		}

		[Test]
		public static void CreateWithReturnWithCallback()
		{
			unsafe static int* PointerReturnCallback() => default;

			var rock = Rock.Create<IHavePointers>();
			rock.Methods().PointerReturn().Callback(PointerReturnCallback);

			var chunk = rock.Instance();
			chunk.PointerReturn();

			rock.Verify();
		}

		[Test]
		public static void CreateWithReturnWithResult()
		{
			var rock = Rock.Create<IHavePointers>();
			rock.Methods().PointerReturn().Returns(default);

			var chunk = rock.Instance();
			chunk.PointerReturn();

			rock.Verify();
		}
	}
}