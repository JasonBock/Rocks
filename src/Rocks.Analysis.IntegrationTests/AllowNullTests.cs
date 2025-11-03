using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Rocks.Analysis.IntegrationTests.AllowNullTestTypes;

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

internal static class AllowNullTests
{
	[Test]
	public static void CreateWithAbstract()
	{
		using var context = new RockContext();
		var expectations = context.Create<IAllowCreateExpectations>();
		expectations.Setups.NewLine.Sets(Arg.Is<string>(null!));

		var mock = expectations.Instance();
		mock.NewLine = null;
	}

	[Test]
	public static void MakeWithAbstract()
	{
		var mock = new IAllowMakeExpectations().Instance();
		mock.NewLine = null;
	}

	[Test]
	public static void CreateWithNonAbstract()
	{
		using var context = new RockContext();
		var expectations = context.Create<AllowCreateExpectations>();
		expectations.Setups.NewLine.Sets(Arg.Is<string>(null!));

		var chunk = expectations.Instance();
		chunk.NewLine = null;
	}

	[Test]
	public static void MakeWithNonAbstract()
	{
		var mock = new AllowMakeExpectations().Instance();
		mock.NewLine = null;
	}
}