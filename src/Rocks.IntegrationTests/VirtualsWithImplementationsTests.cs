using NUnit.Framework;

namespace Rocks.IntegrationTests;

public interface IIndexerPolygon
{
	int SideLength { get; }

	double this[int numberOfSides] => this.SideLength * numberOfSides;
}

public interface IMethodPolygon
{
	int NumberOfSides { get; }
	int SideLength { get; }

	double GetPerimeter() => this.SideLength * this.NumberOfSides;
}

public interface IPropertyPolygon
{
	int NumberOfSides { get; }
	int SideLength { get; }

	double Perimeter => this.SideLength * this.NumberOfSides;
}

public class IndexerPolygon
{
	public virtual int SideLength { get; }

	public virtual double this[int numberOfSides] => this.SideLength * numberOfSides;
}

public class MethodPolygon
{
	public virtual int NumberOfSides { get; }
	public virtual int SideLength { get; }

	public virtual double GetPerimeter() => this.SideLength * this.NumberOfSides;
}

public class PropertyPolygon
{
	public virtual int NumberOfSides { get; }
	public virtual int SideLength { get; }

	public virtual double Perimeter => this.SideLength * this.NumberOfSides;
}

public static class VirtualsWithImplementationsTests
{
	[Test]
	public static void CallVirtualIndexerOnInterfaceWithNoExpectation()
	{
		var polygonMock = Rock.Create<IIndexerPolygon>();
		polygonMock.Properties().Getters().SideLength().Returns(3);

		var polygon = polygonMock.Instance();
		Assert.That(polygon[5], Is.EqualTo(15));

		polygonMock.Verify();
	}

	[Test]
	public static void CallVirtualMethodOnInterfaceWithNoExpectation()
	{
		var polygonMock = Rock.Create<IMethodPolygon>();
		polygonMock.Properties().Getters().SideLength().Returns(3);
		polygonMock.Properties().Getters().NumberOfSides().Returns(5);

		var polygon = polygonMock.Instance();
		Assert.That(polygon.GetPerimeter(), Is.EqualTo(15));

		polygonMock.Verify();
	}

	[Test]
	public static void CallVirtualPropertyOnInterfaceWithNoExpectation()
	{
		var polygonMock = Rock.Create<IPropertyPolygon>();
		polygonMock.Properties().Getters().SideLength().Returns(3);
		polygonMock.Properties().Getters().NumberOfSides().Returns(5);

		var polygon = polygonMock.Instance();
		Assert.That(polygon.Perimeter, Is.EqualTo(15));

		polygonMock.Verify();
	}

	[Test]
	public static void CallVirtualIndexerOnClassWithNoExpectation()
	{
		var polygonMock = Rock.Create<IndexerPolygon>();
		polygonMock.Properties().Getters().SideLength().Returns(3);

		var polygon = polygonMock.Instance();
		Assert.That(polygon[5], Is.EqualTo(15));

		polygonMock.Verify();
	}

	[Test]
	public static void CallVirtualMethodOnClassWithNoExpectation()
	{
		var polygonMock = Rock.Create<MethodPolygon>();
		polygonMock.Properties().Getters().SideLength().Returns(3);
		polygonMock.Properties().Getters().NumberOfSides().Returns(5);

		var polygon = polygonMock.Instance();
		Assert.That(polygon.GetPerimeter(), Is.EqualTo(15));

		polygonMock.Verify();
	}

	[Test]
	public static void CallVirtualPropertyOnClassWithNoExpectation()
	{
		var polygonMock = Rock.Create<PropertyPolygon>();
		polygonMock.Properties().Getters().SideLength().Returns(3);
		polygonMock.Properties().Getters().NumberOfSides().Returns(5);

		var polygon = polygonMock.Instance();
		Assert.That(polygon.Perimeter, Is.EqualTo(15));

		polygonMock.Verify();
	}
}