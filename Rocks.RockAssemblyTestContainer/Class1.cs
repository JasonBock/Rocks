using System;

namespace Rocks.RockAssemblyTestContainer
{
	public class Class1
	{
		public virtual void Method1() { }
		public virtual string Method2(int a) => default; 
		public virtual Guid Method3(string a, int b) => default; 
		public virtual Guid Method4(string a, ref int b) => default; 
		public virtual Guid Method5<U>(string a, ref U b) => default; 
	}
}
