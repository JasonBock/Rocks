# Type Has No Mockable Members
If the given type has no members that can be mocked, a mock cannot be created.
```
public interface ITypeToMock { }

...

// This will generate ROCK3
var rock = Rock.Create<ITypeToMock>();
```