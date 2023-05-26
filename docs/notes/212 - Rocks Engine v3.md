So, first things first. Remove references to these projects:

* Rocks.IntegrationTests
* Rocks.PackageHost

Make a directory, "v3", that will have all the new work. As I make progress, make a "v3" directory in Rocks.Tests, and make/move tests there.

There will be some things I want to keep in Rocks proper. No need to move them to "v3". 

Or, maybe not? Just make RockV3CreateGenerator. That may be a lot easier.

ContainingAssemblyOfInvocationSymbol => compilation.Assembly. Will need to have every "thing" have a "CanBeSeenByCompilationAssembly"




Remember to add references to these projects and fix any issues:
* Rocks.IntegrationTests
* Rocks.PackageHost