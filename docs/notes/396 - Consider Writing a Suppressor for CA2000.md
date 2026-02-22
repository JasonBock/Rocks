TODOs:
* DONE - Add support for `ValueTaskInReturnValueSuppressor` to also look for `ValueTask` usage within a `Callback()` body from an adornment.
* DONE - Add a suppressor to suppress `CA2025` within a `ReturnValue()` or `Callback()` call from an adornment.
    * DONE - Add more tests to cover more cases and also the case where it shouldn't suppress
* Add a suppressor to suppress `CA2000` whenever `Instance(...)` is called on an expectations object.