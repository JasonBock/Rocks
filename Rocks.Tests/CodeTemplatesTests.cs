using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class CodeTemplatesTests
	{
		[Test]
		public void GetAssemblyDelegateTemplate()
		{
			Assert.AreEqual("public delegate a b(c);", CodeTemplates.GetAssemblyDelegateTemplate("a", "b", "c"));
		}

		[Test]
		public void GetPropertyTemplate()
		{
			Assert.AreEqual("public a b { c }", CodeTemplates.GetPropertyTemplate("a", "b", "c"));
		}

		[Test]
		public void GetPropertyIndexerTemplate()
		{
			Assert.AreEqual("public a this[b] { c }", CodeTemplates.GetPropertyIndexerTemplate("a", "b", "c"));
		}

		[Test]
		public void GetPropertyGetWithReferenceTypeReturnValueTemplate()
		{
			Assert.AreEqual(
@"get
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
}", CodeTemplates.GetPropertyGetWithReferenceTypeReturnValueTemplate(1, "b", "c", "d", "e", "f"));
		}

		[Test]
		public void GetPropertyGetWithReferenceTypeReturnValueAndNoIndexersTemplate()
		{
			Assert.AreEqual(
@"get
{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(methodHandler.Method as d)(b) as c :
			(methodHandler as HandlerInformation<c>).ReturnValue;
		methodHandler.IncrementCallCount();
		return result;
	}
	else
	{
		throw new NotImplementedException();
	}
}", CodeTemplates.GetPropertyGetWithReferenceTypeReturnValueAndNoIndexersTemplate(1, "b", "c", "d"));
		}

		[Test]
		public void GetPropertyGetWithValueTypeReturnValueTemplate()
		{
			Assert.AreEqual(
@"get
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
}", CodeTemplates.GetPropertyGetWithValueTypeReturnValueTemplate(1, "b", "c", "d", "e", "f"));
		}

		[Test]
		public void GetPropertyGetWithValueTypeReturnValueAndNoIndexersTemplate()
		{
			Assert.AreEqual(
@"get
{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(c)(methodHandler.Method as d)(b) :
			(methodHandler as HandlerInformation<c>).ReturnValue;
		methodHandler.IncrementCallCount();
		return result;
	}
	else
	{
		throw new NotImplementedException();
	}
}", CodeTemplates.GetPropertyGetWithValueTypeReturnValueAndNoIndexersTemplate(1, "b", "c", "d"));
		}

		[Test]
		public void GetPropertySetTemplate()
		{
			Assert.AreEqual(
@"set
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
}", CodeTemplates.GetPropertySetTemplate(1, "b", "c", "d", "e"));
		}

		[Test]
		public void GetPropertySetAndNoIndexersTemplate()
		{
			Assert.AreEqual(
@"set
{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		var methodHandler = methodHandlers[0];

		if(methodHandler.Method != null)
		{
			(methodHandler.Method as c)(b);
		}
	
		methodHandler.IncrementCallCount();
	}
	else
	{
		throw new NotImplementedException();
	}
}", CodeTemplates.GetPropertySetAndNoIndexersTemplate(1, "b", "c"));
		}

		[Test]
		public void GetEventTemplate()
		{
			Assert.AreEqual("public event a b;", CodeTemplates.GetEventTemplate("a", "b"));
		}

		[Test]
		public void GetExpectationTemplate()
		{
			Assert.AreEqual("(methodHandler.Expectations[\"a\"] as ArgumentExpectation<b>).IsValid(a, \"a\")", CodeTemplates.GetExpectationTemplate("a", "b"));
		}

		[Test]
		public void GetRefOutNotImplementedMethodTemplate()
		{
			Assert.AreEqual(
@"public a
{
	throw new NotImplementedException();
}", CodeTemplates.GetRefOutNotImplementedMethodTemplate("a"));
		}

		[Test]
		public void GetConstructorTemplate()
		{
			Assert.AreEqual(
@"public a(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers, c)
	: base(b)
{
	this.handlers = handlers;
}", CodeTemplates.GetConstructorTemplate("a", "b", "c"));
		}

		[Test]
		public void GetConstructorNoArgumentsTemplate()
		{
			Assert.AreEqual(
@"public a() 
{ 
	this.handlers = new ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>>(
		new System.Collections.Generic.Dictionary<int, ReadOnlyCollection<HandlerInformation>>());
}", CodeTemplates.GetConstructorNoArgumentsTemplate("a"));
		}

		[Test]
		public void GetActionMethodTemplate()
		{
			Assert.AreEqual(
@"public g
{
	e
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
	
				methodHandler.IncrementCallCount();
				break;
			}
		}

		if(!foundMatch)
		{
			throw new ExpectationException($""No handlers were found for f"");
		}
	}
	else
	{
		throw new NotImplementedException();
	}
}", CodeTemplates.GetActionMethodTemplate(1, "b", "c", "d", "e", "f", "g"));
		}

		[Test]
		public void GetActionMethodWithNoArgumentsTemplate()
		{
			Assert.AreEqual(
@"public e
{
	d
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		var methodHandler = methodHandlers[0];
		if(methodHandler.Method != null)
		{
			(methodHandler.Method as c)(b);
		}
	
		methodHandler.IncrementCallCount();
	}
	else
	{
		throw new NotImplementedException();
	}
}", CodeTemplates.GetActionMethodWithNoArgumentsTemplate(1, "b", "c", "d", "e"));
		}

		[Test]
		public void GetClassTemplate()
		{
			Assert.AreEqual(
@"a

namespace h
{
	i
	public sealed class b
		: c, IMock
	{
		private ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers;

		j

		public k(ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers)
		{
			this.handlers = handlers;
		}

		g

		d

		e

		f

		ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> IMock.Handlers
		{
			get { return this.handlers; }
		}
	}

	l
}", CodeTemplates.GetClassTemplate("a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l"));
		}

		[Test]
		public void GetFunctionWithReferenceTypeReturnValueMethodTemplate()
		{
			Assert.AreEqual(
@"public h
{
	f
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
				methodHandler.IncrementCallCount();
				return result;
			}
		}

		throw new ExpectationException($""No handlers were found for g"");
	}
	else
	{
		throw new NotImplementedException();
	}
}", CodeTemplates.GetFunctionWithReferenceTypeReturnValueMethodTemplate(1, "b", "c", "d", "e", "f", "g", "h"));
		}

		[Test]
		public void GetFunctionWithReferenceTypeReturnValueAndNoArgumentsMethodTemplate()
		{
			Assert.AreEqual(
@"public f
{
	e
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(methodHandler.Method as d)(b) as c :
			(methodHandler as HandlerInformation<c>).ReturnValue;
		methodHandler.IncrementCallCount();
		return result;
	}
	else
	{
		throw new NotImplementedException();
	}
}", CodeTemplates.GetFunctionWithReferenceTypeReturnValueAndNoArgumentsMethodTemplate(1, "b", "c", "d", "e", "f"));
		}

		[Test]
		public void GetFunctionWithValueTypeReturnValueMethodTemplate()
		{
			Assert.AreEqual(
@"public h
{
	f
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
				methodHandler.IncrementCallCount();
				return result;
			}
		}

		throw new ExpectationException($""No handlers were found for g"");
	}
	else
	{
		throw new NotImplementedException();
	}
}", CodeTemplates.GetFunctionWithValueTypeReturnValueMethodTemplate(1, "b", "c", "d", "e", "f", "g", "h"));
		}

		[Test]
		public void GetFunctionWithValueTypeReturnValueAndNoArgumentsMethodTemplate()
		{
			Assert.AreEqual(
@"public f
{
	e
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(1, out methodHandlers))
	{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(c)(methodHandler.Method as d)(b) :
			(methodHandler as HandlerInformation<c>).ReturnValue;
		methodHandler.IncrementCallCount();
		return result;
	}
	else
	{
		throw new NotImplementedException();
	}
}", CodeTemplates.GetFunctionWithValueTypeReturnValueAndNoArgumentsMethodTemplate(1, "b", "c", "d", "e", "f"));
		}
	}
}
