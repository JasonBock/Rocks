# Cannot Mock Special Types
Certain types, like enums, delegates, and value types, cannot be mocked.
```csharp
public enum Values { One, Two, Three }

// This will generate ROCK6
var expectations = Rock.Create<Values>();
```