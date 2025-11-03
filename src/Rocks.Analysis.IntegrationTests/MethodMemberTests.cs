using NUnit.Framework;
using System.Globalization;

namespace Rocks.Analysis.IntegrationTests.MethodMemberTestTypes;

public interface IHaveLotsOfParameters
{
	void CallThis(int i0, int i1, int i2, int i3, int i4,
		int i5, int i6, int i7, int i8, int i9,
		int i10, int i11, int i12, int i13, int i14,
		int i15, int i16, int i17, int i18, int i19);
}
public interface IHaveRefAndOut
{
	void RefArgument(ref int a);
	void RefArgumentsWithGenerics<T1, T2>(T1 a, ref T2 b);
	void OutArgument(out int a);
	void OutArgumentsWithGenerics<T1, T2>(T1 a, out T2 b);
}

public interface IHaveRefReturn
{
	ref int MethodRefReturn();
	ref int PropertyRefReturn { get; }
	ref int this[int a] { get; }
	ref readonly int MethodRefReadonlyReturn();
	ref readonly int PropertyRefReadonlyReturn { get; }
	ref readonly int this[string a] { get; }
}

public interface IHaveIn
{
	void InArgument(in int a);
	int this[in int a] { get; }
}

public static class MethodMemberTests
{
	[Test]
	public static void CreateWithLotsOfParameters()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveLotsOfParametersCreateExpectations>();
		expectations.Setups.CallThis(
			0, 1, 2, 3, 4,
			5, 6, 7, 8, 9,
			10, 11, 12, 13, 14,
			15, 16, 17, 18, 19);

		var mock = expectations.Instance();
		mock.CallThis(
			0, 1, 2, 3, 4,
			5, 6, 7, 8, 9,
			10, 11, 12, 13, 14,
			15, 16, 17, 18, 19);
	}

	[Test]
	public static void CreateMethodWithRefReturn()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveRefReturnCreateExpectations>();
		expectations.Setups.MethodRefReturn().ReturnValue(3);

		var mock = expectations.Instance();
		ref var value = ref mock.MethodRefReturn();

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void MakeMethodWithRefReturn()
	{
		var mock = new IHaveRefReturnMakeExpectations().Instance();
		ref var value = ref mock.MethodRefReturn();

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreatePropertyWithRefReturn()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveRefReturnCreateExpectations>();
		expectations.Setups.PropertyRefReturn.Gets().ReturnValue(3);

		var mock = expectations.Instance();
		ref var value = ref mock.PropertyRefReturn;

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void MakePropertyWithRefReturn()
	{
		var mock = new IHaveRefReturnMakeExpectations().Instance();
		ref var value = ref mock.PropertyRefReturn;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateIndexerWithRefReturn()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveRefReturnCreateExpectations>();
		expectations.Setups[3].Gets().ReturnValue(4);

		var mock = expectations.Instance();
		ref var value = ref mock[3];

		Assert.That(value, Is.EqualTo(4));
	}

	[Test]
	public static void MakeIndexerWithRefReturn()
	{
		var mock = new IHaveRefReturnMakeExpectations().Instance();
		ref var value = ref mock[3];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateMethodWithRefReadonlyReturn()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveRefReturnCreateExpectations>();
		expectations.Setups.MethodRefReadonlyReturn().ReturnValue(3);

		var mock = expectations.Instance();
		var value = mock.MethodRefReadonlyReturn();

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void MakeMethodWithRefReadonlyReturn()
	{
		var mock = new IHaveRefReturnMakeExpectations().Instance();
		var value = mock.MethodRefReadonlyReturn();

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreatePropertyWithRefReadonlyReturn()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveRefReturnCreateExpectations>();
		expectations.Setups.PropertyRefReadonlyReturn.Gets().ReturnValue(3);

		var mock = expectations.Instance();
		var value = mock.PropertyRefReadonlyReturn;

		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void MakePropertyWithRefReadonlyReturn()
	{
		var mock = new IHaveRefReturnMakeExpectations().Instance();
		var value = mock.PropertyRefReadonlyReturn;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateIndexerWithRefReadonlyReturn()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveRefReturnCreateExpectations>();
		expectations.Setups["b"].Gets().ReturnValue(4);

		var mock = expectations.Instance();
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(4));
	}

	[Test]
	public static void MakeIndexerWithRefReadonlyReturn()
	{
		var mock = new IHaveRefReturnMakeExpectations().Instance();
		var value = mock["b"];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateMembersWithInParameters()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveInCreateExpectations>();
		expectations.Setups.InArgument(3);
		expectations.Setups[4].Gets().ReturnValue(5);

		var mock = expectations.Instance();
		mock.InArgument(3);
		var value = mock[4];

		Assert.That(value, Is.EqualTo(5));
	}

	[Test]
	public static void MakeMembersWithInParameters()
	{
		var mock = new IHaveInMakeExpectations().Instance();
		var value = mock[4];

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(value, Is.Default);
			Assert.That(() => mock.InArgument(3), Throws.Nothing);
		}
	}

	[Test]
	public static void CreateMemberWithOutParameter()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveRefAndOutCreateExpectations>();
		expectations.Setups.OutArgument(3);

		var mock = expectations.Instance();
		mock.OutArgument(out var value);

		Assert.That(value, Is.Zero);
	}

	[Test]
	public static void MakeMemberWithOutParameter()
	{
		var mock = new IHaveRefAndOutMakeExpectations().Instance();
		mock.OutArgument(out var value);

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateMemberWithOutParameterAndCallback()
	{
		static void OutArgumentCallback(out int a) => a = 4;

		using var context = new RockContext();
		var expectations = context.Create<IHaveRefAndOutCreateExpectations>();
		expectations.Setups.OutArgument(3).Callback(OutArgumentCallback);

		var mock = expectations.Instance();
		mock.OutArgument(out var value);

		Assert.That(value, Is.EqualTo(4));
	}

	[Test]
	public static void CreateMemberWithOutParameterAndGenerics()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveRefAndOutCreateExpectations>();
		expectations.Setups.OutArgumentsWithGenerics<int, string>(3, "b");

		var mock = expectations.Instance();
		mock.OutArgumentsWithGenerics<int, string>(3, out var value);

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(value, Is.Null);
		}
	}

	[Test]
	public static void MakeMemberWithOutParameterAndGenerics()
	{
		var mock = new IHaveRefAndOutMakeExpectations().Instance();
		mock.OutArgumentsWithGenerics<int, string>(3, out var value);

		Assert.That(value, Is.Null);
	}

	[Test]
	public static void CreateMemberWithOutParameterAndGenericsAndCallback()
	{
		static void OutArgumentsWithGenericsCallback(int a, out string b) =>
			b = a.ToString(CultureInfo.CurrentCulture);

		using var context = new RockContext();
		var expectations = context.Create<IHaveRefAndOutCreateExpectations>();
		expectations.Setups.OutArgumentsWithGenerics<int, string>(3, "b")
			.Callback(OutArgumentsWithGenericsCallback);

		var mock = expectations.Instance();
		mock.OutArgumentsWithGenerics<int, string>(3, out var value);

		Assert.That(value, Is.EqualTo("3"));
	}

	[Test]
	public static void CreateMemberWithRefParameter()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveRefAndOutCreateExpectations>();
		expectations.Setups.RefArgument(3);

		var mock = expectations.Instance();
		var value = 3;
		mock.RefArgument(ref value);
	}

	[Test]
	public static void MakeMemberWithRefParameter()
	{
		var mock = new IHaveRefAndOutMakeExpectations().Instance();
		var value = 3;

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(() => mock.RefArgument(ref value), Throws.Nothing);
		}
	}

	[Test]
	public static void CreateMemberWithRefParameterAndCallback()
	{
		static void RefArgumentCallback(ref int a) => a = 4;

		using var context = new RockContext();
		var expectations = context.Create<IHaveRefAndOutCreateExpectations>();
		expectations.Setups.RefArgument(3).Callback(RefArgumentCallback);

		var mock = expectations.Instance();
		var value = 3;
		mock.RefArgument(ref value);

		Assert.That(value, Is.EqualTo(4));
	}

	[Test]
	public static void CreateMemberWithRefParameterAndGenerics()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveRefAndOutCreateExpectations>();
		expectations.Setups.RefArgumentsWithGenerics<int, string>(3, "b");

		var mock = expectations.Instance();
		var value = "b";
		mock.RefArgumentsWithGenerics(3, ref value);
	}

	[Test]
	public static void MakeMemberWithRefParameterAndGenerics()
	{
		var mock = new IHaveRefAndOutMakeExpectations().Instance();
		var value = "b";

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(() => mock.RefArgumentsWithGenerics(3, ref value), Throws.Nothing);
		}
	}

	[Test]
	public static void CreateMemberWithRefParameterAndGenericsAndCallback()
	{
		static void RefArgumentsWithGenericsCallback(int a, ref string b) =>
			b = a.ToString(CultureInfo.CurrentCulture);

		using var context = new RockContext();
		var expectations = context.Create<IHaveRefAndOutCreateExpectations>();
		expectations.Setups.RefArgumentsWithGenerics<int, string>(3, "b").Callback(RefArgumentsWithGenericsCallback);

		var mock = expectations.Instance();
		var value = "b";
		mock.RefArgumentsWithGenerics(3, ref value);

		Assert.That(value, Is.EqualTo("3"));
	}
}