Error:

```
 CreateWhenDuplicatesOccurAsync
   Duration: 29.3 sec

  Message: 
      Context: Diagnostics of test state
    Mismatch between number of diagnostics returned, expected "0" actual "1"
    
    Diagnostics:
    // Rocks\Rocks.RockCreateGenerator\IRuntimeKey_Rock_Create.g.cs(136,87): error CS0102: The type 'CreateExpectationsOfIRuntimeKeyExtensions.RockIRuntimeKey.ShimIKey55018818661256234156060084750235742359064106137' already contains a definition for 'Properties'
    DiagnosticResult.CompilerError("CS0102").WithSpan("Rocks\Rocks.RockCreateGenerator\IRuntimeKey_Rock_Create.g.cs", 136, 87, 136, 97).WithArguments("CreateExpectationsOfIRuntimeKeyExtensions.RockIRuntimeKey.ShimIKey55018818661256234156060084750235742359064106137", "Properties"),
```

```csharp
public interface IReadOnlyKey
{
    bool IsPrimaryKey() => true;

    IReadOnlyList<IReadOnlyProperty> Properties { get; }
}

public interface IKey
    : IReadOnlyKey
{
    Type GetKeyType() => Properties.Count > 1 ? typeof(object[]) : Properties.First().ClrType;

    new IReadOnlyList<IProperty> Properties { get; }
}

private sealed class ShimIKey55018818661256234156060084750235742359064106137
  : global::IKey
{
  private readonly RockIRuntimeKey mock;
  
  public ShimIKey55018818661256234156060084750235742359064106137(RockIRuntimeKey @mock) =>
    this.mock = @mock;
  
  public global::System.Collections.Generic.IReadOnlyList<global::IProperty> Properties
  {
    get => ((global::IKey)this.mock).Properties;
  }
  
  public global::System.Collections.Generic.IReadOnlyList<global::IReadOnlyProperty> Properties
  {
    get => ((global::IKey)this.mock).Properties;
  }
}
```

I'm not looking at `RequiresExplicitInterfaceImplementation` on the members when I generate the shims.