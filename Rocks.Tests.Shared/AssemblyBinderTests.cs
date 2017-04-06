#if !NETCOREAPP1_1
using NUnit.Framework;
using System.Reflection;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class AssemblyBinderTests
	{
		[Test]
		public void Create()
		{
			var binder = new AssemblyBinder();
			Assert.That(binder.Assemblies.Count, Is.EqualTo(0));
		}

		[Test]
		public void AddAssembly()
		{
			var binder = new AssemblyBinder();
			binder.Assemblies.Add(this.GetType().GetTypeInfo().Assembly);
			Assert.That(binder.Assemblies.Count, Is.EqualTo(1));
			Assert.That(binder.Assemblies.Contains(this.GetType().GetTypeInfo().Assembly), Is.True);
		}

		[Test]
		public void AddExistingAssembly()
		{
			var binder = new AssemblyBinder();
			binder.Assemblies.Add(this.GetType().GetTypeInfo().Assembly);
			binder.Assemblies.Add(this.GetType().GetTypeInfo().Assembly);
			Assert.That(binder.Assemblies.Count, Is.EqualTo(1));
			Assert.That(binder.Assemblies.Contains(this.GetType().GetTypeInfo().Assembly), Is.True);
		}

		[Test]
		public void BindToType()
		{
			var binder = new AssemblyBinder();
			binder.Assemblies.Add(this.GetType().GetTypeInfo().Assembly);
			Assert.That(binder.BindToType(this.GetType().GetTypeInfo().Assembly.FullName, this.GetType().FullName),
				Is.EqualTo(this.GetType()));
		}

		[Test]
		public void BindToTypeWhereTypeDoesNotExist()
		{
			var binder = new AssemblyBinder();
			binder.Assemblies.Add(this.GetType().GetTypeInfo().Assembly);
			Assert.That(binder.BindToType(this.GetType().GetTypeInfo().Assembly.FullName, typeof(Rock).FullName), Is.Null);
		}

		[Test]
		public void BindToTypeWhereAssemblyDoesNotExist()
		{
			var binder = new AssemblyBinder();
			binder.Assemblies.Add(this.GetType().GetTypeInfo().Assembly);
			Assert.That(binder.BindToType(typeof(Rock).GetTypeInfo().Assembly.FullName, this.GetType().FullName), Is.Null);
		}
	}
}
#endif