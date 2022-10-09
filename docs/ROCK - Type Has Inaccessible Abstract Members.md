# Type Has Inaccessible Abstract Members
If an interface has any abstract members that are not inaccessible (e.g. an `internal` method and the type is defined in another assembly), it cannot be mocked.
```csharp
// Assume InternalTargets is defined in another assembly.
public abstract class InternalTargets
{
  public abstract void VisibleWork();
  internal abstract void Work();
}

// This will generate ROCK8
var expectations = Rock.Create<InternalTargets>();
```