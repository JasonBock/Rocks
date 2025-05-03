using Rocks;
using Rocks.Scenarios;

[assembly: Rock(typeof(ISuppressValueTypeInReturnValue), BuildType.Create)]

namespace Rocks.Scenarios;

public static class ISuppressValueTypeInReturnValueUser
{
	public static void Use()
	{
		var expectations = new ISuppressValueTypeInReturnValueCreateExpectations();

		expectations.Methods.WorkReturningVoid();
		expectations.Methods.WorkReturningString()
			.ReturnValue("done");
		expectations.Methods.WorkReturningValueTask()
			.ReturnValue(new ValueTask());
		expectations.Methods.WorkReturningValueTaskOfString()
			.ReturnValue(ValueTask.FromResult("done"));
		expectations.Methods.WorkReturningValueTaskOfString()
			.ReturnValue(new ValueTask<string>("done"));
	}
}

public interface ISuppressValueTypeInReturnValue
{
	void WorkReturningVoid();
	string WorkReturningString();
	ValueTask WorkReturningValueTask();
	ValueTask<string> WorkReturningValueTaskOfString();
}