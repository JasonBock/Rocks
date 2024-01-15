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
	/// <typeparam name="THandler">The handler type</typeparam>
	/// <param name="handlers">A list of handlers to verify</param>
	/// <param name="memberIdentifier">The member identifier for the handlers</param>
	/// <returns>A list of failed expectations</returns>
	protected List<string> Verify<THandler>(List<THandler> handlers, uint memberIdentifier)
		where THandler : Handler =>
		handlers.Where(_ => _.ExpectedCallCount != _.CallCount)
			.Select(_ =>
			{
				var member = this.MockType!.GetMemberDescription(memberIdentifier);
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