# Cannot Mock Obsolete Types
If the given type is marked as being obsolete, a mock cannot be created.
```
[Obsolete]
public sealed class TypeToMock { }

...

// This will generate ROCK2
var rock = Rock.Create<TypeToMock>();
```