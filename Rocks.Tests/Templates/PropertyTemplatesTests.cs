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
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		foreach(var methodHandler in methodHandlers)
		{
			if(d)
			{
				var result = methodHandler.Method != null ?
					(methodHandler.Method as e)(b) as c :
					(methodHandler as HandlerInformation<c>).ReturnValue;
				methodHandler.RaiseEvents(this);
				methodHandler.IncrementCallCount();
				return result;
			}
		}

		throw new ExpectationException($""No handlers were found for f"");
	}
	else
	{
		throw new NotImplementedException();
	}
}", PropertyTemplates.GetPropertyGetWithReferenceTypeReturnValue(1, "b", "c", "d", "e", "f", "g"));
		}

		[Test]
		public void GetPropertyGetWithReferenceTypeReturnValueAndNoIndexers()
		{
			Assert.AreEqual(
@"e get
{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(methodHandler.Method as d)(b) as c :
			(methodHandler as HandlerInformation<c>).ReturnValue;
		methodHandler.RaiseEvents(this);
		methodHandler.IncrementCallCount();
		return result;
	}
	else
	{
		throw new NotImplementedException();
	}
}", PropertyTemplates.GetPropertyGetWithReferenceTypeReturnValueAndNoIndexers(1, "b", "c", "d", "e"));
		}

		[Test]
		public void GetPropertyGetWithValueTypeReturnValue()
		{
			Assert.AreEqual(
@"g get
{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		foreach(var methodHandler in methodHandlers)
		{
			if(d)
			{
				var result = methodHandler.Method != null ?
					(c)(methodHandler.Method as e)(b) :
					(methodHandler as HandlerInformation<c>).ReturnValue;
				methodHandler.RaiseEvents(this);
				methodHandler.IncrementCallCount();
				return result;
			}
		}

		throw new ExpectationException($""No handlers were found for f"");
	}
	else
	{
		throw new NotImplementedException();
	}
}", PropertyTemplates.GetPropertyGetWithValueTypeReturnValue(1, "b", "c", "d", "e", "f", "g"));
		}

		[Test]
		public void GetPropertyGetWithValueTypeReturnValueAndNoIndexers()
		{
			Assert.AreEqual(
@"e get
{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(c)(methodHandler.Method as d)(b) :
			(methodHandler as HandlerInformation<c>).ReturnValue;
		methodHandler.RaiseEvents(this);
		methodHandler.IncrementCallCount();
		return result;
	}
	else
	{
		throw new NotImplementedException();
	}
}", PropertyTemplates.GetPropertyGetWithValueTypeReturnValueAndNoIndexers(1, "b", "c", "d", "e"));
		}

		[Test]
		public void GetPropertySet()
		{
			Assert.AreEqual(
@"f set
{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

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
			throw new ExpectationException($""No handlers were found for e"");
		}
	}
	else
	{
		throw new NotImplementedException();
	}
}", PropertyTemplates.GetPropertySet(1, "b", "c", "d", "e", "f"));
		}

		[Test]
		public void GetPropertySetAndNoIndexers()
		{
			Assert.AreEqual(
@"d set
{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

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
		throw new NotImplementedException();
	}
}", PropertyTemplates.GetPropertySetAndNoIndexers(1, "b", "c", "d"));
		}
	}
}
