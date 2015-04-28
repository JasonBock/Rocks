using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class CodeTemplatesTests
	{
		[Test]
		public void GetNonPublicActionImplementationTemplate()
		{
			Assert.AreEqual(
@"a override b
{
	c	
}", CodeTemplates.GetNonPublicActionImplementationTemplate("a", "b", "c"));
		}

		[Test]
		public void GetNonPublicFunctionImplementationTemplate()
		{
			Assert.AreEqual(
@"a override b
{
	c	
	
	return default(d);
}", CodeTemplates.GetNonPublicFunctionImplementationTemplate("a", "b", "c", "d"));
		}

		[Test]
		public void GetAssemblyDelegateTemplateWhenIsUnsafeIsFalse()
		{
			Assert.AreEqual("public  delegate a b(c);", CodeTemplates.GetAssemblyDelegateTemplate("a", "b", "c", false));
		}

		[Test]
		public void GetAssemblyDelegateTemplateWhenIsUnsafeIsTrue()
		{
			Assert.AreEqual("public unsafe delegate a b(c);", CodeTemplates.GetAssemblyDelegateTemplate("a", "b", "c", true));
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
}", CodeTemplates.GetPropertyGetWithReferenceTypeReturnValueTemplate(1, "b", "c", "d", "e", "f", "g"));
		}

		[Test]
		public void GetPropertyGetWithReferenceTypeReturnValueAndNoIndexersTemplate()
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
}", CodeTemplates.GetPropertyGetWithReferenceTypeReturnValueAndNoIndexersTemplate(1, "b", "c", "d", "e"));
		}

		[Test]
		public void GetPropertyGetWithValueTypeReturnValueTemplate()
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
}", CodeTemplates.GetPropertyGetWithValueTypeReturnValueTemplate(1, "b", "c", "d", "e", "f", "g"));
		}

		[Test]
		public void GetPropertyGetWithValueTypeReturnValueAndNoIndexersTemplate()
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
}", CodeTemplates.GetPropertyGetWithValueTypeReturnValueAndNoIndexersTemplate(1, "b", "c", "d", "e"));
		}

		[Test]
		public void GetPropertySetTemplate()
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
}", CodeTemplates.GetPropertySetTemplate(1, "b", "c", "d", "e", "f"));
		}

		[Test]
		public void GetPropertySetAndNoIndexersTemplate()
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
}", CodeTemplates.GetPropertySetAndNoIndexersTemplate(1, "b", "c", "d"));
		}

		[Test]
		public void GetEventTemplate()
		{
			Assert.AreEqual("public a event b c;", CodeTemplates.GetEventTemplate("a", "b", "c"));
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
@"h g
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
	
				methodHandler.RaiseEvents(this);
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
}", CodeTemplates.GetActionMethodTemplate(1, "b", "c", "d", "e", "f", "g", "h"));
		}

		[Test]
		public void GetActionMethodWithNoArgumentsTemplate()
		{
			Assert.AreEqual(
@"f e
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
	
		methodHandler.RaiseEvents(this);
		methodHandler.IncrementCallCount();
	}
	else
	{
		throw new NotImplementedException();
	}
}", CodeTemplates.GetActionMethodWithNoArgumentsTemplate(1, "b", "c", "d", "e", "f"));
		}

		[Test]
		public void GetClassTemplateWhenIsUnsafeIsTrue()
		{
			Assert.AreEqual(
@"a

namespace h
{
	i
	public unsafe sealed class b
		: c, IMock m
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

		void IMock.Raise(string eventName, EventArgs args)
		{
			var thisType = this.GetType();

			var eventDelegate = (MulticastDelegate)thisType.GetField(eventName, 
				BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);

			if (eventDelegate != null)
			{
				foreach (var handler in eventDelegate.GetInvocationList())
				{
					handler.Method.Invoke(handler.Target, new object[] { this, args });
				}
			}
		}	
	}

	l
}", CodeTemplates.GetClassTemplate("a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", true, "m"));
		}

		[Test]
		public void GetClassTemplateWhenIsUnsafeIsFalse()
		{
			Assert.AreEqual(
@"a

namespace h
{
	i
	public  sealed class b
		: c, IMock m
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

		void IMock.Raise(string eventName, EventArgs args)
		{
			var thisType = this.GetType();

			var eventDelegate = (MulticastDelegate)thisType.GetField(eventName, 
				BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);

			if (eventDelegate != null)
			{
				foreach (var handler in eventDelegate.GetInvocationList())
				{
					handler.Method.Invoke(handler.Target, new object[] { this, args });
				}
			}
		}	
	}

	l
}", CodeTemplates.GetClassTemplate("a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", false, "m"));
		}

		[Test]
		public void GetFunctionWithReferenceTypeReturnValueMethodTemplate()
		{
			Assert.AreEqual(
@"i h
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
				methodHandler.RaiseEvents(this);
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
}", CodeTemplates.GetFunctionWithReferenceTypeReturnValueMethodTemplate(1, "b", "c", "d", "e", "f", "g", "h", "i"));
		}

		[Test]
		public void GetFunctionWithReferenceTypeReturnValueAndNoArgumentsMethodTemplate()
		{
			Assert.AreEqual(
@"g f
{
	e
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
}", CodeTemplates.GetFunctionWithReferenceTypeReturnValueAndNoArgumentsMethodTemplate(1, "b", "c", "d", "e", "f", "g"));
		}

		[Test]
		public void GetFunctionWithValueTypeReturnValueMethodTemplate()
		{
			Assert.AreEqual(
@"i h
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
				methodHandler.RaiseEvents(this);
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
}", CodeTemplates.GetFunctionWithValueTypeReturnValueMethodTemplate(1, "b", "c", "d", "e", "f", "g", "h", "i"));
		}

		[Test]
		public void GetFunctionWithValueTypeReturnValueAndNoArgumentsMethodTemplate()
		{
			Assert.AreEqual(
@"g f
{
	e
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
}", CodeTemplates.GetFunctionWithValueTypeReturnValueAndNoArgumentsMethodTemplate(1, "b", "c", "d", "e", "f", "g"));
		}
	}
}
