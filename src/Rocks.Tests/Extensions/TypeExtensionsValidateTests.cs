using NUnit.Framework;
using Rocks.Construction.InMemory;
using Rocks.Options;
using Rocks.Tests.Types;
using System;
using System.Collections.ObjectModel;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	public sealed class TypeExtensionsValidateTests
	{
		[Test]
		public void ValidateWhenTypeIsAbstractAndHasInternalConstructor() =>
			Assert.That(typeof(HaveInternalConstructor).Validate(
				SerializationOption.NotSupported,
				new InMemoryNameGenerator()), Is.Not.Empty);

		[Test]
		public void ValidateWhenTypeIsAbstractAndHasInternalAbstractProperty() =>
			Assert.That(typeof(HaveInternalAbstractProperty).Validate(
				SerializationOption.NotSupported,
				new InMemoryNameGenerator()), Is.Not.Empty);

		[Test]
		public void ValidateWhenTypeIsSealedAndDoesNotHaveHandlerConstructor() =>
			Assert.That(typeof(DoNotHaveHandlerConstructor).Validate(
				SerializationOption.NotSupported,
				new InMemoryNameGenerator()), Is.Not.Empty);

		[Test]
		public void ValidateWhenTypeIsSealedAndHasHandlerConstructor() =>
			Assert.That(typeof(HaveHandlerConstructor).Validate(
				SerializationOption.NotSupported,
				new InMemoryNameGenerator()), Is.Empty);

		[Test]
		public void ValidateWhenTypeIsSealedAndHasObjectConstructor() =>
			Assert.That(typeof(HaveObjectConstructor).Validate(
				SerializationOption.NotSupported,
				new InMemoryNameGenerator()), Is.Not.Empty);

		[Test]
		public void ValidateWhenTypeIsClassAndHasNoPublicNoArgumentConstructorAndSerializationIsSupported() =>
			Assert.That(typeof(HaveNoPublicConstructor).Validate(
				SerializationOption.Supported,
				new InMemoryNameGenerator()), Is.Not.Empty);

		[Test]
		public void ValidateWhenTypeIsClassAndHasNoPublicNoArgumentConstructorAndSerializationIsNotSupported() =>
			Assert.That(typeof(HaveNoPublicConstructor).Validate(
				SerializationOption.NotSupported,
				new InMemoryNameGenerator()), Is.Not.Empty);

		[Test]
		public void ValidateWhenTypeIsInterfaceAndSerializationIsSupported() =>
			Assert.That(typeof(IHaveNoPublicConstructor).Validate(
				SerializationOption.Supported,
				new InMemoryNameGenerator()), Is.Empty);

		[Test]
		public void ValidateWhenTypeIsObsoleteAndErrorIsFalse()
		{
			var obsoleteType = this.GetType().Assembly
				.GetType("Rocks.Tests.Extensions.IAmObsoleteWithErrorAsFalse")!;
			Assert.That(obsoleteType.Validate(
				SerializationOption.Supported,
				new InMemoryNameGenerator()), Is.Empty);
		}

		[Test]
		public void ValidateWhenTypeIsObsoleteAndErrorIsTrue()
		{
			var obsoleteType = this.GetType().Assembly
				.GetType("Rocks.Tests.Extensions.IAmObsoleteWithErrorAsTrue")!;
			Assert.That(obsoleteType.Validate(
				SerializationOption.Supported,
				new InMemoryNameGenerator()), Is.Not.Empty);
		}
	}

	[Obsolete("Don't use me.", false)]
	public interface IAmObsoleteWithErrorAsFalse { }

	[Obsolete("Don't use me.", true)]
	public interface IAmObsoleteWithErrorAsTrue { }

	public sealed class DoNotHaveHandlerConstructor { }

	public sealed class HaveHandlerConstructor
	{
		public HaveHandlerConstructor(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers) { }
	}

	public sealed class HaveObjectConstructor
	{
		public HaveObjectConstructor(object handlers) { }
	}

	public class HaveNoPublicConstructor
	{
		private HaveNoPublicConstructor() { }
	}

	public interface IHaveNoPublicConstructor { }
}