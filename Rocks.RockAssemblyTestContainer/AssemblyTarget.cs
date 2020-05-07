using System;

namespace Rocks.RockAssemblyTestContainer
{
	public class AssemblyTarget
	{
		public virtual void Method1() { }
		public virtual string Method2(int a) => null!; 
		public virtual Guid Method3(string a, int b) => default; 
		public virtual Guid Method4(string a, ref int b) => default; 
		public virtual Guid Method5<T>(string a, ref T b) => default; 
	}
}