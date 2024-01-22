# Cannot Mock Sealed Types
If the given type is sealed, a mock cannot be created.
```csharp
public sealed class TypeToMock { }

// This will generate ROCK1
[assembly: RockCreate<TypeToMock>]
```