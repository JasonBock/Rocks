namespace Rocks;

internal enum MethodMatch
{
   Exact,
   DifferByReturnTypeOrConstraintOnly,
   None,
}