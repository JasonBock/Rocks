using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.RemovalTestTypes;

using RetrieveAdornment = ICustomerServiceCreateExpectations.Adornments.AdornmentsForHandler0;

public sealed record Customer(string Name);

public interface ICustomerService
{
	Customer Retrieve(uint id);
}

[NonParallelizable]
internal sealed class RemovalTests
{
	private RockContext context;
	private ICustomerServiceCreateExpectations customerExpectations;
	private RetrieveAdornment retrieveAdornments;

   [SetUp]
	public void SetUp()
	{
		this.context = new();
		this.customerExpectations = this.context.Create<ICustomerServiceCreateExpectations>();
		this.retrieveAdornments = this.customerExpectations.Setups.Retrieve(123).ReturnValue(new Customer("Jane"));
	}

	[Test]
	public void RetrieveCustomer()
	{
		var customerService = this.customerExpectations.Instance();
		Assert.That(customerService.Retrieve(123).Name, Is.EqualTo("Jane"));
	}

	[Test]
	public void RetrieveDifferentCustomer()
	{
		this.customerExpectations.Remove(this.retrieveAdornments);
		this.retrieveAdornments = this.customerExpectations.Setups.Retrieve(456).ReturnValue(new Customer("Joe"));
		var customerService = this.customerExpectations.Instance();
		Assert.That(customerService.Retrieve(456).Name, Is.EqualTo("Joe"));
	}

	[TearDown]
   public void TearDown() => this.context.Dispose();
}

internal static class RemovalParallelTests
{
	private static (RockContext, ICustomerServiceCreateExpectations, RetrieveAdornment) GetContext()
	{
		var context = new RockContext();
		var customerExpectations = context.Create<ICustomerServiceCreateExpectations>();
		var retrieveAdornments = customerExpectations.Setups.Retrieve(123).ReturnValue(new Customer("Jane"));
		return (context, customerExpectations, retrieveAdornments);
	}

	[Test]
	public static void Retrieve()
	{
		var (context, expectations, _) = RemovalParallelTests.GetContext();
		var customerService = expectations.Instance();
		Assert.That(customerService.Retrieve(123).Name, Is.EqualTo("Jane"));
		context.Dispose();
	}

	[Test]
	public static void RetrieveDifferentCustomer()
	{
		var (context, expectations, retrieveAdornments) = RemovalParallelTests.GetContext();
		expectations.Remove(retrieveAdornments);
		expectations.Setups.Retrieve(456).ReturnValue(new Customer("Joe"));
		var customerService = expectations.Instance();
		Assert.That(customerService.Retrieve(456).Name, Is.EqualTo("Joe"));
		context.Dispose();
	}
}