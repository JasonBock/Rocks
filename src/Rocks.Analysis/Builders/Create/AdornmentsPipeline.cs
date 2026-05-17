using Rocks.Analysis.Models;

namespace Rocks.Analysis.Builders.Create;

internal sealed record AdornmentsPipeline(string Name, string BaseTypeName, string TypeArguments, string Constraints, MethodModel Method, uint MemberIdentifier);