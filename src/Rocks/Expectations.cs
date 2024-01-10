using Rocks.Extensions;

namespace Rocks;

/// <summary>
/// The base class for all expectation types.
/// </summary>
public abstract class Expectations
{
	/// <summary>
	/// Verifies the expectations.
	/// </summary>
	public abstract void Verify();

	/// <summary>
	/// Verifies handlers.
	/// </summary>
	/// <typeparam name="THandler"></typeparam>
	/// <param name="handlers"></param>
	/// <returns></returns>
	protected List<string> Verify<THandler>(List<THandler> handlers)
		where THandler : Handler =>
		handlers.Where(_ => _.ExpectedCallCount != _.CallCount)
			.Select(_ =>
			{
				var member = this.MockType!.GetMemberDescription(0);
				return $"Mock type: {this.MockType!.FullName}, member: {member}, messsage: The expected call count is incorrect. Expected: {_.ExpectedCallCount}, received: {_.CallCount}.";
			}).ToList();

	/// <summary>
	/// Gets or sets the mock type.
	/// </summary>
	protected Type? MockType { get; set; }

	/// <summary>
	/// Gets or sets the flag to determine if an mock instance was created.
	/// </summary>
	protected bool WasInstanceInvoked { get; set; }
}