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
			var rock = Rock.Create<ClassTestsTarget>(new Options(CodeFileOptions.Create));
			rock.HandleAction(_ => _.TargetMethod());

			var chunk = rock.Make();
			chunk.TargetMethod();

			rock.Verify();
		}

		[Test]
		public void MakeWithCustomConstructor()
		{
			var rock = Rock.Create<ClassTestsTarget>();
			rock.HandleAction(_ => _.TargetMethod());

			var chunk = rock.Make(new object[] { 2 });
			chunk.TargetMethod();

			Assert.AreEqual(2, chunk.Value, nameof(chunk.Value));

			rock.Verify();
		}
	}

	public class ClassTestsTarget
	{
		public ClassTestsTarget() { }

		public ClassTestsTarget(int value)
		{
			this.Value = value;
		}

		public virtual void TargetMethod()
		{
			this.TargetEvent(this, EventArgs.Empty);
		}

		public virtual int TargetProperty { get; set; }

		public int Value { get; private set; }

		public virtual event EventHandler TargetEvent;
	}
}
