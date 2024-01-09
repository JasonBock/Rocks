using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Rocks.IntegrationTests;

public interface IAllow
{
	[AllowNull]
	string NewLine { get; set; }
}

public class Allow
{
	[AllowNull]
	public virtual string NewLine { get; set; }
}

public static class AllowNullTests
{
	[Test]
	[RockCreate<IAllow>]
	public static void CreateWithAbstract()
	{
		var expectations = new IAllowCreateExpectations();
		expectations.Properties.Setters.NewLine(Arg.Is<string>(null!));

		var mock = expectations.Instance();
		mock.NewLine = null;

		expectations.Verify();
	}

	[Test]
	[RockMake<IAllow>]
	public static void MakeWithAbstract()
	{
		var mock = new IAllowMakeExpectations().Instance();
		mock.NewLine = null;
	}

	[Test]
	[RockCreate<Allow>]
	public static void CreateWithNonAbstract()
	{
		var expectations = new AllowCreateExpectations();
		expectations.Properties.Setters.NewLine(Arg.Is<string>(null!));

		var chunk = expectations.Instance();
		chunk.NewLine = null;

		expectations.Verify();
	}

	[Test]
	[RockMake<Allow>]
	public static void MakeWithNonAbstract()
	{
		var mock = new AllowMakeExpectations().Instance();
		mock.NewLine = null;
	}
}