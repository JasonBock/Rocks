using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.OpenGenericsTestTypes;

public interface IService<T, TReturn>
{
	TReturn Service(T data);
}

public static class OpenGenericsTests
{
	[Test]
	public static void CreateWithIntAndString()
	{
		using var context = new RockContext();
		var intStringExpectations = context.Create<IServiceCreateExpectations<int, string>>();
		intStringexpectations.Setups.Service(3).ReturnValue("three");

		var intStringMock = intStringExpectations.Instance();
		Assert.That(intStringMock.Service(3), Is.EqualTo("three"));
	}

	[Test]
	public static void CreateWithStringAndInt()
	{
		using var context = new RockContext();
		var stringIntExpectations = context.Create<IServiceCreateExpectations<string, int>>();
		stringIntexpectations.Setups.Service("four").ReturnValue(4);

		var stringIntMock = stringIntExpectations.Instance();
		Assert.That(stringIntMock.Service("four"), Is.EqualTo(4));
	}

	[Test]
	public static void MakeWithIntAndString()
	{
		var intStringMake = new IServiceMakeExpectations<int, string>().Instance();
		Assert.That(intStringMake.Service(3), Is.Null);
	}

	[Test]
	public static void MakeWithStringAndInt()
	{
		var intStringMake = new IServiceMakeExpectations<string, int>().Instance();
		Assert.That(intStringMake.Service("four"), Is.Zero);
	}
}