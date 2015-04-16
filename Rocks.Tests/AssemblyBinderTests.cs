using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class AssemblyBinderTests
	{
		[Test]
		public void Create()
		{
			var binder = new AssemblyBinder();
			Assert.AreEqual(0, binder.Assemblies.Count);
		}

		[Test]
		public void AddAssembly()
		{
			var binder = new AssemblyBinder();
			binder.Assemblies.Add(this.GetType().Assembly);
			Assert.AreEqual(1, binder.Assemblies.Count);
			Assert.IsTrue(binder.Assemblies.Contains(this.GetType().Assembly));
		}

		[Test]
		public void AddExistingAssembly()
		{
			var binder = new AssemblyBinder();
			binder.Assemblies.Add(this.GetType().Assembly);
			binder.Assemblies.Add(this.GetType().Assembly);
			Assert.AreEqual(1, binder.Assemblies.Count);
			Assert.IsTrue(binder.Assemblies.Contains(this.GetType().Assembly));
		}

		[Test]
		public void BindToType()
		{
			var binder = new AssemblyBinder();
			binder.Assemblies.Add(this.GetType().Assembly);
			Assert.AreEqual(this.GetType(), binder.BindToType(this.GetType().Assembly.FullName, this.GetType().FullName));
		}

		[Test]
		public void BindToTypeWhereTypeDoesNotExist()
		{
			var binder = new AssemblyBinder();
			binder.Assemblies.Add(this.GetType().Assembly);
			Assert.IsNull(binder.BindToType(this.GetType().Assembly.FullName, typeof(Rock).FullName));
		}

		[Test]
		public void BindToTypeWhereAssemblyDoesNotExist()
		{
			var binder = new AssemblyBinder();
			binder.Assemblies.Add(this.GetType().Assembly);
			Assert.IsNull(binder.BindToType(typeof(Rock).Assembly.FullName, this.GetType().FullName));
		}
	}
}
