using NUnit.Framework;
using Rocks.Options;
using Rocks.RockAssemblyTestContainer;
using Rocks.RockAssemblyTestContainer.Contracts;
using Rocks.RockAssemblyTestContainer.Extensions.TestAssembly.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using static Rocks.Extensions.IMockExtensions;

namespace Rocks.Tests
{
	public sealed class RockAssemblyTests
	{
		private readonly Assembly assembly;

		public RockAssemblyTests()
		{
			var class1Assembly = typeof(Class1).Assembly;
			this.assembly = new RockAssembly(class1Assembly, 
				new RockOptions(codeFileDirectory: TestContext.CurrentContext.TestDirectory)).Result;
		}

		[Test]
		public void Create()
		{
			Assert.That(this.assembly.GetType($"{typeof(Class1).Namespace}.Rock{nameof(Class1)}"), Is.Not.Null);
			Assert.That(this.assembly.GetType($"{typeof(Interface1<>).Namespace}.Rock{typeof(Interface1<>).Name}"), Is.Not.Null);
			Assert.That(this.assembly.GetType($"{typeof(Class3).Namespace}.Rock{nameof(Class3)}"), Is.Null);
		}

		[Test]
		public void GenerateRock()
		{
			var rock = typeof(Rock).GetMethod(nameof(Rock.Create), Type.EmptyTypes)!.MakeGenericMethod(
				new[] { this.assembly.GetType($"{typeof(Class1).Namespace}.Rock{nameof(Class1)}")! }).Invoke(null, null)!;
			Assert.That(typeof(AssemblyRock<>).IsAssignableFrom(rock.GetType().GetGenericTypeDefinition()), Is.True);
		}

		[Test]
		public void GenerateMockWithHandlers()
		{
			var b = 44;
			
			var rock = Rock.Create<Class1>();
			rock.Handle(_ => _.Method1());

			var method4DelegateType = this.assembly.GetType($"{typeof(Class1).Namespace}.Rock{nameof(Class1)}+Rock{nameof(Class1)}_{nameof(Class1.Method4)}Delegate")!;
			rock.Handle(_ => _.Method4("a", ref b), Delegate.CreateDelegate(
				method4DelegateType, this,
				this.GetType().GetMethod(nameof(RockAssemblyTests.Method4))!));

			var method5DelegateType = this.assembly.GetType($"{typeof(Class1).Namespace}.Rock{nameof(Class1)}+Rock{nameof(Class1)}_{nameof(Class1.Method5)}Delegate`1")!.MakeGenericType(b.GetType());
			rock.Handle(_ => _.Method5("a", ref b), Delegate.CreateDelegate(
				method5DelegateType, this,
				this.GetType().GetMethod(nameof(RockAssemblyTests.Method5))!.MakeGenericMethod(b.GetType())));

			var handlers = rock.GetType().GetMethod("CreateReadOnlyHandlerDictionary", BindingFlags.Instance | BindingFlags.NonPublic)
				!.Invoke(rock, null) as ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>;
			var mock = (Class1)Activator.CreateInstance(this.assembly.GetType($"{typeof(Class1).Namespace}.Rock{nameof(Class1)}")!, handlers)!;

			mock.Method1();
			mock.Method4("a", ref b);
			mock.Method5("a", ref b);

			Assert.That(((IMock)mock).GetVerificationFailures().Count, Is.EqualTo(0));
			Assert.That(this.wasMethod4DelegateCalled, Is.True);
			Assert.That(this.wasMethod5DelegateCalled, Is.True);
		}

		[Test]
		public void GenerateForAssemblyThatContainsObject()
		{
			var assembly = new RockAssembly(typeof(object).Assembly, 
				new RockOptions(
					level: OptimizationSetting.Debug, 
					codeFile: CodeFileOptions.Create,
					codeFileDirectory: TestContext.CurrentContext.TestDirectory));

			Assert.That(assembly.Result.GetTypes().Length, Is.GreaterThan(0));
		}

		public Guid Method4(string a, ref int b)
		{
			this.wasMethod4DelegateCalled = true;
			return default;
		}

		public Guid Method5<U>(string a, ref U b)
		{
			this.wasMethod5DelegateCalled = true;
			return default;
		}

#pragma warning disable CS0414
		private bool wasMethod4DelegateCalled;
		private bool wasMethod5DelegateCalled;
#pragma warning restore CS0414
	}
}