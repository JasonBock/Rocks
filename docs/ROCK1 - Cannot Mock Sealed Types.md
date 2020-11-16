# Cannot Mock Sealed Types
If the given type is sealed, a mock cannot be created.
```
public sealed class TypeToMock { }

...

// This will generate ROCK1
var rock = Rock.Create<TypeToMock>();
```