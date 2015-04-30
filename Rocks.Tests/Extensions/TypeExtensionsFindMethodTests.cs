using NUnit.Framework;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsFindMethodTests
	{
		[Test]
		public void FindMethodWithMethodOnGivenType()
		{
			var method = typeof(IMetadata).GetMethod(nameof(IMetadata.Target));

			var foundMethod = typeof(IMetadata).FindMethod(method.MetadataToken);

			Assert.AreEqual(method, foundMethod);
		}

		[Test]
		public void FindMethodWhenTokenDoesNotExist()
		{
			Assert.IsNull(typeof(IMetadata).FindMethod(0));
		}

		[Test]
		public void FindMethodWithMethodOnBaseType()
		{
			var method = typeof(IMetadata).GetMethod(nameof(IMetadata.Target));

			var foundMethod = typeof(Metadata).FindMethod(method.MetadataToken);

			Assert.AreEqual(method, foundMethod);
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
