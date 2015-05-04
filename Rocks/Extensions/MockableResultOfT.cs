using System.Reflection;

namespace Rocks.Extensions
{
	internal class MockableResult<T>
		where T : MemberInfo
	{
		internal MockableResult(T value, RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation)
		{
			this.Value = value;
			this.RequiresExplicitInterfaceImplementation = requiresExplicitInterfaceImplementation;
		}

		internal RequiresExplicitInterfaceImplementation RequiresExplicitInterfaceImplementation { get; }
      internal T Value { get; }
	}
}
