Why don't I just generate a custom `Expectations<>` class for the mock?

Let's say I have this:

```csharp
public interface ITestInterface
{
	void NoParameters();
	void OneParameter(int a);
	void MultipleParameters(int a, string b);
   	int GetData { get; }
}
```

Right now, Rocks generates something like this (without the property):

```csharp
using Rocks.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;
#nullable enable

namespace Rocks.IntegrationTests
{
	internal static class CreateExpectationsOfITestInterfaceExtensions
	{
		internal static global::Rocks.Expectations.MethodExpectations<global::Rocks.IntegrationTests.ITestInterface> Methods(
            this global::Rocks.Expectations.Expectations<global::Rocks.IntegrationTests.ITestInterface> @self) =>
			    new(@self);
		
		internal static global::Rocks.IntegrationTests.ITestInterface Instance(
            this global::Rocks.Expectations.Expectations<global::Rocks.IntegrationTests.ITestInterface> @self)
		{
			if (!@self.WasInstanceInvoked)
			{
				@self.WasInstanceInvoked = true;
				var @mock = new RockITestInterface(@self);
				@self.MockType = @mock.GetType();
				return @mock;
			}
			else
			{
				throw new global::Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
			}
		}
		
		private sealed class RockITestInterface
			: global::Rocks.IntegrationTests.ITestInterface
		{
			private readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Rocks.HandlerInformation>> handlers;
			
			public RockITestInterface(global::Rocks.Expectations.Expectations<global::Rocks.IntegrationTests.ITestInterface> @expectations)
			{
				this.handlers = @expectations.Handlers;
			}
			
			[global::Rocks.MemberIdentifier(0, "void NoParameters()")]
			public void NoParameters()
			{
				if (this.handlers.TryGetValue(0, out var @methodHandlers))
				{
					var @methodHandler = @methodHandlers[0];
					@methodHandler.IncrementCallCount();
					if (@methodHandler.Method is not null)
					{
						((global::System.Action)@methodHandler.Method)();
					}
				}
				else
				{
					throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void NoParameters()");
				}
			}
			
			[global::Rocks.MemberIdentifier(1, "void OneParameter(int @a)")]
			public void OneParameter(int @a)
			{
				if (this.handlers.TryGetValue(1, out var @methodHandlers))
				{
					var @foundMatch = false;
					
					foreach (var @methodHandler in @methodHandlers)
					{
						if (((global::Rocks.Argument<int>)@methodHandler.Expectations[0]).IsValid(@a))
						{
							@foundMatch = true;
							
							@methodHandler.IncrementCallCount();
							if (@methodHandler.Method is not null)
							{
								((global::System.Action<int>)@methodHandler.Method)(@a);
							}
							break;
						}
					}
					
					if (!@foundMatch)
					{
						throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void OneParameter(int @a)");
					}
				}
				else
				{
					throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void OneParameter(int @a)");
				}
			}
			
			[global::Rocks.MemberIdentifier(2, "void MultipleParameters(int @a, string @b)")]
			public void MultipleParameters(int @a, string @b)
			{
				if (this.handlers.TryGetValue(2, out var @methodHandlers))
				{
					var @foundMatch = false;
					
					foreach (var @methodHandler in @methodHandlers)
					{
						if (((global::Rocks.Argument<int>)@methodHandler.Expectations[0]).IsValid(@a) &&
							((global::Rocks.Argument<string>)@methodHandler.Expectations[1]).IsValid(@b))
						{
							@foundMatch = true;
							
							@methodHandler.IncrementCallCount();
							if (@methodHandler.Method is not null)
							{
								((global::System.Action<int, string>)@methodHandler.Method)(@a, @b);
							}
							break;
						}
					}
					
					if (!@foundMatch)
					{
						throw new global::Rocks.Exceptions.ExpectationException("No handlers match for void MultipleParameters(int @a, string @b)");
					}
				}
				else
				{
					throw new global::Rocks.Exceptions.ExpectationException("No handlers were found for void MultipleParameters(int @a, string @b)");
				}
			}
			
		}
	}
	
	internal static class MethodExpectationsOfITestInterfaceExtensions
	{
		internal static global::Rocks.MethodAdornments<global::Rocks.IntegrationTests.ITestInterface, global::System.Action> NoParameters(
            this global::Rocks.Expectations.MethodExpectations<global::Rocks.IntegrationTests.ITestInterface> @self) =>
			    new global::Rocks.MethodAdornments<global::Rocks.IntegrationTests.ITestInterface, global::System.Action>(
                    @self.Add(0, new global::System.Collections.Generic.List<global::Rocks.Argument>()));

		internal static global::Rocks.MethodAdornments<global::Rocks.IntegrationTests.ITestInterface, global::System.Action<int>> OneParameter(
            this global::Rocks.Expectations.MethodExpectations<global::Rocks.IntegrationTests.ITestInterface> @self, 
            global::Rocks.Argument<int> @a)
		{
			global::System.ArgumentNullException.ThrowIfNull(@a);
			return new global::Rocks.MethodAdornments<global::Rocks.IntegrationTests.ITestInterface, global::System.Action<int>>(@self.Add(1, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @a }));
		}

		internal static global::Rocks.MethodAdornments<global::Rocks.IntegrationTests.ITestInterface, global::System.Action<int, string>> MultipleParameters(
            this global::Rocks.Expectations.MethodExpectations<global::Rocks.IntegrationTests.ITestInterface> @self, 
            global::Rocks.Argument<int> @a, 
            global::Rocks.Argument<string> @b)
		{
			global::System.ArgumentNullException.ThrowIfNull(@a);
			global::System.ArgumentNullException.ThrowIfNull(@b);
			return new global::Rocks.MethodAdornments<global::Rocks.IntegrationTests.ITestInterface, global::System.Action<int, string>>(@self.Add(2, new global::System.Collections.Generic.List<global::Rocks.Argument>(2) { @a, @b }));
		}
	}
}
```

Typical test code looks like this:

```csharp
var expectations = Rock.Create<ITestInterface>();
expectations.Methods().NoParameters();
expectations.Methods().OneParameter(12);
expectations.Properties().Getters().GetData().Returns(3);

var mock = expectations.Instance();
mock.NoParameters();
mock.OneParameter(12);
var value = mock.GetData;

Assert.That(value, Is.EqualTo(3));

expectations.Verify();
```

What I'd **like** is this:

```csharp
using var expectations = Rock.Create<ITestInterface>().Expectations();
expectations.NoParameters();
expectations.OneParameter(12);
expectations.GetData.Returns(3);

var mock = expectations.Instance();
mock.NoParameters();
mock.OneParameter(12);
var value = mock.GetData;

Assert.That(value, Is.EqualTo(3));
```

So, could I generate:

```csharp
internal class ExpectationsForITestInterface
    : Expectations<ITestInterface>, IDisposable
{
    internal static ExpectationsForITestInterface Expectations(this Expectations<ITestInterface> self) => new();
    
    private ExpectationsForITestInterface() { }

    // It might be better to do this to avoid naming conflicts...
    //internal static ITestInterface Instance(this ExpectationsForITestInterface self) 

    internal ITestInterface Instance()
    {
        if (!this.WasInstanceInvoked)
        {
            this.WasInstanceInvoked = true;
            var @mock = new RockITestInterface(this);
            this.MockType = @mock.GetType();
            return @mock;
        }
        else
        {
            throw new Rocks.Exceptions.NewMockInstanceException("Can only create a new mock once.");
        }
    }
    
    private sealed class RockITestInterface
        : global::Rocks.IntegrationTests.ITestInterface
    {
        // Mock implementation goes here...
    }

    internal Rocks.MethodAdornments<ITestInterface, Action> NoParameters() =>
        new Rocks.MethodAdornments<ITestInterface, Action>(this.Add(
            0, new List<Rocks.Argument>()));

    internal Rocks.MethodAdornments<ITestInterface, Action<int>> OneParameter(
        Rocks.Argument<int> @a)
    {
        ArgumentNullException.ThrowIfNull(@a);
        return new Rocks.MethodAdornments<ITestInterface, Action<int>>(
            this.Add(1, new List<Rocks.Argument>(1) { @a }));
    }

    internal Rocks.MethodAdornments<ITestInterface, Action<int, string>> MultipleParameters(
        Rocks.Argument<int> @a, Rocks.Argument<string> @b)
    {
        ArgumentNullException.ThrowIfNull(@a);
        ArgumentNullException.ThrowIfNull(@b);
        return new Rocks.MethodAdornments<ITestInterface, Action<int, string>>(
            this.Add(2, new List<Rocks.Argument>(2) { @a, @b }));
    }

    internal Rocks.PropertyAdornments<ITestInterface, Func<int>, int> GetData
    {
        get => new Rocks.PropertyAdornments<ITestInterface, Func<int>, int>(
            this.Add<int>(3, new List<Rocks.Argument>()));
    }

    public void Dispose() => this.Verify();
}
```

What I like about this:
* It makes the gen'd API arguably cleaner and more concise. 
* It requires less code to be generated. I don't need to make all the static classes for the extension methods.

Note that the above implementation includes resolving this [issue](https://github.com/JasonBock/Rocks/issues/224).

I'm not sure how a setter would fall through with this...need to spec that out.

I may need to implement the `Expectations<>` methods like `Add()` through an explicit interface implementation. This would eliminate any collisions with mock targets that would have a method called `Add()`. All of the calls would be invoked like this:

```csharp
internal Rocks.MethodAdornments<ITestInterface, Action> NoParameters() =>
    new Rocks.MethodAdornments<ITestInterface, Action>(
        ((IExpectations)this).Add(0, new List<Rocks.Argument>()));
```

If explicit interface implementation is needed, that would be done on the `Expectations<>` class as well...kind of like what it's currently done, something like `FooForIInterfaceA`. Not quite sure right now.

Actually, I thought of something last night. If explicit interface implementation is needed, I'd create a "wrapper" class, maybe deriving from the gen'd class:

```csharp
public interface IAmExplicit 
{ 
    void ExplicitCall(); 
}

internal sealed class ExplicitExpectationsForIAmExplicit
    : ExpectationsForITestInterface
{
    private readonly ExpectationsForITestInterface expectations;

    internal ExplicitExpectationsForIAmExplicit(ExpectationsForITestInterface expectations) =>
        this.expectations = expectations;
    
    internal Rocks.MethodAdornments<IAmExplicit, Action> ExplicitCall() =>
        new Rocks.MethodAdornments<IAmExplicit, Action>(((IExpectations)expectations).Add(
            0, new List<Rocks.Argument>()));
}
```

Then, the custom expectations would expose a property like this:

```csharp
internal ExplicitExpectationsForIAmExplicit IAmExplicit
{
    get => new(this);
}    
```

This may lead to a naming conflict, but, the chances are small, and then it would allow this:

```csharp
var expectations = Rock.Create<ITestInterface>().Expectations();
expectations.NoParameters();
expectations.OneParameter(12);
expectations.GetData.Returns(3);
expectations.IAmExplicit.ExplicitCall();
```

If we are really concerned about a naming conflict, the property could be an extension method, so it would look like this:

```csharp
expectations.IAmExplicit().ExplicitCall();
```

That way, **if** there was a property called `IAmExplicit`, we wouldn't collide. Generally, explicit implementations are not common, but they do happen, and I should be defensive with that.

This would be a **lot** of work to change Rocks. But this has a lot of potential.