using NUnit.Framework;
using System;

namespace Rocks.IntegrationTests
{
	public interface IExplicitInterfaceImplementationOne
	{
		void A();
		int B { get; set; }
		int this[int x] { get; set; }
		event EventHandler C;
	}

	public interface IExplicitInterfaceImplementationTwo
	{
		void A();
		int B { get; set; }
		int this[int x] { get; set; }
		event EventHandler C;
	}

	public interface IExplicitInterfaceImplementation
		: IExplicitInterfaceImplementationOne, IExplicitInterfaceImplementationTwo
	{ }

	public static class ExplicitInterfaceImplementationTests
	{
		[Test]
		public static void MockMethod()
		{
			var rock = Rock.Create<IExplicitInterfaceImplementation>();
			rock.ExplicitMethodsForIExplicitInterfaceImplementationOne().A();
			rock.ExplicitMethodsForIExplicitInterfaceImplementationTwo().A();

			var chunk = rock.Instance();
			((IExplicitInterfaceImplementationOne)chunk).A();
			((IExplicitInterfaceImplementationTwo)chunk).A();

			rock.Verify();
		}

		[Test]
		public static void MockProperty()
		{
			var rock = Rock.Create<IExplicitInterfaceImplementation>();
			rock.ExplicitPropertiesForIExplicitInterfaceImplementationOne().Getters().B();
			rock.ExplicitPropertiesForIExplicitInterfaceImplementationOne().Setters().B(Arg.Any<int>());
			rock.ExplicitPropertiesForIExplicitInterfaceImplementationTwo().Getters().B();
			rock.ExplicitPropertiesForIExplicitInterfaceImplementationTwo().Setters().B(Arg.Any<int>());

			var chunk = rock.Instance();
			var oneValue = ((IExplicitInterfaceImplementationOne)chunk).B;
			((IExplicitInterfaceImplementationOne)chunk).B = oneValue;
			var twoValue = ((IExplicitInterfaceImplementationTwo)chunk).B;
			((IExplicitInterfaceImplementationTwo)chunk).B = twoValue;

			rock.Verify();
		}

		[Test]
		public static void MockIndexer()
		{
			var rock = Rock.Create<IExplicitInterfaceImplementation>();
			rock.ExplicitIndexersForIExplicitInterfaceImplementationOne().Getters().This(Arg.Any<int>());
			rock.ExplicitIndexersForIExplicitInterfaceImplementationOne().Setters().This(Arg.Any<int>(), Arg.Any<int>());
			rock.ExplicitIndexersForIExplicitInterfaceImplementationTwo().Getters().This(Arg.Any<int>());
			rock.ExplicitIndexersForIExplicitInterfaceImplementationTwo().Setters().This(Arg.Any<int>(), Arg.Any<int>());

			var chunk = rock.Instance();
			var oneValue = ((IExplicitInterfaceImplementationOne)chunk)[3];
			((IExplicitInterfaceImplementationOne)chunk)[3] = oneValue;
			var twoValue = ((IExplicitInterfaceImplementationTwo)chunk)[3];
			((IExplicitInterfaceImplementationTwo)chunk)[3] = twoValue;

			rock.Verify();
		}
	}
}