namespace Rocks.Exceptions;

/// <summary>
/// Thrown when a mocked method has the <see cref="System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute"/> on it
/// to prevent the method from returning.
/// </summary>
[Serializable]
public sealed class DoesNotReturnException
	: Exception
{ }