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