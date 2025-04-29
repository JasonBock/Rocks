using Rocks.Runtime;
using Rocks.Scenarios;

[assembly: Rock(typeof(AnalyzeSealedType), BuildType.Create)]

namespace Rocks.Scenarios;

public sealed class AnalyzeSealedType { }