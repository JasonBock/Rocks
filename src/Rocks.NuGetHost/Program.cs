using Rocks;

var rock = Rock.Create<ITest>();
rock.Methods().Foo();

var chunk = rock.Instance();
chunk.Foo();

rock.Verify();

public interface ITest
{
	void Foo();
}