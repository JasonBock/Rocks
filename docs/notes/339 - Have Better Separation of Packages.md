Projects:

* Rocks.Runtime - This targets .NET 9, and contains types that are used at compile time and run time. The emphasis is on "run time". Ideally, these types should not be needed by the generator.
* Rocks.Runtime.Tests - This targets .NET 9, tests for Rocks.Runtime
* Rocks.Analysis - This targets NS 2.0, and contains the generator, suppressor, analyzer, and diagnostic. It also contains the models and generator types used to create the mock code.
* Rocks.Analysis.Tests - This targets .NET 9, tests for Rocks.Analysis. Should also have a reference to Rocks.Runtime, and a reference to that project when code is generated.
* Rocks.Analysis.IntegrationTests - This targets .NET 9, integration tests for Rocks.CompilerExtensions (basically what Rocks.IntegrationTests are right now)
* Rocks.Completions - This targets NS 2.0, and contains code fixes (which I don't think I have, but they could go here) and refactorings.
* Rocks.Completions.Tests - This targets .NET 9, tests for Rocks.Completions. Should also have a reference to Rocks.Runtime, and a reference to that project when code is generated.
* Rocks - This turns into the metapackage that bundles all of the projects into one package.


* Code creation
    * Should be `using Rocks.Runtime;`
    * What about projections? Instead of `Rocks.Projections`, change to `Rocks.Runtime.Projections`
