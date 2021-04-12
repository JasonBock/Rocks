# Architecture and Design

The purpose of this document is to describe how Rocks work. I'll go into the details, from start to finish. The inner workings of Rocks is fairly complicated, and having this available may be beneficial to others. I won't cover **every** type, nor is the intention to explain implementations (the code can be perused to do that), but I will dive into those that are core parts of the pipeline.

## The Base: The Solution

The only thing to mention here is that there are a number of configuration files that enforce standards across all of the projects. This includes:

* `.editorconfig` - Sets coding conventions 
* `Directory.Build.props` - Defines values that will be used in all projects, such as `TreatWarningsAsErrors` (which is set to `true` as it **always** should be!)
* `nuget.config` - This defines a local NuGet feed, which is used by `Rocks.NuGetHost` to confirm that a new package works as expected before it's published. If `Rocks.NuGetHost` doesn't build, make sure the solution configuration is set to `Release`. You may need to rebuild a couple of times before `Rocks.NuGetHost` finally realizes the NuGet package is there.

## The Start: The `Rocks` Project

Rocks starts with the `Rocks` project. This is a .NET Standard 2.0-based project that references needed `Microsoft.CodeAnalysis` NuGet packages for source generators. It also references `Microsoft.SourceLink.GitHub` as the project has a number of NuGet-based values to create a package on compilation. Let's cover the specific types involved in generating a new mock at compile-time.

### The Targets

There are two types that have special methods that Rocks is looking for: `Rock` and `RockRepository`. `Rock` defines two static methods, `Create()` and `Make()` that signify if Rocks should make a mock that has the ability to set expectations (`Create()`) or create a mock with nothing but default implementations (`Make()`). `RockRepository` allows a user to group a number of mocks together and verify all their expectations within a disposable object. The main point is that the generators in Rocks are looking for invocations to these methods to trigger source code generation.

Note that from now on, I'll only cover the path where a user types `Rock.Create<>();`. The other two scenarios are similar in terms of what is done.

### The Receivers

Rocks uses receivers (like `RockCreateReceiver`) that implement `ISyntaxContextReceiver`. This allows Rocks to get a reference to the `SyntaxNode` currently being visited as well as the related `SemanticModel`. In `OnVisitSyntaxNode()`, the receiver determines if the node is an `InvocationExpressionSyntax`, and, if so, is the invocation the same as the methods mentioned in the previous section. If so, the type specified in the generic argument of the invocation is paired with the current node in a tuple, and that is stored into the `Targets` list. This will be used by the generator.

### The Generators

The generators (like `RockCreateGenerator`) specify which receiver to use in `Initialize()`. This receiver type is also used in `Execute()` to find the targets that should be processed to create a mock. For each target that was found, it is sent to `MockInformation` first to do some validation checks, and if those pass, it is sent to a builder, which will be discussed in the next section.

`MockInformation` will look for a number of scenarios to ensure a mock can be made, such as:

* The type cannot be sealed
* The type cannot have open generics
* The type cannot be obsolete
* The type has to have at least one mockable member (i.e. one accessible virtual method or property)
* If the type is a class, the class must have an accessible constructor

Diagnostics are created via the types in the `Rocks.Descriptors` namespace. These diagnostics will show up in the build process so the user will get an indication of why a mock wasn't created. The `HelpUrlBuilder` type is used to create a URL that will route to .md files in the `docs` directory, and this URL is included in the diagnostic.

### The Builders

The types in `Rocks.Builders` are reponsible for creating the parts of a mock. Typically, the starting point will be `RockCreateBuilder`. This uses an instance of `NamespaceGatherer` to ensure the right `using` statements are generated uniquely in the mock type file. An `IndentedTextWriter` is used to help make the generated code a little easier to read with indention. Note that if the indentation style is set in configuration, that will be used, otherwise, tab is the default.

`MockBuilder` is the main starting point to creating a mock. It does this in three broad steps:

* It creates any necessary projected types to handle exceptional cases (e.g. creating delegates to support methods with types that can't be used in a `Func` or `Action`) via `MockProjectedTypesBuilder`
* It creates the code necessary to have extension methods for mockable members on a type via `MockMethodExtensionsBuilder`, `MockPropertyExtensionsBuilder`, and `MockConstructorExtensionsBuilder`
* It builds the mock via `MockTypeBuilder`
* It creates expectation handlers for each mockable member via `MethodExpectationsExtensionsBuilder`, `PropertyExpectationsExtensionsBuilder`, and `EventExpectationsExtensionsBuilder`.

All of the expectations are driven from the types in `Rocks.Expectations`. The generated extension methods take as their first argument a type that derives from `Expectations<T>`. This base type is used by all of the other expectation types (e.g. `MethodExpectations<T>`), and all expectations are gathered via an `Expectations<T>` instance returned by `Rock.Create<>()` or `Create<>()` on an instance of `RockRepository`. The `Verify()` method is used by a user in a test to verify all of the expectations gathered during the setup phase.

## The Next Step: Testing via The `Rocks.Tests` and `Rocks.IntegrationTests` Projects

Code should have tests, and source generators are no exception. `Rocks.Tests` has tests to cover the mechanics of receivers and generators in `Rocks`. This is the first line of defense in that this test project will respond to changes that have been made in `Rocks` because it references that project normally. However, because of that, it doesn't actually get the source generator experience.

This is what `Rocks.IntegrationTests` is for. It references the `Rocks` project with `OutputItemType="Analyzer"`. This has the advantage that source generation will occur, so tests are written in a way to verify that the generation works as expected. However, the disadvantge is that, if you're using Visual Studio, once the referenced assembly is loaded, it can't be unloaded. Therefore, any changes made in `Rocks` won't be seen by this assembly. To see those changes, Visual Studio needs to be shut down and started up again. If you run `dotnet test`, you won't have this issue.

## The Final Step: The `Rocks.NuGetHost` Project

The last step is to ensure the NuGet package is good. The `Rocks.NuGetHost` references the `Rocks` NuGet package locally via the path set in `nuget.config`. I typically don't do this until I think I have a final version that I want to publish to nuget.org. Keep in mind that you may need to clear your local cache if you've built `Rocks` numerous times with the same version number (to do this, look at the link in the `nuget.config` file).