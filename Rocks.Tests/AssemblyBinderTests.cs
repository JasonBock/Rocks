using NUnit.Framework;

namespace Rocks.Tests
{
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
			binder.Assemblies.Add(this.GetType().Assembly);
			Assert.That(binder.Assemblies.Count, Is.EqualTo(1));
			Assert.That(binder.Assemblies.Contains(this.GetType().Assembly), Is.True);
		}

		[Test]
		public void AddExistingAssembly()
		{
			var binder = new AssemblyBinder();
			binder.Assemblies.Add(this.GetType().Assembly);
			binder.Assemblies.Add(this.GetType().Assembly);
			Assert.That(binder.Assemblies.Count, Is.EqualTo(1));
			Assert.That(binder.Assemblies.Contains(this.GetType().Assembly), Is.True);
		}

		[Test]
		public void BindToType()
		{
			var binder = new AssemblyBinder();
			binder.Assemblies.Add(this.GetType().Assembly);
			Assert.That(binder.BindToType(this.GetType().Assembly.FullName, this.GetType().FullName),
				Is.EqualTo(this.GetType()));
		}

		[Test]
		public void BindToTypeWhereTypeDoesNotExist()
		{
			var binder = new AssemblyBinder();
			binder.Assemblies.Add(this.GetType().Assembly);
			Assert.That(binder.BindToType(this.GetType().Assembly.FullName, typeof(Rock).FullName), Is.Null);
		}

		[Test]
		public void BindToTypeWhereAssemblyDoesNotExist()
		{
			var binder = new AssemblyBinder();
			binder.Assemblies.Add(this.GetType().Assembly);
			Assert.That(binder.BindToType(typeof(Rock).Assembly.FullName, this.GetType().FullName), Is.Null);
		}
	}
}