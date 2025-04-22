namespace Rocks.Analysis;

internal enum MethodMatch
{
   Exact,
   DifferByReturnTypeOrConstraintOnly,
   None,
}