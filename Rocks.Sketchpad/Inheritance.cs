using System;
using System.Reflection;

namespace Rocks.Sketchpad
{
	public static class Inheritance
	{
		public static void Test()
		{
			var subclass = typeof(InheritanceBase);

			//foreach (var method in subclass.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
			//{
			//	Console.Out.WriteLine(method.Name);
			//}

			foreach(var @interface in typeof(IC).GetInterfaces())
			{
				Console.Out.WriteLine(@interface.Name);
			}
		}
	}

	public interface IA { }

	public interface IB : IA { }

	public interface IC : IB { }

   public interface IInheritanceBase
	{
		void BaseTarget();
	}

	public interface IInheritanceSubclass
		: IInheritanceBase
	{
		void SubclassTarget();
	}

	public abstract class InheritanceBase
		: IInheritanceSubclass
	{
		public virtual void BaseTarget()
		{
			throw new NotImplementedException();
		}

		public abstract void AbstractTarget();
		public abstract void SubclassTarget();
	}
}
