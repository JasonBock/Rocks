# Type Has No Mockable Members
If the given type has no members that can be mocked, a mock cannot be created.
```csharp
public interface ITypeToMock { }

// This will generate ROCK3
var expectations = Rock.Create<ITypeToMock>();
```