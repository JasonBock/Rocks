﻿using NUnit.Framework;
using Rocks.Options;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using static Rocks.Extensions.IMockExtensions;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class SerializationTests
	{
		[Test]
		public void RoundtripWithExpressions()
		{
			var rock = Rock.Create<IAmSerializable>(new RockOptions(serialization: SerializationOptions.Supported));
			rock.Handle(_ => _.Target(Arg.Is<string>(p => p == "44" || p == "55")));

			var chunk = rock.Make();

			var formatter = new BinaryFormatter()
			{
				Binder = Rock.Binder
			};

			using (var stream = new MemoryStream())
			{
				Assert.That(() => formatter.Serialize(stream, chunk), Throws.TypeOf<SerializationException>());
			}
		}

		[Test]
		public void RoundtripWithBinary()
		{
			var rock = Rock.Create<IAmSerializable>(new RockOptions(serialization: SerializationOptions.Supported));
			rock.Handle(_ => _.Target("44"));

			var chunk = rock.Make();
			IAmSerializable newChunk = null;

			var formatter = new BinaryFormatter()
			{
				Binder = Rock.Binder
			};
			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, chunk);
				stream.Position = 0;
				newChunk = formatter.Deserialize(stream) as IAmSerializable;
			}

			newChunk.Target("44");
			(newChunk as IMock).Verify();
		}

		[Test]
		public void RoundtripWhenMockIsNotSerializable()
		{
			var rock = Rock.Create<IAmNotSerializable>();
			rock.Handle(_ => _.Target("44"));

			var chunk = rock.Make();

			var formatter = new BinaryFormatter();

			using (var stream = new MemoryStream())
			{
				Assert.That(() => formatter.Serialize(stream, chunk), Throws.TypeOf<SerializationException>());
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