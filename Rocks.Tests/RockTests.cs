using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class RockTests
	{
		[Test]
		public void Create()
		{
			Assert.IsNotNull(Rock.Create<IRockTests>(), nameof(Rock.Create));
		}

		[Test]
		public void CreateWhenTypeIsSealed()
		{
			Assert.Throws<ValidationException>(() => Rock.Create<string>());
		}

		[Test]
		public void TryCreate()
		{
			var result = Rock.TryCreate<IRockTests>();
			Assert.IsTrue(result.IsSuccessful, nameof(result.IsSuccessful));
			Assert.IsNotNull(result.Result, nameof(result.Result));
		}

		[Test]
		public void TryCreateWhenTypeIsSealed()
		{
			var result = Rock.TryCreate<string>();
         Assert.IsFalse(result.IsSuccessful);
			Assert.IsNull(result.Result, nameof(result.Result));
		}

		[Test]
		public void Make()
		{
			var rock = Rock.Create<IRockTests>();
			rock.HandleAction(_ => _.Member());

			var chunk = rock.Make();
			var chunkType = chunk.GetType();
         Assert.AreEqual(typeof(IRockTests).Namespace, chunkType.Namespace, nameof(chunkType.Namespace));

			var chunkAsRock = chunk as IRock;
         Assert.AreEqual(1, chunkAsRock.Handlers.Count, nameof(chunkAsRock.Handlers.Count));
		}
	}

	public interface IRockTests
	{
		void Member();
	}
}
