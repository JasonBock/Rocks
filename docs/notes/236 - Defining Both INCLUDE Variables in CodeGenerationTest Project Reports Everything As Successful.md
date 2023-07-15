error CS8785: Generator 'RockCreateGenerator' failed to generate source. It will not contribute to the output and compilation errors may occur as a result. Exception was of type 'ArgumentException' with message 'The hintName 'Arn_Rock_Create.g.cs' of the added source file must be unique within a generator. (Parameter 'hintName')'

using Rocks;
using System;

[assembly: CLSCompliant(false)]

public static class GenerateCode
{
	public static void Go()
	{
		var r0 = Rock.Create<FluentAssertions.Equivalency.Node>();
		var r1 = Rock.Create<Microsoft.CodeAnalysis.Operations.IRecursivePatternOperation>();
		var r2 = Rock.Create<MassTransit.Contracts.JobService.JobSubmissionAccepted>();
        // ...
		var r4007 = Rock.Create<Microsoft.CodeAnalysis.Operations.ILocalReferenceOperation>();
		var r4008 = Rock.Create<MassTransit.Internals.GraphValidation.DependencyGraph<object>>();
		var r4009 = Rock.Create<System.Reflection.PortableExecutable.ResourceSectionBuilder>();
	}
}

I think what's happening is that when all of them are run, the SG gets that file duplication (hint name) error, and then it just compiles this simple code, so it looks fine. I think once I address the unique name issue...the results here should be interesting.

Side thing: I should put in a list of NuGet packages I've tried, and there were no targets, so I don't try to do that again the future (or at least I know the last time I tried it, I didn't find anything)

Try to add these:
Sigil
Wolverine