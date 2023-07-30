# Duplicate Constructors
If a mock would end up having duplicate constructors, it can't be mocked.
```csharp
public class AnyOf<T1, T2>
{
  public AnyOf(T1 value) { }

  public AnyOf(T2 value) { }

  public virtual object GetValue() => new();			
}

// This will not generate ROCK12
var validExpectations = Rock.Create<AnyOf<string, int>>();

// This will generate ROCK12
var invalidExpectations = Rock.Create<AnyOf<string, string>>();
```