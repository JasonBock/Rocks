using Rocks;
using RocksTest;
using System;

using var repository = new RockRepository();

var firstRock = repository.Create<IFirstRepository>();
firstRock.Methods().Foo();

var secondRock = repository.Create<ISecondRepository>();
secondRock.Methods().Bar();

var firstChunk = firstRock.Instance();
firstChunk.Foo();

var secondChunk = secondRock.Instance();
secondChunk.Bar();

Console.Out.WriteLine("Success!");

namespace RocksTest
{
	public interface IFirstRepository
	{
		void Foo();
	}

	public interface ISecondRepository
	{
		void Bar();
	}
}