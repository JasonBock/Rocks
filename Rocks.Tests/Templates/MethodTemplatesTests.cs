using NUnit.Framework;
using Rocks.Templates;
using System.Threading.Tasks;

namespace Rocks.Tests.Templates
{
	[TestFixture]
	public sealed class MethodTemplatesTests
	{
		[Test]
		public void GetDefaultReturnValueForGenericTask() =>
			Assert.That(MethodTemplates.GetDefaultReturnValue(typeof(Task<int>)),
				Is.EqualTo("STT.Task.FromResult<Int32>(default(Int32))"));

		[Test]
		public void GetDefaultReturnValueForTask() =>
			Assert.That(MethodTemplates.GetDefaultReturnValue(typeof(Task)),
				Is.EqualTo("STT.Task.CompletedTask"));

		[Test]
		public void GetDefaultReturnValueForNonTaskType() =>
			Assert.That(MethodTemplates.GetDefaultReturnValue(typeof(int)),
				Is.EqualTo("default(Int32)"));

		[Test]
		public void GetNonPublicActionImplementation() =>
			Assert.That(MethodTemplates.GetNonPublicActionImplementation("a", "b", "c", "d"), Is.EqualTo(
@"a d override b
{
	c	
}"));

		[Test]
		public void GetNonPublicFunctionImplementation() =>
			Assert.That(MethodTemplates.GetNonPublicFunctionImplementation("a", "b", "c", typeof(int), "e", "f"), Is.EqualTo(
@"fa e override b
{
	c	
	
	return default(Int32);
}"));

		[Test]
		public void GetAssemblyDelegateTemplateWhenIsUnsafeIsFalse() =>
			Assert.That(MethodTemplates.GetAssemblyDelegate("a", "b", "c", false),
				Is.EqualTo("public  delegate a b(c);"));

		[Test]
		public void GetAssemblyDelegateTemplateWhenIsUnsafeIsTrue() =>
			Assert.That(MethodTemplates.GetAssemblyDelegate("a", "b", "c", true),
				Is.EqualTo("public unsafe delegate a b(c);"));

		[Test]
		public void GetRefOutNotImplementedMethod() =>
			Assert.That(MethodTemplates.GetRefOutNotImplementedMethod("a"), Is.EqualTo(
@"public a
{
	throw new S.NotImplementedException();
}"));

		[Test]
		public void GetActionMethod() =>
			Assert.That(MethodTemplates.GetActionMethod(1, "b", "c", "d", "e", "f", "g", "h"), Is.EqualTo(
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
}"));

		[Test]
		public void GetActionMethodForMake() =>
			Assert.That(MethodTemplates.GetActionMethodForMake("a", "b", "c"), Is.EqualTo(
@"c b
{
	a
}"));

		[Test]
		public void GetActionMethodWithNoArguments() =>
			Assert.That(MethodTemplates.GetActionMethodWithNoArguments(1, "b", "c", "d", "e", "f"), Is.EqualTo(
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
}"));

		[Test]
		public void GetActionMethodWithNoArgumentsForMake() =>
			Assert.That(MethodTemplates.GetActionMethodWithNoArgumentsForMake("a", "b", "c"), Is.EqualTo(
@"c b
{
	a
}"));

		[Test]
		public void GetFunctionWithReferenceTypeReturnValueMethod() =>
			Assert.That(MethodTemplates.GetFunctionWithReferenceTypeReturnValue(1, "b", "c", "d", "e", "f", "g", "h", "i", "j", "k"), Is.EqualTo(
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
}"));

		[Test]
		public void GetFunctionForMake() =>
			Assert.That(MethodTemplates.GetFunctionForMake("a", "b", "c", "d", "e", typeof(int)), Is.EqualTo(
@"ec d b
{
	a

	return default(Int32);
}"));

		[Test]
		public void GetFunctionWithReferenceTypeReturnValueAndNoArgumentsMethod() =>
			Assert.That(MethodTemplates.GetFunctionWithReferenceTypeReturnValueAndNoArguments(1, "b", "c", "d", "e", "f", "g", "h", "i"), Is.EqualTo(
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
}"));

		[Test]
		public void GetFunctionWithValueTypeReturnValueMethod() =>
			Assert.That(MethodTemplates.GetFunctionWithValueTypeReturnValue(1, "b", "c", "d", "e", "f", "g", "h", "i", "j", "k"), Is.EqualTo(
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
}"));

		[Test]
		public void GetFunctionWithValueTypeReturnValueAndNoArgumentsMethod() =>
			Assert.That(MethodTemplates.GetFunctionWithValueTypeReturnValueAndNoArguments(1, "b", "c", "d", "e", "f", "g", "h", "i"), Is.EqualTo(
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
}"));
	}
}
