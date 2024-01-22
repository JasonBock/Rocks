# Type Error
If the given mock type has any type errors, it cannot be mocked.
```csharp
public interface IBase<T1, T2> 
{
  void Foo(T1 a, T2 b);
}

// This will generate ROCK13
[assembly: RockCreate<IBase<,>>]

// This will not generate ROCK13
[assembly: RockCreate<IBase<int, int>>]
```