namespace Rocks;

/// <summary>
/// Specifies the type of mock to make.
/// </summary>
[Flags]
public enum BuildType
{
	/// <summary>
	/// Specifies a strict mock - that is, one that requires expectations.
	/// </summary>
   Create = 1, 
	/// <summary>
	/// Specifies an opaque mock - that is, one that has no expectations.
	/// </summary>
	Make = 2
}