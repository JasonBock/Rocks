using Rocks;
using RocksTest;
using System;

unsafe
{
	var value = 10;

	var rock = Rock.Create<IHavePointers>();
	rock.Methods().PointerParameter(new()).Callback(_ => *_ = 20);

	var chunk = rock.Instance();
	chunk.PointerParameter(&value);

	rock.Verify();

	Console.Out.WriteLine($"Create Success! {nameof(value)} is {value}");
}

namespace RocksTest
{
	public unsafe interface IHavePointers
	{
		void PointerParameter(int* value);
	}
}