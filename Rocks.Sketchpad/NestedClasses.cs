using System;

namespace Rocks.Sketchpad
{
	public class NestedClasses
	{
		public class SubnestedClass
		{
			public interface IAmSubnested { }
		}

		public interface IAmNested { }

		public static void Test()
		{
			var type = typeof(NestedClasses.SubnestedClass.IAmSubnested);
         Console.Out.WriteLine($"{nameof(type.Name)}: {type.Name}");
			Console.Out.WriteLine($"{nameof(type.FullName)}: {type.FullName}");
			Console.Out.WriteLine($"{nameof(type.Namespace)}: {type.Namespace}");
		}
	}
}
