using NUnit.Framework;
using Rocks.Exceptions;
using System;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	public static class TypeExtensionsFindPropertyTests
	{
		[Test]
		public static void FindPropertyWhenPropertyDoesNotExist() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty("x", PropertyAccessors.Get), 
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public static void FindReadOnlyPropertyWhenPropertyAccessorIsGet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.ReadOnly), PropertyAccessors.Get).Name, 
				Is.EqualTo(nameof(ITypeExtensions.ReadOnly)));

		[Test]
		public static void FindReadOnlyPropertyWhenPropertyAccessorIsGetAndSet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.ReadOnly), PropertyAccessors.GetAndSet),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public static void FindReadOnlyPropertyWhenPropertyAccessorIsSet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.ReadOnly), PropertyAccessors.Set),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public static void FindWriteOnlyPropertyWhenPropertyAccessorIsGet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.WriteOnly), PropertyAccessors.Get),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public static void FindWriteOnlyPropertyWhenPropertyAccessorIsGetAndSet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.WriteOnly), PropertyAccessors.GetAndSet),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public static void FindWriteOnlyPropertyWhenPropertyAccessorIsSet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.WriteOnly), PropertyAccessors.Set).Name,
				Is.EqualTo(nameof(ITypeExtensions.WriteOnly)));

		[Test]
		public static void FindReadWritePropertyWhenPropertyAccessorIsGet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.Property), PropertyAccessors.Get).Name,
				Is.EqualTo(nameof(ITypeExtensions.Property)));

		[Test]
		public static void FindReadWritePropertyWhenPropertyAccessorIsGetAndSet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.Property), PropertyAccessors.GetAndSet).Name,
				Is.EqualTo(nameof(ITypeExtensions.Property)));

		[Test]
		public static void FindReadWritePropertyWhenPropertyAccessorIsSet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.Property), PropertyAccessors.Set).Name,
				Is.EqualTo(nameof(ITypeExtensions.Property)));

		[Test]
		public static void FindIndexerPropertyWhenPropertyDoesNotExist() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(new Type[] { typeof(double) }, PropertyAccessors.Get),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public static void FindReadOnlyIndexerPropertyWhenPropertyAccessorIsGet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(new[] { typeof(string) }, PropertyAccessors.Get).Name,
				Is.EqualTo("Item"));

		[Test]
		public static void FindReadOnlyIndexerPropertyWhenPropertyAccessorIsGetAndSet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(string) }, PropertyAccessors.GetAndSet),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public static void FindReadOnlyIndexerPropertyWhenPropertyAccessorIsSet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(string) }, PropertyAccessors.Set),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public static void FindWriteOnlyIndexerPropertyWhenPropertyAccessorIsGet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(int) }, PropertyAccessors.Get),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public static void FindWriteOnlyIndexerPropertyWhenPropertyAccessorIsGetAndSet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(int) }, PropertyAccessors.GetAndSet),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public static void FindWriteOnlyIndexerPropertyWhenPropertyAccessorIsSet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(new[] { typeof(int) }, PropertyAccessors.Set).Name,
				Is.EqualTo("Item"));

		[Test]
		public static void FindReadWriteIndexerPropertyWhenPropertyAccessorIsGet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(new[] { typeof(Guid) }, PropertyAccessors.Get).Name,
				Is.EqualTo("Item"));

		[Test]
		public static void FindReadWriteIndexerPropertyWhenPropertyAccessorIsGetAndSet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(new[] { typeof(Guid) }, PropertyAccessors.GetAndSet).Name,
				Is.EqualTo("Item"));

		[Test]
		public static void FindReadWriteIndexerPropertyWhenPropertyAccessorIsSet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(new[] { typeof(Guid) }, PropertyAccessors.Set).Name,
				Is.EqualTo("Item"));
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
