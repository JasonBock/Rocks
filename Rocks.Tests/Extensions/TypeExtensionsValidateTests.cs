using NUnit.Framework;
using Rocks.Construction;
using Rocks.Tests.Types;
using System.Collections.ObjectModel;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsValidateTests
	{
		[Test]
		public void ValidateWhenTypeIsAbstractAndHasInternalAbstractProperty()
		{
			Assert.AreNotEqual(string.Empty, typeof(HaveInternalAbstractProperty).Validate(
				SerializationOptions.NotSupported, new InMemoryNameGenerator()));
		}

		[Test]
		public void ValidateWhenTypeIsSealedAndDoesNotHaveHandlerConstructor()
		{
			Assert.AreNotEqual(string.Empty, typeof(DoNotHaveHandlerConstructor).Validate(
				SerializationOptions.NotSupported, new InMemoryNameGenerator()));
		}

		[Test]
		public void ValidateWhenTypeIsSealedAndHasHandlerConstructor()
		{
			Assert.AreEqual(string.Empty, typeof(HaveHandlerConstructor).Validate(
				SerializationOptions.NotSupported, new InMemoryNameGenerator()));
		}

		[Test]
		public void ValidateWhenTypeIsSealedAndHasObjectConstructor()
		{
			Assert.AreNotEqual(string.Empty, typeof(HaveObjectConstructor).Validate(
				SerializationOptions.NotSupported, new InMemoryNameGenerator()));
		}

		[Test]
		public void ValidateWhenTypeIsClassAndHasNoPublicNoArgumentConstructorAndSerializationIsSupported()
		{
			Assert.AreNotEqual(string.Empty, typeof(HaveNoPublicConstructor).Validate(
				SerializationOptions.Supported, new InMemoryNameGenerator()));
		}

		[Test]
		public void ValidateWhenTypeIsClassAndHasNoPublicNoArgumentConstructorAndSerializationIsNotSupported()
		{
			Assert.AreEqual(string.Empty, typeof(HaveNoPublicConstructor).Validate(
				SerializationOptions.NotSupported, new InMemoryNameGenerator()));
		}

		[Test]
		public void ValidateWhenTypeIsInterfaceAndSerializationIsSupported()
		{
			Assert.AreEqual(string.Empty, typeof(IHaveNoPublicConstructor).Validate(
				SerializationOptions.Supported, new InMemoryNameGenerator()));
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
