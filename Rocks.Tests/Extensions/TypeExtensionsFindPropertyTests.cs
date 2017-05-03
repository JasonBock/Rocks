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
		public void FindPropertyWhenPropertyDoesNotExist() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty("x", PropertyAccessors.Get), 
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public void FindReadOnlyPropertyWhenPropertyAccessorIsGet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.ReadOnly), PropertyAccessors.Get).Name, 
				Is.EqualTo(nameof(ITypeExtensions.ReadOnly)));

		[Test]
		public void FindReadOnlyPropertyWhenPropertyAccessorIsGetAndSet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.ReadOnly), PropertyAccessors.GetAndSet),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public void FindReadOnlyPropertyWhenPropertyAccessorIsSet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.ReadOnly), PropertyAccessors.Set),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public void FindWriteOnlyPropertyWhenPropertyAccessorIsGet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.WriteOnly), PropertyAccessors.Get),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public void FindWriteOnlyPropertyWhenPropertyAccessorIsGetAndSet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.WriteOnly), PropertyAccessors.GetAndSet),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public void FindWriteOnlyPropertyWhenPropertyAccessorIsSet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.WriteOnly), PropertyAccessors.Set).Name,
				Is.EqualTo(nameof(ITypeExtensions.WriteOnly)));

		[Test]
		public void FindReadWritePropertyWhenPropertyAccessorIsGet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.Property), PropertyAccessors.Get).Name,
				Is.EqualTo(nameof(ITypeExtensions.Property)));

		[Test]
		public void FindReadWritePropertyWhenPropertyAccessorIsGetAndSet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.Property), PropertyAccessors.GetAndSet).Name,
				Is.EqualTo(nameof(ITypeExtensions.Property)));

		[Test]
		public void FindReadWritePropertyWhenPropertyAccessorIsSet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(nameof(ITypeExtensions.Property), PropertyAccessors.Set).Name,
				Is.EqualTo(nameof(ITypeExtensions.Property)));

		[Test]
		public void FindIndexerPropertyWhenPropertyDoesNotExist() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(new Type[] { typeof(double) }, PropertyAccessors.Get),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public void FindReadOnlyIndexerPropertyWhenPropertyAccessorIsGet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(new[] { typeof(string) }, PropertyAccessors.Get).Name,
				Is.EqualTo("Item"));

		[Test]
		public void FindReadOnlyIndexerPropertyWhenPropertyAccessorIsGetAndSet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(string) }, PropertyAccessors.GetAndSet),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public void FindReadOnlyIndexerPropertyWhenPropertyAccessorIsSet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(string) }, PropertyAccessors.Set),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public void FindWriteOnlyIndexerPropertyWhenPropertyAccessorIsGet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(int) }, PropertyAccessors.Get),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public void FindWriteOnlyIndexerPropertyWhenPropertyAccessorIsGetAndSet() =>
			Assert.That(() => typeof(ITypeExtensions).FindProperty(new[] { typeof(int) }, PropertyAccessors.GetAndSet),
				Throws.TypeOf<PropertyNotFoundException>());

		[Test]
		public void FindWriteOnlyIndexerPropertyWhenPropertyAccessorIsSet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(new[] { typeof(int) }, PropertyAccessors.Set).Name,
				Is.EqualTo("Item"));

		[Test]
		public void FindReadWriteIndexerPropertyWhenPropertyAccessorIsGet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(new[] { typeof(Guid) }, PropertyAccessors.Get).Name,
				Is.EqualTo("Item"));

		[Test]
		public void FindReadWriteIndexerPropertyWhenPropertyAccessorIsGetAndSet() =>
			Assert.That(typeof(ITypeExtensions).FindProperty(new[] { typeof(Guid) }, PropertyAccessors.GetAndSet).Name,
				Is.EqualTo("Item"));

		[Test]
		public void FindReadWriteIndexerPropertyWhenPropertyAccessorIsSet() =>
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
