# Cannot Specify Type With Open Generic Parameters
If the given type has open generic parameters, a mock cannot be created.
```csharp
public interface IMock<T> { ... }

// This will generate ROCK5
[assembly: RockCreate<IMock<T>>]
```