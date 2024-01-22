# Type Has No Accessible Constructors
If the given type is a class that has no accessible constructors, a mock cannot be created.
```csharp
public class TypeToMock 
{ 
  private TypeToMock()
    : base() { }
		
  public virtual void Foo() { }
}

// This will generate ROCK4
[assembly: RockCreate<TypeToMock>]
```