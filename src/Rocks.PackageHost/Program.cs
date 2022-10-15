using Rocks;

RunITestCase();

static void RunITestCase()
{
	var expectations = Rock.Create<ITest>();
	expectations.Methods().Foo();

	var mock = expectations.Instance();
	mock.Foo();

	expectations.Verify();
}

public interface ITest
{
	void Foo();
}