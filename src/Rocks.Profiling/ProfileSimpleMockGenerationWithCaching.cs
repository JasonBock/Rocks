using Rocks.Options;

namespace Rocks.Profiling
{
	internal static class ProfileSimpleMockGenerationWithCaching
	{
		private const uint Iterations = 1000;

		internal static void Profile()
		{
			for(var i = 0; i < ProfileSimpleMockGenerationWithCaching.Iterations; i++)
			{
				var rock = Rock.Create<IAmSimple>();
				rock.Handle(_ => _.DoIt());
				rock.Make();
			}
		}
	}
}
