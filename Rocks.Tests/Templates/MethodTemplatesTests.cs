using NUnit.Framework;
using Rocks.Templates;
using System.Threading.Tasks;

namespace Rocks.Tests.Templates
{
	public static class MethodTemplatesTests
	{
		[Test]
		public static void GetDefaultReturnValueForGenericTask() =>
			Assert.That(MethodTemplates.GetDefaultReturnValue(typeof(Task<int>)),
				Is.EqualTo("STT.Task.FromResult<int>(default(int))"));

		[Test]
		public static void GetDefaultReturnValueForTask() =>
			Assert.That(MethodTemplates.GetDefaultReturnValue(typeof(Task)),
				Is.EqualTo("STT.Task.CompletedTask"));

		[Test]
		public static void GetDefaultReturnValueForNonTaskType() =>
			Assert.That(MethodTemplates.GetDefaultReturnValue(typeof(int)),
				Is.EqualTo("default(int)"));

		[Test]
		public static void GetNonPublicActionImplementation() =>
			Assert.That(MethodTemplates.GetNonPublicActionImplementation("a", "b", "c", "d"), Is.EqualTo(
@"a d override b
{
	c	
}"));

		[Test]
		public static void GetNonPublicFunctionImplementation() =>
			Assert.That(MethodTemplates.GetNonPublicFunctionImplementation("a", "b", "c", typeof(int), "e", "f"), Is.EqualTo(
@"fa e override b
{
	c	
	
	return default(int);
}"));

		[Test]
		public static void GetAssemblyDelegateTemplateWhenIsUnsafeIsFalse() =>
			Assert.That(MethodTemplates.GetAssemblyDelegate("a", "b", "c", false),
				Is.EqualTo("public  delegate a b(c);"));

		[Test]
		public static void GetAssemblyDelegateTemplateWhenIsUnsafeIsTrue() =>
			Assert.That(MethodTemplates.GetAssemblyDelegate("a", "b", "c", true),
				Is.EqualTo("public unsafe delegate a b(c);"));

		[Test]
		public static void GetRefOutNotImplementedMethod() =>
			Assert.That(MethodTemplates.GetNotImplementedMethod("a"), Is.EqualTo(
@"public a =>
	throw new S.NotImplementedException();"));

		[Test]
		public static void GetActionMethodWhenHasEventsIsTrue() =>
			Assert.That(MethodTemplates.GetActionMethod(1, "b", "c", "d", "e", "f", "g", "h", true), Is.EqualTo(
@"h g
{
	e

	if (this.handlers.TryGetValue(1, out var methodHandlers))
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
		public static void GetActionMethodWhenHasEventsIsFalse() =>
			Assert.That(MethodTemplates.GetActionMethod(1, "b", "c", "d", "e", "f", "g", "h", false), Is.EqualTo(
@"h g
{
	e

	if (this.handlers.TryGetValue(1, out var methodHandlers))
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
			throw new RE.ExpectationException($""No handlers were found for f"");
		}
	}
	else
	{
		throw new S.NotImplementedException();
	}
}"));

		[Test]
		public static void GetActionMethodForMake() =>
			Assert.That(MethodTemplates.GetActionMethodForMake("a", "b", "c"), Is.EqualTo(
@"c b
{
	a
}"));

		[Test]
		public static void GetActionMethodWithNoArgumentsAndHasEventsIsTrue() =>
			Assert.That(MethodTemplates.GetActionMethodWithNoArguments(1, "b", "c", "d", "e", "f", true), Is.EqualTo(
@"f e
{
	d

	if (this.handlers.TryGetValue(1, out var methodHandlers))
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
		public static void GetActionMethodWithNoArgumentsAndHasEventsIsFalse() =>
			Assert.That(MethodTemplates.GetActionMethodWithNoArguments(1, "b", "c", "d", "e", "f", false), Is.EqualTo(
@"f e
{
	d

	if (this.handlers.TryGetValue(1, out var methodHandlers))
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
		throw new S.NotImplementedException();
	}
}"));

		[Test]
		public static void GetActionMethodWithNoArgumentsForMake() =>
			Assert.That(MethodTemplates.GetActionMethodWithNoArgumentsForMake("a", "b", "c"), Is.EqualTo(
@"c b
{
	a
}"));

		[Test]
		public static void GetFunctionWithReferenceTypeReturnValueMethodAndHasEventsIsTrue() =>
			Assert.That(MethodTemplates.GetFunctionWithReferenceTypeReturnValue(1, "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", true), Is.EqualTo(
@"ki j h
{
	f

	if (this.handlers.TryGetValue(1, out var methodHandlers))
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
		public static void GetFunctionWithReferenceTypeReturnValueMethodAndHasEventsIsFalse() =>
			Assert.That(MethodTemplates.GetFunctionWithReferenceTypeReturnValue(1, "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", false), Is.EqualTo(
@"ki j h
{
	f

	if (this.handlers.TryGetValue(1, out var methodHandlers))
	{
		foreach(var methodHandler in methodHandlers)
		{
			if(d)
			{
				var result = methodHandler.Method != null ?
					(methodHandler.Method as e)(b) as c :
					(methodHandler as R.HandlerInformation<c>).ReturnValue;
				
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
		public static void GetFunctionForMake() =>
			Assert.That(MethodTemplates.GetFunctionForMake("a", "b", "c", "d", "e", typeof(int)), Is.EqualTo(
@"ec d b
{
	a

	return default(int);
}"));

		[Test]
		public static void GetFunctionWithReferenceTypeReturnValueAndNoArgumentsMethodAndHasEventsIsTrue() =>
			Assert.That(MethodTemplates.GetFunctionWithReferenceTypeReturnValueAndNoArguments(1, "b", "c", "d", "e", "f", "g", "h", "i", true), Is.EqualTo(
@"ig h f
{
	e

	if (this.handlers.TryGetValue(1, out var methodHandlers))
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
		public static void GetFunctionWithReferenceTypeReturnValueAndNoArgumentsMethodAndHasEventsIsFalse() =>
			Assert.That(MethodTemplates.GetFunctionWithReferenceTypeReturnValueAndNoArguments(1, "b", "c", "d", "e", "f", "g", "h", "i", false), Is.EqualTo(
@"ig h f
{
	e

	if (this.handlers.TryGetValue(1, out var methodHandlers))
	{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(methodHandler.Method as d)(b) as c :
			(methodHandler as R.HandlerInformation<c>).ReturnValue;
		
		methodHandler.IncrementCallCount();
		return result;
	}
	else
	{
		throw new S.NotImplementedException();
	}
}"));

		[Test]
		public static void GetFunctionWithValueTypeReturnValueMethodAndHasEventsIsTrue() =>
			Assert.That(MethodTemplates.GetFunctionWithValueTypeReturnValue(1, "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", true), Is.EqualTo(
@"ki j h
{
	f

	if (this.handlers.TryGetValue(1, out var methodHandlers))
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
		public static void GetFunctionWithValueTypeReturnValueMethodAndHasEventsIsFalse() =>
			Assert.That(MethodTemplates.GetFunctionWithValueTypeReturnValue(1, "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", false), Is.EqualTo(
@"ki j h
{
	f

	if (this.handlers.TryGetValue(1, out var methodHandlers))
	{
		foreach(var methodHandler in methodHandlers)
		{
			if(d)
			{
				var result = methodHandler.Method != null ?
					(c)(methodHandler.Method as e)(b) :
					(methodHandler as R.HandlerInformation<c>).ReturnValue;
				
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
		public static void GetFunctionWithValueTypeReturnValueAndNoArgumentsMethodAndHasEventsIsTrue() =>
			Assert.That(MethodTemplates.GetFunctionWithValueTypeReturnValueAndNoArguments(1, "b", "c", "d", "e", "f", "g", "h", "i", true), Is.EqualTo(
@"ig h f
{
	e

	if (this.handlers.TryGetValue(1, out var methodHandlers))
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

		[Test]
		public static void GetFunctionWithValueTypeReturnValueAndNoArgumentsMethodAndHasEventsIsFalse() =>
			Assert.That(MethodTemplates.GetFunctionWithValueTypeReturnValueAndNoArguments(1, "b", "c", "d", "e", "f", "g", "h", "i", false), Is.EqualTo(
@"ig h f
{
	e

	if (this.handlers.TryGetValue(1, out var methodHandlers))
	{
		var methodHandler = methodHandlers[0];
		var result = methodHandler.Method != null ?
			(c)(methodHandler.Method as d)(b) :
			(methodHandler as R.HandlerInformation<c>).ReturnValue;
		
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
