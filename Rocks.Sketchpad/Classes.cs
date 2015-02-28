using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocks.Sketchpad
{
	public interface IBase
	{
		void BaseMethod();
		void BaseMethodThatIsNotMadeVirtual();
	}

	public class BaseClass : IBase
	{
		public virtual void BaseMethod()
		{
			throw new NotImplementedException();
		}

		public void BaseMethodThatIsNotMadeVirtual()
		{
			throw new NotImplementedException();
		}

		public virtual void TargetMethod() { }

		public virtual int TargetProperty { get; set; }

		public virtual event EventHandler TargetEvent;
	}

	public class RockClass : BaseClass
	{
		public override void BaseMethod()
		{
		}

		public override bool Equals(object obj)
		{
			return default(bool);
		}

		public override int GetHashCode()
		{
			return default(int);
		}

		public override event EventHandler TargetEvent;

		public override void TargetMethod()
		{
		}

		public override int TargetProperty
		{
			get; set;
		}

		public override string ToString()
		{
			return default(string);
		}
	}
}
