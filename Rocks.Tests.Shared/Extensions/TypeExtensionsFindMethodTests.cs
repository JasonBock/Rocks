using NUnit.Framework;
using System.Reflection;
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

			Assert.That(foundMethod, Is.EqualTo(method));
		}

		[Test]
		public void FindMethodWhenTokenDoesNotExist()
		{
			Assert.That(typeof(IMetadata).FindMethod(0), Is.Null);
		}

		[Test]
		public void FindMethodWithMethodOnBaseType()
		{
			var method = typeof(IMetadata).GetTypeInfo().GetMethod(nameof(IMetadata.Target));

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
