# Interface Has Static Abstract Members
If an interface has any static abstract members, it cannot be mocked (see [this issue](https://github.com/dotnet/csharplang/issues/5955
) for details).
```csharp
public interface IHaveStaticAbstractMembers
{
  static abstract void Foo();
}

// This will generate ROCK7
var expectations = Rock.Create<IHaveStaticAbstractMembers>();
```