using NUnit.Framework;
using static Rocks.Extensions.MethodInfoExtensions;

namespace Rocks.Tests.Extensions
{
	public sealed class MethodInfoExtensionsGetDelegateCastTests
	{
		[Test]
		public void GetDelegateCastWithNoArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithNoArguments));
			Assert.That(target.GetDelegateCast(), Is.EqualTo("Action"));
		}

		[Test]
		public void GetDelegateCastWithNoArgumentsAndReturnValue()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithNoArgumentsAndReturnValue));
			Assert.That(target.GetDelegateCast(), Is.EqualTo("Func<int>"));
		}

		[Test]
		public void GetDelegateCastWithArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithArguments));
			Assert.That(target.GetDelegateCast(), Is.EqualTo("Action<int, string>"));
		}

		[Test]
		public void GetDelegateCastWithComplexGenericArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithComplexGeneric));
			Assert.That(target.GetDelegateCast(), Is.EqualTo("Func<IGeneric<int>, IGeneric<int>>"));
		}

		[Test]
		public void GetDelegateCastWithArgumentsAndReturnValue()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithArgumentsAndReturnValue));
			Assert.That(target.GetDelegateCast(), Is.EqualTo("Func<int, string, int>"));
		}

		[Test]
		public void GetDelegateCastWithGenerics()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenerics));
			Assert.That(target.GetDelegateCast(), Is.EqualTo("Action<int, U, string, V>"));
		}

		[Test]
		public void GetDelegateCastWithGenericsAndReturnValue()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenericsAndReturnValue));
			Assert.That(target.GetDelegateCast(), Is.EqualTo("Func<int, U, string, V, U>"));
		}

		public void TargetWithNoArguments() { }
		public int TargetWithNoArgumentsAndReturnValue() => 0; 
		public void TargetWithArguments(int a, string c) { }
		public int TargetWithArgumentsAndReturnValue(int a, string c) => 0; 
		public void TargetWithGenerics<U, V>(int a, U b, string c, V d) { }
		public IGeneric<int> TargetWithComplexGeneric(IGeneric<int> a) => null; 
		public U TargetWithGenericsAndReturnValue<U, V>(int a, U b, string c, V d) => default; 
	}

	public interface IGeneric<T> { }
}
