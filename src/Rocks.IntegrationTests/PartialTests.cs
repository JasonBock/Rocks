﻿using NUnit.Framework;

namespace Rocks.IntegrationTests.PartialTestTypes;

public interface ITargetPartial
{
	void Work();
}

public interface IGenericTargetPartial<T>
{
	T Work();
}

[RockPartial(typeof(ITargetPartial), BuildType.Create)]
public sealed partial class PartialCreateExpectations;

[RockPartial(typeof(ITargetPartial), BuildType.Make)]
public sealed partial class PartialMakeExpectations;

[RockPartial(typeof(IGenericTargetPartial<>), BuildType.Create)]
public sealed partial class GenericPartialCreateExpectations<T>;

[RockPartial(typeof(IGenericTargetPartial<>), BuildType.Make)]
public sealed partial class GenericPartialMakeExpectations<T>;

public static class PartialTests
{
	[Test]
	public static void Create()
	{
		var expectations = new PartialCreateExpectations();
		expectations.Methods.Work();

		var mock = expectations.Instance();
		mock.Work();

		expectations.Verify();
	}

	[Test]
	public static void CreateGeneric()
	{
		var expectations = new GenericPartialCreateExpectations<int>();
		expectations.Methods.Work().ReturnValue(3);

		var mock = expectations.Instance();
		Assert.That(mock.Work(), Is.EqualTo(3));

		expectations.Verify();
	}

	[Test]
	public static void Make()
	{
		var mock = new PartialMakeExpectations().Instance();
		mock.Work();
	}

	[Test]
	public static void MakeGeneric()
	{
		var mock = new GenericPartialMakeExpectations<int>().Instance();
		_ = mock.Work();
	}
}