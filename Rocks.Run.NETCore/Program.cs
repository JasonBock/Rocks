namespace Rocks.Run.NETCore
{
	class Program
	{
		static void Main(string[] args)
		{
			var rock = Rock.Create<IAmSimple>();
			rock.Handle(_ => _.TargetAction());
			rock.Handle(_ => _.TargetFunc()).Returns(44);

			var chunk = rock.Make();
			chunk.TargetAction();
			var result = chunk.TargetFunc();

			rock.Verify();
		}
	}

	public interface IAmSimple
	{
		void TargetAction();
		int TargetFunc();
	}
}