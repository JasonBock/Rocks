using System;
using NUnit.Framework;
using Microsoft.CodeAnalysis;

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
