using NUnit.Framework;
using Rocks.Construction.InMemory;
using Rocks.Options;
using Rocks.Tests.Types;
using System.Collections.ObjectModel;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsValidateTests
	{
		[Test]
		public void ValidateWhenTypeIsAbstractAndHasInternalConstructor()
		{
			Assert.That(typeof(HaveInternalConstructor).Validate(
				SerializationOptions.NotSupported, new InMemoryNameGenerator()), Is.Not.Empty);
		}

		[Test]
		public void ValidateWhenTypeIsAbstractAndHasInternalAbstractProperty()
		{
			Assert.That(typeof(HaveInternalAbstractProperty).Validate(
				SerializationOptions.NotSupported, new InMemoryNameGenerator()), Is.Not.Empty);
		}

		[Test]
		public void ValidateWhenTypeIsSealedAndDoesNotHaveHandlerConstructor()
		{
			Assert.That(typeof(DoNotHaveHandlerConstructor).Validate(
				SerializationOptions.NotSupported, new InMemoryNameGenerator()), Is.Not.Empty);
		}

		[Test]
		public void ValidateWhenTypeIsSealedAndHasHandlerConstructor()
		{
			Assert.That(typeof(HaveHandlerConstructor).Validate(
				SerializationOptions.NotSupported, new InMemoryNameGenerator()), Is.Empty);
		}

		[Test]
		public void ValidateWhenTypeIsSealedAndHasObjectConstructor()
		{
			Assert.That(typeof(HaveObjectConstructor).Validate(
				SerializationOptions.NotSupported, new InMemoryNameGenerator()), Is.Not.Empty);
		}

		[Test]
		public void ValidateWhenTypeIsClassAndHasNoPublicNoArgumentConstructorAndSerializationIsSupported()
		{
			Assert.That(typeof(HaveNoPublicConstructor).Validate(
				SerializationOptions.Supported, new InMemoryNameGenerator()), Is.Not.Empty);
		}

		[Test]
		public void ValidateWhenTypeIsClassAndHasNoPublicNoArgumentConstructorAndSerializationIsNotSupported()
		{
			Assert.That(typeof(HaveNoPublicConstructor).Validate(
				SerializationOptions.NotSupported, new InMemoryNameGenerator()), Is.Not.Empty);
		}

		[Test]
		public void ValidateWhenTypeIsInterfaceAndSerializationIsSupported()
		{
			Assert.That(typeof(IHaveNoPublicConstructor).Validate(
				SerializationOptions.Supported, new InMemoryNameGenerator()), Is.Empty);
		}
	}

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
