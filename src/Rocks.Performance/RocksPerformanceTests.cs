using BenchmarkDotNet.Attributes;
using System;

namespace Rocks.Performance
{
	[MemoryDiagnoser]
	public class RocksPerformanceTests
	{
		[Benchmark]
		public int MakeRefActionWithDelegate()
		{
			var a = 1;
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Handle(_ => _.RefTarget(ref a), new RefTarget(this.MyActionRefTarget));

			var chunk = rock.Make();
			chunk.RefTarget(ref a);

			rock.Verify();
			return a;
		}

		private void MyActionRefTarget(ref int a) => a = 2;
	}

	public delegate void RefTarget(ref int a);

	public interface IHaveRefAndOut
	{
		event EventHandler TargetEvent;
		void OutTarget(out int a);
		int OutTargetWithReturn(out int a);
		void RefTarget(ref int a);
		void RefTargetWithGeneric<T>(ref T a);
		int RefTargetWithReturn(ref int a);
	}
}
