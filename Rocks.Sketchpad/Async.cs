using System.Threading.Tasks;

namespace Rocks.Sketchpad
{
	public class Async
	{
		public virtual async Task<int> GoAsync()
		{
			return await Task.FromResult<int>(44);
		}

		public virtual Task<int> Go()
		{
			return Task.FromResult<int>(44);
		}
	}

	public class SubAsync : Async
	{
		public override Task<int> GoAsync()
		{
			return base.GoAsync();
		}
	}
}
