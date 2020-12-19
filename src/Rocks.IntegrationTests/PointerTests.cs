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
			rock.Methods().PointerParameter(new ArgForintPointer(pValue));

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
			rock.Methods().PointerParameter(new ArgForintPointer(pValue));

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.PointerParameter(pOtherValue), Throws.TypeOf<ExpectationException>());
			});
		}

		[Test]
		public static void CreateWithParameterWithCallbackNoReturn()
		{

		}

		[Test]
		public static void CreateWithReturn()
		{

		}

		[Test]
		public static void CreateWithReturnWithCallback()
		{

		}

		[Test]
		public static void CreateWithReturnWithResult()
		{

		}
	}
}