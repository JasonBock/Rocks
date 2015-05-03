using NUnit.Framework;
using static Rocks.Extensions.MethodInfoExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsGetDelegateCastTests
	{
		[Test]
		public void GetDelegateCastWithNoArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithNoArguments));
			Assert.AreEqual("Action", target.GetDelegateCast());
		}

		[Test]
		public void GetDelegateCastWithNoArgumentsAndReturnValue()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithNoArgumentsAndReturnValue));
			Assert.AreEqual("Func<Int32>", target.GetDelegateCast());
		}

		[Test]
		public void GetDelegateCastWithArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithArguments));
			Assert.AreEqual("Action<Int32, String>", target.GetDelegateCast());
		}

		[Test]
		public void GetDelegateCastWithComplexGenericArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithComplexGeneric));
			Assert.AreEqual("Func<IGeneric<Int32>, IGeneric<Int32>>", target.GetDelegateCast());
		}

		[Test]
		public void GetDelegateCastWithArgumentsAndReturnValue()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithArgumentsAndReturnValue));
			Assert.AreEqual("Func<Int32, String, Int32>", target.GetDelegateCast());
		}

		[Test]
		public void GetDelegateCastWithGenerics()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenerics));
			Assert.AreEqual("Action<Int32, U, String, V>", target.GetDelegateCast());
		}

		[Test]
		public void GetDelegateCastWithGenericsAndReturnValue()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenericsAndReturnValue));
			Assert.AreEqual("Func<Int32, U, String, V, U>", target.GetDelegateCast());
		}

		public void TargetWithNoArguments() { }
		public int TargetWithNoArgumentsAndReturnValue() { return 0; }
		public void TargetWithArguments(int a, string c) { }
		public int TargetWithArgumentsAndReturnValue(int a, string c) { return 0; }
		public void TargetWithGenerics<U, V>(int a, U b, string c, V d) { }
		public IGeneric<int> TargetWithComplexGeneric(IGeneric<int> a) { return null; }
		public U TargetWithGenericsAndReturnValue<U, V>(int a, U b, string c, V d) { return default(U); }
	}

	public interface IGeneric<T> { }
}
