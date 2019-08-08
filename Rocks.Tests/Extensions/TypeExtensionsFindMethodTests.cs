using NUnit.Framework;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	public static class TypeExtensionsFindMethodTests
	{
		[Test]
		public static void FindMethodWithMethodOnGivenType()
		{
			var method = typeof(IMetadata).GetMethod(nameof(IMetadata.Target))!;
			var foundMethod = typeof(IMetadata).FindMethod(method.MetadataToken);
			Assert.That(foundMethod, Is.EqualTo(method));
		}

		[Test]
		public static void FindMethodWhenTokenDoesNotExist() =>
			Assert.That(typeof(IMetadata).FindMethod(0), Is.Null);

		[Test]
		public static void FindMethodWithMethodOnBaseType()
		{
			var method = typeof(IMetadata).GetMethod(nameof(IMetadata.Target))!;
			var foundMethod = typeof(Metadata).FindMethod(method.MetadataToken);
			Assert.That(foundMethod, Is.EqualTo(method));
		}
	}

	public interface IMetadata
	{
		void Target();
	}

	public class Metadata
		: IMetadata
	{
		public void Target() { }
	}
}
