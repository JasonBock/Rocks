using NUnit.Framework;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Rocks.Tests.Exceptions
{
	public abstract class ExceptionTests<T, TInner>
		where T : Exception, new()
		where TInner : Exception, new()
	{
		protected ExceptionTests()
			: base()
		{ }

		protected void CreateExceptionTest()
		{
			var exception = new T();
			Assert.That(exception.Message, Is.Not.Null, nameof(exception.Message));
			Assert.That(exception.InnerException, Is.Null, nameof(exception.InnerException));
		}

		protected void CreateExceptionWithMessageTest(string message)
		{
			var exception = (T)Activator.CreateInstance(typeof(T), message);
			Assert.That(exception.Message, Is.EqualTo(message), nameof(exception.Message));
			Assert.That(exception.InnerException, Is.Null, nameof(exception.InnerException));
		}

		protected void CreateExceptionWithMessageAndInnerExceptionTest(string message)
		{
			var innerException = new TInner();
			var exception = (T)Activator.CreateInstance(typeof(T), message, innerException);
			Assert.That(exception.Message, Is.EqualTo(message), nameof(exception.Message));
			Assert.That(exception.InnerException, Is.EqualTo(innerException), nameof(exception.InnerException));
		}

		protected void RoundtripExceptionTest(string message)
		{
			var exception = Activator.CreateInstance(typeof(T), message) as T;
			T newException;

			var formatter = new BinaryFormatter();

			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, exception);
				stream.Position = 0;
				newException = (T)formatter.Deserialize(stream);
			}

			Assert.That(newException, Is.Not.Null);
			Assert.That(newException.Message, Is.EqualTo(message), nameof(exception.Message));
		}
	}
}
