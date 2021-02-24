using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests
{
	public unsafe interface IHavePointers
	{
		void DelegatePointerParameter(delegate*<int, void> value);
		delegate*<int, void> DelegatePointerReturn();
		void PointerParameter(int* value);
		int* PointerReturn();
	}

	public unsafe static class PointerTests
	{
		[Test]
		public static void CreateWithPointerParameterNoReturn()
		{
			var value = 10;
			var pValue = &value;

			var rock = Rock.Create<IHavePointers>();
			rock.Methods().PointerParameter(new(pValue));

			var chunk = rock.Instance();
			chunk.PointerParameter(pValue);

			rock.Verify();
		}

		[Test]
		public static void MakeWithPointerParameterNoReturn()
		{
			var value = 10;
			var pValue = &value;

			var chunk = Rock.Make<IHavePointers>().Instance();
			chunk.PointerParameter(pValue);
		}

		[Test]
		public static void CreateWithPointerParameterNoReturnUsingImplicitConversion()
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
		public static void CreateWithPointerParameterNoReturnExpectationsNotMet()
		{
			var value = 10;
			var pValue = &value;
			var otherValue = 10;
			var pOtherValue = &otherValue;

			var rock = Rock.Create<IHavePointers>();
			rock.Methods().PointerParameter(pValue);

			var chunk = rock.Instance();

			Assert.That(() => chunk.PointerParameter(pOtherValue), Throws.TypeOf<ExpectationException>());
		}

		[Test]
		public static void CreateWithPointerParameterWithCallbackNoReturn()
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
		public static void CreateWithPointerReturn()
		{
			var rock = Rock.Create<IHavePointers>();
			rock.Methods().PointerReturn();

			var chunk = rock.Instance();
			chunk.PointerReturn();

			rock.Verify();
		}

		[Test]
		public static void MakeWithPointerReturn() =>
			Rock.Make<IHavePointers>().Instance().PointerReturn();

		[Test]
		public static void CreateWithPointerReturnWithCallback()
		{
			unsafe static int* PointerReturnCallback() => default;

			var rock = Rock.Create<IHavePointers>();
			rock.Methods().PointerReturn().Callback(PointerReturnCallback);

			var chunk = rock.Instance();
			chunk.PointerReturn();

			rock.Verify();
		}

		[Test]
		public static void CreateWithPointerReturnWithResult()
		{
			var rock = Rock.Create<IHavePointers>();
			rock.Methods().PointerReturn().Returns(default);

			var chunk = rock.Instance();
			chunk.PointerReturn();

			rock.Verify();
		}

		[Test]
		public static void CreateWithDelegatePointerParameterNoReturn()
		{
			static void DelegatePointerParameterDelegate(int a) { }

			var rock = Rock.Create<IHavePointers>();
			rock.Methods().DelegatePointerParameter(new(&DelegatePointerParameterDelegate));

			var chunk = rock.Instance();
			chunk.DelegatePointerParameter(&DelegatePointerParameterDelegate);

			rock.Verify();
		}

		[Test]
		public static void CreateWithDelegatePointerParameterNoReturnExpectationsNotMet()
		{
			static void DelegatePointerParameterDelegate(int a) { }
			static void OtherDelegatePointerParameterDelegate(int a) { }

			var rock = Rock.Create<IHavePointers>();
			rock.Methods().DelegatePointerParameter(new(&DelegatePointerParameterDelegate));

			var chunk = rock.Instance();

			Assert.That(() => chunk.DelegatePointerParameter(&OtherDelegatePointerParameterDelegate), Throws.TypeOf<ExpectationException>());
		}

		[Test]
		public static void CreateWithDelegatePointerParameterWithCallbackNoReturn()
		{
			var wasCalled = false;
			static void DelegatePointerParameterDelegate(int a) { }
			unsafe void DelegatePointerParameterCallback(delegate*<int, void> value) => wasCalled = true;

			var value = 10;
			var pValue = &value;

			var rock = Rock.Create<IHavePointers>();
			rock.Methods().DelegatePointerParameter(new(&DelegatePointerParameterDelegate))
				.Callback(DelegatePointerParameterCallback);

			var chunk = rock.Instance();
			chunk.DelegatePointerParameter(&DelegatePointerParameterDelegate);

			rock.Verify();

			Assert.That(wasCalled, Is.True);
		}

		[Test]
		public static void CreateWithDelegatePointerReturn()
		{
			var rock = Rock.Create<IHavePointers>();
			rock.Methods().DelegatePointerReturn();

			var chunk = rock.Instance();
			chunk.DelegatePointerReturn();

			rock.Verify();
		}

		[Test]
		public static void CreateWithDelegatePointerReturnWithCallback()
		{
			static void DelegatePointerParameterDelegate(int a) { }

			var wasCalled = false;

			unsafe delegate*<int, void> DelegatePointerReturnCallback()
			{
				wasCalled = true;
				return &DelegatePointerParameterDelegate;
			}

			var rock = Rock.Create<IHavePointers>();
			rock.Methods().DelegatePointerReturn().Callback(DelegatePointerReturnCallback);

			var chunk = rock.Instance();
			var value = chunk.DelegatePointerReturn();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasCalled, Is.True);
			});
		}

		[Test]
		public static void CreateWithDelegatePointerReturnWithResult()
		{
			static void DelegatePointerParameterDelegate(int a) { }

			var rock = Rock.Create<IHavePointers>();
			rock.Methods().DelegatePointerReturn().Returns(&DelegatePointerParameterDelegate);

			var chunk = rock.Instance();
			chunk.DelegatePointerReturn();

			rock.Verify();
		}
	}
}