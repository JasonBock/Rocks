using Rocks.Options;

namespace Rocks.Sketchpad
{
	internal static class Demo
	{
		internal static void Demonstrate()
		{
			//var rock = Rock.Create<IService>(
			//	new RockOptions(level: OptimizationSetting.Debug, codeFile: CodeFileOption.Create));
			var rock = Rock.Create<IService>();
			//rock.Handle(_ => _.ServiceOne());
			rock.Handle(_ => _.ServiceTwo(3));

			var chunk = rock.Make();

			var user = new UsesService(chunk);
			user.Use(3);

			rock.Verify();
		}
	}

	public interface IService
	{
		void ServiceOne();
		void ServiceTwo(int id);
	}

	public class UsesService
	{
		private readonly IService service;

		public UsesService(IService service) => 
			this.service = service;

		public void Use(int id)
		{
			this.service.ServiceTwo(id);
			this.service.ServiceOne();
		}
	}
}