using NUnit.Framework;

namespace Rocks.IntegrationTests
{
	public unsafe interface IHavePointers
	{
		void PointerParameter(int* value);
		int* PointerReturn();
	}

	public static class PointerTests
	{
		[Test]
		public static void CreateWithParameterNoReturn()
		{
			//var rock = Rock.Create<IHavePointers>();
			//rock.Methods()
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