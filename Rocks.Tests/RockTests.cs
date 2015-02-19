using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Exceptions;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class RockTests
	{
		[Test]
		public void Run()
		{
			var rock = Rock.Create<ITest>();
			rock.Handle<int>(
				_ => _.Foo(default(int)),
				a => { });
			rock.Handle<string, Guid, int>(
				_ => _.Bar(default(Guid), default(int)),
				(a, b) => { return a.ToString() + " - " + b.ToString(); });

			var chunk = rock.Make();
			var result = chunk.Bar(Guid.NewGuid(), 44);

			Assert.IsTrue(result.Length > 0);
		}

		[Test]
		public void Verify()
		{
			var rock = Rock.Create<ITest>();
			rock.Handle<int>(
				_ => _.Foo(default(int)),
				a => { });

			var chunk = rock.Make();
			chunk.Foo(44);

			rock.Verify();
		}

		[Test]
		public void VerifyWhenMethodIsNotCalled()
		{
			var rock = Rock.Create<ITest>();
			rock.Handle<int>(
				_ => _.Foo(default(int)),
				a => { });

			var chunk = rock.Make();

			Assert.Throws<RockVerificationException>(() => rock.Verify());
		}

		[Test]
		public void RunWhenMethodIsCalledWithoutHandle()
		{
			var rock = Rock.Create<ITest>();
			var chunk = rock.Make();

			Assert.Throws<NotImplementedException>(() => chunk.Foo(44));
		}

		[Test]
		public void RunWithDebug()
		{
			var rock = Rock.Create<ITestDebug>(
				new RockOptions(OptimizationLevel.Debug, true));
			rock.Handle<int>(
				_ => _.Foo(default(int)),
				a => { });
			rock.Handle<string, Guid, int>(
				_ => _.Bar(default(Guid), default(int)),
				(a, b) => { return a.ToString() + " - " + b.ToString(); });

			var chunk = rock.Make();
			var result = chunk.Bar(Guid.NewGuid(), 44);

			Assert.IsTrue(result.Length > 0);
		}
	}

	public interface ITest
	{
		void Foo(int x);
		string Bar(Guid a, int b);
	}

	public interface ITestDebug
	{
		void Foo(int x);
		string Bar(Guid a, int b);
	}
}
