using NUnit.Framework;
using Rocks.Templates;

namespace Rocks.Tests.Templates
{
	[TestFixture]
	public sealed class PropertyTemplatesTests
	{
		[Test]
		public void GetProperty() =>
			Assert.That(PropertyTemplates.GetProperty("a", "b", "c", "d", "e"),
				Is.EqualTo("d a eb { c }"));

		[Test]
		public void GetPropertyIndexer() =>
			Assert.That(PropertyTemplates.GetPropertyIndexer("a", "b", "c", "d", "e"),
				Is.EqualTo("d a ethis[b] { c }"));

		[Test]
		public void GetPropertyGetWithReferenceTypeReturnValue() =>
			Assert.That(PropertyTemplates.GetPropertyGetWithReferenceTypeReturnValue(1, "b", "c", "d", "e", "f", "g"), Is.EqualTo(
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
}"));

		[Test]
		public void GetPropertyGetWithReferenceTypeReturnValueAndNoIndexers() =>
			Assert.That(PropertyTemplates.GetPropertyGetWithReferenceTypeReturnValueAndNoIndexers(1, "b", "c", "d", "e"), Is.EqualTo(
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
}"));

		[Test]
		public void GetPropertyGetWithValueTypeReturnValue() =>
			Assert.That(PropertyTemplates.GetPropertyGetWithValueTypeReturnValue(1, "b", "c", "d", "e", "f", "g"), Is.EqualTo(
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
}"));

		[Test]
		public void GetPropertyGetWithValueTypeReturnValueAndNoIndexers() =>
			Assert.That(PropertyTemplates.GetPropertyGetWithValueTypeReturnValueAndNoIndexers(1, "b", "c", "d", "e"), Is.EqualTo(
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
}"));

		[Test]
		public void GetPropertyGetForMake() =>
			Assert.That(PropertyTemplates.GetPropertyGetForMake("a", "b"), Is.EqualTo(
@"a get
{
	return default(b);
}"));

		[Test]
		public void GetPropertySet() =>
			Assert.That(PropertyTemplates.GetPropertySet(1, "b", "c", "d", "e", "f"), Is.EqualTo(
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
}"));

		[Test]
		public void GetPropertySetAndNoIndexers() =>
			Assert.That(PropertyTemplates.GetPropertySetAndNoIndexers(1, "b", "c", "d"), Is.EqualTo(
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
}"));

		[Test]
		public void GetPropertySetForMake() =>
			Assert.That(PropertyTemplates.GetPropertySetForMake("a"),
				Is.EqualTo("a set { }"));
	}
}
