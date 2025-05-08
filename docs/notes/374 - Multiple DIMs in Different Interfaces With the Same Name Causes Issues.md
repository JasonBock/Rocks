There are two things to fix:

* The field names for shims should just be `shim0`, `shim1`, and keep a mapping between the shim type and the field name when referenced.
* When the shim type is generated within the mock type, if it's generic, it needs to have the type parameters as well.