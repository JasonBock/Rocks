using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class RefAndOutTests
	{
		private void MyActionOutTarget(out int a)
		{
			a = 2;
		}

		private void MyActionRefTarget(ref int a)
		{
			a = 2;
		}

		private void MyActionRefGuidTarget(ref Guid a)
		{
			a = Guid.NewGuid();
		}

		private int MyFuncRefTarget(ref int a)
		{
			a = 2;
			return 4;
		}

		private int MyFuncOutTarget(out int a)
		{
			a = 2;
			return 4;
		}

		[Test]
		public void MakeRefActionWithDelegate()
		{
			var a = 1;
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Handle(_ => _.RefTarget(ref a), new RefTarget(this.MyActionRefTarget));

			var chunk = rock.Make();
			chunk.RefTarget(ref a);

			Assert.AreEqual(2, a, nameof(a));
			rock.Verify();
		}

		[Test]
		public void MakeRefActionWithDelegateAndEventRaised()
		{
			var a = 1;
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Handle(_ => _.RefTarget(ref a), new RefTarget(this.MyActionRefTarget))
				.Raises(nameof(IHaveRefAndOut.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.RefTarget(ref a);

			Assert.AreEqual(2, a, nameof(a));
			Assert.AreEqual(1, eventRaisedCount);
			rock.Verify();
		}

		[Test]
		public void MakeRefActionWithGenericDelegate()
		{
			var a = Guid.Empty;
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Handle(_ => _.RefTargetWithGeneric<Guid>(ref a), 
				new RefTargetWithGeneric<Guid>(this.MyActionRefGuidTarget));

			var chunk = rock.Make();
			chunk.RefTargetWithGeneric(ref a);

			Assert.AreNotEqual(Guid.Empty, a, nameof(a));
			rock.Verify();
		}

		[Test]
		public void MakeRefActionWithDelegateAndExpectedCallCount()
		{
			var a = 1;
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Handle(_ => _.RefTarget(ref a), new RefTarget(this.MyActionRefTarget), 2);

			var chunk = rock.Make();
			chunk.RefTarget(ref a);
			chunk.RefTarget(ref a);

			Assert.AreEqual(2, a, nameof(a));
			rock.Verify();
		}

		[Test]
		public void MakeRefActionWithDelegateAndEventRaisedAndExpectedCallCount()
		{
			var a = 1;
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Handle(_ => _.RefTarget(ref a), new RefTarget(this.MyActionRefTarget), 2)
				.Raises(nameof(IHaveRefAndOut.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.RefTarget(ref a);
			chunk.RefTarget(ref a);

			Assert.AreEqual(2, a, nameof(a));
			Assert.AreEqual(2, eventRaisedCount);
			rock.Verify();
		}

		[Test]
		public void MakeOutActionWithDelegate()
		{
			var a = 1;
			var rock = Rock.Create<IHaveRefAndOut>(new Options(codeFile: CodeFileOptions.Create));
			rock.Handle(_ => _.OutTarget(out a), new OutTarget(this.MyActionOutTarget));

			var chunk = rock.Make();
			chunk.OutTarget(out a);

			Assert.AreEqual(2, a, nameof(a));
			rock.Verify();
		}

		[Test]
		public void MakeOutActionWithDelegateAndExpectedCallCount()
		{
			var a = 1;
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Handle(_ => _.OutTarget(out a), new OutTarget(this.MyActionOutTarget), 2);

			var chunk = rock.Make();
			chunk.OutTarget(out a);
			chunk.OutTarget(out a);

			Assert.AreEqual(2, a, nameof(a));
			rock.Verify();
		}

		[Test]
		public void MakeRefFuncWithDelegate()
		{
			var a = 1;
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Handle(_ => _.RefTargetWithReturn(ref a), new RefTargetWithReturn(this.MyFuncRefTarget));

			var chunk = rock.Make();
			chunk.RefTargetWithReturn(ref a);

			Assert.AreEqual(2, a, nameof(a));
			rock.Verify();
		}

		[Test]
		public void MakeRefFuncWithDelegateAndExpectedCallCount()
		{
			var a = 1;
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Handle(_ => _.RefTargetWithReturn(ref a), new RefTargetWithReturn(this.MyFuncRefTarget), 2);

			var chunk = rock.Make();
			chunk.RefTargetWithReturn(ref a);
			chunk.RefTargetWithReturn(ref a);

			Assert.AreEqual(2, a, nameof(a));
			rock.Verify();
		}

		[Test]
		public void MakeOutFuncWithDelegate()
		{
			var a = 1;
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Handle(_ => _.OutTargetWithReturn(out a), new OutTargetWithReturn(this.MyFuncOutTarget));

			var chunk = rock.Make();
			chunk.OutTargetWithReturn(out a);

			Assert.AreEqual(2, a, nameof(a));
			rock.Verify();
		}

		[Test]
		public void MakeOutFuncWithDelegateWithExpectedCallCount()
		{
			var a = 1;
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Handle(_ => _.OutTargetWithReturn(out a), new OutTargetWithReturn(this.MyFuncOutTarget), 2);

			var chunk = rock.Make();
			chunk.OutTargetWithReturn(out a);
			chunk.OutTargetWithReturn(out a);

			Assert.AreEqual(2, a, nameof(a));
			rock.Verify();
		}
	}

	public interface IHaveRefAndOut
	{
		event EventHandler TargetEvent;
		void OutTarget(out int a);
		int OutTargetWithReturn(out int a);
		void RefTarget(ref int a);
		void RefTargetWithGeneric<T>(ref T a);
		int RefTargetWithReturn(ref int a);
	}

	public delegate void OutTarget(out int a);
	public delegate int OutTargetWithReturn(out int a);
	public delegate void RefTarget(ref int a);
	public delegate void RefTargetWithGeneric<T>(ref T a);
	public delegate int RefTargetWithReturn(ref int a);
}
