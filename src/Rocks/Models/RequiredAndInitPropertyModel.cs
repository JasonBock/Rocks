using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

/// <summary>
/// Defines a property that can be mocked.
/// </summary>
internal record RequiredAndInitPropertyModel
{
	/// <summary>
	/// Creates a new <see cref="RequiredAndInitPropertyModel"/> instance.
	/// </summary>
	/// <param name="value">The <see cref="IPropertySymbol"/> to obtain information from.</param>
	internal RequiredAndInitPropertyModel(IPropertySymbol value) 
	{
		this.IsRequired = value.IsRequired;
	}

   internal bool IsRequired { get; }
}
