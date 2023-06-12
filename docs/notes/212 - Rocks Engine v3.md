So, first things first. Remove references to these projects:

* Rocks.IntegrationTests
* Rocks.PackageHost

Make a directory, "v3", that will have all the new work. As I make progress, make a "v3" directory in Rocks.Tests, and make/move tests there.

There will be some things I want to keep in Rocks proper. No need to move them to "v3". 

Or, maybe not? Just make RockV3CreateGenerator. That may be a lot easier.

ContainingAssemblyOfInvocationSymbol => compilation.Assembly. Will need to have every "thing" have a "CanBeSeenByCompilationAssembly"



Tasks

DONE - `TypeModel mockType` is inconsistent. Should I always name it `mockType`, or `type`? Or change the name of the model to `MockTypeModel`?


DONE - Consider having a ContainingType for each model - MethodModel, PropertyModel, EventModel


DONE - Address all TODOs


DONE - Get the Rock.Make() stuff working


DONE - Not sure that AttributeModel is necessary.


Consider using NotNullIfNotNullAttribute for models that have boolean properties that specify if other properties will or will not be null. Not sure that will work though.


DONE - I think the shim generation is causing a stack overflow. I create new shims based on the types needed for shims...which detect that shims need to be created...which, etc. etc. I think when I create a MockModel I need to state "do not create shims" in that case. 


I really need to sort all of the arrays (except method parameters) so equatable works...though this is going to mess up all of the generator tests.


Remove all the .ConfigureAwait(false), and disable that in the projects. No need for it.


It may be worth having a dictionary of models when I'm building a MockTypeModel. That way, I'm not calculating a TypeReferenceModel for every string or int I run into. I mention this in Episode 464 (around the 3:00 mark)


DONE - Need tests for event generation, especially when it's for explicit implementation. Use this for definition:

```
public interface IExplicitInterfaceImplementationOne
{
	void A();
	event EventHandler C;
}

public interface IExplicitInterfaceImplementationTwo
{
	void A();
	event EventHandler C;
}

public interface IExplicitInterfaceImplementation
	: IExplicitInterfaceImplementationOne, IExplicitInterfaceImplementationTwo
{ }
```

DONE - Write a test that sees if multiple documents are created if there are more than one Rock.Create<IType>() in play.


DONE - Problems:
* DONE - Need to ensure all tests are using the V3 versions for generation (this is what uncovered the next two errors I believe)
* DONE - In MockMethodValueBuilderV3 for makes, I'm doing "method.ReturnType as INamedTypeSymbol", but that's completely wrong
* DONE - DelegateInvokeMethod on TypeReferenceModel is IMethodSymbol, and that's wrong

Model tests
* DONE - Need to do MockModelTests like I have MockInformationTests
* Tests for the remaining models

Do I look at generic constraints, and look at types used in the constraints to see if they have any obsolete members? Do I need to? E.g. ComputeSharp, D2D1TransformMapper<>

Reminders:
* Remove Unsafe.As<>(). Probably no good reason to have it. Look for "global::System.Runtime.CompilerServices.Unsafe.As". Will cause some churn, but...probably for the best.
* Use WriteLines() wherever possible. Examples
    * ShimBuilderV3.Build()
* DONE - The compilation should only be in the models, not the builders.
* XML comments
  * Update all XML doc elements in Rocks
  * Consider generating XML comments (may end up being useful)
* On all the models, ensure all properties are EquatableArray<>, NOT ImmutableArray<>
* Remove unnecessary usings everywhere
* ConfigurationValues should go away
* Add these projects back in:
    * DONE - Rocks.IntegrationTests
    * DONE - Rocks.CodeGenerationTests
    * Rocks.Performance
    * Rocks.PerformanceTests