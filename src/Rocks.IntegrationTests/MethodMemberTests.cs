using NUnit.Framework;
using System.Globalization;

namespace Rocks.IntegrationTests;

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
		var expectations = Rock.Create<IHaveLotsOfParameters>();
		expectations.Methods().CallThis(
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

		expectations.Verify();
	}
	
	[Test]
	public static void CreateMethodWithRefReturn()
	{
		var expectations = Rock.Create<IHaveRefReturn>();
		expectations.Methods().MethodRefReturn().Returns(3);

		var mock = expectations.Instance();
		ref var value = ref mock.MethodRefReturn();

		expectations.Verify();
		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void MakeMethodWithRefReturn()
	{
		var mock = Rock.Make<IHaveRefReturn>().Instance();
		ref var value = ref mock.MethodRefReturn();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreatePropertyWithRefReturn()
	{
		var expectations = Rock.Create<IHaveRefReturn>();
		expectations.Properties().Getters().PropertyRefReturn().Returns(3);

		var mock = expectations.Instance();
		ref var value = ref mock.PropertyRefReturn;

		expectations.Verify();
		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void MakePropertyWithRefReturn()
	{
		var mock = Rock.Make<IHaveRefReturn>().Instance();
		ref var value = ref mock.PropertyRefReturn;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateIndexerWithRefReturn()
	{
		var expectations = Rock.Create<IHaveRefReturn>();
		expectations.Indexers().Getters().This(3).Returns(4);

		var mock = expectations.Instance();
		ref var value = ref mock[3];

		expectations.Verify();
		Assert.That(value, Is.EqualTo(4));
	}

	[Test]
	public static void MakeIndexerWithRefReturn()
	{
		var mock = Rock.Make<IHaveRefReturn>().Instance();
		ref var value = ref mock[3];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateMethodWithRefReadonlyReturn()
	{
		var expectations = Rock.Create<IHaveRefReturn>();
		expectations.Methods().MethodRefReadonlyReturn().Returns(3);

		var mock = expectations.Instance();
		var value = mock.MethodRefReadonlyReturn();

		expectations.Verify();
		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void MakeMethodWithRefReadonlyReturn()
	{
		var mock = Rock.Make<IHaveRefReturn>().Instance();
		var value = mock.MethodRefReadonlyReturn();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreatePropertyWithRefReadonlyReturn()
	{
		var expectations = Rock.Create<IHaveRefReturn>();
		expectations.Properties().Getters().PropertyRefReadonlyReturn().Returns(3);

		var mock = expectations.Instance();
		var value = mock.PropertyRefReadonlyReturn;

		expectations.Verify();
		Assert.That(value, Is.EqualTo(3));
	}

	[Test]
	public static void MakePropertyWithRefReadonlyReturn()
	{
		var mock = Rock.Make<IHaveRefReturn>().Instance();
		var value = mock.PropertyRefReadonlyReturn;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateIndexerWithRefReadonlyReturn()
	{
		var expectations = Rock.Create<IHaveRefReturn>();
		expectations.Indexers().Getters().This("b").Returns(4);

		var mock = expectations.Instance();
		var value = mock["b"];

		expectations.Verify();
		Assert.That(value, Is.EqualTo(4));
	}

	[Test]
	public static void MakeIndexerWithRefReadonlyReturn()
	{
		var mock = Rock.Make<IHaveRefReturn>().Instance();
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateMembersWithInParameters()
	{
		var expectations = Rock.Create<IHaveIn>();
		expectations.Methods().InArgument(3);
		expectations.Indexers().Getters().This(4).Returns(5);

		var mock = expectations.Instance();
		mock.InArgument(3);
		var value = mock[4];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(5));
	}

	[Test]
	public static void MakeMembersWithInParameters()
	{
		var mock = Rock.Make<IHaveIn>().Instance();
		var value = mock[4];

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.EqualTo(default(int)));
			Assert.That(() => mock.InArgument(3), Throws.Nothing);
		});
	}

	[Test]
	public static void CreateMemberWithOutParameter()
	{
		var expectations = Rock.Create<IHaveRefAndOut>();
		expectations.Methods().OutArgument(3);

		var mock = expectations.Instance();
		mock.OutArgument(out var value);

		expectations.Verify();

		Assert.That(value, Is.EqualTo(0));
	}

	[Test]
	public static void MakeMemberWithOutParameter()
	{
		var mock = Rock.Make<IHaveRefAndOut>().Instance();
		mock.OutArgument(out var value);

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateMemberWithOutParameterAndCallback()
	{
		static void OutArgumentCallback(out int a) => a = 4;

		var expectations = Rock.Create<IHaveRefAndOut>();
		expectations.Methods().OutArgument(3).Callback(OutArgumentCallback);

		var mock = expectations.Instance();
		mock.OutArgument(out var value);

		expectations.Verify();

		Assert.That(value, Is.EqualTo(4));
	}

	[Test]
	public static void CreateMemberWithOutParameterAndGenerics()
	{
		var expectations = Rock.Create<IHaveRefAndOut>();
		expectations.Methods().OutArgumentsWithGenerics<int, string>(3, "b");

		var mock = expectations.Instance();
		mock.OutArgumentsWithGenerics<int, string>(3, out var value);

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(value, Is.Null);
		});
	}

	[Test]
	public static void MakeMemberWithOutParameterAndGenerics()
	{
		var mock = Rock.Make<IHaveRefAndOut>().Instance();
		mock.OutArgumentsWithGenerics<int, string>(3, out var value);

		Assert.That(value, Is.EqualTo(default(string)));
	}

	[Test]
	public static void CreateMemberWithOutParameterAndGenericsAndCallback()
	{
		static void OutArgumentsWithGenericsCallback(int a, out string b) =>
			b = a.ToString(CultureInfo.CurrentCulture);

		var expectations = Rock.Create<IHaveRefAndOut>();
		expectations.Methods().OutArgumentsWithGenerics<int, string>(3, "b")
			.Callback(OutArgumentsWithGenericsCallback);

		var mock = expectations.Instance();
		mock.OutArgumentsWithGenerics<int, string>(3, out var value);

		expectations.Verify();

		Assert.That(value, Is.EqualTo("3"));
	}

	[Test]
	public static void CreateMemberWithRefParameter()
	{
		var expectations = Rock.Create<IHaveRefAndOut>();
		expectations.Methods().RefArgument(3);

		var mock = expectations.Instance();
		var value = 3;
		mock.RefArgument(ref value);

		expectations.Verify();
	}

	[Test]
	public static void MakeMemberWithRefParameter()
	{
		var mock = Rock.Make<IHaveRefAndOut>().Instance();
		var value = 3;

		Assert.Multiple(() =>
		{
			Assert.That(() => mock.RefArgument(ref value), Throws.Nothing);
		});
	}

	[Test]
	public static void CreateMemberWithRefParameterAndCallback()
	{
		static void RefArgumentCallback(ref int a) => a = 4;

		var expectations = Rock.Create<IHaveRefAndOut>();
		expectations.Methods().RefArgument(3).Callback(RefArgumentCallback);

		var mock = expectations.Instance();
		var value = 3;
		mock.RefArgument(ref value);

		expectations.Verify();

		Assert.That(value, Is.EqualTo(4));
	}

	[Test]
	public static void CreateMemberWithRefParameterAndGenerics()
	{
		var expectations = Rock.Create<IHaveRefAndOut>();
		expectations.Methods().RefArgumentsWithGenerics<int, string>(3, "b");

		var mock = expectations.Instance();
		var value = "b";
		mock.RefArgumentsWithGenerics(3, ref value);

		expectations.Verify();
	}

	[Test]
	public static void MakeMemberWithRefParameterAndGenerics()
	{
		var mock = Rock.Make<IHaveRefAndOut>().Instance();
		var value = "b";

		Assert.Multiple(() =>
		{
			Assert.That(() => mock.RefArgumentsWithGenerics(3, ref value), Throws.Nothing);
		});
	}

	[Test]
	public static void CreateMemberWithRefParameterAndGenericsAndCallback()
	{
		static void RefArgumentsWithGenericsCallback(int a, ref string b) =>
			b = a.ToString(CultureInfo.CurrentCulture);

		var expectations = Rock.Create<IHaveRefAndOut>();
		expectations.Methods().RefArgumentsWithGenerics<int, string>(3, "b").Callback(RefArgumentsWithGenericsCallback);

		var mock = expectations.Instance();
		var value = "b";
		mock.RefArgumentsWithGenerics(3, ref value);

		expectations.Verify();

		Assert.That(value, Is.EqualTo("3"));
	}
}