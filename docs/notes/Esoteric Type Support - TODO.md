DONE - Add a IsEsoteric to ITypeSymbolExtensions


DONE - Need to generate names for pointers, "int*" and "IntPointer"


DONE - Need to generate a delegate for the callback (might be able to do this in BuildDelegates()):

	public unsafe delegate bool intPointerCallback(int* a);


DONE - Need to know if a type member (method, property/indexer) needs to be "unsafe" as the generated unsafe mock members need to be declared as such.


DONE - The generated unsafe mock members (create and make) need to be "unsafe"


DONE - The member description needs the "*" for pointer types (create and make). Also for parameter/return-type


DONE - Need to generate an entire "Arg" class on its own ("Pointer" and "RefLike" (need a better name here) arg types), similar to generating delegates: BuildCustomArgTypes. Name == "ArgFor{parameterType.GetName(...)}. Will need to make ValidationState public. Note that the constructor for validation needs to use the generated delegate. Span-like types cannot keep the value in a field, so the only method available to them will be "Validate()". Also need the validation delegate created.


DONE - "Projected" Types. "ProjectedTypesBuilder" Anything involved from the moment Rock.Create() is called that can "return" a value based on an esoteric type needs to be "projected" such that they are created for the specific esoteric type.

Also, all projection builders must have GetProjected{xyz}Name() methods so it's easy to figure out the name of what was created elsewhere within Rocks, rather than relying on naming conventions (e.g. is it "Of" or "For"?)

What does this involve?

* DONE - HandlerInformation type - Need to create a HandlerInformationFor{type} that is based on HandlerInformation.
* DONE - Expectation classes - They are all based on ExpectationsWrapper<T>, which in and of itself is germane, but...there's an Add<TReturn> extension method that can't be used. Need to create an extension method like this:

public static HandlerInformationFor{type} AddFor{type}(this {adornment}Expectations<T> self, int memberIdentifier, List<Arg> arguments)
{
	var information = new HandlerInformationFor{type}(arguments.ToImmutableArray());
	this.Expectations.Handlers.AddOrUpdate(memberIdentifier,
		() => new List<HandlerInformation> { information }, _ => _.Add(information));
	return information;
}

* DONE - Adornment classes
** public sealed class MethodAdornments<T, TCallback, TResult> => public sealed class MethodAdornmentsFor{name}<T, TCallback>
** public sealed class PropertyAdornments<T, TCallback, TResult> => public sealed class PropertyAdornmentsFor{name}<T, TCallback>
** public sealed class IndexerAdornments<T, TCallback, TResult> => public sealed class IndexerAdornmentsFor{name}<T, TCallback>

They could probably inherit from the "Type<T, TCallback>" types. The only way to "return" a value would be to use Callback(). Trying to generate a base "HandlerInformationFor{name}" to get a return value would be ... well, even if I did it, then I couldn't inherit from it. I'd have to generated the handler type, and then generate something like:

* MethodAdornmentsFor{name}<T, TCallback> : IAdornments<HandlerInformationFor{name}>

Which I believe will work. But again, this is only for members that return values.


DONE - Need to make sure the "Instance()" methods are marked as unsafe if they take pointer types.


Any of the methods/indexers/properties that have parameters with "esoteric" types needs to use projected types in the extension methods, and specify the callback type to the generated delegate value. If it needs to return a value, it needs to use the name of the projected type. Also, the "AddFor{type}" extension method needs to be used for members that return an esoteric type

Need to make sure that the adornment name has "Explicit" for explicit implementations.

* DONE - MethodExpectationsExtensionsBuilder.Build
* DONE - PropertyExpectationsExtensionsBuilder.Build
	* DONE - Properties
	* DONE - Explicit Properties
* DONE - PropertyExpectationsExtensionsBuilder.Build
	* DONE - Properties
	* DONE - Explicit Properties

DONE - Projecting the Arg type needs to include open generics for Span<T> if it exists. Then need to "fill" that hole when necessary.


DONE - The AddFor{type} needs to be in a class. I could do this:

internal partial static class MethodExpectationsOfIHavePointersExtensions

Both for the AddFor{type} method AND where this extension type is being made.


DONE - "ValidationState.Value => a == this.value," - it's not "a", the name of the parameter is "value"


"throw new InvalidEnumArgumentException" - the namespace isn't there


DONE - In ExpectationsWrapperExtensions, "this.Expectations.Handlers.AddOrUpdate", believe it should be "self.Handlers.AddOrUpdate"



DONE - Anything that puts the value into the string for an exception...can't use the value. So, instead of $"..., {a}, ...", it would be $"..., a, ...". Side note, the generated exception string doesn't seem to be completely correct all the time right now, need to review that.



DONE - The HandlerInformationFor{type} class should be unsafe.



DONE - All of the generated and projected types should be "internal"


DONE - MockCreateBuilder.Build
* DONE - The cast to call IsValid() on the parameter needs to be on the projected Arg type.
* DONE - The cast to call the callback needs to be the generated callback delegate


"Rock.Make<IHavePointers>()" - needs to work as well.


Look at putting the static methods on Arg either on Arg<T>, or another type (Param?, Argument?). Then projected types could make their own static factory methods and be more consistent.

Or....just get rid of the static methods altogether? Remember, "new(_ => _ == 3)" is a thing.

Should generate the implicit conversion method in the projected Argument types.



Add tests for INamespaceSymbolExtensions and NamespaceGatherer

Future Issues: 
Use nameof() in code generation as much as possible for type name
Create "advanced" architecture/design doc