using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.VirtualsWithImplementationsTestTypes;

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
		using var context = new RockContext();
		var expectations = context.Create<IIndexerPolygonCreateExpectations>();
		expectations.Properties.Getters.SideLength().ReturnValue(3);

		var mock = expectations.Instance();
		Assert.That(mock[5], Is.EqualTo(15));
	}

	[Test]
	public static void CallVirtualMethodOnInterfaceWithNoExpectation()
	{
		using var context = new RockContext();
		var expectations = context.Create<IMethodPolygonCreateExpectations>();
		expectations.Properties.Getters.SideLength().ReturnValue(3);
		expectations.Properties.Getters.NumberOfSides().ReturnValue(5);

		var mock = expectations.Instance();
		Assert.That(mock.GetPerimeter(), Is.EqualTo(15));
	}

	[Test]
	public static void CallVirtualPropertyOnInterfaceWithNoExpectation()
	{
		using var context = new RockContext();
		var expectations = context.Create<IPropertyPolygonCreateExpectations>();
		expectations.Properties.Getters.SideLength().ReturnValue(3);
		expectations.Properties.Getters.NumberOfSides().ReturnValue(5);

		var mock = expectations.Instance();
		Assert.That(mock.Perimeter, Is.EqualTo(15));
	}

	[Test]
	public static void CallVirtualIndexerOnClassWithNoExpectation()
	{
		using var context = new RockContext();
		var expectations = context.Create<IndexerPolygonCreateExpectations>();
		expectations.Properties.Getters.SideLength().ReturnValue(3);

		var mock = expectations.Instance();
		Assert.That(mock[5], Is.EqualTo(15));
	}

	[Test]
	public static void CallVirtualMethodOnClassWithNoExpectation()
	{
		using var context = new RockContext();
		var expectations = context.Create<MethodPolygonCreateExpectations>();
		expectations.Properties.Getters.SideLength().ReturnValue(3);
		expectations.Properties.Getters.NumberOfSides().ReturnValue(5);

		var mock = expectations.Instance();
		Assert.That(mock.GetPerimeter(), Is.EqualTo(15));
	}

	[Test]
	public static void CallVirtualPropertyOnClassWithNoExpectation()
	{
		using var context = new RockContext();
		var expectations = context.Create<PropertyPolygonCreateExpectations>();
		expectations.Properties.Getters.SideLength().ReturnValue(3);
		expectations.Properties.Getters.NumberOfSides().ReturnValue(5);

		var mock = expectations.Instance();
		Assert.That(mock.Perimeter, Is.EqualTo(15));
	}
}