using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Exceptions;
using System.IO;
using System.Runtime.InteropServices;
using System;
using Rocks.Options;

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
			rock.Handle(_ => _.Member());

			var chunk = rock.Make();
			var chunkType = chunk.GetType();
         Assert.AreEqual(typeof(IRockTests).Namespace, chunkType.Namespace, nameof(chunkType.Namespace));

			var chunkAsRock = chunk as IMock;
         Assert.AreEqual(1, chunkAsRock.Handlers.Count, nameof(chunkAsRock.Handlers.Count));
		}

		[Test]
		public void MakeWithFile()
		{
			var rock = Rock.Create<IFileTests>(new RockOptions(OptimizationSetting.Debug, CodeFileOptions.Create));
			rock.Handle(_ => _.Member("a", 44));

			var chunk = rock.Make();
			var chunkType = chunk.GetType();
			Assert.AreEqual(typeof(IFileTests).Namespace, chunkType.Namespace, nameof(chunkType.Namespace));
			Assert.IsTrue(File.Exists($"{chunkType.Name}.cs"), nameof(File.Exists));

			var chunkAsRock = chunk as IMock;
			Assert.AreEqual(1, chunkAsRock.Handlers.Count, nameof(chunkAsRock.Handlers.Count));

			chunk.Member("a", 44);
			rock.Verify();
		}

		[Test]
		public void Remake()
		{
			var rock = Rock.Create<IRockTests>();
			rock.Handle(_ => _.Member());

			var chunk = rock.Make();
			chunk.Member();

			rock.Verify();

			var secondRock = Rock.Create<IRockTests>();
			secondRock.Handle(_ => _.SecondMember());

			var secondChunk = secondRock.Make();
			secondChunk.SecondMember();

			secondRock.Verify();
		}

		[Test]
		public void RemakeWithSameOptions()
		{
			var rock = Rock.Create<ISameRemake>(new RockOptions(serialization: SerializationOptions.Supported));
			var chunk = rock.Make();

			var secondRock = Rock.Create<ISameRemake>(new RockOptions(serialization: SerializationOptions.Supported));
			var secondChunk = secondRock.Make();

			Assert.AreEqual(chunk.GetType(), secondChunk.GetType());
		}

		[Test]
		public void RemakeWithDifferentOptions()
		{
			var rock = Rock.Create<IDifferentRemake>(new RockOptions(serialization: SerializationOptions.NotSupported));
			var chunk = rock.Make();

			var secondRock = Rock.Create<IDifferentRemake>(new RockOptions(serialization: SerializationOptions.Supported));
			var secondChunk = secondRock.Make();

			Assert.AreNotEqual(chunk.GetType(), secondChunk.GetType());
		}
	}

	public interface ISameRemake
	{
		void Target();
	}

	public interface IDifferentRemake
	{
		void Target();
	}

	public interface IRockTests
	{
		void Member();
		void SecondMember();
	}

	public interface IFileTests
	{
		void Member(string a, int b);
	}
}
