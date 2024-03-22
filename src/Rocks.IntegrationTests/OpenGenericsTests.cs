﻿using NUnit.Framework;

namespace Rocks.IntegrationTests;

public interface IService<T, TReturn>
{
	TReturn Service(T data);
}

public static class OpenGenericsTests
{
	[Test]
	[RockCreate(typeof(IService<,>))]
	public static void CreateWithIntAndString()
	{
		var intStringExpectations = new IServiceCreateExpectations<int, string>();
		intStringExpectations.Methods.Service(3).ReturnValue("three");

		var intStringMock = intStringExpectations.Instance();
		Assert.That(intStringMock.Service(3), Is.EqualTo("three"));
		intStringExpectations.Verify();
	}

	[Test]
	[RockCreate(typeof(IService<,>))]
	public static void CreateWithStringAndInt()
	{
		var stringIntExpectations = new IServiceCreateExpectations<string, int>();
		stringIntExpectations.Methods.Service("four").ReturnValue(4);

		var stringIntMock = stringIntExpectations.Instance();
		Assert.That(stringIntMock.Service("four"), Is.EqualTo(4));
		stringIntExpectations.Verify();
	}

	[Test]
	[RockMake(typeof(IService<,>))]
	public static void MakeWithIntAndString()
	{
		var intStringMake = new IServiceMakeExpectations<int, string>().Instance();
		Assert.That(intStringMake.Service(3), Is.Null);
	}

	[Test]
	[RockMake(typeof(IService<,>))]
	public static void MakeWithStringAndInt()
	{
		var intStringMake = new IServiceMakeExpectations<string, int>().Instance();
		Assert.That(intStringMake.Service("four"), Is.EqualTo(0));
	}
}