using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;

public class x : System.Runtime.Remoting.Messaging.IRemotingFormatter
{
	public SerializationBinder Binder
	{
		get
		{
			throw new NotImplementedException();
		}

		set
		{
			throw new NotImplementedException();
		}
	}

	public StreamingContext Context
	{
		get
		{
			throw new NotImplementedException();
		}

		set
		{
			throw new NotImplementedException();
		}
	}

	public ISurrogateSelector SurrogateSelector
	{
		get
		{
			throw new NotImplementedException();
		}

		set
		{
			throw new NotImplementedException();
		}
	}

	public object Deserialize(Stream serializationStream)
	{
		throw new NotImplementedException();
	}

	public object Deserialize(Stream serializationStream, HeaderHandler handler)
	{
		throw new NotImplementedException();
	}

	public void Serialize(Stream serializationStream, object graph)
	{
		throw new NotImplementedException();
	}

	public void Serialize(Stream serializationStream, object graph, Header[] headers)
	{
		throw new NotImplementedException();
	}
}
