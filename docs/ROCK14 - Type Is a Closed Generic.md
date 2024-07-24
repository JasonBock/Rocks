# Type Is a Closed Generic
While Rocks has allowed closed generics to mock, now that Rocks supports open generics, it is the better approach. Specifying a closed generic type should be avoided.
```csharp
// This will not generate ROCK14
[assembly: Rock(typeof(IList<>), BuildType.Create)]

// This will generate ROCK14
[assembly: Rock(typeof(IList<string>), BuildType.Create)]
```