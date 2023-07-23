# Member Is Obsolete
If the type to mock has any mockable members that are marked as obsolete such that it's considered an error, the type cannot be mocked.
```csharp
public interface IUseObsolete
{
  [Obsolete("Don't use", error: true)]
  void Foo();
} 

// This will generate ROCK10
var expectations = Rock.Create<IUseObsolete>();
```