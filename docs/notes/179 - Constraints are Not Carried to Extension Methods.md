OK, so...to be clear, right now no constraints are going to the extension methods. In this case:

```csharp
public interface IMethodConstraints
{
	void Foo<T>() where T : class;
}
```

It looks like the constraints do not need to be on the extension method. The gen-d code compiles fine. But it doesn't seem like it would hurt anything either.

If I use the code from the GitHub issue, then I have an issue.

So, I think I should always add the constraints. This would only be for methods extensions. Properties/indexers don't have their own generics.