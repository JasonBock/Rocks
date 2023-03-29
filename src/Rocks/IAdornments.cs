namespace Rocks;

/// <summary>
/// Defines the base interface for all adornment types.
/// </summary>
/// <typeparam name="THandler">The type of the handler.</typeparam>
public interface IAdornments<out THandler>
	where THandler : HandlerInformation
{
	/// <summary>
	/// Gets the handler for this adornment.
	/// </summary>
	THandler Handler { get; }
}