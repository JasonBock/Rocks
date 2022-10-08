https://sourceroslyn.io/#Microsoft.CodeAnalysis.CSharp/Symbols/OverriddenOrHiddenMembersResult.cs,22

This would be nice if it was available, but it isn't, so...unfortunately, I have to find a way to compute this myself.

What does `new` do? https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/new-modifier

I think this:

```csharp
var methodToRemove = methods.SingleOrDefault(_ => _.Value.Match(hierarchyMethod) == MethodMatch.Exact &&
	!_.Value.ContainingType.Equals(hierarchyMethod.ContainingType));
```

should change to this:

```csharp
// In other words, the match can be Exact or DifferByReturnTypeOnly, but I don't want
// to change this to the other style of LINQ just to define a `let` in the query.
var methodToRemove = methods.SingleOrDefault(_ => _.Value.Match(hierarchyMethod) != MethodMatch.None &&
	!_.Value.ContainingType.Equals(hierarchyMethod.ContainingType));
```

I think I was assuming that a `new` wouldn't have a "differ by return type only" scenario, which is obviously not the case. Generics aren't even the problem:

```csharp
public class BaseClass
{
    public virtual void Foo() { }   
}

public class SubClass : BaseClass
{
    public new virtual int Foo() => 2;  
}
```

That's perfectly acceptable. In fact, all of this is legit:

```csharp
public class BaseClass
{
    public virtual void Foo() { }   
    
    public virtual string Data { get; set; }
    
    public virtual string this[int a, string b] { get => "2"; }
}

public class SubClass : BaseClass
{
    public new virtual int Foo() => 2;  

    public new virtual int Data { get; set; }
    
    public new virtual int this[int a, string b] { get => 3; }
}
```

And it works on interfaces as well:

```csharp
public interface BaseClass
{
    void Foo();
    
    string Data { get; set; }
	
	string this[int a, string b] { get; }
}

public interface SubClass : BaseClass
{
    new int Foo();

    new int Data { get; set; }

	new int this[int a, string b] { get; }
}
```

I think the rule is, if a method in a subtype "matches", either by being exact or return type only, if the "base" member is an interface, it needs to be explicitly implemented - otherwise, it needs to be ignored.

For interfaces, if the match is Exact, the subtype implementation is fine. If it's None...it's not shadowing. Only with DifferByReturnTypeOnly should it do an explicit implementation. The method scenario is correct, but the properties and indexers aren't.