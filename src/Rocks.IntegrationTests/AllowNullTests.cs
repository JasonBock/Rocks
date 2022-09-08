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
	public static void CreateWithAbstract()
	{
		var expectations = Rock.Create<IAllow>();
		expectations.Properties().Setters().NewLine(Arg.Is<string>(null!));

		var mock = expectations.Instance();
		mock.NewLine = null;

		expectations.Verify();
	}

	[Test]
	public static void MakeWithAbstract()
	{
		var mock = Rock.Make<IAllow>().Instance();
		mock.NewLine = null;
	}

	[Test]
	public static void CreateWithNonAbstract()
	{
		var expectations = Rock.Create<Allow>();
		expectations.Properties().Setters().NewLine(Arg.Is<string>(null!));

		var chunk = expectations.Instance();
		chunk.NewLine = null;

		expectations.Verify();
	}

	[Test]
	public static void MakeWithNonAbstract()
	{
		var mock = Rock.Make<Allow>().Instance();
		mock.NewLine = null;
	}
}