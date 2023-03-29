namespace Rocks.Exceptions;

/// <summary>
/// Thrown when a mocked method has the <c>System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute</c> on it
/// to prevent the method from returning.
/// </summary>
[Serializable]
public sealed class DoesNotReturnException
	: Exception
{ }