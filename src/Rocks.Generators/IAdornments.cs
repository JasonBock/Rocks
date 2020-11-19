namespace Rocks
{
	public interface IAdornments<out THandler>
		where THandler : HandlerInformation
	{
		THandler Handler { get; }
	}
}