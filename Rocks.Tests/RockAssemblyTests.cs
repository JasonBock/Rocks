using NUnit.Framework;
using Rocks.RockAssemblyTestContainer;
using Rocks.RockAssemblyTestContainer.Contracts;
using Rocks.RockAssemblyTestContainer.Extensions.TestAssembly.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using static Rocks.Extensions.IMockExtensions;

namespace Rocks.Tests
{
	[TestFixture]
	public class RockAssemblyTests
	{
		private Assembly assembly;

		public RockAssemblyTests()
		{
			this.assembly = new RockAssembly(typeof(Class1).Assembly,
				new Options(codeFile: CodeFileOptions.Create)).Result;
		}

		[Test]
		public void Create()
		{
			Assert.IsNotNull(this.assembly.GetType($"{typeof(Class1).Namespace}.Rock{nameof(Class1)}"));
			Assert.IsNotNull(this.assembly.GetType($"{typeof(Interface1<>).Namespace}.Rock{typeof(Interface1<>).Name}"));
         Assert.IsNull(this.assembly.GetType($"{typeof(Class3).Namespace}.Rock{nameof(Class3)}"));
		}

		[Test]
		public void GenerateRock()
		{
			var rock = typeof(Rock).GetMethod(nameof(Rock.Create), Type.EmptyTypes).MakeGenericMethod(
				new[] { this.assembly.GetType($"{typeof(Class1).Namespace}.Rock{nameof(Class1)}") }).Invoke(null, null);
			Assert.IsTrue(typeof(AssemblyRock<>).IsAssignableFrom(rock.GetType().GetGenericTypeDefinition()));
		}

		[Test]
		public void GenerateMockWithHandlers()
		{
			var b = 44;
			
			var rock = Rock.Create<Class1>();
			rock.Handle(_ => _.Method1());

			var method4DelegateType = this.assembly.GetType($"{typeof(Class1).Namespace}.Rock{nameof(Class1)}+Rock{nameof(Class1)}_{nameof(Class1.Method4)}Delegate");
         rock.Handle(_ => _.Method4("a", ref b), Delegate.CreateDelegate(
				method4DelegateType, this,
				this.GetType().GetMethod(nameof(RockAssemblyTests.Method4))));

			var method5DelegateType = this.assembly.GetType($"{typeof(Class1).Namespace}.Rock{nameof(Class1)}+Rock{nameof(Class1)}_{nameof(Class1.Method5)}Delegate`1").MakeGenericType(b.GetType());
			rock.Handle(_ => _.Method5("a", ref b), Delegate.CreateDelegate(
				method5DelegateType, this,
				this.GetType().GetMethod(nameof(RockAssemblyTests.Method5)).MakeGenericMethod(b.GetType())));

			var handlers = rock.GetType().GetMethod("CreateReadOnlyHandlerDictionary", BindingFlags.Instance | BindingFlags.NonPublic)
				.Invoke(rock, null) as ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>;
         var mock = Activator.CreateInstance(this.assembly.GetType($"{typeof(Class1).Namespace}.Rock{nameof(Class1)}"), handlers) as Class1;

			mock.Method1();
			mock.Method4("a", ref b);
			mock.Method5("a", ref b);

			Assert.AreEqual(0, (mock as IMock).GetVerificationFailures().Count);
			Assert.IsTrue(this.wasMethod4DelegateCalled);
			Assert.IsTrue(this.wasMethod5DelegateCalled);
		}

		[Test, Ignore("A")]
		public void GenerateForMscorlib()
		{
			var stopwatch = System.Diagnostics.Stopwatch.StartNew();

			new RockAssembly(typeof(object).Assembly);

			stopwatch.Stop();

			Assert.Pass($"Total time to generate mocks for mscorlib: {stopwatch.Elapsed}");
		}

		public Guid Method4(string a, ref int b)
		{
			this.wasMethod4DelegateCalled = true;
			return default(Guid);
		}

		public Guid Method5<U>(string a, ref U b)
		{
			this.wasMethod5DelegateCalled = true;
			return default(Guid);
		}

		private bool wasMethod4DelegateCalled;
		private bool wasMethod5DelegateCalled;
	}
}
