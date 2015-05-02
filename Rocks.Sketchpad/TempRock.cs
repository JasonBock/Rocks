using System;
using System.Collections;
using System.Collections.Generic;

public class x<T> : IEnumerator<T>
{
	T IEnumerator<T>.Current
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public object Current
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public void Dispose()
	{
		throw new NotImplementedException();
	}

	public bool MoveNext()
	{
		throw new NotImplementedException();
	}

	public void Reset()
	{
		throw new NotImplementedException();
	}
}
