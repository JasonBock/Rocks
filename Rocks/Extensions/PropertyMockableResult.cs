using System.Reflection;

namespace Rocks.Extensions
{
	internal sealed class PropertyMockableResult
		: MockableResult<PropertyInfo>
	{
		internal PropertyMockableResult(PropertyInfo value, bool requiresExplicitInterfaceImplementation, PropertyAccessors accessors)
			: base(value, requiresExplicitInterfaceImplementation)
		{
			this.Accessors = accessors;
		}

		internal PropertyAccessors Accessors { get; }
	}
}
