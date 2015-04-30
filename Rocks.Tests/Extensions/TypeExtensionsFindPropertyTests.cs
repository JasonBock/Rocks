using NUnit.Framework;
using Rocks.Exceptions;
using System;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsFindPropertyTests
	{
		[Test]
		public void FindPropertyWhenPropertyDoesNotExist()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty("x", PropertyAccessors.Get));
		}

		[Test]
		public void FindReadOnlyPropertyWhenPropertyAccessorIsGet()
		{
			var name = nameof(ITypeExtensions.ReadOnly);
			Assert.AreEqual(name, typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.ReadOnly), PropertyAccessors.Get).Name);
		}

		[Test]
		public void FindReadOnlyPropertyWhenPropertyAccessorIsGetAndSet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.ReadOnly), PropertyAccessors.GetAndSet));
		}

		[Test]
		public void FindReadOnlyPropertyWhenPropertyAccessorIsSet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.ReadOnly), PropertyAccessors.Set));
		}

		[Test]
		public void FindWriteOnlyPropertyWhenPropertyAccessorIsGet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.WriteOnly), PropertyAccessors.Get));
		}

		[Test]
		public void FindWriteOnlyPropertyWhenPropertyAccessorIsGetAndSet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.WriteOnly), PropertyAccessors.GetAndSet));
		}

		[Test]
		public void FindWriteOnlyPropertyWhenPropertyAccessorIsSet()
		{
			var name = nameof(ITypeExtensions.WriteOnly);
			Assert.AreEqual(name, typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.WriteOnly), PropertyAccessors.Set).Name);
		}

		[Test]
		public void FindReadWritePropertyWhenPropertyAccessorIsGet()
		{
			var name = nameof(ITypeExtensions.Property);
			Assert.AreEqual(name, typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.Property), PropertyAccessors.Get).Name);
		}

		[Test]
		public void FindReadWritePropertyWhenPropertyAccessorIsGetAndSet()
		{
			var name = nameof(ITypeExtensions.Property);
			Assert.AreEqual(name, typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.Property), PropertyAccessors.GetAndSet).Name);
		}

		[Test]
		public void FindReadWritePropertyWhenPropertyAccessorIsSet()
		{
			var name = nameof(ITypeExtensions.Property);
			Assert.AreEqual(name, typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.Property), PropertyAccessors.Set).Name);
		}

		[Test]
		public void FindIndexerPropertyWhenPropertyDoesNotExist()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(new Type[] { typeof(double) }, PropertyAccessors.Get));
		}

		[Test]
		public void FindReadOnlyIndexerPropertyWhenPropertyAccessorIsGet()
		{
			Assert.AreEqual("Item", typeof(ITypeExtensions).FindProperty(new[] { typeof(string) }, PropertyAccessors.Get).Name);
		}

		[Test]
		public void FindReadOnlyIndexerPropertyWhenPropertyAccessorIsGetAndSet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(string) }, PropertyAccessors.GetAndSet));
		}

		[Test]
		public void FindReadOnlyIndexerPropertyWhenPropertyAccessorIsSet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(string) }, PropertyAccessors.Set));
		}

		[Test]
		public void FindWriteOnlyIndexerPropertyWhenPropertyAccessorIsGet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(int) }, PropertyAccessors.Get));
		}

		[Test]
		public void FindWriteOnlyIndexerPropertyWhenPropertyAccessorIsGetAndSet()
		{
			Assert.Throws<PropertyNotFoundException>(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(int) }, PropertyAccessors.GetAndSet));
		}

		[Test]
		public void FindWriteOnlyIndexerPropertyWhenPropertyAccessorIsSet()
		{
			Assert.AreEqual("Item", typeof(ITypeExtensions).FindProperty(new[] { typeof(int) }, PropertyAccessors.Set).Name);
		}

		[Test]
		public void FindReadWriteIndexerPropertyWhenPropertyAccessorIsGet()
		{
			Assert.AreEqual("Item", typeof(ITypeExtensions).FindProperty(new[] { typeof(Guid) }, PropertyAccessors.Get).Name);
		}

		[Test]
		public void FindReadWriteIndexerPropertyWhenPropertyAccessorIsGetAndSet()
		{
			Assert.AreEqual("Item", typeof(ITypeExtensions).FindProperty(new[] { typeof(Guid) }, PropertyAccessors.GetAndSet).Name);
		}

		[Test]
		public void FindReadWriteIndexerPropertyWhenPropertyAccessorIsSet()
		{
			Assert.AreEqual("Item", typeof(ITypeExtensions).FindProperty(new[] { typeof(Guid) }, PropertyAccessors.Set).Name);
		}
	}

	public interface ITypeExtensions
	{
		int Property { get; set; }
		int ReadOnly { get; }
		int WriteOnly { set; }
		string this[Guid data] { get; set; }
		string this[string key] { get; }
		string this[int index] { set; }
		event EventHandler Event;
		event EventHandler<MyGenericEventArgs> GenericEvent;
	}
}
