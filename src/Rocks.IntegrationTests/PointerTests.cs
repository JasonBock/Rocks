using NUnit.Framework;
using Rocks;
using Rocks.Exceptions;
using Rocks.IntegrationTests;

[assembly: RockCreate<ISurface>]
[assembly: RockMake<ISurface>]
[assembly: RockCreate<IHavePointers>]
[assembly: RockMake<IHavePointers>]

namespace Rocks.IntegrationTests;

public interface ISurface
{
	unsafe void Create<T>(T* allocator) where T : unmanaged;
}

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
	public static void CreateWithPointerTypeParameters()
	{
		var value = 10;
		var pValue = &value;

		var expectations = new ISurfaceCreateExpectations();
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

		var mock = new ISurfaceMakeExpectations().Instance();
		mock.Create(pValue);
	}

	[Test]
	public static void CreateWithPointerParameterNoReturn()
	{
		var value = 10;
		var pValue = &value;

		var expectations = new IHavePointersCreateExpectations();
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

		var mock = new IHavePointersMakeExpectations().Instance();
		mock.PointerParameter(pValue);
	}

	[Test]
	public static void CreateWithPointerParameterNoReturnUsingImplicitConversion()
	{
		var value = 10;
		var pValue = &value;

		var expectations = new IHavePointersCreateExpectations();
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

		var expectations = new IHavePointersCreateExpectations();
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

		var expectations = new IHavePointersCreateExpectations();
		expectations.Methods.PointerParameter(pValue).Callback(PointerParameterCallback);

		var mock = expectations.Instance();
		mock.PointerParameter(pValue);

		expectations.Verify();

		Assert.That(value, Is.EqualTo(20));
	}

	[Test]
	public static void CreateWithPointerReturn()
	{
		var expectations = new IHavePointersCreateExpectations();
		expectations.Methods.PointerReturn();

		var mock = expectations.Instance();
		mock.PointerReturn();

		expectations.Verify();
	}

	[Test]
	public static void MakeWithPointerReturn() =>
		new IHavePointersMakeExpectations().Instance().PointerReturn();

	[Test]
	public static void CreateWithPointerReturnWithCallback()
	{
		unsafe static int* PointerReturnCallback() => default;

		var expectations = new IHavePointersCreateExpectations();
		expectations.Methods.PointerReturn().Callback(PointerReturnCallback);

		var mock = expectations.Instance();
		mock.PointerReturn();

		expectations.Verify();
	}

	[Test]
	public static void CreateWithPointerReturnWithResult()
	{
		var expectations = new IHavePointersCreateExpectations();
		expectations.Methods.PointerReturn().ReturnValue(default);

		var mock = expectations.Instance();
		mock.PointerReturn();

		expectations.Verify();
	}

	[Test]
	public static void CreateWithDelegatePointerParameterNoReturn()
	{
		static void DelegatePointerParameterDelegate(int a) { }

		var expectations = new IHavePointersCreateExpectations();
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

		var expectations = new IHavePointersCreateExpectations();
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

		var expectations = new IHavePointersCreateExpectations();
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
		var expectations = new IHavePointersCreateExpectations();
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

		var expectations = new IHavePointersCreateExpectations();
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

		var expectations = new IHavePointersCreateExpectations();
		expectations.Methods.DelegatePointerReturn().ReturnValue(&DelegatePointerParameterDelegate);

		var mock = expectations.Instance();
		mock.DelegatePointerReturn();

		expectations.Verify();
	}
}