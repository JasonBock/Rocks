using NUnit.Framework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Rocks.Extensions;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class SerializationTests
	{
		[Test]
		public void Roundtrip()
		{
			var rock = Rock.Create<IAmSerializable>(new Options(SerializationOptions.Supported));
			rock.HandleAction(_ => _.Target("44"));

			var chunk = rock.Make();
			IAmSerializable newChunk = null;

			var binder = new AssemblyBinder();
			binder.Assemblies.Add(chunk.GetType().Assembly);

			var formatter = new BinaryFormatter();
			formatter.Binder = binder;

			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, chunk);
				stream.Position = 0;
				newChunk = formatter.Deserialize(stream) as IAmSerializable;
			}

			newChunk.Target("44");
			(newChunk as IRock).Verify();
      }

		[Test]
		public void RoundtripWhenMockIsNotSerializable()
		{

		}
	}

	public interface IAmSerializable
	{
		void Target(string a);
	}
}
