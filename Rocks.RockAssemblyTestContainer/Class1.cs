using System;

namespace Rocks.RockAssemblyTestContainer
{
	public class Class1
	{
		public virtual void Method1() { }
		public virtual string Method2(int a) { return default(string); }
		public virtual Guid Method3(string a, int b) { return default(Guid); }
		public virtual Guid Method4(string a, ref int b) { return default(Guid); }
	}
}
