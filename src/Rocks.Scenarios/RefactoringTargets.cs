namespace Rocks.Scenarios;

public abstract class BaseType
{
	protected BaseType() { }
}

public class DerivedType
	: BaseType
{
}

public class PropertyType { }

public class ConsumingType
{
   public virtual void DoStuff(PropertyType type)
	{
		var x = Guid.NewGuid();
		var y = x.ToString() + this.ToString();
	}

   public required PropertyType MyProperty { get; set; }
}