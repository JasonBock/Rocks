using Rocks.Analysis.Models;

namespace Rocks.Analysis.Builders.Create;

internal sealed record AdornmentsPipeline(string FullyQualifiedName, string TypeArguments, string Constraints, MethodModel Method, uint MemberIdentifier);