using NUnit.Framework;

namespace Rocks.IntegrationTests;

public interface IPolygon
{
	int NumberOfSides { get; }
	int SideLength { get; }

	double GetPerimeter() => this.SideLength * this.NumberOfSides;
}

public class Polygon
{
	public virtual int NumberOfSides { get; }
	public virtual int SideLength { get; }

	public virtual double GetPerimeter() => this.SideLength * this.NumberOfSides;
}

public static class VirtualsWithImplementationsTests
{
	[Test]
	public static void CallVirtualMethodOnInterfaceWithNoExpectation()
	{
		var polygonMock = Rock.Create<IPolygon>();
		polygonMock.Methods().GetPerimeter().Returns(15);

		var polygon = polygonMock.Instance();
		Assert.That(polygon.GetPerimeter(), Is.EqualTo(15));

		polygonMock.Verify();
	}

	[Test]
	public static void CallVirtualMethodOnClassWithNoExpectation()
	{
		var polygonMock = Rock.Create<Polygon>();
		polygonMock.Properties().Getters().SideLength().Returns(3);
		polygonMock.Properties().Getters().NumberOfSides().Returns(5);

		var polygon = polygonMock.Instance();
		Assert.That(polygon.GetPerimeter(), Is.EqualTo(15));

		polygonMock.Verify();
	}
}