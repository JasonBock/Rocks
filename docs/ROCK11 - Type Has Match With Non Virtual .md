# Type Has Match With Non Virtual
If the type to mock has any mockable members that match a non-virtual member, and that mockable member is abstract, then the type cannot be mocked.
```csharp
public class TypeNode { }

public interface IIRTypeContext { }

public interface ITypeConverter<TType>
  where TType : TypeNode
{
  TypeNode ConvertType<TTypeContext>(TTypeContext typeContext, TType type)
    where TTypeContext : IIRTypeContext;
}

public abstract class TypeConverter<TType> : ITypeConverter<TypeNode>
  where TType : TypeNode
{
  protected abstract TypeNode ConvertType<TTypeContext>(
    TTypeContext typeContext,
    TType type)
    where TTypeContext : IIRTypeContext;

  public TypeNode ConvertType<TTypeContext>(
    TTypeContext typeContext,
    TypeNode type)
    where TTypeContext : IIRTypeContext => new();
}

// This will generate ROCK11
[assembly: RockCreate<TypeConverter<TypeNode>>]
```