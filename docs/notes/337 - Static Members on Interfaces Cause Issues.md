```c#
var hasStaticAbstractMembers = false;

// Report if this method is a static abstract method.
hasStaticAbstractMembers |= selfMethod.IsStatic && (selfMethod.IsVirtual || selfMethod.IsAbstract);
```