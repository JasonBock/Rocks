using NUnit.Framework;
using System;

namespace Rocks.IntegrationTests
{
	public interface IExplicitInterfaceImplementationTestsOne
	{
		void A();
		int B { get; set; }
		int this[int x] { get; set; }
		event EventHandler C;
	}

	public interface IExplicitInterfaceImplementationTestsTwo
	{
		void A();
		int B { get; set; }
		int this[int x] { get; set; }
		event EventHandler C;
	}

	public interface IExplicitInterfaceImplementationTests
		: IExplicitInterfaceImplementationTestsOne, IExplicitInterfaceImplementationTestsTwo
	{ }

	public class X : IExplicitInterfaceImplementationTests
	{
		public int this[int x]
		{
			get => 0;
			set { }
		}

		public int B { get; set; }

		public event EventHandler? C;

		public void A() { }
		//void IExplicitInterfaceImplementationTestsOne.A() => throw new NotImplementedException();
		//void IExplicitInterfaceImplementationTestsTwo.A() => throw new NotImplementedException();
	}

	public interface IA
	{
		void Foo();
	}

	public interface IB
	{
		void Foo();
	}

	public interface IC
		: IA, IB
	{ }

	public class C : IC
	{
		void IA.Foo() { }

		void IB.Foo() { }
	}

	public static class ExplicitInterfaceImplementationTests
	{
		[Test]
		public static void MockMethod()
		{
			var rock = Rock.Create<IExplicitInterfaceImplementationTests>();
			rock.ExplicitMethodsForIExplicitInterfaceImplementationTestsOne().A();
			rock.ExplicitMethodsForIExplicitInterfaceImplementationTestsTwo().A();

			var chunk = rock.Instance();
			((IExplicitInterfaceImplementationTestsOne)chunk).A();
			((IExplicitInterfaceImplementationTestsTwo)chunk).A();

			rock.Verify();
		}
	}
}