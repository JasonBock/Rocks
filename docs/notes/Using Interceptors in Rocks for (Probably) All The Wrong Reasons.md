# Using Interceptors in Rocks for (Probably) All The Wrong Reasons

In a future C# version, there will be a feature called interceptors (maybe, unless it gets pulled). Details about it can be found [here](https://github.com/dotnet/csharplang/issues/7009) and [here](https://github.com/dotnet/roslyn/blob/main/docs/features/interceptors.md#interceptslocationattribute). It's currently an experimental feature, and there is no guarantee that it'll ever ship. In the interim, let's see if it might be useful in Rocks.

Currently, you can only mock instance members that are virtual or abstract:

```csharp
public class Target
{
    public virtual void Mockable() { }

    public void NotMockable() { }
}

var expectations = Rock.Create<Target>();
expectations.Methods().Mockable();

// You can't do this right now,
// as Rocks has no way to "override"
// a non-virtual method.
//expectations.Methods().NotMockable();

var mock = expectations.Instance();
mock.Mockable();
// Calling this has no expectations:
mock.NotMockable();

expectations.Verify();
```

With interceptors, it **might** be possible to provide a hook for `NotMockable()` (note that right now, only methods can be intercepted):

```csharp
internal partial static class Interceptors
{
    [InterceptsLocation(@"C:\SomeDirectory\SomeTestClass.cs", line: 24, column: 6)]
    public static void NotMockable(this Target self) =>  /* now what?? */;
}
```

Rocks would have to look for any non-virtual, instance method invocation on a mock to determine if it needs interception. If so, it would generate a partial class with a unique name with a method that would wire up the interception. One way it can do this is to see, at the call site, if the `IMethodSymbol` is on a type that implements `IMock` (more on this new interface later on). I'm not entirely sure I can do this in Rocks' incremental generator - I'll have to do some work to see if I can determine that through symbols. I think I can, and then finding the location and the file should hopefully be straightforward. Note that since the calls to the non-virtual methods on a mock would happen in the project (typically a test project), finding this invocation location information is doable.

To see if an invocation is done on a type that implements `IMock`, I may need to make the mock type accessible. That means it'll no longer be a `private` nested type; it'll be an `internal` type. I may keep it nested, because I'd like to keep the constructor `private`. But, if I make the mock type accessible, then I should be able to determine that the type making a non-virtual invocation implements `IMock`, though, again, I'll need to test this out to make sure this will work before I continue on with this general idea.

The problem is that, once the interceptor is fired, what do we do then? The intent would be to do something like this:

```csharp
internal partial static class Interceptors
{
    [InterceptsLocation(@"C:\SomeDirectory\SomeTestClass.cs", line: 24, column: 6)]
    public static void NotMockable(this Target @self)
    {
        if (@self.handlers.TryGetValue(3, out var @methodHandlers))
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
            @self.NotMockable();
        }
    }
}
```

In other words, we'd want to move the mock validation inside this interception method. Note that if an expectation wasn't set, we'd simply fall back to the "normal" `NotMockable()` invocation. Since that one isn't tracked with an interceptor, it'll execute as it typically would.

With this in place, and, assuming I added in the appropriate handler and adornment machinery for `NotMockable()`, I can do this:

```csharp
var expectations = Rock.Create<Target>();
expectations.Methods().Mockable();
expectations.Methods().NotMockable();

var mock = expectations.Instance();
mock.Mockable();
mock.NotMockable();

expectations.Verify();
```

A more complex example with a method that takes a parameters and returns a value would look like this:

```csharp
public int GetLength(string value) => value.Length;

internal static class Interceptors
{
    [InterceptsLocation(@"C:\SomeDirectory\SomeTestClass.cs", line: 24, column: 6)]
    public static int GetLength(this Target @self, string @value)
    {
        if (@self.handlers.TryGetValue(4, out var @methodHandlers))
        {
            foreach (var @methodHandler in @methodHandlers)
            {
                if (((global::Rocks.Argument<string>)@methodHandler.Expectations[0]).IsValid(@value))
                {
                    @methodHandler.IncrementCallCount();
                    var @result = @methodHandler.Method is not null ?
                        ((global::System.Func<string, int>)@methodHandler.Method)(@value) :
                        ((global::Rocks.HandlerInformation<bool>)@methodHandler).ReturnValue;
                    return @result!;
                }
            }
            
            throw new global::Rocks.Exceptions.ExpectationException("No handlers match for int GetLength(string @value)");
        }
        else
        {
            return @self.GetLength(@value);
        }
    }
}
```

The only problem is getting a reference to the `private` field, `handlers`. This is typed as `Dictionary<int, List<HandlerInformation>>`. One thought is to create an `IMock` interface in Rocks that every mock would explicitly implement:

```csharp
// Put all the attributes and comments on this to try
// and "warn" users not to use this interface or the
// `Handlers` property.
public interface IMock
{
    Dictionary<int, List<HandlerInformation>> Handlers { get; }
}

private sealed class RockTarget
    : Target, IMock
{
    public RockTarget(Dictionary<int, List<HandlerInformation>> handlers)
    {
        this.Handlers = handlers;
    }

    Dictionary<int, List<HandlerInformation>> IMock.Handlers { get; }
}
```

 Then all the interceptor would need to do is cast to `IMock`:

```csharp
if (((IMock)@self).Handlers.TryGetValue(4, out var @methodHandlers))
```

If I do this, I should probably make the type an `ImmutableDictionary<int, ImmutableList<HandlerInformation>>`, just in case someone would try to do weird things with the information in the collection.

One other interesting aspect to interceptors: you can intercept static calls. Let's say I added this static factory method to `Person`:

```csharp
public static Person Create(string name, uint age) => new(name, age);
```

I use it to create a new `Person`:

```csharp
var createPerson = Person.Create("Joe", 30);
Console.WriteLine(createPerson.Compose());
```

I can intercept this:

```csharp
[InterceptsLocation(@"C:\SomeDirectory\SomeTestClass.cs", line: 8, column: 27)]
public static Person Create(string name, uint age) => new("Intercepted", 21);
```

Rocks has never had any support for mocking statics (note that static abstract members in interfaces (SAMIs) is something that can't be done in Rocks because you can't pass an interface with SAMs to a type parameter). So...I **could** consider adding static methods now in Rocks, though I'm not sure how that would happen. For example:

```csharp
var expectations = Rock.Create<Target>();
expectations.Methods().Create();

var mock = expectations.Instance();
// How do I know this should be intercepted?
Person.Create();

expectations.Verify();
```

Since `Create()` is defined on `Person`, I can't check at the call site that it implements `IMock`. How do I know for certain that this invocation should be intercepted? I'm guessing I would need some kind of "scope" to do this. I have considered having `Expectations<T>` implement `IDisposable` ([issue #224](https://github.com/JasonBock/Rocks/issues/224)), so that might have some use here. But...I'm also considering changing the creation of an expectations object (notes are [here](https://github.com/JasonBock/Rocks/blob/main/docs/notes/Creating%20a%20Custom%20Expectations%20Object%20to%20Simplify%20Generated%20Code.md)). That may not work well with setting expectations on static methods. If I wanted to use interceptors now, I would probably defer handling statics until I had a design in place that I'm comfortable with.

One interesting consequence if I use interceptors in Rocks is, would it be able to mock sealed and struct types? Here's an example:

```csharp
public struct Pairs
{
    public Pairs(int x, int y) =>
        (this.X, this.Y) = (x, y);

    public int Calculate() => this.X + this.Y;
    
    public int X { get; }
    public int Y { get; }
}
```

`Pairs` has one method that could be intercepted. Assuming that I relax the checks in Rocks to allow sealed and struct types to be mocked, I could do this:

```csharp
var expectations = Rock.Create<Pairs>();
expectations.Methods().Calculate().Returns(42);

var mock = expectations.Instance(3, 4);
Assert.That(mock.Calculate(), Is.EqualTo(42));

expectations.Verify();
```

The `Instance()` methods would just return an instance of the given type to mock. In this case, it would be `Pairs`. The trick would be that I can't look at the method invocation on `mock.Calculate()` to see if it's done on something that was **intended** to be mocked. This is somewhat similar to the issue with a static method, in that I need some kind of "scope" to determine which calls should be intercepted. If there was a way to say, "if the instance that `Calculate()` is being called on came from an invocation of an `Instance()` method, then put in an interceptor". However, the other, bigger problem with this is that, I don't have any way to reference the handlers if the return value from `Instance()` does not implement `IMock`.

One possible way to handle this is to use data flow analysis to determine if a variable within a `using` block created by `Rock.Create<>().Expectations()` that was instantiated with an `Instance(...)` call has any method invocations (or maybe just the invocations on a particular variable, though doing reassignments and having a private helper method to create the mock may complicate things). This may be complicated to do, but there is data flow analysis in Roslyn:

* [Writing dataflow analysis based analyzers](https://github.com/dotnet/roslyn-analyzers/blob/main/docs/Writing%20dataflow%20analysis%20based%20analyzers.md)
* [Learn Roslyn Now: Part 8 Data Flow Analysis](https://joshvarty.com/2015/02/05/learn-roslyn-now-part-8-data-flow-analysis/)

I haven't worked with this API before, so I'll need to do some POCs to see how this works. Even if it does, I'll need to create some kind of static `Dictionary<object, handler-dictionary>` property off of `Rock` (or somewhere else). When `Instance(...)` is called, it squirrels away the returned instance along with its set of expectations into this dictionary. Any method invocations on this instance will be intercepted, and the interception code would look for the handlers within this static dictionary:

```csharp
public int GetLength(string value) => value.Length;

internal static class Interceptors
{
    [InterceptsLocation(@"C:\SomeDirectory\SomeTestClass.cs", line: 24, column: 6)]
    public static int GetLength(this Target @self, string @value)
    {
        if (Rock.Handlers[@self].TryGetValue(4, out var @methodHandlers))
        {
            foreach (var @methodHandler in @methodHandlers)
            {
                if (((global::Rocks.Argument<string>)@methodHandler.Expectations[0]).IsValid(@value))
                {
                    @methodHandler.IncrementCallCount();
                    var @result = @methodHandler.Method is not null ?
                        ((global::System.Func<string, int>)@methodHandler.Method)(@value) :
                        ((global::Rocks.HandlerInformation<bool>)@methodHandler).ReturnValue;
                    return @result!;
                }
            }
            
            throw new global::Rocks.Exceptions.ExpectationException("No handlers match for int GetLength(string @value)");
        }
        else
        {
            return @self.GetLength(@value);
        }
    }
}
```

Note that the handlers are now retrieved from `Rock.Handlers[@self]`.

Once `Dispose()`, or, more precisely, `Verify()` is called, it'll remove the dictionary entry. This isn't ideal and I don't like having a global, static location for this stuff, but this **might** let me capture invocations on an instance from a sealed type. And I wouldn't do this for types that are not sealed; they would use the handlers within the mock as it currently works.

Overall, I think interceptors provide a novel way to handle members in Rocks that couldn't be addressed before. In fact, it may be possible to do **everything** with interceptors instead of generating a custom mock type. I'm not sure I think that's the right thing to do, though it would be interesting to see if performance would be slightly better with using interceptors everywhere.