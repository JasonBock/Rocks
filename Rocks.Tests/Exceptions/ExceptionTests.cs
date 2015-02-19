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
			Assert.IsNotNull(exception.Message, nameof(exception.Message));
			Assert.IsNull(exception.InnerException, nameof(exception.InnerException));
		}

		protected void CreateExceptionWithMessageTest(string message)
		{
			var exception = Activator.CreateInstance(typeof(T), message) as T;
			Assert.AreEqual(message, exception.Message, nameof(exception.Message));
			Assert.IsNull(exception.InnerException, nameof(exception.InnerException));
		}

		protected void CreateExceptionWithMessageAndInnerExceptionTest(string message)
		{
			var innerException = new TInner();
			var exception = Activator.CreateInstance(typeof(T), message, innerException) as T;
			Assert.AreEqual(message, exception.Message, nameof(exception.Message));
			Assert.AreEqual(innerException, exception.InnerException, nameof(exception.InnerException));
		}

		protected void RoundtripExceptionTest(string message)
		{
			var exception = Activator.CreateInstance(typeof(T), message) as T;
			T newException = null;

			var formatter = new BinaryFormatter();

			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, exception);
				stream.Position = 0;
				newException = formatter.Deserialize(stream) as T;
			}

			Assert.IsNotNull(newException);
			Assert.AreEqual(message, newException.Message, nameof(exception.Message));
		}
	}
}
