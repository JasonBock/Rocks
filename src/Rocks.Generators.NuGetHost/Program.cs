using Rocks;
using RocksTest;
using System;

var rock = Rock.Create<IMockTest>();
rock.Methods().Foo();

var chunk = rock.Instance();
chunk.Foo();

rock.Verify();

Console.Out.WriteLine("Success!");

namespace RocksTest
{
	public interface IMockTest
	{
		void Foo();
	}
}