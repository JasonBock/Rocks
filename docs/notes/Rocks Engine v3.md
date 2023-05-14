Rocks Engine v3

When Rocks first started, it generated C# code, but it used Reflection to read the contents of the target type to mock at runtime. Roslyn was barely used, other than to compile the code. This was still easier than trying to build a type using IL, but it wasn't ideal. Call this "version 1" of the "engine".

When source generators were added to C# 9, I changed how Rocks worked such that the code was generated at compile-time. Call this "version 2".

With "version 3", the idea is to keep Rocks as a source generator, but look at how to optimize how the generation is done. Primarily, I feel like I do way too many "visits" of items from the syntax tree and semantic model. What I'm thinking of doing is having my own view on top of Roslyn, something like this:

MockedType
	MockedMembers
		MockedMethods
			MockedMethod
				MockedParameters
					MockedParameter
				MockedTypedParameters
					MockedTypedParameter
				MockReturnType
				MockAttributes
					MockAttribute
		MockedProperties
			MockedProperty
				MockAttributes
					MockAttribute
		MockedIndexers
			MockedIndexers
				MockAttributes
					MockAttribute
		MockedEvents
			MockedEvent
				MockAttributes
					MockAttribute
		MockedConstructors
			MockedConstructors
				MockAttributes
					MockAttribute

This may not be the exact model I end up with; this is more to show the intent of where I want to go with the new version. When `Rock.Create<>()` or `Rock.Make<>()` is invoked, the generator will still go through `MockInformation`, but what that will do is pass the type to `MockedType` and that will visit all the members **once**. It would also generate the code content needed to make a mock. If this ever ended up finding something that would prevent a mock from being made (e.g. a member that is `internal` and `virtual` or `abstract` and exists in another assembly), it would immediately halt all processing (maybe by throwing an exception that the called to `MockInformation` would be expected to handle).

I really don't know if this would be "better", but I think it's a solid enough idea to try it out.

Other clean-ups:
* Should have `PropertyInitExpectationsOf...Extensions` when it's an `init`


https://www.meziantou.net/testing-roslyn-incremental-source-generators.htm

First thing, get this reproduced, probably as a console application, might be easier.

Then, create my own generator that looks for "Report.Type<SomeType>()", similar to what Rocks does. Then create a data structure from the given symbol/node/whatever like this:

public record ReportedType(uint MethodCount, uint PropertyCount);

For this, I don't care if they're static/instance or public/non-public. Just to get a count.

Then:
* If I add a type somewhere, my generator shouldn't do anything
* If I add a call to "Report.Type<>()" for a type I've already done before, it should be "cached"
* If I add a call to "Report.Type<>()" for a different type, that should be generated
* If I update any of the types I've reported before...what I would hope is that it should be regenerated.

Side note: Assuming this goes the way I want it to, I should strongly consider adding this works into Rocks as tests to ensure incremental caching.

So, it seems like doing the data model will give us what we need. Now, how do we change this? I definitely want to be able to define the new generator without disturbing the current structure too much.

* I want test(s) that ensure that the current generator is not cache-friendly, and also shows that the one I'm creating is friendly.
* I could make a new folder/namespace called `v3`. Then move everything I need in there, even if it has to copy what I currently have.

Or...

Just change the current generators to use a new strategy. Basically they is to change MockInformation to take equatable lists of MethodData, PropertyData, and EventData. These classes will have what we want.

This probably means we can't really tests our caching hopes until I'm all done, and that's not good.

Or...

Write a test that shows my current generator isn't cache-friendly. Then change the generator to start going with a data model approach, and show that the tests.

Doing the new work in isolation feels like the right thing. Then I can delete all the old stuff, move the new stuff into the root, update namespaces, and...then OK?