using Microsoft.CodeAnalysis;

namespace Rocks.Extensions
{
	internal static class IPropertySymbolExtensions
	{
		internal static PropertyAccessor GetAccessors(this IPropertySymbol self) =>
			self.GetMethod is not null && self.SetMethod is not null ?
				PropertyAccessor.GetAndSet : (self.SetMethod is null ? PropertyAccessor.Get : PropertyAccessor.Set);
	}
}