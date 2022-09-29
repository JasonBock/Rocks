

Where do I need FQNs?

* DONE - Miscellaneous Code Inclusions
	* DONE - Unsafe.As
	* DONE - [MemberIdentifier
	* DONE - nameof(..)
	* DONE - ArgumentNullException.ThrowIfNull
	* DONE - ValidationState
* Create
	* DONE - Methods
		* DONE - Parameters
		* DONE - Return value (if not void)
		* DONE - Casting in body
		* DONE - Expectation extension methods
		* DONE - Explicit implementation type names
	* DONE - Properties
		* DONE - Property type
		* DONE - Casting in body
		* DONE - Expectation extension methods
		* DONE - Explicit implementation type names
	* DONE - Indexers
		* DONE - Property type
		* DONE - All the indexer parameters
		* DONE - Casting in body
		* DONE - Expectation extension methods
		* DONE - Explicit implementation type names
	* DONE - Constructors
		* DONE - Parameters
		* DONE - Extensions
	* DONE - Constraints
		* DONE - GetConstraints()
	* Attributes
		* DONE - GetAttributes() extensions
		* To be consistent, make "global::Rocks.MemberIdentifier" be "global::Rocks.MemberIdentifierAttribute"
	* DONE - Mock
		* DONE - Inherited types
	* DONE - Rocks type usage
		* DONE - MemberIdentifier
		* DONE - Exceptions
	* DONE - Events
		* DONE - Not entirely sure
	* Projected Types
		* Not sure, but need to visit this.
	* DONE - Well-Known Names
		* DONE - Maybe I should just ditch these
* DONE - Makes
	* DONE - Whatever needs to be done
	
For projected types, we need to do the full projected type "name". That means something like this:

```csharp
var projectionsNamespace = $"ProjectionsFor{information.TypeToMock!.FlattenedName}";
var projectedTypeName = $"global::{{this.information.TypeToMock!.Type.ContainingNamespace!.ToDisplayString()}.{projectionsNamespace}";
```

* DONE - `RefLikeArgTypeBuilder`
  * DONE - `GetProjectedName()` - needs `GetProjectedFullyQualifiedName()`
  * DONE - `GetProjectedEvaluationDelegateName()` - needs `GetProjectedEvaluationDelegateFullyQualifiedName()`
* DONE - `MockProjectedDelegateBuilder`  
  * DONE - `GetProjectedCallbackDelegateName()` - needs `GetProjectedCallbackDelegateFullyQualifiedName()`
  * DONE - `GetProjectedReturnValueDelegateName()` - needs `GetProjectedReturnValueDelegateFullyQualifiedName()`
* DONE - `PointerArgTypeBuilder`
  * DONE - `GetProjectedName()` - needs `GetProjectedFullyQualifiedName()`
  * DONE - `GetProjectedEvaluationDelegateName()` - needs `GetProjectedEvaluationDelegateFullyQualifiedName()`
* DONE - `MockProjectedTypesAdornmentsBuilder`
  * DONE - `GetProjectedAdornmentName()` - needs `GetProjectedAdornmentFullyQualifiedName()`
  * DONE - `GetProjectedHandlerInformationName()` - needs `GetProjectedHandlerInformationFullyQualifiedName()`
  * DONE - `GetProjectedAddExtensionMethodName()` - needs `GetProjectedAddExtensionMethodFullyQualifiedName()`
  
Look for where `GetProjectedAdornmentName()` is called, and change it to `GetProjectedAdornmentFullyQualifiedName()`

Once these are in and tested, then change in code where a FQN is needed for these projected types.

* Methods
  * MockMethodValueBuilder
  * MockMethodVoidBuilder
* Properties
  * ?
* Indexers
  * ?
  
So....because of extension methods, I think I need to include a couple:

* using System.Collections.Generic; // Because I use an AddOrUpdate extension method.
* using {mockType.Namespace}.ProjectionsFor{mockTypeName}; // I create extension methods for projected types, and they're in here, but I only need to do this if the projected types are made.

I think this should be OK.  

Finally, success!

Other Issues
* DONE - The types and members generated in `PointerArgTypeBuilder` and `RefLikeArgTypeBuilder` can probably be `internal`, not `public`
* `ConstructorProperties` can be `internal`, not `public`
* Invoking an event seems reflection-heavy, can that be simplified?