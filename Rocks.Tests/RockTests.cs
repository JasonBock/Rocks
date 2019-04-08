using NUnit.Framework;
using Rocks.Exceptions;
using Rocks.Options;
using System.IO;

namespace Rocks.Tests
{
	public static class RockTests
	{
		[Test]
		public static void Create() =>
			Assert.That(Rock.Create<IRockTests>(), Is.Not.Null, nameof(Rock.Create));

		[Test]
		public static void CreateWhenTypeIsSealed() =>
			Assert.That(() => Rock.Create<string>(), Throws.TypeOf<ValidationException>());

		[Test]
		public static void TryCreate()
		{
			var (isSuccessful, result) = Rock.TryCreate<IRockTests>();
			Assert.That(isSuccessful, Is.True, nameof(isSuccessful));
			Assert.That(result, Is.Not.Null, nameof(result));
		}

		[Test]
		public static void TryCreateWhenTypeIsSealed()
		{
			var (isSuccessful, result) = Rock.TryCreate<string>();
			Assert.That(isSuccessful, Is.False, nameof(isSuccessful));
			Assert.That(result, Is.Null, nameof(result));
		}

		[Test]
		public static void Make()
		{
			var rock = Rock.Create<IRockTests>();
			rock.Handle(_ => _.Member());

			var chunk = rock.Make();
			var chunkType = chunk.GetType();
			Assert.That(chunkType.Namespace, Is.EqualTo(typeof(IRockTests).Namespace), nameof(chunkType.Namespace));

			var chunkAsRock = (IMock)chunk;
			Assert.That(chunkAsRock.Handlers.Count, Is.EqualTo(1), nameof(chunkAsRock.Handlers.Count));
		}

		[Test]
		public static void MakeWithFile()
		{
			var testDirectory = TestContext.CurrentContext.TestDirectory;
			var rock = Rock.Create<IFileTests>(
				new RockOptions(
					level: OptimizationSetting.Debug,
					codeFile: CodeFileOptions.Create,
					codeFileDirectory: testDirectory));
			rock.Handle(_ => _.Member("a", 44));

			var chunk = rock.Make();
			var chunkType = chunk.GetType();
			Assert.That(chunkType.Namespace, Is.EqualTo(typeof(IFileTests).Namespace), nameof(chunkType.Namespace));
			Assert.That(File.Exists(Path.Combine(testDirectory, $"{chunkType.Name}.cs")), Is.True, nameof(File.Exists));

			var chunkAsRock = (IMock)chunk;
			Assert.That(chunkAsRock.Handlers.Count, Is.EqualTo(1), nameof(chunkAsRock.Handlers.Count));

			chunk.Member("a", 44);
			rock.Verify();
		}

		[Test]
		public static void MakeWhenTypeNameExistsInRocksAssembly()
		{
			var rock = Rock.Create<SomeNamespaceOtherThanRocks.IMock>();
			Assert.That(() => rock.Make(), Throws.Nothing);
		}

		[Test]
		public static void Remake()
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
		public static void RemakeWithSameOptions()
		{
			var rock = Rock.Create<ISameRemake>(new RockOptions(level: OptimizationSetting.Release));
			var chunk = rock.Make();

			var secondRock = Rock.Create<ISameRemake>(new RockOptions(level: OptimizationSetting.Release));
			var secondChunk = secondRock.Make();

			Assert.That(secondChunk.GetType(), Is.EqualTo(chunk.GetType()));
		}

		[Test]
		public static void RemakeWithDifferentOptions()
		{
			var rock = Rock.Create<IDifferentRemake>(new RockOptions(level: OptimizationSetting.Debug));
			var chunk = rock.Make();

			var secondRock = Rock.Create<IDifferentRemake>(new RockOptions(level: OptimizationSetting.Release));
			var secondChunk = secondRock.Make();

			Assert.That(secondChunk.GetType(), Is.Not.EqualTo(chunk.GetType()));
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

	namespace SomeNamespaceOtherThanRocks
	{
		public abstract class ArgumentExpectation { }
		public interface IMock
		{
			void Target(ArgumentExpectation a);
		}
	}
}
