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

	if (this.handlers.TryGetValue(""a"", out methodHandlers))
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
}", CodeTemplates.GetPropertyGetWithReferenceTypeReturnValueTemplate("a", "b", "c", "d", "e", "f"));
		}

		[Test]
		public void GetPropertyGetWithReferenceTypeReturnValueAndNoIndexersTemplate()
		{
			Assert.AreEqual(
@"get
{
	ReadOnlyCollection<HandlerInformation> methodHandlers = null;

	if (this.handlers.TryGetValue(""a"", out methodHandlers))
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
}", CodeTemplates.GetPropertyGetWithReferenceTypeReturnValueAndNoIndexersTemplate("a", "b", "c", "d"));
		}
	}
}
