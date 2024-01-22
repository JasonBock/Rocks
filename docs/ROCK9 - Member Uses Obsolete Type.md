# Member Uses Obsolete Type
If the type to mock uses any types that are marked as obsolete such that it's considered an error, the type cannot be mocked.
```csharp
[Obsolete("Don't use", error: true)]
public class DoNotUse { }

public interface IUseObsolete
{
  void Foo(DoNotUse doNotUse);
} 

// This will generate ROCK9
[assembly: RockCreate<IUseObsolete>]
```