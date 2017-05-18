using Rocks.Options;
using System;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Rocks.Sketchpad
{
	public static class Program
	{
		static void Main(string[] args)
		{
			var rock = Rock.Create<UnicodeEncoding>();
			rock.Make();

			//new RockAssembly(typeof(object).GetTypeInfo().Assembly,
			//	new RockOptions(
			//		level: OptimizationSetting.Debug,
			//		codeFile: CodeFileOptions.Create));

			//var iObsoleteType = typeof(Program).GetTypeInfo().Assembly.GetTypes()
			//	.Where(_ => _.Name == "IObsoleteType")
			//	.Single();

			//var createMethod = typeof(Rock).GetMethod("Create", Type.EmptyTypes)
			//		.MakeGenericMethod(iObsoleteType);
			//var rock = createMethod.Invoke(null, null);

			//var makeMethod = rock.GetType().GetTypeInfo().GetMethod("Make", Type.EmptyTypes);
			//makeMethod.Invoke(rock, null);

			//var rock = Rock.Create<IObsoleteType>();
			//rock.Handle(_ => _.OK());

			//var chunk = rock.Make();
			//chunk.OK();

			//rock.Verify();
		}
	}

	//[Obsolete("Don't use this", false)]
	public interface IObsoleteType
	{
		void OK();
		[Obsolete("Don't call this", true)]
		void Obsolete();
	}
}
