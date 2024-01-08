using NUnit.Framework;
using Rocks;
using Rocks.Exceptions;
using Rocks.IntegrationTests;

[assembly: RockCreate<ISurfaceV4>]
[assembly: RockMake<ISurfaceV4>]
[assembly: RockCreate<IHavePointersV4>]
[assembly: RockMake<IHavePointersV4>]

namespace Rocks.IntegrationTests;

public interface ISurfaceV4
{
	unsafe void Create<T>(T* allocator) where T : unmanaged;
}

public unsafe interface IHavePointersV4
{
	void DelegatePointerParameter(delegate*<int, void> value);
	delegate*<int, void> DelegatePointerReturn();
	void PointerParameter(int* value);
	int* PointerReturn();
}

public unsafe static class PointerTestsV4
{
	[Test]
	public static void CreateWithPointerTypeParameters()
	{
		var value = 10;
		var pValue = &value;

		var expectations = new ISurfaceV4CreateExpectations();
		expectations.Methods.Create<int>(pValue);

		var mock = expectations.Instance();
		mock.Create(pValue);

		expectations.Verify();
	}

	[Test]
	public static void MakeWithPointerTypeParameters()
	{
		var value = 10;
		var pValue = &value;

		var mock = new ISurfaceV4MakeExpectations().Instance();
		mock.Create(pValue);
	}

	[Test]
	public static void CreateWithPointerParameterNoReturn()
	{
		var value = 10;
		var pValue = &value;

		var expectations = new IHavePointersV4CreateExpectations();
		expectations.Methods.PointerParameter(new(pValue));

		var mock = expectations.Instance();
		mock.PointerParameter(pValue);

		expectations.Verify();
	}

	[Test]
	public static void MakeWithPointerParameterNoReturn()
	{
		var value = 10;
		var pValue = &value;

		var mock = new IHavePointersV4MakeExpectations().Instance();
		mock.PointerParameter(pValue);
	}

	[Test]
	public static void CreateWithPointerParameterNoReturnUsingImplicitConversion()
	{
		var value = 10;
		var pValue = &value;

		var expectations = new IHavePointersV4CreateExpectations();
		expectations.Methods.PointerParameter(pValue);

		var mock = expectations.Instance();
		mock.PointerParameter(pValue);

		expectations.Verify();
	}

	[Test]
	public static void CreateWithPointerParameterNoReturnExpectationsNotMet()
	{
		var value = 10;
		var pValue = &value;
		var otherValue = 10;
		var pOtherValue = &otherValue;

		var expectations = new IHavePointersV4CreateExpectations();
		expectations.Methods.PointerParameter(pValue);

		var mock = expectations.Instance();

		Assert.That(() => mock.PointerParameter(pOtherValue), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithPointerParameterWithCallbackNoReturn()
	{
		unsafe static void PointerParameterCallback(int* callbackValue) => *callbackValue = 20;

		var value = 10;
		var pValue = &value;

		var expectations = new IHavePointersV4CreateExpectations();
		expectations.Methods.PointerParameter(pValue).Callback(PointerParameterCallback);

		var mock = expectations.Instance();
		mock.PointerParameter(pValue);

		expectations.Verify();

		Assert.That(value, Is.EqualTo(20));
	}

	[Test]
	public static void CreateWithPointerReturn()
	{
		var expectations = new IHavePointersV4CreateExpectations();
		expectations.Methods.PointerReturn();

		var mock = expectations.Instance();
		mock.PointerReturn();

		expectations.Verify();
	}

	[Test]
	public static void MakeWithPointerReturn() =>
		new IHavePointersV4MakeExpectations().Instance().PointerReturn();

	[Test]
	public static void CreateWithPointerReturnWithCallback()
	{
		unsafe static int* PointerReturnCallback() => default;

		var expectations = new IHavePointersV4CreateExpectations();
		expectations.Methods.PointerReturn().Callback(PointerReturnCallback);

		var mock = expectations.Instance();
		mock.PointerReturn();

		expectations.Verify();
	}

	[Test]
	public static void CreateWithPointerReturnWithResult()
	{
		var expectations = new IHavePointersV4CreateExpectations();
		expectations.Methods.PointerReturn().ReturnValue(default);

		var mock = expectations.Instance();
		mock.PointerReturn();

		expectations.Verify();
	}

	[Test]
	public static void CreateWithDelegatePointerParameterNoReturn()
	{
		static void DelegatePointerParameterDelegate(int a) { }

		var expectations = new IHavePointersV4CreateExpectations();
		expectations.Methods.DelegatePointerParameter(new(&DelegatePointerParameterDelegate));

		var mock = expectations.Instance();
		mock.DelegatePointerParameter(&DelegatePointerParameterDelegate);

		expectations.Verify();
	}

	[Test]
	public static void CreateWithDelegatePointerParameterNoReturnExpectationsNotMet()
	{
		static void DelegatePointerParameterDelegate(int a) { }
		static void OtherDelegatePointerParameterDelegate(int a) { }

		var expectations = new IHavePointersV4CreateExpectations();
		expectations.Methods.DelegatePointerParameter(new(&DelegatePointerParameterDelegate));

		var mock = expectations.Instance();

		Assert.That(() => mock.DelegatePointerParameter(&OtherDelegatePointerParameterDelegate), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithDelegatePointerParameterWithCallbackNoReturn()
	{
		var wasCalled = false;
		static void DelegatePointerParameterDelegate(int a) { }
		unsafe void DelegatePointerParameterCallback(delegate*<int, void> value) => wasCalled = true;

		var value = 10;
		var pValue = &value;

		var expectations = new IHavePointersV4CreateExpectations();
		expectations.Methods.DelegatePointerParameter(new(&DelegatePointerParameterDelegate))
			.Callback(DelegatePointerParameterCallback);

		var mock = expectations.Instance();
		mock.DelegatePointerParameter(&DelegatePointerParameterDelegate);

		expectations.Verify();

		Assert.That(wasCalled, Is.True);
	}

	[Test]
	public static void CreateWithDelegatePointerReturn()
	{
		var expectations = new IHavePointersV4CreateExpectations();
		expectations.Methods.DelegatePointerReturn();

		var mock = expectations.Instance();
		mock.DelegatePointerReturn();

		expectations.Verify();
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

		var expectations = new IHavePointersV4CreateExpectations();
		expectations.Methods.DelegatePointerReturn().Callback(DelegatePointerReturnCallback);

		var mock = expectations.Instance();
		var value = mock.DelegatePointerReturn();

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(wasCalled, Is.True);
		});
	}

	[Test]
	public static void CreateWithDelegatePointerReturnWithResult()
	{
		static void DelegatePointerParameterDelegate(int a) { }

		var expectations = new IHavePointersV4CreateExpectations();
		expectations.Methods.DelegatePointerReturn().Returns(&DelegatePointerParameterDelegate);

		var mock = expectations.Instance();
		mock.DelegatePointerReturn();

		expectations.Verify();
	}
}