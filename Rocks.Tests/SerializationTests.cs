﻿using NUnit.Framework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Rocks.Extensions;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class SerializationTests
	{
		[Test]
		public void RoundtripWithBinary()
		{
			var rock = Rock.Create<IAmSerializable>(new Options(SerializationOptions.Supported));
			rock.HandleAction(_ => _.Target("44"));

			var chunk = rock.Make();
			IAmSerializable newChunk = null;

			var formatter = new BinaryFormatter();
			formatter.Binder = Rock.Binder;

			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, chunk);
				stream.Position = 0;
				newChunk = formatter.Deserialize(stream) as IAmSerializable;
			}

			newChunk.Target("44");
			(newChunk as IRock).Verify();
      }

		[Test, Ignore("Dictionary doesn't serialize with XmlSerializer by default.")]
		public void RoundtripWithXml()
		{
			var rock = Rock.Create<IAmSerializable>(new Options(SerializationOptions.Supported));
			rock.HandleAction(_ => _.Target("44"));

			var chunk = rock.Make();
			IAmSerializable newChunk = null;

			var serializer = new XmlSerializer(chunk.GetType());

			using (var stream = new MemoryStream())
			{
				serializer.Serialize(stream, chunk);
				stream.Position = 0;
				newChunk = serializer.Deserialize(stream) as IAmSerializable;
			}

			newChunk.Target("44");
			(newChunk as IRock).Verify();
		}

		[Test]
		public void RoundtripWithNetDataContract()
		{
			var rock = Rock.Create<IAmSerializable>(new Options(SerializationOptions.Supported));
			rock.HandleAction(_ => _.Target("44"));

			var chunk = rock.Make();
			IAmSerializable newChunk = null;

			var serializer = new NetDataContractSerializer();
			serializer.Binder = Rock.Binder;

			using (var stream = new MemoryStream())
			{
				serializer.WriteObject(stream, chunk);
				stream.Position = 0;
				newChunk = serializer.ReadObject(stream) as IAmSerializable;
			}

			newChunk.Target("44");
			(newChunk as IRock).Verify();
		}

		[Test]
		public void RoundtripWhenMockIsNotSerializable()
		{
			var rock = Rock.Create<IAmNotSerializable>();
			rock.HandleAction(_ => _.Target("44"));

			var chunk = rock.Make();

			var formatter = new BinaryFormatter();

			using (var stream = new MemoryStream())
			{
				Assert.Throws<SerializationException>(() => formatter.Serialize(stream, chunk));
			}
		}
	}

	public interface IAmSerializable
	{
		void Target(string a);
	}

	public interface IAmNotSerializable
	{
		void Target(string a);
	}
}
