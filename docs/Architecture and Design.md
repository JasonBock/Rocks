# Architecture and Design

The purpose of this document is to describe how Rocks work. I'll go into the details, from start to finish. The inner workings of Rocks is fairly complicated, and having this available may be beneficial to others. I won't cover **every** type, nor is the intention to explain implementations (the code can be perused to do that), but I will dive into those that are core parts of the pipeline.

## The Base: The Solution

The only thing to mention here is that there are a number of configuration files that enforce standards across all of the projects. This includes:

* `.editorconfig` - Sets coding conventions 
* `Directory.Build.props` - Defines values that will be used in all projects, such as `TreatWarningsAsErrors` (which is set to `true` as it **always** should be!)
* `Directory.Packages.props` - Rocks uses central package management, so this is where all the package versions are stored

## The Start: The `Rocks.Analysis` Project

Rocks starts with the `Rocks.Analysis` project. This is a .NET Standard 2.0-based project that references needed `Microsoft.CodeAnalysis` NuGet packages for source generators. Let's cover the specific types involved in generating a new mock at compile-time.

### The Targets

There are two attributes that Rocks is looking for: `RockAttribute` and `RockPartialAttribute`. These are defined in the `Rocks` library. These attributes specify which "kind" of mock types to generate: creates and/or makes. `RockContext` allows a user to group a number of mocks together and verify all their expectations within a disposable object. The main point is that the generator in Rocks is looking for these attributes to trigger source code generation.

### The Generators

The generator, `RockGenerator`, is the source generator that uses `ForAttributeWithMetadataName()` to kick off code generation. For each target that was found, it is sent to `MockModel.Create()` first to do some validation checks, and if those pass, it is sent to a builder, which will be discussed in the next section.

`MockModel.Create()` will look for a number of scenarios to ensure a mock can be made, such as:

* The type cannot be sealed
* The type cannot have open generics
* The type cannot be obsolete
* The type has to have at least one mockable member (i.e. one accessible virtual method or property)
* If the type is a class, the class must have an accessible constructor

Diagnostics are created via the types in the `Rocks.Descriptors` namespace. These diagnostics will show up in the build process so the user will get an indication of why a mock wasn't created. The `HelpUrlBuilder` type is used to create a URL that will route to .md files in the `docs` directory, and this URL is included in the diagnostic.

Note that Rocks also has a `RockAnalyzer` type that will look for `RockAttribute` and `RockPartialAttribute` definitions and look for the same diagnostics, except with the analyzer, these will be flagged as errors.

### The Builders

The types in `Rocks.Builders` are reponsible for creating the parts of a mock. Typically, the starting point will be `RockCreateBuilder`. An `IndentedTextWriter` is used to help make the generated code a little easier to read with indention.

`MockBuilder` is the main starting point to creating a mock. It does this in the following (broad) steps:

* It creates the "setups" - that is, the members the user calls to specify that they expect the member on the mock to be called in a certain way
* It creates handler types to track expectations that were set
* It builds the mock via `MockTypeBuilder`
* It creates adornments for each mockable member

All of the expectations are driven from the types in `Rocks.Expectations`. The `Verify()` method is used by a user in a test to verify all of the expectations gathered during the setup phase.

## The Next Step: Testing via The `Rocks.Analysis.Tests` and `Rocks.Analysis.IntegrationTests` Projects

Code should have tests, and source generators are no exception. `Rocks.Analysis.Tests` has tests to cover the generator mechanics in `Rocks.Analysis`. This is the first line of defense in that this test project will respond to changes that have been made in `Rocks.Analysis` because it references that project normally. However, because of that, it doesn't actually get the source generator experience.

This is what `Rocks.Analysis.IntegrationTests` is for. It references the `Rocks.Analysis` project with `OutputItemType="Analyzer"`. This has the advantage that source generation will occur, so tests are written in a way to verify that the generation works as expected. In older versions of Visual Studio, it didn't have great support in refreshing generated code trigged on changes in the source generator implementation itself, but in recent versions of Visual Studio 2022 and in 2026, this experience has been greatly improved such that you don't have to "shut down and restart VS" every time you change the source generator.

## The Refactoring Step: `Rocks.Completions`

There's another library called `Rocks.Completions` that contains a refactoring that can make it easier to add either a `[Rock]` or `[RockPartial]` attribute in code. The `Rocks.Completions.Tests` project contains the tests for this refactoring.

## The Final Step: The `Rocks.Package` Project

The last step is building the NuGet package itself. That's what `Rocks.Package` is for. It bundles the `Rocks`, `Rocks.Analysis`, and `Rocks.Completions` into one package file. This is what is uploaded to NuGet when a new version is ready.