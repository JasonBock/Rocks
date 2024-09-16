using Rocks.Models;

namespace Rocks.Builders.Create;

internal sealed record AdornmentsPipeline(string FullyQualifiedName, string TypeArguments, string Constraints, MethodModel Method, uint MemberIdentifier);
