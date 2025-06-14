﻿using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Analysis.IntegrationTests.ExpectationExceptionTestTypes;

public interface ISetAfterMock
{
	void Work();
	void WorkWithData(string data1, int data2, char[] data3);
	string Data { get; set; }
	int this[string Index] { get; set; }
}

public static class ExpectationExceptionTests
{
	[Test]
	public static void GetExceptionMessage()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Methods.WorkWithData("a", 2, new[] { 'c', 'd' });

		var mock = expectations.Instance();
		Assert.That(() => mock.WorkWithData("b", 3, ['e', 'f', 'g']), 
			Throws.TypeOf<ExpectationException>()
				.With.Message.EqualTo(
					"""
					No handlers match for Void WorkWithData(System.String, Int32, Char[])
						data1: b
						data2: 3
						data3: System.Char[], Count = 3
					"""));
	}

	[Test]
	public static void SetMethodExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Methods.Work();

		var mock = expectations.Instance();

		Assert.That(expectations.Methods.Work, Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void SetPropertyGetterExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Properties.Getters.Data();

		var mock = expectations.Instance();

		Assert.That(expectations.Properties.Getters.Data, Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void SetPropertySetterExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Properties.Setters.Data("a");

		var mock = expectations.Instance();

		Assert.That(() => expectations.Properties.Setters.Data("a"), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void SetIndexerGetterExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Indexers.Getters.This("a");

		var mock = expectations.Instance();

		Assert.That(() => expectations.Indexers.Getters.This("a"), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void SetIndexerSetterExpectationAfterMockIsCreate()
	{
		var expectations = new ISetAfterMockCreateExpectations();
		expectations.Indexers.Setters.This(1, "a");

		var mock = expectations.Instance();

		Assert.That(() => expectations.Indexers.Setters.This(1, "a"), Throws.TypeOf<ExpectationException>());
	}
}