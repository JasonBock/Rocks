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

		[Test]
		public static void MockProperty()
		{
			var rock = Rock.Create<IExplicitInterfaceImplementationTests>();
			rock.ExplicitPropertiesForIExplicitInterfaceImplementationTestsOne().Getters().B();
			rock.ExplicitPropertiesForIExplicitInterfaceImplementationTestsOne().Setters().B(Arg.Any<int>());
			rock.ExplicitPropertiesForIExplicitInterfaceImplementationTestsTwo().Getters().B();
			rock.ExplicitPropertiesForIExplicitInterfaceImplementationTestsTwo().Setters().B(Arg.Any<int>());

			var chunk = rock.Instance();
			var oneValue = ((IExplicitInterfaceImplementationTestsOne)chunk).B;
			((IExplicitInterfaceImplementationTestsOne)chunk).B = oneValue;
			var twoValue = ((IExplicitInterfaceImplementationTestsTwo)chunk).B;
			((IExplicitInterfaceImplementationTestsTwo)chunk).B = twoValue;

			rock.Verify();
		}

		[Test]
		public static void MockIndexer()
		{
			var rock = Rock.Create<IExplicitInterfaceImplementationTests>();
			rock.ExplicitIndexersForIExplicitInterfaceImplementationTestsOne().Getters().This(Arg.Any<int>());
			rock.ExplicitIndexersForIExplicitInterfaceImplementationTestsOne().Setters().This(Arg.Any<int>(), Arg.Any<int>());
			rock.ExplicitIndexersForIExplicitInterfaceImplementationTestsTwo().Getters().This(Arg.Any<int>());
			rock.ExplicitIndexersForIExplicitInterfaceImplementationTestsTwo().Setters().This(Arg.Any<int>(), Arg.Any<int>());

			var chunk = rock.Instance();
			var oneValue = ((IExplicitInterfaceImplementationTestsOne)chunk)[3];
			((IExplicitInterfaceImplementationTestsOne)chunk)[3] = oneValue;
			var twoValue = ((IExplicitInterfaceImplementationTestsTwo)chunk)[3];
			((IExplicitInterfaceImplementationTestsTwo)chunk)[3] = twoValue;

			rock.Verify();
		}
	}
}