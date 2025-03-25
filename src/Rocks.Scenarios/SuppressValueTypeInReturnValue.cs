using Rocks;
using Rocks.Scenarios;

[assembly: Rock(typeof(ISuppressValueTypeInReturnValue), BuildType.Create)]

namespace Rocks.Scenarios;

public static class ISuppressValueTypeInReturnValueUser
{
	public static void Use()
	{
		var expectations = new ISuppressValueTypeInReturnValueCreateExpectations();
		expectations.Methods.Work()
			.ReturnValue(new ValueTask());
		expectations.Methods.WorkWithResult()
			.ReturnValue(ValueTask.FromResult("done"));
		expectations.Methods.WorkWithResult()
			.ReturnValue(new ValueTask<string>("done"));
	}
}

public interface ISuppressValueTypeInReturnValue
{
	ValueTask Work();
	ValueTask<string> WorkWithResult();
}