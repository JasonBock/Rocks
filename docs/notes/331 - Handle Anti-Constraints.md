* DONE - Add `allows ref struct` to mock type
* DONE - Add `allows ref struct` to a method when the anti-constraint exists
* DONE - In `TypeReferenceModel`
    * DONE - If a parameter type is either a `ref struct` or it's a type parameter that `allows ref struct`, we must use `RefStructArgument<>`, in the handlers and in the expectations (and adornments?)
    * DONE - If a return type is either a `ref struct` or it's a type parameter that `allows ref struct`, the `ReturnValue` must be a `Func<...>`