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
	[RockCreate<IIndexerPolygon>]
	public static void CallVirtualIndexerOnInterfaceWithNoExpectation()
	{
		var expectations = new IIndexerPolygonCreateExpectations();
		expectations.Properties.Getters.SideLength().ReturnValue(3);

		var mock = expectations.Instance();
		Assert.That(mock[5], Is.EqualTo(15));

		expectations.Verify();
	}

	[Test]
	[RockCreate<IMethodPolygon>]
	public static void CallVirtualMethodOnInterfaceWithNoExpectation()
	{
		var expectations = new IMethodPolygonCreateExpectations();
		expectations.Properties.Getters.SideLength().ReturnValue(3);
		expectations.Properties.Getters.NumberOfSides().ReturnValue(5);

		var mock = expectations.Instance();
		Assert.That(mock.GetPerimeter(), Is.EqualTo(15));

		expectations.Verify();
	}

	[Test]
	[RockCreate<IPropertyPolygon>]
	public static void CallVirtualPropertyOnInterfaceWithNoExpectation()
	{
		var expectations = new IPropertyPolygonCreateExpectations();
		expectations.Properties.Getters.SideLength().ReturnValue(3);
		expectations.Properties.Getters.NumberOfSides().ReturnValue(5);

		var mock = expectations.Instance();
		Assert.That(mock.Perimeter, Is.EqualTo(15));

		expectations.Verify();
	}

	[Test]
	[RockCreate<IndexerPolygon>]
	public static void CallVirtualIndexerOnClassWithNoExpectation()
	{
		var expectations = new IndexerPolygonCreateExpectations();
		expectations.Properties.Getters.SideLength().ReturnValue(3);

		var mock = expectations.Instance();
		Assert.That(mock[5], Is.EqualTo(15));

		expectations.Verify();
	}

	[Test]
	[RockCreate<MethodPolygon>]
	public static void CallVirtualMethodOnClassWithNoExpectation()
	{
		var expectations = new MethodPolygonCreateExpectations();
		expectations.Properties.Getters.SideLength().ReturnValue(3);
		expectations.Properties.Getters.NumberOfSides().ReturnValue(5);

		var mock = expectations.Instance();
		Assert.That(mock.GetPerimeter(), Is.EqualTo(15));

		expectations.Verify();
	}

	[Test]
	[RockCreate<PropertyPolygon>]
	public static void CallVirtualPropertyOnClassWithNoExpectation()
	{
		var expectations = new PropertyPolygonCreateExpectations();
		expectations.Properties.Getters.SideLength().ReturnValue(3);
		expectations.Properties.Getters.NumberOfSides().ReturnValue(5);

		var mock = expectations.Instance();
		Assert.That(mock.Perimeter, Is.EqualTo(15));

		expectations.Verify();
	}
}