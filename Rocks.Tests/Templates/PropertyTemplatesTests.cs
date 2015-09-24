using NUnit.Framework;
using Rocks.Templates;

namespace Rocks.Tests.Templates
{
	[TestFixture]
	public sealed class PropertyTemplatesTests
	{
		[Test]
		public void GetProperty()
		{
			Assert.AreEqual("d a eb { c }", PropertyTemplates.GetProperty("a", "b", "c", "d", "e"));
		}

		[Test]
		public void GetPropertyIndexer()
		{
			Assert.AreEqual("d a ethis[b] { c }", PropertyTemplates.GetPropertyIndexer("a", "b", "c", "d", "e"));
		}

		[Test]
		public void GetPropertyGetWithReferenceTypeReturnValue()
		{
			Assert.AreEqual(
@"g get
{
	SCO.ReadOnlyCollection<R.HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		foreach(var methodHandler in methodHandlers)
		{
			if(d)
			{
				var result = methodHandler.Method != null ?
					(methodHandler.Method as e)(b) as c :
					(methodHandler as R.HandlerInformation<c>).ReturnValue;
				methodHandler.RaiseEvents(this);
				methodHandler.IncrementCallCount();
				return result;
			}
		}

		throw new RE.ExpectationException($""No handlers were found for f"");
	}
	else
	{
		throw new S.NotImplementedException();
	}
}", PropertyTemplates.GetPropertyGetWithReferenceTypeReturnValue(1, "b", "c", "d", "e", "f", "g"));
		}

		[Test]
		public void GetPropertyGetWithReferenceTypeReturnValueAndNoIndexers()
		{
			Assert.AreEqual(
@"e get
{
	SCO.ReadOnlyCollection<R.HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(methodHandler.Method as d)(b) as c :
			(methodHandler as R.HandlerInformation<c>).ReturnValue;
		methodHandler.RaiseEvents(this);
		methodHandler.IncrementCallCount();
		return result;
	}
	else
	{
		throw new S.NotImplementedException();
	}
}", PropertyTemplates.GetPropertyGetWithReferenceTypeReturnValueAndNoIndexers(1, "b", "c", "d", "e"));
		}

		[Test]
		public void GetPropertyGetWithValueTypeReturnValue()
		{
			Assert.AreEqual(
@"g get
{
	SCO.ReadOnlyCollection<R.HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		foreach(var methodHandler in methodHandlers)
		{
			if(d)
			{
				var result = methodHandler.Method != null ?
					(c)(methodHandler.Method as e)(b) :
					(methodHandler as R.HandlerInformation<c>).ReturnValue;
				methodHandler.RaiseEvents(this);
				methodHandler.IncrementCallCount();
				return result;
			}
		}

		throw new RE.ExpectationException($""No handlers were found for f"");
	}
	else
	{
		throw new S.NotImplementedException();
	}
}", PropertyTemplates.GetPropertyGetWithValueTypeReturnValue(1, "b", "c", "d", "e", "f", "g"));
		}

		[Test]
		public void GetPropertyGetWithValueTypeReturnValueAndNoIndexers()
		{
			Assert.AreEqual(
@"e get
{
	SCO.ReadOnlyCollection<R.HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(c)(methodHandler.Method as d)(b) :
			(methodHandler as R.HandlerInformation<c>).ReturnValue;
		methodHandler.RaiseEvents(this);
		methodHandler.IncrementCallCount();
		return result;
	}
	else
	{
		throw new S.NotImplementedException();
	}
}", PropertyTemplates.GetPropertyGetWithValueTypeReturnValueAndNoIndexers(1, "b", "c", "d", "e"));
		}

		[Test]
		public void GetPropertyGetForMake()
		{
			Assert.AreEqual(
@"a get
{
	return default(b);
}", PropertyTemplates.GetPropertyGetForMake("a", "b"));
		}

		[Test]
		public void GetPropertySet()
		{
			Assert.AreEqual(
@"f set
{
	SCO.ReadOnlyCollection<R.HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		var foundMatch = false;

		foreach(var methodHandler in methodHandlers)
		{
			if(c)
			{
				foundMatch = true;

				if(methodHandler.Method != null)
				{
					(methodHandler.Method as d)(b);
				}
	
				methodHandler.RaiseEvents(this);
				methodHandler.IncrementCallCount();
				break;
			}
		}

		if(!foundMatch)
		{
			throw new RE.ExpectationException($""No handlers were found for e"");
		}
	}
	else
	{
		throw new S.NotImplementedException();
	}
}", PropertyTemplates.GetPropertySet(1, "b", "c", "d", "e", "f"));
		}

		[Test]
		public void GetPropertySetAndNoIndexers()
		{
			Assert.AreEqual(
@"d set
{
	SCO.ReadOnlyCollection<R.HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		var methodHandler = methodHandlers[0];

		if(methodHandler.Method != null)
		{
			(methodHandler.Method as c)(b);
		}
	
		methodHandler.RaiseEvents(this);
		methodHandler.IncrementCallCount();
	}
	else
	{
		throw new S.NotImplementedException();
	}
}", PropertyTemplates.GetPropertySetAndNoIndexers(1, "b", "c", "d"));
		}

		[Test]
		public void GetPropertySetForMake()
		{
			Assert.AreEqual("a set { }", PropertyTemplates.GetPropertySetForMake("a"));
		}
	}
}
