using System.Reflection;

namespace Rocks.Extensions
{
	internal sealed class MockableResult<T>
		where T : MemberInfo
	{
		internal MockableResult(T value, bool requiresExplicitInterfaceImplementation)
		{
			this.Value = value;
			this.RequiresExplicitInterfaceImplementation = requiresExplicitInterfaceImplementation;
		}

		internal bool RequiresExplicitInterfaceImplementation { get; }
      internal T Value { get; }
	}
}
