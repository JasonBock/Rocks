using Rocks;

#pragma warning disable CA1852 // Seal internal types
RunITestCase();

static void RunITestCase()
{
	var expectations = Rock.Create<ITest>();
	expectations.Methods().Foo();

	var mock = expectations.Instance();
	mock.Foo();

	expectations.Verify();
}

#pragma warning disable CA1050 // Declare types in namespaces
public interface ITest
#pragma warning restore CA1050 // Declare types in namespaces
{
	void Foo();
}
#pragma warning restore CA1852 // Seal internal types