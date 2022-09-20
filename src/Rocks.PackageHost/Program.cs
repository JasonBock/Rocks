using ComputeSharp.D2D1;
using Csla;
using Rocks;

RunITestCase();
RunID2D1TransformMapperFactory();
RunBusinessBaseCase();

static void RunITestCase()
{
	var expectations = Rock.Create<ITest>();
	expectations.Methods().Foo();

	var mock = expectations.Instance();
	mock.Foo();

	expectations.Verify();
}

static void RunID2D1TransformMapperFactory()
{
	var expectations = Rock.Create<ID2D1TransformMapperFactory<Struct_4910431>>();
	expectations.Methods().Create();

	var mock = expectations.Instance();
	mock.Create();

	expectations.Verify();
}

static void RunBusinessBaseCase()
{
	var expectations = Rock.Create<Customer>();
	expectations.Methods().Delete();

	//var mock = expectations.In
}

[Serializable]
public class Customer
	: BusinessBase<Customer>
{ }

public partial struct Struct_4910431 : ID2D1PixelShader
{
	public float4 Execute() => default;
}

public interface ITest
{
	void Foo();
}