using NUnit.Framework;
using System.Reflection;
using static Rocks.Extensions.MethodInfoExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsGetDelegateCastTests
	{
		[Test]
		public void GetDelegateCastWithNoArguments()
		{
			var target = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithNoArguments));
			Assert.That(target.GetDelegateCast(), Is.EqualTo("Action"));
		}

		[Test]
		public void GetDelegateCastWithNoArgumentsAndReturnValue()
		{
			var target = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithNoArgumentsAndReturnValue));
			Assert.That(target.GetDelegateCast(), Is.EqualTo("Func<Int32>"));
		}

		[Test]
		public void GetDelegateCastWithArguments()
		{
			var target = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithArguments));
			Assert.That(target.GetDelegateCast(), Is.EqualTo("Action<Int32, String>"));
		}

		[Test]
		public void GetDelegateCastWithComplexGenericArguments()
		{
			var target = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithComplexGeneric));
			Assert.That(target.GetDelegateCast(), Is.EqualTo("Func<IGeneric<Int32>, IGeneric<Int32>>"));
		}

		[Test]
		public void GetDelegateCastWithArgumentsAndReturnValue()
		{
			var target = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithArgumentsAndReturnValue));
			Assert.That(target.GetDelegateCast(), Is.EqualTo("Func<Int32, String, Int32>"));
		}

		[Test]
		public void GetDelegateCastWithGenerics()
		{
			var target = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithGenerics));
			Assert.That(target.GetDelegateCast(), Is.EqualTo("Action<Int32, U, String, V>"));
		}

		[Test]
		public void GetDelegateCastWithGenericsAndReturnValue()
		{
			var target = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithGenericsAndReturnValue));
			Assert.That(target.GetDelegateCast(), Is.EqualTo("Func<Int32, U, String, V, U>"));
		}

		public void TargetWithNoArguments() { }
		public int TargetWithNoArgumentsAndReturnValue() => 0; 
		public void TargetWithArguments(int a, string c) { }
		public int TargetWithArgumentsAndReturnValue(int a, string c) => 0; 
		public void TargetWithGenerics<U, V>(int a, U b, string c, V d) { }
		public IGeneric<int> TargetWithComplexGeneric(IGeneric<int> a) => null; 
		public U TargetWithGenericsAndReturnValue<U, V>(int a, U b, string c, V d) => default(U); 
	}

	public interface IGeneric<T> { }
}
