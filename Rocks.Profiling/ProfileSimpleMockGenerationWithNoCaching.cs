using Rocks.Options;

namespace Rocks.Profiling
{
	internal static class ProfileSimpleMockGenerationWithNoCaching
	{
		private const uint Iterations = 50000;

		internal static void Profile()
		{
			for(var i = 0; i < ProfileSimpleMockGenerationWithNoCaching.Iterations; i++)
			{
				var rock = Rock.Create<IAmSimple>(
					new RockOptions(caching: CachingOption.GenerateNewVersion));
				rock.Handle(_ => _.DoIt());
				try
				{
					rock.Make();
				}
				catch { }
			}
		}
	}
}
