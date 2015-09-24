using NUnit.Framework;
using Rocks.Templates;
using System.Threading.Tasks;

namespace Rocks.Tests.Templates
{
	[TestFixture]
	public sealed class MethodTemplatesTests
	{
		[Test]
		public void GetDefaultReturnValueForGenericTask()
		{
			Assert.AreEqual("STT.Task.FromResult<Int32>(default(Int32))",
				MethodTemplates.GetDefaultReturnValue(typeof(Task<int>)));
		}

		[Test]
		public void GetDefaultReturnValueForTask()
		{
			Assert.AreEqual("STT.Task.CompletedTask",
				MethodTemplates.GetDefaultReturnValue(typeof(Task)));
		}

		[Test]
		public void GetDefaultReturnValueForNonTaskType()
		{
			Assert.AreEqual("default(Int32)",
				MethodTemplates.GetDefaultReturnValue(typeof(int)));
		}

		[Test]
		public void GetNonPublicActionImplementation()
		{
			Assert.AreEqual(
@"a d override b
{
	c	
}", MethodTemplates.GetNonPublicActionImplementation("a", "b", "c", "d"));
		}

		[Test]
		public void GetNonPublicFunctionImplementation()
		{
			Assert.AreEqual(
@"fa e override b
{
	c	
	
	return default(Int32);
}", MethodTemplates.GetNonPublicFunctionImplementation("a", "b", "c", typeof(int), "e", "f"));
		}

		[Test]
		public void GetAssemblyDelegateTemplateWhenIsUnsafeIsFalse()
		{
			Assert.AreEqual("public  delegate a b(c);", MethodTemplates.GetAssemblyDelegate("a", "b", "c", false));
		}

		[Test]
		public void GetAssemblyDelegateTemplateWhenIsUnsafeIsTrue()
		{
			Assert.AreEqual("public unsafe delegate a b(c);", MethodTemplates.GetAssemblyDelegate("a", "b", "c", true));
		}

		[Test]
		public void GetRefOutNotImplementedMethod()
		{
			Assert.AreEqual(
@"public a
{
	throw new S.NotImplementedException();
}", MethodTemplates.GetRefOutNotImplementedMethod("a"));
		}

		[Test]
		public void GetActionMethod()
		{
			Assert.AreEqual(
@"h g
{
	e
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
			throw new RE.ExpectationException($""No handlers were found for f"");
		}
	}
	else
	{
		throw new S.NotImplementedException();
	}
}", MethodTemplates.GetActionMethod(1, "b", "c", "d", "e", "f", "g", "h"));
		}

		[Test]
		public void GetActionMethodForMake()
		{
			Assert.AreEqual(
@"c b
{
	a
}", MethodTemplates.GetActionMethodForMake("a", "b", "c"));
		}

		[Test]
		public void GetActionMethodWithNoArguments()
		{
			Assert.AreEqual(
@"f e
{
	d
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
}", MethodTemplates.GetActionMethodWithNoArguments(1, "b", "c", "d", "e", "f"));
		}

		[Test]
		public void GetActionMethodWithNoArgumentsForMake()
		{
			Assert.AreEqual(
@"c b
{
	a
}", MethodTemplates.GetActionMethodWithNoArgumentsForMake("a", "b", "c"));
		}

		[Test]
		public void GetFunctionWithReferenceTypeReturnValueMethod()
		{
			Assert.AreEqual(
@"ki j h
{
	f
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

		throw new RE.ExpectationException($""No handlers were found for g"");
	}
	else
	{
		throw new S.NotImplementedException();
	}
}", MethodTemplates.GetFunctionWithReferenceTypeReturnValue(1, "b", "c", "d", "e", "f", "g", "h", "i", "j", "k"));
		}

		[Test]
		public void GetFunctionForMake()
		{
			Assert.AreEqual(
@"ec d b
{
	a

	return default(Int32);
}", MethodTemplates.GetFunctionForMake("a", "b", "c", "d", "e", typeof(int)));
		}

		[Test]
		public void GetFunctionWithReferenceTypeReturnValueAndNoArgumentsMethod()
		{
			Assert.AreEqual(
@"ig h f
{
	e
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
}", MethodTemplates.GetFunctionWithReferenceTypeReturnValueAndNoArguments(1, "b", "c", "d", "e", "f", "g", "h", "i"));
		}

		[Test]
		public void GetFunctionWithValueTypeReturnValueMethod()
		{
			Assert.AreEqual(
@"ki j h
{
	f
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

		throw new RE.ExpectationException($""No handlers were found for g"");
	}
	else
	{
		throw new S.NotImplementedException();
	}
}", MethodTemplates.GetFunctionWithValueTypeReturnValue(1, "b", "c", "d", "e", "f", "g", "h", "i", "j", "k"));
		}

		[Test]
		public void GetFunctionWithValueTypeReturnValueAndNoArgumentsMethod()
		{
			Assert.AreEqual(
@"ig h f
{
	e
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
}", MethodTemplates.GetFunctionWithValueTypeReturnValueAndNoArguments(1, "b", "c", "d", "e", "f", "g", "h", "i"));
		}
	}
}
