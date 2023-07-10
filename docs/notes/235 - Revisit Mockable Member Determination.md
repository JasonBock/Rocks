One idea is to use what is potentially in Roslyn. For example, if you have this interface:

```csharp
public interface ID<TD>
{
	void Whatever<T>() where T : TD;
}
```

You can write this in VS:

```csharp
public class Rock
	: ID<object>
```

And use the "Implement interface" refactoring to create this:

```csharp
public class Rock
	: ID<object>
{
   public void Whatever<T>() => throw new NotImplementedException();
}
```

This won't have the right constraints (which I've already fixed in a bug), but at least it gives me the right member.

It'll also handle explicit implementations correctly. For example:

```csharp
public interface IRequest<T>
  where T : class
{
	Task<T> Send(Guid requestId, object values);
	Task Send(Guid requestId, T message);
}
```

This will create the following definition:

```csharp
public class Rock
	: IRequest<object>
{
	public Task<object> Send(Guid requestId, object values) => throw new NotImplementedException();
	Task IRequest<object>.Send(Guid requestId, object message) => throw new NotImplementedException();
}
```

If I have a class:

```csharp
public class Stuff
{
	public virtual void Foo() { }
}
```

You can pick the "Generate overrides" refactoring, which, if you pick all of them, you get this:

```csharp
public class SubStuff
	: Stuff
{
   public override bool Equals(object? obj) => base.Equals(obj);
   public override void Foo() => base.Foo();
   public override int GetHashCode() => base.GetHashCode();
   public override string? ToString() => base.ToString();
}
```

This will also work if the class is `abstract`.

The issue is...where is this code? I don't think it's in VS proper, as SharpLab seems to use the same functionality (though I don't think it's exactly the same). I found [`AbstractChangeImplementationCodeRefactoringProvider`](https://github.com/dotnet/roslyn/blob/d11caad0ab35ec679716353eead320f92d05e753/src/Features/CSharp/Portable/ImplementInterface/AbstractChangeImplementationCodeRefactoringProvider.cs). Unfortunately, it's `internal`, but maybe I can copy/pasta what I need, or there's another way to get this mapping for classes and interfaces.