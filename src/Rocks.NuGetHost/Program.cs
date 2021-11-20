using Rocks;
using Rocks.NuGetHost;

var rock = Rock.Create<ITest>();
rock.Methods().Foo();

var chunk = rock.Instance();
chunk.Foo();

rock.Verify();