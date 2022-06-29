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
		var rock = Rock.Create<IAllow>();
		rock.Properties().Setters().NewLine(Arg.Is<string>(null!));

		var chunk = rock.Instance();
		chunk.NewLine = null;

		rock.Verify();
	}

	[Test]
	public static void MakeWithAbstract()
	{
		var chunk = Rock.Make<IAllow>().Instance();
		chunk.NewLine = null;
	}

	[Test]
	public static void CreateWithNonAbstract()
	{
		var rock = Rock.Create<Allow>();
		rock.Properties().Setters().NewLine(Arg.Is<string>(null!));

		var chunk = rock.Instance();
		chunk.NewLine = null;

		rock.Verify();
	}

	[Test]
	public static void MakeWithNonAbstract()
	{
		var chunk = Rock.Make<Allow>().Instance();
		chunk.NewLine = null;
	}
}