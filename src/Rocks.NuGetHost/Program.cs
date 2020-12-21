using Rocks;
using RocksTest;
using System;

unsafe
{
	var value = 10;

	var rock = Rock.Create<IHavePointers>();
	rock.Methods().PointerParameter(new()).Callback(PointerParameterCallback);

	var chunk = rock.Instance();
	chunk.PointerParameter(&value);

	rock.Verify();

	Console.Out.WriteLine($"Create Success! {nameof(value)} is {value}");
}

unsafe static void PointerParameterCallback(int* callbackValue) => *callbackValue = 20;

namespace RocksTest
{
	public unsafe interface IHavePointers
	{
		void PointerParameter(int* value);
	}
}
