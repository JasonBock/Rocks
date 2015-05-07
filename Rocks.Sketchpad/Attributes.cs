using System;
using System.Linq;
using System.Runtime.InteropServices;

public static class Attributes
{
	public static void Test()
	{
		var attributeUsageData = typeof(MyReturnValueAttribute).GetCustomAttributesData()[0];
		var name = attributeUsageData.AttributeType.Name.Replace("Attribute", string.Empty);
		var constructorArguments = string.Join(", ", attributeUsageData.ConstructorArguments.Select(_ => $"({_.ArgumentType.Name}){_.Value}").ToArray());
		var namedArguments = string.Join(", ", attributeUsageData.NamedArguments.Select(_ => $"{_.MemberName} = {_.TypedValue.Value}").ToArray());
		var arguments = !string.IsNullOrWhiteSpace(constructorArguments) && !string.IsNullOrWhiteSpace(namedArguments) ?
			$"{constructorArguments}, {namedArguments}" :
			!string.IsNullOrWhiteSpace(constructorArguments) ? constructorArguments : namedArguments;
		var attribute = $"{name}({arguments})";
		Console.Out.WriteLine(attribute);

		var x = typeof(IMy).GetMethod("Target");

		foreach(var y in x.GetParameters())
		{
			Console.Out.WriteLine(y.GetCustomAttributesData());
		}
   }
}

public sealed class TestAttribute : Attribute
{
	public TestAttribute(Guid targetGuid)
	{
		this.TargetGuid = targetGuid;
	}

	public TestAttribute(int targetInt)
	{
		this.TargetInt = targetInt;
	}

	public Guid TargetGuid { get; }
	public int TargetInt { get; set; }
}

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class MyParameterAttribute : Attribute { }

[AttributeUsage(AttributeTargets.ReturnValue)]
public sealed class MyReturnValueAttribute : Attribute { }

[AttributeUsage(AttributeTargets.ReturnValue)]
public sealed class MyReturnValue2Attribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public sealed class MyMethodAttribute : Attribute { }

[AttributeUsage(AttributeTargets.GenericParameter)]
public sealed class MyGenericParameterAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property)]
public sealed class MyPropertyAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Event)]
public sealed class MyEventAttribute : Attribute { }

public interface IMy
{
	[MyEvent]
	event EventHandler TargetEvent;

	[MyProperty]
	string TargetProperty { get; set; }

	[MyMethod]
	[return: MyReturnValue]
	[return: MyReturnValue2]
	Guid Target<[MyGenericParameter] T>([MarshalAs(UnmanagedType.Bool), MyParameter, Out] int a);
}

public class OhMy : IMy
{
	public string TargetProperty
	{
		get
		{
			throw new NotImplementedException();
		}

		set
		{
			throw new NotImplementedException();
		}
	}

	public event EventHandler TargetEvent;

	[return: MyReturnValue]
	public Guid Target<[MyGenericParameter]T>([MarshalAs(UnmanagedType.Bool), MyParameter, Out]int a)
	{
		throw new NotImplementedException();
	}
}
