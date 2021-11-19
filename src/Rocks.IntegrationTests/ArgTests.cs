using NUnit.Framework;

namespace Rocks.IntegrationTests;

public interface IHaveArgument
{
	void Foo(int a);
	void Bar(int a = 3);
}

public static class ArgTests
{
	[Test]
	public static void DeclareArgumentWithValue()
	{
		var rock = Rock.Create<IHaveArgument>();
		rock.Methods().Foo(3);

		var chunk = rock.Instance();
		chunk.Foo(3);

		rock.Verify();
	}

	[Test]
	public static void DeclareArgumentWithIs()
	{
		var rock = Rock.Create<IHaveArgument>();
		rock.Methods().Foo(Arg.Is(3));

		var chunk = rock.Instance();
		chunk.Foo(3);

		rock.Verify();
	}

	[Test]
	public static void DeclareArgumentWithAny()
	{
		var rock = Rock.Create<IHaveArgument>();
		rock.Methods().Foo(Arg.Any<int>());

		var chunk = rock.Instance();
		chunk.Foo(3);

		rock.Verify();
	}

	[Test]
	public static void DeclareArgumentWithValidate()
	{
		var rock = Rock.Create<IHaveArgument>();
		rock.Methods().Foo(Arg.Validate<int>(_ => _ > 20 && _ < 30));

		var chunk = rock.Instance();
		chunk.Foo(25);

		rock.Verify();
	}

	[Test]
	public static void DeclareArgumentWithDefault()
	{
		var rock = Rock.Create<IHaveArgument>();
		rock.Methods().Bar(Arg.IsDefault<int>());

		var chunk = rock.Instance();
		chunk.Bar(3);

		rock.Verify();
	}
}