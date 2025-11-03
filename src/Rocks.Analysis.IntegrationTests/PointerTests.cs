using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Analysis.IntegrationTests.PointerTestTypes;

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

internal static unsafe class PointerTests
{
	[Test]
	public static void CreateWithPointerTypeParameters()
	{
		var value = 10;
		var pValue = &value;

		using var context = new RockContext();
		var expectations = context.Create<ISurfaceCreateExpectations>();
		expectations.Setups.Create<int>(pValue);

		var mock = expectations.Instance();
		mock.Create(pValue);
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

		using var context = new RockContext();
		var expectations = context.Create<IHavePointersCreateExpectations>();
		expectations.Setups.PointerParameter(new(pValue));

		var mock = expectations.Instance();
		mock.PointerParameter(pValue);
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

		using var context = new RockContext();
		var expectations = context.Create<IHavePointersCreateExpectations>();
		expectations.Setups.PointerParameter(pValue);

		var mock = expectations.Instance();
		mock.PointerParameter(pValue);
	}

	[Test]
	public static void CreateWithPointerParameterNoReturnExpectationsNotMet()
	{
		var value = 10;
		var pValue = &value;
		var otherValue = 10;
		var pOtherValue = &otherValue;

		var expectations = new IHavePointersCreateExpectations();
		expectations.Setups.PointerParameter(pValue);

		var mock = expectations.Instance();

		Assert.That(() => mock.PointerParameter(pOtherValue), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithPointerParameterWithCallbackNoReturn()
	{
		unsafe static void PointerParameterCallback(int* callbackValue) => *callbackValue = 20;

		var value = 10;
		var pValue = &value;

		using var context = new RockContext();
		var expectations = context.Create<IHavePointersCreateExpectations>();
		expectations.Setups.PointerParameter(pValue).Callback(PointerParameterCallback);

		var mock = expectations.Instance();
		mock.PointerParameter(pValue);

		Assert.That(value, Is.EqualTo(20));
	}

	[Test]
	public static void CreateWithPointerReturn()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHavePointersCreateExpectations>();
		expectations.Setups.PointerReturn();

		var mock = expectations.Instance();
		mock.PointerReturn();
	}

	[Test]
	public static void MakeWithPointerReturn() =>
		new IHavePointersMakeExpectations().Instance().PointerReturn();

	[Test]
	public static void CreateWithPointerReturnWithCallback()
	{
		unsafe static int* PointerReturnCallback() => default;

		using var context = new RockContext();
		var expectations = context.Create<IHavePointersCreateExpectations>();
		expectations.Setups.PointerReturn().Callback(PointerReturnCallback);

		var mock = expectations.Instance();
		mock.PointerReturn();
	}

	[Test]
	public static void CreateWithPointerReturnWithResult()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHavePointersCreateExpectations>();
		expectations.Setups.PointerReturn().ReturnValue(default);

		var mock = expectations.Instance();
		mock.PointerReturn();
	}

	[Test]
	public static void CreateWithDelegatePointerParameterNoReturn()
	{
		static void DelegatePointerParameterDelegate(int a) { }

		using var context = new RockContext();
		var expectations = context.Create<IHavePointersCreateExpectations>();
		expectations.Setups.DelegatePointerParameter(new(&DelegatePointerParameterDelegate));

		var mock = expectations.Instance();
		mock.DelegatePointerParameter(&DelegatePointerParameterDelegate);
	}

	[Test]
	public static void CreateWithDelegatePointerParameterNoReturnExpectationsNotMet()
	{
		static void DelegatePointerParameterDelegate(int a) { }
		static void OtherDelegatePointerParameterDelegate(int a) { }

		var expectations = new IHavePointersCreateExpectations();
		expectations.Setups.DelegatePointerParameter(new(&DelegatePointerParameterDelegate));

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

		using var context = new RockContext();
		var expectations = context.Create<IHavePointersCreateExpectations>();
		expectations.Setups.DelegatePointerParameter(new(&DelegatePointerParameterDelegate))
			.Callback(DelegatePointerParameterCallback);

		var mock = expectations.Instance();
		mock.DelegatePointerParameter(&DelegatePointerParameterDelegate);

		Assert.That(wasCalled, Is.True);
	}

	[Test]
	public static void CreateWithDelegatePointerReturn()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHavePointersCreateExpectations>();
		expectations.Setups.DelegatePointerReturn();

		var mock = expectations.Instance();
		mock.DelegatePointerReturn();
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

		using var context = new RockContext();
		var expectations = context.Create<IHavePointersCreateExpectations>();
		expectations.Setups.DelegatePointerReturn().Callback(DelegatePointerReturnCallback);

		var mock = expectations.Instance();
		var value = mock.DelegatePointerReturn();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(wasCalled, Is.True);
		}
	}

	[Test]
	public static void CreateWithDelegatePointerReturnWithResult()
	{
		static void DelegatePointerParameterDelegate(int a) { }

		using var context = new RockContext();
		var expectations = context.Create<IHavePointersCreateExpectations>();
		expectations.Setups.DelegatePointerReturn().ReturnValue(&DelegatePointerParameterDelegate);

		var mock = expectations.Instance();
		mock.DelegatePointerReturn();
	}
}