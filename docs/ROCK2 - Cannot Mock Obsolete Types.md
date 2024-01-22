# Cannot Mock Obsolete Types
If the given type is marked as being obsolete, a mock cannot be created.
```csharp
[Obsolete]
public sealed class TypeToMock { }

// This will generate ROCK2
[assembly: RockCreate<TypeToMock>]
```