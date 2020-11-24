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