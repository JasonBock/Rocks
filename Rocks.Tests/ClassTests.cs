using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class ClassTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<ClassTestsTarget>();
			rock.HandleAction(_ => _.TargetMethod());

			var chunk = rock.Make();
			chunk.TargetMethod();

			rock.Verify();
		}
	}

	public class ClassTestsTarget
	{
		public virtual void TargetMethod()
		{
			this.TargetEvent(this, EventArgs.Empty);
		}

		public virtual int TargetProperty { get; set; }

		public virtual event EventHandler TargetEvent;
	}
}
